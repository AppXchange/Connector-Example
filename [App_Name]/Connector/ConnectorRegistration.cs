namespace Connector;

using Connector.App.v1;
using Connector.Client;
using Connector.Connections;
using ESR.Hosting;
using ESR.Hosting.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.Client.Testing;

/// <summary>
/// The registration object for your connector. This will be passed to the <see cref = "Extensions.UseGenericServiceRun"/> method at
/// Program startup. <see cref = "Program.Main(string[])"/>
/// </summary>
public class ConnectorRegistration : IConnectorRegistration<ConnectorRegistrationConfig>, IConfigureConnectorApiClient
{
    /// <summary>
    /// Registers any objects that are needed for dependency injection for the connector. 
    /// </summary>
    /// <param name = "serviceCollection"><see cref = "IServiceCollection"/> to register connector types with.</param>
    /// <param name = "hostContext">Host context that provides any configuration for the service run.</param>
    public void ConfigureServices(IServiceCollection serviceCollection, IHostContext hostContext)
    {
        var connectorRegistrationConfig = JsonSerializer.Deserialize<ConnectorRegistrationConfig>(hostContext.GetSystemConfig().Configuration);
        serviceCollection.AddSingleton(connectorRegistrationConfig!);
        serviceCollection.AddTransient<RetryPolicyHandler>();
    }

    /// <summary>
    /// Registers all <see cref = "IConnectorServiceDefinition"/> implementations. If using the xchange CLI tooling, it will normally
    /// add these for you when adding a new module to the connector. Most modules will have an Action processor service definition
    /// and a Cache Writer service definition
    /// </summary>
    /// <param name = "serviceCollection"></param>
    public void RegisterServiceDefinitions(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IConnectorServiceDefinition, AppV1ActionProcessorServiceDefinition>();
        serviceCollection.AddSingleton<IConnectorServiceDefinition, AppV1CacheWriterServiceDefinition>();
    }

    /// <summary>
    /// Configures the API client for the connector, including setting up any authentication needed.
    /// </summary>
    /// <param name="serviceCollection">The service collection to configure dependencies for.</param>
    /// <param name="hostConnectionContext">The host context containing active connection details.</param>
    public void ConfigureConnectorApiClient(IServiceCollection serviceCollection, IHostConnectionContext hostConnectionContext)
    {
        var activeConnection = hostConnectionContext.GetConnection();
        
        // Resolve the active connection and register services based on the authentication type
        serviceCollection.AddHttpClient<ApiClient, ApiClient>(client =>
        {
            var config = activeConnection.ResolveConnectionDefinition<BasicAuth>();
            client.BaseAddress = new Uri(config.BaseUrl);
        })
        .AddHttpMessageHandler<BasicAuthHandler>();

        // Register the BasicAuth configuration and handler
        serviceCollection.AddSingleton<IBasicAuth>(activeConnection.ResolveConnectionDefinition<BasicAuth>());
        serviceCollection.AddTransient<BasicAuthHandler>();
    }

    /// <summary>
    /// Registers the connection test handler to verify the connector's API connection.
    /// </summary>
    /// <param name="serviceCollection">The service collection to register the test handler.</param>
    public void RegisterConnectionTestHandler(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IConnectionTestHandler, ConnectionTestHandler>();
    }
}
