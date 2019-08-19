using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Orchestrator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddXmlFile("settings.xml", false);
            configBuilder.AddXmlFile("secrets.xml", true);
            var config = configBuilder.Build();

            var app = new Application(config);
            await app.Run();
        }
    }
}
