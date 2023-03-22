using System;

namespace ScriptTool
{
	internal class Finalize : ScriptSection
	{
		public override bool isOptional
		{
			get
			{
				return true;
			}
		}
		
		public override string ToString()
		{
			return "\r\n//Optional\r\n//last function called during the level structure building\r\n//function Finalize(){\r\n    //Add here the structure of your room\r\n//}\r\n";
		}

		private const string defaultFinalize = "\r\n//Optional\r\n//last function called during the level structure building\r\n//function Finalize(){\r\n    //Add here the structure of your room\r\n//}\r\n";
	}
}
