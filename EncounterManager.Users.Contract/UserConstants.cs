namespace EncounterManager.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class UserConstants
    {
        public static Uri UserUri { get; } = new Uri("fabric:/Infrastructure/User");
        public static Uri UserLoaderUri { get; } = new Uri("fabric:/Infrastructure/UserLoader");
        public static Uri UserRegistryUri { get; } = new Uri("fabric:/Infrastructure/UserRegistry");
    }
}
