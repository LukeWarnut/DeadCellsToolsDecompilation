using System.IO;

namespace PAKTool
{
	internal abstract class EntryData
	{
		public abstract bool isDirectory { get; }
		public string name { get; private set; }
		public DirectoryData parent { get; private set; }

		public string fullName
		{
			get
			{
				DirectoryData parent = this.parent;
				if (parent != null)
				{
					return Path.Combine(parent.fullName, this.name);
				}
				return this.name;
			}
		}

		public EntryData(DirectoryData _parent, string _name)
		{
			this.name = _name;
			this.parent = _parent;
		}
	}
}
