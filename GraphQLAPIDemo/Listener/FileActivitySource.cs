using System.Diagnostics;

namespace GraphQLAPIDemo.Listener
{
    public static class FileActivitySource
    {
        public static readonly ActivitySource GraphQLDemoActivitySource = new ActivitySource("GraphQLAPIDemo");
    }
}
