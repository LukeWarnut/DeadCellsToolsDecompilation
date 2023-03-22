using System;

namespace ScriptTool
{
	internal class WrongFormattedScriptException : Exception
	{
		public WrongFormattedScriptException(string _message)
			: base(_message)
		{
		}
	}
}
