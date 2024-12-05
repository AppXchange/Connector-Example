using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using System.Threading;
using Connector.Client;
using ESR.Hosting;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.App.v1.Employees;

public class EmployeesDataReader : TypedAsyncDataReaderBase<EmployeesDataObject>
{
    private readonly ILogger<EmployeesDataReader> _logger;
    private readonly IHostContext _hostContext;
    private readonly ApiClient _apiClient;

    public EmployeesDataReader(
        ILogger<EmployeesDataReader> logger,
        IHostContext hostContext,
        ApiClient apiClient)
    {
        _logger = logger;
        _hostContext = hostContext;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<EmployeesDataObject> GetTypedDataAsync(DataObjectCacheWriteArguments? dataObjectRunArguments, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ApiResponse<EmployeesDataObject[]> response;
        try
        {
            // Fetch employees from the API client
            response = await _apiClient.GetEmployees(cancellationToken).ConfigureAwait(false);
        }
        catch (ApiException exception)
        {
            if (exception.InnerException != null)
            {
                _logger.LogError(exception.InnerException, "Unexpected error while retrieving records for 'EmployeesDataObject'");
                _logger.LogError("API response: {Message}", exception.Message);
                throw;
            }
            if (exception.StatusCode is >= 200 and <= 299)
                yield break;
            throw;
        }

        var result = response.Result;

        if (result == null || !result.Any())
        {
            _logger.LogWarning("No data returned from API for 'EmployeesDataObject'");
            yield break;
        }

        // Iterate over each item from the API response and map it to EmployeesDataObject
        foreach (var item in result)
        {
            var resource = new EmployeesDataObject
            {
                Id = item.Id,
                EmployeeId = item.EmployeeId,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Active = item.Active
            };
            yield return resource;
        }
    }
}
