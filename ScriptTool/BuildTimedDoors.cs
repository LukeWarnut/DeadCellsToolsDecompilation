using System;

namespace ScriptTool
{
	internal class BuildTimedDoors : ScriptSection
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
			return "\r\n//Optional\r\n//Note that if you want to build timed doors, be sure your setLevelProps function is defined and sets timed properties properly\r\n//function BuildTimedDoors(){\r\n    //Build timed doors here\r\n//}\r\n";
		}

		private const string defaultBuildTimedDoors = "\r\n//Optional\r\n//Note that if you want to build timed doors, be sure your setLevelProps function is defined and sets timed properties properly\r\n//function BuildTimedDoors(){\r\n    //Build timed doors here\r\n//}\r\n";
	}
}
