using System;
using System.Collections.Generic;

namespace ScriptTool
{
	internal class ScriptWriter
	{
		public static ScriptWriter instance
		{
			get
			{
				if (ScriptWriter.m_Instance == null)
				{
					ScriptWriter.m_Instance = new ScriptWriter();
					ScriptWriter.m_Instance.m_ScriptSections.Add(new BuildMainRooms());
					ScriptWriter.m_Instance.m_ScriptSections.Add(new BuildSecondaryRooms());
					ScriptWriter.m_Instance.m_ScriptSections.Add(new BuildTriggeredDoors());
					ScriptWriter.m_Instance.m_ScriptSections.Add(new BuildTimedDoors());
					ScriptWriter.m_Instance.m_ScriptSections.Add(new AddTeleports());
					ScriptWriter.m_Instance.m_ScriptSections.Add(new Finalize());
					ScriptWriter.m_Instance.m_ScriptSections.Add(new BuildMobRoster());
					ScriptWriter.m_Instance.m_ScriptSections.Add(new SetLevelInfo());
					ScriptWriter.m_Instance.m_ScriptSections.Add(new SetLevelProps());
				}

				return ScriptWriter.m_Instance;
			}
		}

		public string WriteWholeScript()
		{
			string text = "";
			
			foreach (ScriptSection scriptSection in this.m_ScriptSections)
			{
				text = text + scriptSection.ToString() + "\r\n\r\n";
			}

			return text;
		}

		private ScriptWriter()
		{
		}

		private static ScriptWriter m_Instance;
		private List<ScriptSection> m_ScriptSections = new List<ScriptSection>();
	}
}
