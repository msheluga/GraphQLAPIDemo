using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using System.Diagnostics;
using System.Text;

namespace GraphQLAPIDemo.Listener
{
    
    public class FileActivityExporter : BaseExporter<Activity>
    {
        private const int RightPaddingLength = 30;
        
        public override ExportResult Export(in Batch<Activity> batch)
        {
            

            // SuppressInstrumentationScope should be used to prevent exporter
            // code from generating telemetry and causing live-loop.
            using var scope = SuppressInstrumentationScope.Begin();

            var sb = new StringBuilder();
            foreach (var activity in batch)
            {
                sb.AppendLine($"Activity.Id:          {activity.Id}");
                if (!string.IsNullOrEmpty(activity.ParentId))
                {
                    sb.AppendLine($"Activity.ParentId:    {activity.ParentId}");
                }

                sb.AppendLine($"Activity.ActivitySourceName: {activity.Source.Name}");
                sb.AppendLine($"Activity.DisplayName: {activity.DisplayName}");
                sb.AppendLine($"Activity.Kind:        {activity.Kind}");
                sb.AppendLine($"Activity.StartTime:   {activity.StartTimeUtc:yyyy-MM-ddTHH:mm:ss.fffffffZ}");
                sb.AppendLine($"Activity.Duration:    {activity.Duration}");
                if (activity.TagObjects.Any())
                {
                    sb.AppendLine("Activity.TagObjects:");
                    foreach (var tag in activity.TagObjects)
                    {
                        var array = tag.Value as Array;

                        if (array == null)
                        {
                            sb.AppendLine($"    {tag.Key}: {tag.Value}");
                            continue;
                        }

                        sb.AppendLine($"    {tag.Key}: [{string.Join(", ", array.Cast<object>())}]");
                    }
                }

                if (activity.Events.Any())
                {
                    sb.AppendLine("Activity.Events:");
                    foreach (var activityEvent in activity.Events)
                    {
                        sb.AppendLine($"    {activityEvent.Name} [{activityEvent.Timestamp}]");
                        foreach (var attribute in activityEvent.Tags)
                        {
                            sb.AppendLine($"        {attribute.Key}: {attribute.Value}");
                        }
                    }
                }

                var resource = this.ParentProvider.GetResource();
                if (resource != Resource.Empty)
                {
                    sb.AppendLine("Resource associated with Activity:");
                    foreach (var resourceAttribute in resource.Attributes)
                    {
                        sb.AppendLine($"    {resourceAttribute.Key}: {resourceAttribute.Value}");
                    }
                }
            }
            //adding a line break
            sb.AppendLine(Environment.NewLine);           
           var fullFilePath = System.IO.Path.GetDirectoryName(Directory.GetCurrentDirectory()) +  "//" + String.Format("Log_Trace_{0}.txt", DateTimeOffset.UtcNow.UtcDateTime.ToString("yyyyMMdd"));
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
