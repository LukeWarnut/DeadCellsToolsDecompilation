using System;

namespace ScriptTool
{
	internal class BuildSecondaryRooms : ScriptSection
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
			return "\r\n//This is optional\r\n//function BuildSecondaryRooms(){\r\n    //Add here some more rooms\r\n//}\r\n";
		}
		
		private const string defaultBuildSecondaryRooms = "\r\n//This is optional\r\n//function BuildSecondaryRooms(){\r\n    //Add here some more rooms\r\n//}\r\n";
	}
}
