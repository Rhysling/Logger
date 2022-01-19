using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger.Tests.Services
{
	public class AppSettings
	{
		public string? IsProductionString { get; set; }

		public bool IsProduction => IsProductionString == "true";

		public string? TwitterApiAuth { get; set; }

		public AS_Cloudant? Cloudant { get; set; }

		public AS_Mailgun? Mailgun { get; set; }


	}


	public class AS_Cloudant
	{
		public string? Account { get; set; }
		public string? FeederName { get; set; }
		public string? FeederAuth { get; set; }
	}

	public class AS_Mailgun
	{
		public string? FromDomain { get; set; }
		public string? AuthValue { get; set; }
	}

}
