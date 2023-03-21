using System;
using System.Collections.Generic;

namespace PAKTool
{
	internal class DirectoryData : EntryData
	{
		public override bool isDirectory
		{
			get
			{
				return true;
			}
		}

		public DirectoryData(DirectoryData _parent, string _name)
			: base(_parent, _name)
		{
		}

		public void AddEntry(EntryData _entry)
		{
			if (_entry.isDirectory)
			{
				this.directories.Add((DirectoryData)_entry);
				return;
			}

			this.files.Add((FileData)_entry);
		}

		public List<FileData> files { get; private set; } = new List<FileData>();
		public List<DirectoryData> directories { get; private set; } = new List<DirectoryData>();
	}
}
