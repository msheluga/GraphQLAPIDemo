using OpenTelemetry;
using System.Diagnostics;

namespace GraphQLAPIDemo.Listener
{
    public class FileProcessor : BaseProcessor<Activity>
    {
        private readonly string name;

        public FileProcessor(string name = "MyProcessor")
        {
            this.name = name;
        }
        public override void OnStart(Activity activity)
        {
            Console.WriteLine($"{this.name}.OnStart({activity.DisplayName})");
        }

        public override void OnEnd(Activity activity)
        {
            Console.WriteLine($"{this.name}.OnEnd({activity.DisplayName})");
        }

        protected override bool OnForceFlush(int timeoutMilliseconds)
        {
            Console.WriteLine($"{this.name}.OnForceFlush({timeoutMilliseconds})");
            return true;
        }

        protected override bool OnShutdown(int timeoutMilliseconds)
        {
            Console.WriteLine($"{this.name}.OnShutdown({timeoutMilliseconds})");
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            Console.WriteLine($"{this.name}.Dispose({disposing})");
        }
    }
}
