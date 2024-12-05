using System;
using Xchange.Connector.SDK.Client.AuthTypes;
using Xchange.Connector.SDK.Client.ConnectionDefinitions.Attributes;

namespace Connector.Connections;

[ConnectionDefinition(title: "BasicAuth", description: "")]
public class BasicAuth : IBasicAuth
{
    [ConnectionProperty(title: "Username", description: "", isRequired: true, isSensitive: false)]
    public string Username { get; init; } = string.Empty;

    [ConnectionProperty(title: "Password", description: "", isRequired: true, isSensitive: true)]
    public string Password { get; init; } = string.Empty;

    [ConnectionProperty(title: "Connection Environment", description: "", isRequired: true, isSensitive: false)]
    public ConnectionEnvironmentBasicAuth ConnectionEnvironment { get; set; } = ConnectionEnvironmentBasicAuth.Unknown;

    public string BaseUrl
    {
        get
        {
            switch (ConnectionEnvironment)
            {
                case ConnectionEnvironmentBasicAuth.Production:
                    return "http://prod.example.com";
                case ConnectionEnvironmentBasicAuth.Test:
                    return "http://test.example.com";
                default:
                    throw new Exception("No base url was set.");
            }
        }
    }
}

public enum ConnectionEnvironmentBasicAuth
{
    Unknown = 0,
    Production = 1,
    Test = 2
}