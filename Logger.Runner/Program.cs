using Logger.Tests.Scenarios;
using Logger.Tests.Services;
using Microsoft.Extensions.Configuration;

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddEnvironmentVariables();

var configurationRoot = builder.Build();

var config = configurationRoot.Get<AppSettings>();

//await Basic.ErrWithMailTarget(config.Mailgun!.FromDomain!, config.Mailgun.AuthValue!);
await Basic.InfoObjWithMailTarget(config.Mailgun!.FromDomain!, config.Mailgun.AuthValue!);


Console.WriteLine();
Console.WriteLine("Done.");
Console.ReadKey();