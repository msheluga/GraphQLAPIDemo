using OpenTelemetry;
using OpenTelemetry.Trace;


namespace GraphQLAPIDemo.Listener
{
    internal static class FileTraceExtensions
    {
        public static TracerProviderBuilder AddFileExporter(this TracerProviderBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.AddProcessor(new BatchActivityExportProcessor(new FileActivityExporter()));
        }
    }
}
