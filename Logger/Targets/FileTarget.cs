using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger.Models;

namespace Logger.Targets
{
	public class FileTarget : ITarget
	{
		readonly string logFilePath;
		readonly LogLevelEnum minLevel;

		public FileTarget(string logFilePath, LogLevelEnum minLevel)
		{
			this.logFilePath = logFilePath;
			this.minLevel = minLevel;

			if (!File.Exists(logFilePath))
			{
				using StreamWriter sw = File.AppendText(logFilePath);
				sw.WriteLine(FirstLine());
			}

		}


		public async Task SaveAsync(LogItem item)
		{
			if (minLevel == LogLevelEnum.None || item.LevelId < (int)minLevel)
				return;

			using (StreamWriter sw = File.AppendText(logFilePath))
			{
				sw.WriteLine(BuildLine(item));
			}
			await Task.FromResult(0);
			return;
		}

		// private ***

		private static string FirstLine()
		{
			var sb = new StringBuilder();
			sb.Append("LogDateGMT");
			sb.Append('\t');
			sb.Append("LogApplication");
			sb.Append('\t');
			sb.Append("LogLevel");
			sb.Append('\t');
			sb.Append("LogMessage");
			sb.Append('\t');
			sb.Append("LogIsException");

			return sb.ToString();
		}

		private static string BuildLine(LogItem item)
		{
			var sb = new StringBuilder();
			sb.Append(item.EventDateGmt);
			sb.Append('\t');
			sb.Append(item.AppName ?? "None");
			sb.Append('\t');
			sb.Append(item.LevelName ?? "None");
			sb.Append('\t');
			sb.Append(item.Message ?? "None");
			sb.Append('\t');
			sb.Append(item.IsException ? "Yes" : "No");

			return sb.ToString();
		}

	}
}