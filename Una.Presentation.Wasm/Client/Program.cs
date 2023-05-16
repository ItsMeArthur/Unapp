using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Una.Presentation.Wasm;
using Una.Presentation.Wasm.Services.Providers;

namespace Una.Presentation.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped<IRequestProvider, RequestProvider>();

            await builder.Build().RunAsync();
        }
    }
}