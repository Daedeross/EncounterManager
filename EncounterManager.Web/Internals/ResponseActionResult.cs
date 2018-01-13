namespace EncounterManager.Web.Internals
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Threading.Tasks;
    using Foundation;
    using Foundation.ServiceFabric;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Custom IActionResult implementation that is also a response visitor.
    /// This class is capable of visiting a response implementation and generating a proper IActionResult from the
    /// response state and values.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.IActionResult" />
    /// <seealso cref="Foundation.IResponseVisitor" />
    public class ResponseActionResult : IActionResult, IResponseVisitor
    {
        private readonly IRouteContext _routeContext;

        private IActionResult _result;

        private string _message;
        private object _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseActionResult"/> class.
        /// </summary>
        public ResponseActionResult(IRouteContext routeContext)
        {
            _routeContext = routeContext;
        }

        /// <summary>
        /// Execute this IActionResult
        /// </summary>
        /// <param name="context"><see cref="ActionContext"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task ExecuteResultAsync(ActionContext context)
        {
            if (_result == null)
            {
                _result = CreateError();
            }
            await _result.ExecuteResultAsync(context);
        }

        #region IResponseVisitor

        /// <summary>
        /// Visit a basic response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns><see cref="T:Insignia.Foundation.Response" /></returns>
        public Response Visit(Response response)
        {
            _message = response.Message;
            _result = CreateResultFromState(response.ResponseState);
            return response;
        }

        /// <summary>
        /// Visit a value response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">The response.</param>
        /// <returns><see cref="T:Insignia.Foundation.Response`1" /></returns>
        public Response<T> Visit<T>(Response<T> response)
        {
            _message = response.Message;
            _value = response.Value;
            _result = CreateResultFromState(response.ResponseState);
            return response;
        }

        /// <summary>
        /// Visit a query response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">The response.</param>
        /// <returns><see cref="T:Insignia.Foundation.QueryResponse`1" /></returns>
        public QueryResponse<T> Visit<T>(QueryResponse<T> response)
        {
            _message = response.Message;
            _value = response;
            _result = CreateResultFromState(response.ResponseState);
            return response;
        }

        #endregion IResponseVisitor

        private IActionResult CreateResultFromState(ResponseState state)
        {
            switch (state)
            {
                case ResponseState.Ok:
                    return CreateOk();
                case ResponseState.Created:
                    return CreateCreated();
                case ResponseState.Accepted:
                    return CreateAccepted();
                case ResponseState.BadRequest:
                    return CreateBadRequest();
                case ResponseState.Forbidden:
                    return CreateForbidden();
                case ResponseState.NotFound:
                    return CreateNotFound();
                case ResponseState.Conflict:
                    return CreateConflict();
                case ResponseState.Error:
                    return CreateError();
                default:
                    return CreateUnknown(state);
            }
        }

        private IActionResult CreateOk()
        {
            if (_value != null)
            {
                return new OkObjectResult(_value);
            }
            if (!string.IsNullOrEmpty(_message))
            {
                return new OkObjectResult(_message);
            }
            return new OkResult();
        }

        private IActionResult CreateCreated()
        {
            var eo = new ExpandoObject();
            var routeDictionary = (IDictionary<string, object>)eo;
            if (_routeContext?.RouteValues != null)
            {
                foreach (var routeValue in _routeContext.RouteValues)
                {
                    routeDictionary.Add(routeValue.Key, routeValue.Value);
                }
            }

            object responseValue = null;

            var record = _value as IRecord;
            if (record != null)
            {
                var identity = record.Id;
                routeDictionary.Add("id", identity);
                responseValue = identity;
            }
            else if (_value != null)
            {
                responseValue = _value;
            }
            else if (!string.IsNullOrEmpty(_message))
            {
                responseValue = _message;
            }
            return string.IsNullOrEmpty(_routeContext?.RouteName)
                ? new CreatedAtRouteResult(eo, responseValue)
                : new CreatedAtRouteResult(_routeContext.RouteName, eo, responseValue);
        }

        private IActionResult CreateAccepted()
        {
            return new AcceptedResult();
        }

        private IActionResult CreateBadRequest()
        {
            if (_value != null)
            {
                return new BadRequestObjectResult(_value);
            }
            if (!string.IsNullOrEmpty(_message))
            {
                return new BadRequestObjectResult(_message);
            }
            return new BadRequestResult();
        }

        private IActionResult CreateForbidden()
        {
            return new ForbidResult();
        }

        private IActionResult CreateNotFound()
        {
            if (_value != null)
            {
                return new NotFoundObjectResult(_value);
            }
            if (!string.IsNullOrEmpty(_message))
            {
                return new NotFoundObjectResult(_message);
            }
            return  new NotFoundResult();
        }

        private IActionResult CreateConflict()
        {
            return new StatusCodeResult(StatusCodes.Status409Conflict);
        }

        private IActionResult CreateError()
        {
            if (_message != null)
            {
                ServiceEventSource.Current.Message($"Response Status(500): {_message}");
            }
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        private IActionResult CreateUnknown(ResponseState state)
        {
            ServiceEventSource.Current.Message($"Unhandled ResponseState:{state}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}