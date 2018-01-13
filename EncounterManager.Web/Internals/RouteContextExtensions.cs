namespace EncounterManager.Web.Internals
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    internal static class RouteContextExtensions
    {
        public static IRouteContext AsRouteContext(this ControllerContext controllerContext)
        {
            return new RouteContext
            {
                RouteValues = controllerContext.RouteData.Values.ToDictionary(p => p.Key, p => p.Value)
            };
        }

        public static IRouteContext Action(this IRouteContext context, string actionName)
        {
            context.RouteValues["action"] = actionName;
            return context;
        }

        public static IRouteContext Controller(this IRouteContext context, string controllerName)
        {
            context.RouteValues["controller"] = controllerName;
            return context;
        }
    }
}