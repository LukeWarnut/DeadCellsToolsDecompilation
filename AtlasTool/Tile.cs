using System;
using System.Drawing;

namespace AtlasTool
{
	internal class Tile
	{
		public string name;
		public int index;
		public int x;
		public int y;
		public int width;
		public int height;
		public int offsetX;
		public int offsetY;
		public int originalWidth;
		public int originalHeight;
		public string originalFilePath;
		public Bitmap bitmap;
		public bool hasNormal;
		public Tile duplicateOf;
		public int atlasIndex;
	}
}
