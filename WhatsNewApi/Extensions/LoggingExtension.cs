namespace WhatsNewApi.Extensions;

public static class LoggingExtension
{
    public static void LogException(this ILogger logger, Exception ex)
    {
        logger.LogError(ex.Message);
        logger.LogDebug(ex.StackTrace);
    }
}

