using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ScriptTool
{
	internal class BuildMobRoster : ScriptSection
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
			if (LevelMobForm.mobRoster.Count > 0)
			{
				string text = "";
				text = "function buildMobRoster(_mobList){\r\n    ";
				HashSet<string> hashSet = new HashSet<string>();

				foreach (Mob mob in LevelMobForm.mobRoster)
				{
					if (mob.mobName != "null" && hashSet.Add(mob.mobName))
					{
						List<string> list = JsonConvert.SerializeObject(mob).Split(new char[] { ',' }).ToList<string>();
						string text2 = "mob" + mob.mobName;
						int i = 0;
						while (i < list.Count)
						{
							list[i] = list[i].Replace("\"", "");
							list[i] = list[i].Replace(":", " = ");
							string text3 = list[i].Trim();

							if (text3.IndexOf('{') == 0)
							{
								list[i] = list[i].Substring(list[i].IndexOf('{') + 1);

								list[i] = string.Concat(new string[]
								{
									"    ",
									text2,
									".",
									list[i].Replace(" = ", " = \""),
									"\";"
								});

								list.Insert(i, "{");
								list.Insert(i, "var " + text2 + " = new Mob();");
								i += 3;
							}

							else if (text3.IndexOf('}') == text3.Length - 1)
							{
								list[i] = string.Concat(new string[]
								{
									"    ",
									text2,
									".",
									list[i].Substring(0, list[i].IndexOf('}')),
									";"
								});
								
								list.Insert(i + 1, "}");
								i = list.Count;
							}

							else
							{
								list[i] = string.Concat(new string[]
								{
									"    ",
									text2,
									".",
									list[i],
									";"
								});

								i++;
							}
						}

						list.Add("_mobList.push(" + text2 + ");\r\n");

						foreach (string text4 in list)
						{
							text = text + "\r\n    " + text4;
						}
					}
				}

				text += "\n}";
				return text;
			}

			return "function buildMobRoster(_mobList){\r\n    //You can copy the roster of an existing level by calling :\r\n    addMobRosterFrom(\"PrisonStart\", _mobList); // Valid values for level name are : PrisonStart, PrisonCourtyard, PrisonDepths, SewerShort, PrisonRoof,\r\n                                                   //Ossuary, SewerDepths, Bridge, BeholderPit, PrisonCorrupt, StiltVillage, AncienTemple, Crypt, \r\n                                                   //ClockTower, TopClockTower, Castle, Throne\r\n    //Add here mobs to the roster passed as argument\r\n    //ex to add a mob : copy/paste this block for each different type of mob you want (mobName)\r\n    //var mobZombie = new Mob();\r\n    //{\r\n    //    mobZombie.mobName = \"Zombie\"; //Valid values are: Zombie, FastZombie, FlyZombie, S_Fly, WormZombie, S_Worm, S_WallEggWorm, EliteSideKick,\r\n                                          //Scorpio, Hammer, Grenader, ClusterGrenader, Archer, Ninja, Runner, LeapingDuelyst, Shield, SipkedStatyr,\r\n                                          //Worm, BatDasher, BatKamikaze, Fly, Shielder, Comboter, Hooker, AxeThrower, Spawner, Spawnling, Spinner,\r\n                                          //PirateChief, Spiker, Shoker, Mage360, OrbLauncher, Golem (careful with room sizes), Fogger, CastleKnight,\r\n                                          //Lancer\r\n    //    mobZombie.quantityFactor = 1.0;\r\n    //    mobZombie.singleRoom = false;\r\n    //    mobZombie.singleRoomRatio = -1; //-1 is default\r\n    //    mobZombie.minCombatRoomsBefore = 0;\r\n    //    mobZombie.maxCombatRoomsBefore = 0;\r\n    //    mobZombie.minDifficulty = 0;\r\n    //    mobZombie.maxDifficulty = 5; //Be careful that max difficulty < current difficulty will not spawn the mob\r\n    //}\r\n    //_mobList.push(mobZombie);\r\n    //Uncomment next line to deactivate the automatic mob scaling (activated by default is nothing is called)\r\n    //setAutomaticMobScaling(false);\r\n}\r\n";
		}

		private const string defaultMobRoster = "function buildMobRoster(_mobList){\r\n    //You can copy the roster of an existing level by calling :\r\n    addMobRosterFrom(\"PrisonStart\", _mobList); // Valid values for level name are : PrisonStart, PrisonCourtyard, PrisonDepths, SewerShort, PrisonRoof,\r\n                                                   //Ossuary, SewerDepths, Bridge, BeholderPit, PrisonCorrupt, StiltVillage, AncienTemple, Crypt, \r\n                                                   //ClockTower, TopClockTower, Castle, Throne\r\n    //Add here mobs to the roster passed as argument\r\n    //ex to add a mob : copy/paste this block for each different type of mob you want (mobName)\r\n    //var mobZombie = new Mob();\r\n    //{\r\n    //    mobZombie.mobName = \"Zombie\"; //Valid values are: Zombie, FastZombie, FlyZombie, S_Fly, WormZombie, S_Worm, S_WallEggWorm, EliteSideKick,\r\n                                          //Scorpio, Hammer, Grenader, ClusterGrenader, Archer, Ninja, Runner, LeapingDuelyst, Shield, SipkedStatyr,\r\n                                          //Worm, BatDasher, BatKamikaze, Fly, Shielder, Comboter, Hooker, AxeThrower, Spawner, Spawnling, Spinner,\r\n                                          //PirateChief, Spiker, Shoker, Mage360, OrbLauncher, Golem (careful with room sizes), Fogger, CastleKnight,\r\n                                          //Lancer\r\n    //    mobZombie.quantityFactor = 1.0;\r\n    //    mobZombie.singleRoom = false;\r\n    //    mobZombie.singleRoomRatio = -1; //-1 is default\r\n    //    mobZombie.minCombatRoomsBefore = 0;\r\n    //    mobZombie.maxCombatRoomsBefore = 0;\r\n    //    mobZombie.minDifficulty = 0;\r\n    //    mobZombie.maxDifficulty = 5; //Be careful that max difficulty < current difficulty will not spawn the mob\r\n    //}\r\n    //_mobList.push(mobZombie);\r\n    //Uncomment next line to deactivate the automatic mob scaling (activated by default is nothing is called)\r\n    //setAutomaticMobScaling(false);\r\n}\r\n";
	}
}
