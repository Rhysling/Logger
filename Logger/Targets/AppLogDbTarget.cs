using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPoco;
using LoggerDb.Models;

namespace LoggerDb.Targets
{
	public class AppLogDbTarget : ITarget
	{
		private NPoco.Database db;
		private string connString;
		private LogLevelEnum minLevel;

		public AppLogDbTarget(string _connectionString, LogLevelEnum _minLevel)
		{
			connString = _connectionString;
			minLevel = _minLevel;
		}


		public int Save(AppLog item)
		{
			if (minLevel == LogLevelEnum.None || item.LogLevelId < minLevel)
				return 0;

			db = new NPoco.Database(connString, DatabaseType.SqlServer2012);

			db.Save<AppLog>(item);
			return 1;
		}

		public async Task<int> SaveAsync(AppLog item)
		{
			if (minLevel == LogLevelEnum.None || item.LogLevelId < minLevel)
				return 0;

			db = new NPoco.Database(connString, DatabaseType.SqlServer2012);

			// No Async for you!
			db.Save<AppLog>(item);

			//Task t = Task.Run(() =>
			//{
			//	db.Save<AppLog>(item);
			//});

			return 1;
		}

	}
}
