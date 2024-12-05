using ESR.Hosting;
using ESR.Hosting.Action;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Connector.Client;
using Xchange.Connector.SDK.Action;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.App.v1.HealthCheck.Add;

public class AddHealthCheckHandler : IActionHandler<AddHealthCheckAction>
{
    private readonly IHostContext _hostContext;
    private readonly ILogger<AddHealthCheckHandler> _logger;
    private readonly AddHealthCheckConfig _handlerConfig;

    public AddHealthCheckHandler(
        IHostContext hostContext, 
        ILogger<AddHealthCheckHandler> logger, 
        AppV1ActionProcessorConfig config)
    {
        _hostContext = hostContext;
        _logger = logger;
        _handlerConfig = config.AddHealthCheckConfig;
    }

    public Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        AddHealthCheckActionInput? input = JsonSerializer.Deserialize<AddHealthCheckActionInput>(actionInstance.InputJson);
        try
        {
            var response = new ApiResponse<JsonDocument>();

            //await _apiClient.PostHealthCheck(
            //body: JsonSerializer.SerializeToNode(input), cancellationToken)
            //.ConfigureAwait(false);

            DateTime inputTimestamp = input?.Timestamp ?? DateTime.Now;

            if (_handlerConfig.TestAPIFailure)
            {
                throw new ApiException()
                {
                    StatusCode = 500,
                };
            }

            Guid newID = Guid.NewGuid();

            HealthCheckDataObject newHealthCheck = new()
            {
                Id = newID,
                Timestamp = inputTimestamp,
            };

            var operations = new List<SyncOperation>();

            var keyResolver = new DefaultDataObjectKey();
            var key = keyResolver.BuildKeyResolver()(newHealthCheck);
            operations.Add(SyncOperation.CreateSyncOperation("upsert", key.UrlPart, key.PropertyNames, newHealthCheck));

            if (_handlerConfig.TestCacheSyncFailure)
            {
                throw new Exception("Test Cache Sync Failure");
            }

            var resultList = new List<CacheSyncCollection>
            {
                new() { DataObjectType = typeof(HealthCheckDataObject), CacheChanges = operations.ToArray() }
            };

            AddHealthCheckActionOutput resource = new()
            {
                Id = newID,
                Success = true // Just return a success every time
            };

            return Task.FromResult(ActionHandlerOutcome.Successful(resource, resultList));
        }
        catch (ApiException exception)
        {
            // Common to create extension methods to map to Standard Action Failure
            var errorSource = new List<string> { "AddHealthCheckHandler" };
            if (string.IsNullOrEmpty(exception.Source)) errorSource.Add(exception.Source!);

            return Task.FromResult(
                ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = exception.StatusCode.ToString(),
                    Errors = new[]
                    {
                        new Xchange.Connector.SDK.Action.Error
                        {
                            Source = errorSource.ToArray(),
                            Text = exception.Message
                        }
                    }
                })
            );
        }
    }
}