
using Logger.Targets;
using Logger.Tests.Services;
using Logger.Models;

namespace Logger.Tests.Scenarios
{
	public static class Basic
	{
		public static async Task ThrowErr()
		{
			try
			{
				throw new Exception("Boom!");
			}
			catch (Exception ex)
			{
				var logger = new LogService("Testing", null!, true);

				await logger.LogAsync(ex);
			}
		}

		public static async Task ErrWithCloudantTarget()
		{
			try
			{
				throw new Exception("Boom!");
			}
			catch (Exception ex)
			{
				var targets = new List<ITarget> {
					new CloudantDbTarget("dummyAcct", "dummyDbName", "dummyAuth", true)
				};

				var logger = new LogService("Testing", targets, false);

				await logger.LogAsync(ex);

			}
		}


		public static async Task ErrWithMailTarget(string mailgunFromDomain, string mailgunAuthValue)
		{
			try
			{
				throw new Exception("Boom!");
			}
			catch (Exception ex)
			{
				var targets = new List<ITarget> {
					new MailgunTarget(mailgunFromDomain, mailgunAuthValue, "noreply@" + mailgunFromDomain, "rpkummer@hotmail.com,rpkummer@gmail.com", LogLevelEnum.Info)
				};

				var logger = new LogService("Testing", targets, false);

				await logger.LogAsync(ex);

			}
		}

		public static async Task InfoObjWithMailTarget(string mailgunFromDomain, string mailgunAuthValue)
		{
			var targets = new List<ITarget> {
					new MailgunTarget(mailgunFromDomain, mailgunAuthValue, "noreply@" + mailgunFromDomain, "rpkummer@hotmail.com,rpkummer@gmail.com", LogLevelEnum.Info)
				};

			var logger = new LogService("Testing", targets, false);

			string? s = null;

			var extra = new {
				First = 1,
				Second = "Second item",
				Third = true,
				Fourth = 123.45d,
				Nada = s,
				Fifth = new { Id = 10, Msg = "This is a sub-object", IsStrange = false, StillNada = s }
			};

				await logger.LogWarningAsync("Warning message here.", extra);
		}

	}
}
