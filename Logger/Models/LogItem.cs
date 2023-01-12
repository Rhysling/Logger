using UtilitiesMaster.ExtMethods.ExtDateTime;

namespace Logger.Models;

public class LogItem
{
	private readonly LogLevelEnum level;
	private readonly DateTime eventDate;

	public LogItem(LogLevelEnum level)
	{
		this.level = level;
		eventDate = DateTime.Now;
	}

	public long EventTimestamp => eventDate.ToJsTime();

	public string EventDateGmt => eventDate.ToUniversalTime().ToString("r");

	public string LevelName => Enum.GetName(typeof(LogLevelEnum), level) ?? "missing";

	public int LevelId => (int)level;


	public string? AppName { get; set; }

	public string? Message { get; set; }

	public bool IsException { get; set; }

	public object? InfoObj { get; set; }
}
