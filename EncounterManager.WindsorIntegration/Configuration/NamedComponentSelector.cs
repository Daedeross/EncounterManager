namespace EncounterManager.Configuration
{
    using System.Reflection;
    using Castle.Facilities.TypedFactory;

    public class NamedComponentSelector : DefaultTypedFactoryComponentSelector
    {
        private readonly string _suffix;

        public NamedComponentSelector(string suffix = null)
            : base(fallbackToResolveByTypeIfNameNotFound: true)
        {
            _suffix = suffix;
        }

        protected override string GetComponentName(MethodInfo method, object[] arguments)
        {
            var service = arguments[0] as string;

            return string.IsNullOrEmpty(service) ? null : $"{service}{_suffix}";
        }
    }
}