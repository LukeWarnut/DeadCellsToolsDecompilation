using System;

namespace PAKTool
{
	internal class FileData : EntryData
	{
		public int position { get; private set; }
		public int size { get; private set; }
		public int checksum { get; private set; }

		public override bool isDirectory
		{
			get
			{
				return false;
			}
		}

		public FileData(DirectoryData _parent, string _name, int _position, int _size, int _crc) : base(_parent, _name)
		{
			this.position = _position;
			this.size = _size;
			this.checksum = _crc;
		}
	}
}
