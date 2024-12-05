using Microsoft.Extensions.DependencyInjection;
using System;
using Xchange.Connector.SDK.Client.ConnectivityApi.Models;
using ESR.Hosting.Client;
using System.Text.Json;
using Xchange.Connector.SDK.Client.AuthTypes;
using Connector.Connections;

namespace Connector.Client
{
    public static class ClientHelper
    {
        public static class AuthTypeKeyEnums
        {
            public const string BasicAuth = "basicAuth";
        }

        public static void ResolveServices(this IServiceCollection serviceCollection, ConnectionContainer activeConnection)
        {
            serviceCollection.AddTransient<RetryPolicyHandler>();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            switch (activeConnection.DefinitionKey)
            {
                case AuthTypeKeyEnums.BasicAuth:
                    var configBasicAuth = JsonSerializer.Deserialize<BasicAuth>(activeConnection.Configuration, options);
                    serviceCollection.AddSingleton<IBasicAuth>(configBasicAuth!);
                    serviceCollection.AddTransient<RetryPolicyHandler>();
                    serviceCollection.AddTransient<BasicAuthHandler>();
                    serviceCollection.AddHttpClient<ApiClient, ApiClient>(client => new ApiClient(client, configBasicAuth!.BaseUrl)).AddHttpMessageHandler<BasicAuthHandler>().AddHttpMessageHandler<RetryPolicyHandler>();
                    break;
                default:
                    throw new Exception($"Unable to find services for definition key {activeConnection.DefinitionKey}");
            }
        }
    }
}