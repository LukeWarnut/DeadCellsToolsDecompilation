using System;

namespace ModTools
{
	public class Log
	{
		public static Log.Level level { get; set; } = Log.Level.Error;

		public static void Error(string _message, string _callstack = "")
		{
			if (Log.level >= Log.Level.Error)
			{
				ConsoleColor foregroundColor = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine(_message);

				if (_callstack != "")
				{
					Console.Write(_callstack);
				}
				
				Console.ForegroundColor = foregroundColor;
			}
		}

		public static void Message(string _message)
		{
			if (Log.level >= Log.Level.Message)
			{
				Console.WriteLine(_message);
			}
		}

		public enum Level
		{
				NoMessage,
				Error,
				Warning,
				Message,
				VerboseMessage,
				Full
		}
	}
}
