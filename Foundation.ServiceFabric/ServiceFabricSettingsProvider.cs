namespace Foundation.ServiceFabric
{
    using System.Fabric;
    using Foundation.Utilities;

    public class ServiceFabricSettingsProvider : SettingsProviderBase
    {
        private readonly ICodePackageActivationContext _context;

        public ServiceFabricSettingsProvider(ICodePackageActivationContext context)
        {
            Args.NotNull(context, nameof(context));

            _context = context;
        }

        protected override bool TryGetSetting(string config, string section, string parameter, out string value)
        {
            return _context.TryGetSetting(config, section, parameter, out value);
        }
    }
}