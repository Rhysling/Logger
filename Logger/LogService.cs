using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logger.Models;
using Logger.Targets;

namespace Logger
{
	public class LogService
	{
		private readonly string applicationName;
		private readonly List<ITarget> targets;
		private readonly bool isTestMode;

		public LogService(string applicationName, List<ITarget> targets, bool isTestMode)
		{
			this.applicationName = applicationName;
			this.targets = targets;
			this.isTestMode = isTestMode;
		}

		public async Task LogErrorAsync(string message, object? infoObj = null)
		{
			var item = new LogItem(LogLevelEnum.Error)
			{
				Message = message,
				IsException = false,
				InfoObj = infoObj
			};

			await LogAsync(item).ConfigureAwait(false);
		}

		public async Task LogWarningAsync(string message, object? infoObj = null)
		{
			var item = new LogItem(LogLevelEnum.Warning)
			{
				Message = message,
				IsException = false,
				InfoObj = infoObj
			};

			await LogAsync(item).ConfigureAwait(false);
		}

		public async Task LogInfoAsync(string message, object? infoObj = null)
		{
			var item = new LogItem(LogLevelEnum.Info)
			{
				Message = message,
				IsException = false,
				InfoObj = infoObj
			};

			await LogAsync(item).ConfigureAwait(false);
		}


		public async Task LogAsync(Exception ex)
		{
			if (ex.Source == "Logger")
			{
				Console.WriteLine("Loger threw -- " + ex.Message);
				Console.WriteLine(ex.StackTrace);
				return;
			}

			var item = new LogItem(LogLevelEnum.Error)
			{
				Message = ex.Source + "--" + ex.Message,
				IsException = true,
				InfoObj = new { ex.Source, ex.StackTrace }
			};

			await LogAsync(item).ConfigureAwait(false);
		}

		public async Task LogAsync(LogItem item)
		{
			item.AppName = applicationName;

			if (isTestMode)
			{
				Console.WriteLine("Log-{0} on {1} -- {2}", item.LevelName, item.EventDateGmt, item.Message ?? "No msg.");
			}
			else
			{
				var tasks = new List<Task>();
				foreach (var t in targets)
					tasks.Add(t.SaveAsync(item));

				await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);
			}
		}


		// ****** Synchronous Version ******
		public void LogError(string message, object? infoObj = null)
		{
			Task.Run(() => LogErrorAsync(message, infoObj).ConfigureAwait(false)).Wait();
		}

		public void LogWarning(string message, object? infoObj = null)
		{
			Task.Run(() => LogWarningAsync(message, infoObj).ConfigureAwait(false)).Wait();
		}

		public void LogInfo(string message, object? infoObj = null)
		{
			Task.Run(() => LogInfoAsync(message, infoObj).ConfigureAwait(false)).Wait();
		}

		public void Log(Exception ex)
		{
			Task.Run(() => LogAsync(ex).ConfigureAwait(false)).Wait();
		}

		public void Log(LogItem item)
		{
			Task.Run(() => LogAsync(item).ConfigureAwait(false)).Wait();
		}

	}

}
