using System;
using System.Collections.Generic;
using System.Linq;

namespace Logger.Services
{
	public class MessageBase
	{
		protected string toEmails; // ',' delimited
		protected string subject;
		protected bool isHtml;
		protected string body;
		protected Dictionary<string, string> mergeFields;

		public MessageBase(bool isHtml = true)
		{
			this.isHtml = isHtml;
			TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
			DateTime pstNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pstZone);

			mergeFields = new Dictionary<string, string> {
				{"DateNow", pstNow.ToShortDateString()},
				{"TimeNow", pstNow.ToLongTimeString() + (pstZone.IsDaylightSavingTime(pstNow) ? " PDT" : " PST")}
			};

			// to be overridden
			toEmails = "";
			subject = "";
			body = "";

		}

		public string ToEmails
		{
			get
			{
				if (String.IsNullOrWhiteSpace(toEmails))
					throw new ArgumentNullException("ToEmails cannot be empty.");

				return toEmails;
			}
		}

		public string Subject
		{
			get
			{
				if (String.IsNullOrWhiteSpace(subject))
					throw new ArgumentNullException("Subject cannot be empty.");

				return subject;
			}
		}

		public bool IsHtml
		{
			get
			{
				return isHtml;
			}
		}

		public string RebderBody()
		{
			if (String.IsNullOrWhiteSpace(body))
				throw new ArgumentNullException("Body cannot be empty.");

			foreach (var r in mergeFields)
				body = body.Replace("[" + r.Key + "]", r.Value);

			return body;
		}

	}
}