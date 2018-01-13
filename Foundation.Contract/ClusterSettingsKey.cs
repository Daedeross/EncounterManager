namespace Foundation
{
    public static class ClusterSettingKeys
    {
        public static SettingKey<string> FullyQualifiedDomainName { get; } = new SettingKey<string>("Config", "Cluster", "FullyQualifiedDomainName");
        public static SettingKey<int> Port { get; } = new SettingKey<int>("Config", "Cluster", "Port");
        public static SettingKey<string> ApiApplicationName { get; } = new SettingKey<string>("Config", "Cluster", "ApiApplicationName");
        public static SettingKey<string> ServiceApplicationName { get; } = new SettingKey<string>("Config", "Cluster", "ServiceApplicationName");
    }
}
