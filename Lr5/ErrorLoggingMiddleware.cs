public class ErrorLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            LogException(ex);
            throw;
        }
    }

    private void LogException(Exception ex)
    {
        File.AppendAllText("error.log", $"{DateTime.Now}: {ex.Message}\n");
    }
}

