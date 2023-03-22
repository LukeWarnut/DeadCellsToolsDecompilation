using System;

namespace ScriptTool
{
	internal class BuildMainRooms : ScriptSection
	{
		public override bool isOptional
		{
			get
			{
				return false;
			}
		}

		public override string ToString()
		{
			return "function buildMainRooms(){\r\n    //Add here the main structure of your level (ex : entrance, exits, shops, treasures etc)\r\n    //Creating an entrance and calling it \"start\" is mandatory.\r\n    //Example:\r\n    Struct.createRoomWithType(\"Entrance\").setName(\"start\")\r\n        .chain(Struct.createRoomWithType(\"Combat\"))\r\n        .chain(Struct.createExit(\"Main\").setTitleAndColor(\"My Awesome Level\", 8888888).setName(\"Main\");\r\n}\r\n";
		}
		
		private const string defaultBuildMainRooms = "function buildMainRooms(){\r\n    //Add here the main structure of your level (ex : entrance, exits, shops, treasures etc)\r\n    //Creating an entrance and calling it \"start\" is mandatory.\r\n    //Example:\r\n    Struct.createRoomWithType(\"Entrance\").setName(\"start\")\r\n        .chain(Struct.createRoomWithType(\"Combat\"))\r\n        .chain(Struct.createExit(\"Main\").setTitleAndColor(\"My Awesome Level\", 8888888).setName(\"Main\");\r\n}\r\n";
	}
}
