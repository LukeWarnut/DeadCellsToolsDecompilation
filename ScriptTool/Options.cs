using System;
using Newtonsoft.Json;

namespace ScriptTool
{
	internal class Options
	{
		public string refCDBPath { get; set; } = "";
		
		public static Options FromJson(string _jsonString)
		{
			return JsonConvert.DeserializeObject<Options>(_jsonString);
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
