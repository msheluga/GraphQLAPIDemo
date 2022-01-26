using OpenTelemetry;
using OpenTelemetry.Logs;

namespace GraphQLAPIDemo.Listener
{
    internal static class LoggerExtensions
    {
        public static OpenTelemetryLoggerOptions AddMyExporter(this OpenTelemetryLoggerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return options.AddProcessor(new BatchLogRecordExportProcessor(new MyExporter()));
        }
    }
}
