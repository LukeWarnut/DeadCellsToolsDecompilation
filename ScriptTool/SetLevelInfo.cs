using System;

namespace ScriptTool
{
	internal class SetLevelInfo : ScriptSection
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
			return "function setLevelInfo(_levelInfo){\r\n    //Use a preset from existing level by calling the \"setLevelInfoFrom\" and changing the LevelName to a valid levelName.\r\n    setLevelInfoFrom(\"PrisonStart\", _levelInfo); //Valid levelName are : PrisonStart, PrisonCourtyard, PrisonDepths, SewerShort, PrisonRoof,\r\n                                //Ossuary, SewerDepths, Bridge, BeholderPit, PrisonCorrupt, StiltVillage, AncienTemple, Crypt, ClockTower, TopClockTower,\r\n                                //Castle, Throne\r\n\r\n    //Change here the level parameters you want:\r\n    //_levelInfo.name = \"MyLevel\";\r\n    //_levelInfo.biome = \"PrisonStart\"; //Valid values are : PrisonStart, PrisonDepths, PrisonRoof, Sewer, SewerOld, Bridge, Ossueary, StiltVillage,\r\n                            //PrisonCourtyard, Cemetery, Crypt, ClockTower, TopClockTower, AncientTemple, Castle, CastleVegan, CastleAlchemy, \r\n                            //CastleTorture, BeholderPit, Throne\r\n    //_levelInfo.baseLootLevel = 1;\r\n    //_levelInfo.cellBonus = 0.1;\r\n    //_levelInfo.tripleUps = 1;\r\n    //_levelInfo.doubleUps = 1;\r\n    //_levelInfo.mobDensity = 0.9; //Be careful that 0 will spwan no mobs even if you set a roster. Same : if you set it other than 0 but do not define\r\n                                   //a roster, no mob will be swpanned.\r\n    //_levelInfo.eliteWanderChance = 0.15;\r\n    //_levelInfo.eliteRoomChance = 0.05;\r\n    //_levelInfo.goldBonus = 0.2;\r\n    //setMobTier(10); //default is 1\r\n}";
		}
		
		private const string defaultLevelInfo = "function setLevelInfo(_levelInfo){\r\n    //Use a preset from existing level by calling the \"setLevelInfoFrom\" and changing the LevelName to a valid levelName.\r\n    setLevelInfoFrom(\"PrisonStart\", _levelInfo); //Valid levelName are : PrisonStart, PrisonCourtyard, PrisonDepths, SewerShort, PrisonRoof,\r\n                                //Ossuary, SewerDepths, Bridge, BeholderPit, PrisonCorrupt, StiltVillage, AncienTemple, Crypt, ClockTower, TopClockTower,\r\n                                //Castle, Throne\r\n\r\n    //Change here the level parameters you want:\r\n    //_levelInfo.name = \"MyLevel\";\r\n    //_levelInfo.biome = \"PrisonStart\"; //Valid values are : PrisonStart, PrisonDepths, PrisonRoof, Sewer, SewerOld, Bridge, Ossueary, StiltVillage,\r\n                            //PrisonCourtyard, Cemetery, Crypt, ClockTower, TopClockTower, AncientTemple, Castle, CastleVegan, CastleAlchemy, \r\n                            //CastleTorture, BeholderPit, Throne\r\n    //_levelInfo.baseLootLevel = 1;\r\n    //_levelInfo.cellBonus = 0.1;\r\n    //_levelInfo.tripleUps = 1;\r\n    //_levelInfo.doubleUps = 1;\r\n    //_levelInfo.mobDensity = 0.9; //Be careful that 0 will spwan no mobs even if you set a roster. Same : if you set it other than 0 but do not define\r\n                                   //a roster, no mob will be swpanned.\r\n    //_levelInfo.eliteWanderChance = 0.15;\r\n    //_levelInfo.eliteRoomChance = 0.05;\r\n    //_levelInfo.goldBonus = 0.2;\r\n    //setMobTier(10); //default is 1\r\n}";
	}
}
