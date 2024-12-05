using System.IO;

namespace Connector.Client;

public class ApiResponse
{
    public bool IsSuccessful { get; init; }
    public int StatusCode { get; init; }
    // Not safe to read if `Result` is not null
    public Stream? RawResult { get; init; }
}

public class ApiResponse<TResult> : ApiResponse
{
    public TResult? Result { get; init; }
}