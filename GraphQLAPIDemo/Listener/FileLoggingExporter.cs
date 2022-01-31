using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using System.Text;

namespace GraphQLAPIDemo.Listener
{
    public class FileLoggingExporter : BaseExporter<LogRecord>
    {
        private const int RightPaddingLength = 30;
        public override ExportResult Export(in Batch<LogRecord> batch)
        {
            // SuppressInstrumentationScope should be used to prevent exporter
            // code from generating telemetry and causing live-loop.
            using var scope = SuppressInstrumentationScope.Begin();

            var sb = new StringBuilder();
            foreach (var logRecord in batch)
            {
                sb.AppendLine($"{"LogRecord.TraceId:".PadRight(RightPaddingLength)}{logRecord.TraceId}");
                sb.AppendLine($"{"LogRecord.SpanId:".PadRight(RightPaddingLength)}{logRecord.SpanId}");
                sb.AppendLine($"{"LogRecord.Timestamp:".PadRight(RightPaddingLength)}{logRecord.Timestamp:yyyy-MM-ddTHH:mm:ss.fffffffZ}");
                sb.AppendLine($"{"LogRecord.EventId:".PadRight(RightPaddingLength)}{logRecord.EventId.Id}");
                sb.AppendLine($"{"LogRecord.EventName:".PadRight(RightPaddingLength)}{logRecord.EventId.Name}");
                sb.AppendLine($"{"LogRecord.CategoryName:".PadRight(RightPaddingLength)}{logRecord.CategoryName}");
                sb.AppendLine($"{"LogRecord.LogLevel:".PadRight(RightPaddingLength)}{logRecord.LogLevel}");
                sb.AppendLine($"{"LogRecord.TraceFlags:".PadRight(RightPaddingLength)}{logRecord.TraceFlags}");
                if (logRecord.FormattedMessage != null)
                {
                    sb.AppendLine($"{"LogRecord.FormattedMessage:".PadRight(RightPaddingLength)}{logRecord.FormattedMessage}");
                }

                if (logRecord.State != null)
                {
                    sb.AppendLine($"{"LogRecord.State:".PadRight(RightPaddingLength)}{logRecord.State}");
                }
                else if (logRecord.StateValues != null)
                {
                    sb.AppendLine("LogRecord.StateValues (Key:Value):");
                    for (int i = 0; i < logRecord.StateValues.Count; i++)
                    {
                        sb.AppendLine($"{logRecord.StateValues[i].Key.PadRight(RightPaddingLength)}{logRecord.StateValues[i].Value}");
                    }
                }

                if (logRecord.Exception is { })
                {
                    sb.AppendLine($"{"LogRecord.Exception:".PadRight(RightPaddingLength)}{logRecord.Exception?.Message}");
                }
                var resource = this.ParentProvider.GetResource();
                if (resource != Resource.Empty)
                {
                    sb.AppendLine("Resource associated with LogRecord:");
                    foreach (var resourceAttribute in resource.Attributes)
                    {
                        sb.AppendLine($"    {resourceAttribute.Key}: {resourceAttribute.Value}");
                    }
                }
            }
            //adding a line break
            sb.AppendLine(Environment.NewLine);
            
            var fullFilePath = System.IO.Path.GetDirectoryName(Directory.GetCurrentDirectory()) +  "//" + String.Format("Log_Record_{0}.txt", DateTimeOffset.UtcNow.UtcDateTime.ToString("yyyyMMdd"));
            if (File.Exists(fullFilePath))
            {
                //append to the file
                File.AppendAllTextAsync(fullFilePath, sb.ToString());
            }
            else
            {
                File.WriteAllTextAsync(fullFilePath, sb.ToString());
            }
            
            return ExportResult.Success;
        }
    }
}
