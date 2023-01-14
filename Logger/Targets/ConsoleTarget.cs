using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Logger.Models;
using Newtonsoft.Json;

namespace Logger.Targets
{
	public class ConsoleTarget : ITarget
	{
		readonly LogLevelEnum minLevel;

		public ConsoleTarget(LogLevelEnum minLevel = LogLevelEnum.Info)
		{
			this.minLevel = minLevel;
		}


		public async Task<int> SaveAsync(LogItem item)
		{
			if (minLevel == LogLevelEnum.None)
			{
				Console.WriteLine("Log Level set to 'None.' Nothing logged.");
				return 200;
			}

			if (item.LevelId < (int)minLevel)
			{
				Console.WriteLine($"Log min level is {minLevel}. Item level is {item.LevelName}. Nothing logged.");
				return 200;
			}

			string json = "None.";

			if (item.InfoObj is not null)
				json = JsonConvert.SerializeObject(item.InfoObj, Formatting.Indented);

			Console.WriteLine("LogDateGMT");
			Console.WriteLine($"\t{item.EventDateGmt}");
			Console.WriteLine("LogApplication");
			Console.WriteLine($"\t{item.AppName ?? "None"}");
			Console.WriteLine("LogLevel");
			Console.WriteLine($"\t{item.LevelName ?? "None"}");
			Console.WriteLine("LogMessage");
			Console.WriteLine($"\t{item.Message ?? "None"}");
			Console.WriteLine("LogIsException");
			Console.WriteLine($"\t{(item.IsException ? "Yes" : "No")}");
			Console.WriteLine("Info Object");
			Console.WriteLine(json);

			await ValueTask.FromResult(0);
			return 200;
		}

	}
}