using System;

namespace ScriptTool
{
	internal class BuildTriggeredDoors : ScriptSection
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
			return "\r\n//Optional\r\n//function BuildTriggeredDoors(_combatRooms : Array<RoomNode>){\r\n//}\r\n";
		}
		
		private const string defaultBuildTriggeredDoors = "\r\n//Optional\r\n//function BuildTriggeredDoors(_combatRooms : Array<RoomNode>){\r\n//}\r\n";
	}
}
