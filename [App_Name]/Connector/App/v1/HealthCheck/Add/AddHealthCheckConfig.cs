using System.ComponentModel;
using Xchange.Connector.SDK.Action;

namespace Connector.App.v1.HealthCheck.Add;

public class AddHealthCheckConfig : DefaultActionHandlerConfig
{
    [Description("Use this configuration to test an API failure.")]
    public bool TestAPIFailure { get; set; }
    [Description("Use this configuration to test a cache sync failure.")]
    public bool TestCacheSyncFailure { get; set; }
}