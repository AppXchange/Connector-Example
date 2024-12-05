using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Connector.Client;
using ESR.Hosting;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.App.v1.HealthCheck;

public class HealthCheckDataReader : TypedAsyncDataReaderBase<HealthCheckDataObject>
{
    private readonly ILogger<HealthCheckDataReader> _logger;
    private readonly IHostContext _hostContext;
    private readonly HealthCheckConfig _healthCheckConfig;

    public HealthCheckDataReader(
        ILogger<HealthCheckDataReader> logger,
        IHostContext hostContext,
        AppV1CacheWriterConfig config)
    {
        _logger = logger;
        _hostContext = hostContext;
        _healthCheckConfig = config.HealthCheckConfig;
    }

    public override IAsyncEnumerable<HealthCheckDataObject> GetTypedDataAsync(DataObjectCacheWriteArguments? dataObjectRunArguments, CancellationToken cancellationToken)
    {
        var healthCheckCollection = new List<HealthCheckDataObject>
        {
            new() { Id = Guid.Parse("da7e0001-d858-47e0-912f-3221a38d8860"), Timestamp = DateTime.UnixEpoch },
            new() { Id = Guid.Parse("da7e0002-1964-41f1-8bb6-34fba1d485ee"), Timestamp = DateTime.UnixEpoch },
            new() { Id = Guid.Parse("da7e0003-743a-4aef-9d6b-b5c7ddbf5bef"), Timestamp = DateTime.UnixEpoch }
        };

        if (_healthCheckConfig.ValidateCacheInsert)
        {
            // First-run : Inserts a new Health Check Data Object
            // Subsequent runs : Inserts a new Health Check Data Object and delete prior run insert
            healthCheckCollection.Add(new HealthCheckDataObject { Id = Guid.NewGuid(), Timestamp = DateTime.Now });
        }
        if (_healthCheckConfig.ValidateCacheUpdate)
        {
            // Every run : Updates the timestamp of each Health Check Data Object
            foreach (var healthCheckDataObject in healthCheckCollection)
            {
                healthCheckDataObject.Timestamp = DateTime.Now;
            }
        }
        if (_healthCheckConfig.ValidateCacheDelete)
        {
            // First-run : Deletes the specific Health Check Data Object
            // Subsequent runs : Ensures it is missing from the collection
            healthCheckCollection.FindAll(
                obj => obj.Id == Guid.Parse("da7e0001-d858-47e0-912f-3221a38d8860")
            ).ForEach(obj => healthCheckCollection.Remove(obj));
        }

        return healthCheckCollection.ToAsyncEnumerable();
    }
}