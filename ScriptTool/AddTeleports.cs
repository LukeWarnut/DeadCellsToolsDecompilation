using System;

namespace ScriptTool
{
	internal class AddTeleports : ScriptSection
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
			return "\r\n//Optional : allows you to define how to add teleports in your level\r\n//keep it commented to have a default behavior : some teleport will be created automatically\r\n//uncomment and keep empty to have no teleport at all\r\n//function AddTeleports(){\r\n    //Add here the code to add teleporters inside the level\r\n//}\r\n";
		}

		private const string defaultAddTeleports = "\r\n//Optional : allows you to define how to add teleports in your level\r\n//keep it commented to have a default behavior : some teleport will be created automatically\r\n//uncomment and keep empty to have no teleport at all\r\n//function AddTeleports(){\r\n    //Add here the code to add teleporters inside the level\r\n//}\r\n";
	}
}
