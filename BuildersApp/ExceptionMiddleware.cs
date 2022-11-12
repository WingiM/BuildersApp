namespace BuildersApp;

public class ExceptionMiddleware
{
    public readonly RequestDelegate Next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        Next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await Next(context);
        }
        catch (Exception e)
        {
            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;
            _logger.Log(LogLevel.Information, e.ToString());
            await context.Response.WriteAsJsonAsync(new
            {
                Message =
                    "Internal error. " +
                    "Contact developer to solve the problem."
            });
        }
    }
}