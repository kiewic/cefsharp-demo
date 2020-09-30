using System;
using System.Threading.Tasks;

namespace CefSharpApp
{
    class BoundObject
    {
        public string Div(int dividend, int divisor)
        {
            var exploreClientTask = GetClientConceptualSchemaAsync(dividend, divisor);
            exploreClientTask.Wait();
            return exploreClientTask.Result;
        }

        public async Task<string> GetClientConceptualSchemaAsync(int dividend, int divisor)
        {
            await RunAsync();
            return (dividend / divisor).ToString();
        }

        public Task RunAsync()
        {
            return Task.Run(() => Run());
        }

        public void Run()
        {
            Console.WriteLine("Hello from Run()!");
        }
    }
}
