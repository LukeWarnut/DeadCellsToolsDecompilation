using System;

namespace ScriptTool
{
	public class Mob
	{
		public string mobName { get; set; }
		public int quantityFactor { get; set; }
		public bool singleRoom { get; set; }
		public int singleRoomRatio { get; set; }
		public int minCombatRoomsBefore { get; set; }
		public int maxCombatRoomsBefore { get; set; }
		public int minDifficulty { get; set; }
		public int maxDifficulty { get; set; }
	}
}
