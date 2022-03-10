using HotChocolate.Execution;
using HotChocolate.Execution.Instrumentation;

/// <summary>
/// Example Logging Listener
/// </summary>
public class MyListener : ExecutionDiagnosticEventListener
{
    private readonly ILogger<MyListener> _logger;

    public MyListener(ILogger<MyListener> logger)
        => _logger = logger;

    public override void RequestError(IRequestContext context,
        Exception exception)
    {
        _logger.LogError(exception, "A request error occured!");
    }
    public override IDisposable ExecuteRequest(IRequestContext context)
    {
        if (context != null && context.Request != null && context.Request.Query != null)
        {
            if (context.Request.Query.ToString().Trim().ToLower().StartsWith("{query("))
            {
                _logger.LogWarning("A query occured!");
            }
            else
            {
                _logger.LogWarning("a mutation has occured");
            }
        }
        return base.ExecuteRequest(context);
    }
}