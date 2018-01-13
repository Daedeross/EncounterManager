namespace EncounterManager.Web.Internals
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public interface IRouteContext
    {
        /// <summary>
        /// 
        /// </summary>
        string RouteName { get; }

        /// <summary>
        /// 
        /// </summary>
        IDictionary<string, object> RouteValues { get; }
    }
}