using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace FlicWebServer
{
    class Program
    {
        private static CancellationTokenSource _serviceCts = new();

        static async Task Main(string[] args)
        {
            
            try
            {
                await HostBuilder.Init(_serviceCts.Token);
                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        
    }
}
