namespace EncounterManager.Web.Internals
{
    using System.Threading.Tasks;
    using Foundation;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class ActionResultFactory : IActionResultFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTask"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<IActionResult> FromResponseAsync<T>(Task<T> responseTask, IRouteContext context) where T : Response
        {
            return responseTask.ContinueWith(t => {
                if (t.IsFaulted)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                return FromResponse(t.Result, context);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IActionResult FromResponse<T>(T response, IRouteContext context) where T : Response
        {
            var result = new ResponseActionResult(context);
            response.Accept(result);
            return result;
        }
    }
}