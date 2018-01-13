namespace EncounterManager.Web.Internals
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class RouteContext : IRouteContext
    {
        /// <summary>
        /// 
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> RouteValues { get; set; }
    }
}