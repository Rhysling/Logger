using Logger.Models;

namespace Logger.Targets
{
	public interface ITarget
	{
		Task<int> SaveAsync(LogItem item);
	}
}
