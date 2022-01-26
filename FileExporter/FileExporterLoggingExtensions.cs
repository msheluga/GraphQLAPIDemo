using OpenTelemetry;
using OpenTelemetry.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTPFileExporter
{
    public static class FileExporterLoggingExtensions
    {
        public static OpenTelemetryLoggerOptions AddConsoleExporter(this OpenTelemetryLoggerOptions loggerOptions, Action<FileExporterOptions> configure = null)
        {
            Guard.ThrowIfNull(loggerOptions, nameof(loggerOptions));

            var options = new FileExporterOptions();
            configure?.Invoke(options);
            return loggerOptions.AddProcessor(new SimpleLogRecordExportProcessor(new FileLogRecordExporter(options)));
        }
    }
}
