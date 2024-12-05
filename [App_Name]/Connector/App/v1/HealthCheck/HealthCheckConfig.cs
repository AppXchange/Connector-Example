using System.ComponentModel;
using ESR.Hosting.CacheWriter;

namespace Connector.App.v1.HealthCheck;

public class HealthCheckConfig : CacheWriterObjectConfig
{
    [Description("Use this configuration to validate a cache insert event.")]
    public bool ValidateCacheInsert { get; set; }
    [Description("Use this configuration to validate a cache update event.")]
    public bool ValidateCacheUpdate { get; set; }
    [Description("Use this configuration to validate a cache delete event.")]
    public bool ValidateCacheDelete { get; set; }
}