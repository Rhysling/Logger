using Logger.Models;
using Logger.Targets;

namespace Logger
{
	public class LogService
	{
		private readonly string applicationName;
		private readonly List<ITarget> targets;
		private readonly bool isTestMode = false;

		public LogService(string applicationName, List<ITarget> targets, bool isTestMode = false)
		{
			this.applicationName = applicationName;
			this.targets = targets;
			this.isTestMode = isTestMode;
		}

		public async Task<int[]> LogErrorAsync(string message, object? infoObj = null)
		{
			var item = new LogItem(LogLevelEnum.Error)
			{
				Message = message,
				IsException = (infoObj != null) && (infoObj is Exception),
				InfoObj = infoObj
			};

			return await LogAsync(item).ConfigureAwait(false);
		}

		public async Task<int[]> LogWarningAsync(string message, object? infoObj = null)
		{
			var item = new LogItem(LogLevelEnum.Warning)
			{
				Message = message,
				IsException = (infoObj != null) && (infoObj is Exception),
				InfoObj = infoObj
			};

			return await LogAsync(item).ConfigureAwait(false);
		}

		public async Task<int[]> LogInfoAsync(string message, object? infoObj = null)
		{
			var item = new LogItem(LogLevelEnum.Info)
			{
				Message = message,
				IsException = (infoObj != null) && (infoObj is Exception),
				InfoObj = infoObj
			};

			return await LogAsync(item).ConfigureAwait(false);
		}


		public async Task<int[]> LogAsync(Exception ex)
		{
			if ((ex.Source ?? "x").StartsWith("Logger"))
			{
				Console.WriteLine("Loger threw -- " + ex.Message);
				Console.WriteLine(ex.StackTrace ?? "No stack trace.");
				return new int[] { 999 };
			}

			var item = new LogItem(LogLevelEnum.Error)
			{
				Message = (ex.Source ?? "No Source") + "--" + ex.Message,
				IsException = true,
				InfoObj = new { ex.Source, ex.StackTrace }
			};

			return await LogAsync(item).ConfigureAwait(false);
		}

		public async Task<int[]> LogAsync(LogItem item)
		{
			item.AppName = applicationName;

			if (isTestMode)
			{
				Console.WriteLine("Log-{0} on {1} -- {2}", item.LevelName, item.EventDateGmt, item.Message ?? "No msg.");
				return Array.Empty<int>();
			}
			
			var tasks = new List<Task<int>>();
			foreach (var t in targets)
				tasks.Add(t.SaveAsync(item));

			return await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);
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
