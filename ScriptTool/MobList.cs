using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ScriptTool
{
	internal class MobList
	{
		public static List<string> names { get; private set; } = new List<string>();
		public static List<string> ids { get; private set; } = new List<string>();

		public static void InitMobList(string _CDBJson)
		{
			MobList.names.Clear();
			JArray jarray = (JArray)((JObject)JsonConvert.DeserializeObject(_CDBJson))["sheets"];
			JObject jobject = null;
			int num;

			foreach (JToken jtoken in jarray)
			{
				if (jtoken["name"].ToString() == "mob")
				{
					jobject = (JObject)jtoken;
					break;
				}
			}

			int.TryParse(jobject["separators"][1].ToString(), out num);

			for (int i = 0; i < num; i++)
			{
				JToken jtoken2 = jobject["lines"][i];
				MobList.names.Add(jtoken2["name"].ToString());
				MobList.ids.Add(jtoken2["id"].ToString());
			}
		}
	}
}
