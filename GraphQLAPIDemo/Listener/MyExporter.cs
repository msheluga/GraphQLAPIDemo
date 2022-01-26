using OpenTelemetry;
using OpenTelemetry.Logs;
using System.Text;

namespace GraphQLAPIDemo.Listener
{
    public class MyExporter : BaseExporter<LogRecord>
    {
        private const int RightPaddingLength = 30;
        public override ExportResult Export(in Batch<LogRecord> batch)
        {
            

            // SuppressInstrumentationScope should be used to prevent exporter
            // code from generating telemetry and causing live-loop.
            using var scope = SuppressInstrumentationScope.Begin();
            var sb = new StringBuilder();
            foreach (var record in batch)
            {
                if (sb.Length > 0)
                {
                    sb.AppendLine(", ");
                }

                sb.AppendLine($"{"LogRecord.TraceId:".PadRight(RightPaddingLength)}{record.TraceId}");
                sb.AppendLine($"{"LogRecord.TraceId:".PadRight(RightPaddingLength)}{record.SpanId}");
                sb.AppendLine($"{"LogRecord.Timestamp:".PadRight(RightPaddingLength)}{record.Timestamp:yyyy-MM-ddTHH:mm:ss.fffffffZ}");
                sb.AppendLine($"{"LogRecord.EventId:".PadRight(RightPaddingLength)}{record.EventId.Id}");
                sb.AppendLine($"{"LogRecord.EventName:".PadRight(RightPaddingLength)}{record.EventId.Name}");
                sb.AppendLine($"{"LogRecord.CategoryName:".PadRight(RightPaddingLength)}{record.CategoryName}");
                sb.AppendLine($"{"LogRecord.LogLevel:".PadRight(RightPaddingLength)}{record.LogLevel}");
                sb.AppendLine($"{"LogRecord.TraceFlags:".PadRight(RightPaddingLength)}{record.TraceFlags}");

                int scopeDepth = -1;

                record.ForEachScope(ProcessScope, sb);

                void ProcessScope(LogRecordScope scope, StringBuilder builder)
                {
                    if (++scopeDepth == 0)
                    {
                        builder.AppendLine("LogRecord.ScopeValues (Key:Value):");
                    }

                    foreach (KeyValuePair<string, object> scopeItem in scope)
                    {
                        builder.AppendLine($"[Scope.{scopeDepth}]:{scopeItem.Key.PadRight(RightPaddingLength)}{scopeItem.Value}");
                    }
                }

                sb.Append(')');
            }

            //Console.WriteLine($"{this.name}.Export([{sb.ToString()}])");
            var systemPath = AppDomain.CurrentDomain.BaseDirectory;
            File.WriteAllText(systemPath + "//" +  DateTime.UtcNow.ToFileTimeUtc()+ "Log.txt", sb.ToString());
            return ExportResult.Success;
        }
    
        
    }
}
