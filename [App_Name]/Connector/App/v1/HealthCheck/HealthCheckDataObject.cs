using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.App.v1.HealthCheck;

[PrimaryKey("id", nameof(Id))]
[Description("Represents a health check for an object.")]
public class HealthCheckDataObject
{
    [JsonPropertyName("id")]
    [Description("An id for the object.")]
    public Guid Id { get; set; }

    [JsonPropertyName("timestamp")]
    [Description("The last time the heartbeat was updated.")]
    public DateTime Timestamp { get; set; }
}