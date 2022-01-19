using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger.Models;

namespace Logger.Targets
{
	public interface ITarget
	{
		Task SaveAsync(LogItem item);
	}
}
