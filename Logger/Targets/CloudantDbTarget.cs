using System;
using System.Threading.Tasks;
using Logger.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Logger.Targets
{
	public class CloudantDbTarget : ITarget
	{
		private CloudantDb.Services.DbService db;
		private bool isTesting = false;

		public CloudantDbTarget(string account, string dbName, string dbAuth)
		{
			db = new CloudantDb.Services.DbService(account, dbName, dbAuth);
		}

		public async Task SaveAsync(LogItem item)
		{
			var serializer = new JsonSerializer()
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				NullValueHandling = NullValueHandling.Ignore
			};

			string ts = item.EventTimestamp.ToString().PadLeft(19, '0');

			var jo = JObject.FromObject(item, serializer);
			jo["eventTimestamp"] = ts;
			jo.Add("_id", "Log-" + ts);
			jo.Add("tbl", "log");

			if (isTesting)
				Console.WriteLine(jo.ToString());
			else
				await db.CreateItemAsync(jo.ToString());
		}
	}
}
