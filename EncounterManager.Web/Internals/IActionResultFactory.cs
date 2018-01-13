namespace EncounterManager.Web.Internals
{
    using System.Threading.Tasks;
    using Foundation;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Constructs <see cref="IActionResult"/> out of <see cref="Response"/>, <see cref="Response{T}"/> or <see cref="QueryResponse{T}"/>
    /// </summary>
    public interface IActionResultFactory
    {
        /// <summary>
        /// Construct <see cref="IActionResult"/> from an asynchronous Task
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTask">The response task.</param>
        /// <param name="context"><see cref="IRouteContext"/></param>
        /// <returns><see cref="Task{TResult}"/> of type <see cref="IActionResult"/></returns>
        Task<IActionResult> FromResponseAsync<T>(Task<T> responseTask, IRouteContext context) where T : Response;

        /// <summary>
        /// Construct <see cref="IActionResult"/> from <see cref="Response"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">The response.</param>
        /// <param name="context"><see cref="IRouteContext"/></param>
        /// <returns><see cref="IActionResult"/></returns>
        IActionResult FromResponse<T>(T response, IRouteContext context) where T : Response;
    }
}