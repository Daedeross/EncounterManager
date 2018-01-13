namespace EncounterManager.Web.Configuration
{
    using Foundation;

    /// <summary>
    /// SettingKey constants for Azure specific settings
    /// </summary>
    public static class AzureSettingKeys
    {
        /// <summary>
        /// Azure Client Identifier
        /// </summary>
        public static SettingKey<string> ClientId { get; } = new SettingKey<string>("Config", "Azure", "ClientId");
        /// <summary>
        /// Azure Client Secret
        /// </summary>
        public static SettingKey<string> ClientSecret { get; } = new SettingKey<string>("Config", "Azure", "ClientSecret");
        /// <summary>
        /// Azure Tenant
        /// </summary>
        public static SettingKey<string> Tenant { get; } = new SettingKey<string>("Config", "Azure", "Tenant");
        /// <summary>
        /// Azure Tenant ID
        /// </summary>
        public static SettingKey<string> TenantId { get; } = new SettingKey<string>("Config", "Azure", "TenantId");
        /// <summary>
        /// Azure Active Directory Instance
        /// </summary>
        public static SettingKey<string> AADInstance { get; } = new SettingKey<string>("Config", "Azure", "AADInstance");
        /// <summary>
        /// Application Domain
        /// </summary>
        public static SettingKey<string> Domain { get; } = new SettingKey<string>("Config", "Azure", "Domain");
        /// <summary>
        /// Gets the login callback path.
        /// </summary>
        public static SettingKey<string> LoginCallbackPath { get; } = new SettingKey<string>("Config", "Azure", "LoginCallbackPath");
        /// <summary>
        /// Gets the logout callback path.
        /// </summary>
        public static SettingKey<string> LogoutCallbackPath { get; } = new SettingKey<string>("Config", "Azure", "LogoutCallbackPath");
    }
}