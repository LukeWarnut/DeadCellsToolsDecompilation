using System;

namespace CDBTool
{
	internal class Separator
	{
		public int id { get; set; }

		public string name { get; private set; }

		public int lineIndex { get; private set; }

		public Separator(int _id, string _name, int _lineIndex)
		{
			this.id = _id;
			this.name = _name;
			this.lineIndex = _lineIndex;
		}

		public void pushLine()
		{
			int lineIndex = this.lineIndex;
			this.lineIndex = lineIndex + 1;
		}
	}
}
