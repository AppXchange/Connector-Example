using System;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xchange.Connector.SDK.Client.AuthTypes;

namespace Connector.Client;

public class BasicAuthHandler : DelegatingHandler
{
    private readonly IBasicAuth _basicAuth;

    public BasicAuthHandler(IBasicAuth basicAuth)
    {
        _basicAuth = basicAuth;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Remove("Authorization");
        var byteArray = Encoding
            .ASCII
            .GetBytes($"{_basicAuth.Username}:{_basicAuth.Password}");
        request.Headers.Add("Authorization", $"Basic {Convert.ToBase64String(byteArray)}");

        return await base.SendAsync(request, cancellationToken);
    }
}