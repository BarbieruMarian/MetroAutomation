namespace TestFramework.Configuration
{
    public class ConsulOptions
    {
        public bool? InUse { get; set; }
        public string Instance { get; set; }
        public string Token { get; set; }
        public string ServiceName { get; set; }
        public string EnvironmentName { get; set; }
        public string Country { get; set; }
        public string DeploymentType { get; set; }
        public int? DeploymentId { get; set; }
        public bool IsHqEnvironment { get; set; }
    }
}
