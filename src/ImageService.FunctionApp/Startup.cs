using ImageService.Core;
using ImageService.FunctionApp;
using ImageService.FunctionApp.Helpers;
using ImageService.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace ImageService.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IFileRepository, AzureStorageFileRepository>();
            builder.Services.AddSingleton<IRequestHelper, RequestHelper>();
        }
    }
}