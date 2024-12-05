using System;
using System.Text.Json.Serialization;
using Json.Schema.Generation;
using Xchange.Connector.SDK.Action;

namespace Connector.App.v1.HealthCheck.Add;

/// <summary>
/// Add a health check
/// </summary>
public class AddHealthCheckAction : IStandardAction<AddHealthCheckActionInput, AddHealthCheckActionOutput>
{
    public AddHealthCheckActionInput ActionInput { get; set; } = new();
    public AddHealthCheckActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class AddHealthCheckActionInput
{
    [Description("The timestamp of the health check.")]
    public DateTime Timestamp { get; set; }
}

public class AddHealthCheckActionOutput
{
    [JsonPropertyName("id")]
    [Description("The ID of the created health check.")]
    public Guid Id { get; set; }
    public bool Success { get; set; }
}