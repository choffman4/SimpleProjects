using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SaltedHashLibrary.BusinessLogic;
using EncryptDecryptLibrary.BusinessLogic;
using SEN320Labs;

internal class Program
{
    private static void Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        ILogger<Program> _logger = services.GetRequiredService<ILogger<Program>>();
        _logger.LogInformation("Application Booting Up...");

        try
        {
            services.GetRequiredService<App>().Run();
        } catch (Exception ex)
        {
            _logger.LogError($"An error occurred: {ex.Message}");
        }
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Information);
            })
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<IHashService, HashService>();
                services.AddSingleton<IXORCipher, XORCipherService>();
                services.AddSingleton<App>();
            });
    }
}