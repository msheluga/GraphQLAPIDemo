using OpenTelemetry;
using OpenTelemetry.Logs;

namespace GraphQLAPIDemo.Listener
{
    public static class FileLoggingExtension
    {
        public static OpenTelemetryLoggerOptions AddFileExporter(this OpenTelemetryLoggerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return options.AddProcessor(new BatchLogRecordExportProcessor(new FileLoggingExporter()));
        }
    }
}
