using Logger.Models;
using Logger.Services;

namespace Logger.Targets
{
	public class MailgunTarget : ITarget
	{
		private HttpClient client;
		private const string baseAddress = "https://api.mailgun.net/v3/{0}/messages";
		private string fromAddress;
		private string toAddresses;
		private LogLevelEnum minLevel;
		//private bool isProduction;

		public MailgunTarget(
			string domain,
			string authValue,
			string fromAddress,
			string toAddresses, // ',' delimited
			LogLevelEnum minLevel = LogLevelEnum.Error
		)
		{
			client = new HttpClient();
			client.BaseAddress = new Uri(String.Format(baseAddress, domain));
			client.DefaultRequestHeaders.Add("Authorization", authValue);

			this.fromAddress = fromAddress;
			this.toAddresses = toAddresses;
			this.minLevel = minLevel;
			//this.isProduction = isProduction;
		}

		public async Task SaveAsync(LogItem item)
		{
			if (item.LevelId < (int)minLevel)
				return;

			var msg = new LogMessage(item, toAddresses);

			var parameters = new Dictionary<string, string> {
				{ "from", fromAddress },
				{ "to", toAddresses },
				{ "subject", msg.Subject },
				{ "text", item.LevelName + ": " + (item.Message ?? "No Message") },
				{ "html", msg.RebderBody() }
			};

			var encodedContent = new FormUrlEncodedContent(parameters);

			using (var res = await client.PostAsync("", encodedContent).ConfigureAwait(false))
			{
				res.EnsureSuccessStatusCode();
			}

		}

	}
}