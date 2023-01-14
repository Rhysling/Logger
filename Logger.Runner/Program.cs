using Logger.Tests.Scenarios;
using Logger.Tests.Services;
using Microsoft.Extensions.Configuration;

//var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddEnvironmentVariables();

var configurationRoot = builder.Build();

AppSettings config = configurationRoot.Get<AppSettings>();

//await TestScenarios.ThrowErr();
//await TestScenarios.ErrWithLocalTargets(config.TestLogFilePath!);
var res = await TestScenarios.ErrWithMailTarget(config.Mailgun!.FromDomain!, config.Mailgun.AuthValue!);
//await TestScenarios.InfoObjWithMailTarget(config.Mailgun!.FromDomain!, config.Mailgun.AuthValue!);
//var res = await TestScenarios.InfoObjWithAllTargets(config.Mailgun!.FromDomain!, config.Mailgun.AuthValue!, config.TestLogFilePath!);

if (res.Any(a => a > 299))
  Console.WriteLine("At least one target result code shows not success.");

Console.WriteLine();
Console.WriteLine("Done.");
Console.ReadKey();