using HotChocolate.Execution;
using HotChocolate.Execution.Instrumentation;

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
}