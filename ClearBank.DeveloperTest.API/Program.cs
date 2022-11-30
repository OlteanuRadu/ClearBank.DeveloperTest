using ClearBank.DeveloperTest.Configs;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(serviceCollection => {
        serviceCollection
        .AddOptions<PaymentServiceOptions>()
        .Configure<IConfiguration>((settings, config) => config.GetSection(nameof(PaymentServiceOptions)).Bind(settings));

        serviceCollection.AddTransient<IPaymentService, PaymentService>();
        serviceCollection.AddTransient<IAccountDataStoreProvider, AccountDataStoreProvider>();
        serviceCollection.AddTransient<IAccountValidator, AccountValidator>();
    })
    .Build();

host.Run();
