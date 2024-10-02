using CommandLine;
using KafkaWriter;
using KafkaWriter.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Parser.Default.ParseArguments<Options>(args)
    .WithParsed<Options>(o =>
    {
        bool doRead = false;
        var writer = GetHost().Services.GetService<Publisher>();
        string key = Task.Run(async () => await writer!.WriteToKafka()).Result;
        Console.WriteLine($"Finished Write. Key: '{key}'");
        Console.WriteLine("");

        var cancelToken = new CancellationTokenSource();
        CancellationToken token = cancelToken.Token;
        token.Register(() => Console.WriteLine("Cancellation requested."));

        if (doRead)
        {
            var consumer = GetHost().Services.GetService<Consumer>();
            Task t = Task.Run(async () => await consumer!.ReadFromKafka(key, token));
            t.Wait();

            Console.WriteLine("Finished Read.");
        }
    });

static IHost GetHost()
{
    var config = new ConfigurationBuilder()
            .AddJsonFile(
                "Settings/appsettings3.json",
                optional: false,
                reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

    var host = Host.CreateDefaultBuilder()
    .ConfigureServices(
        (_,
            services) =>
        {
            services.AddSingleton(config.GetSection(AppSettingConstants.KafkaPublisherConfig).Get<KafkaPublisherConfig>()!);
            services.AddSingleton(config.GetSection(AppSettingConstants.KafkaConsumerConfig).Get<KafkaConsumerConfig>()!);
            services.AddSingleton(config.GetSection(AppSettingConstants.MessageConfig).Get<MessageConfig>()!);
            services.AddScoped<Publisher>();
            services.AddScoped<Consumer>();
        })
    .Build();
    return host;
}

public class Options
{
}