using System;
using System.Drawing;

namespace AtlasTool
{
	// Token: 0x02000009 RID: 9
	internal class Tile
	{
		// Token: 0x0400001F RID: 31
		public string name;

		// Token: 0x04000020 RID: 32
		public int index;

		// Token: 0x04000021 RID: 33
		public int x;

		// Token: 0x04000022 RID: 34
		public int y;

		// Token: 0x04000023 RID: 35
		public int width;

		// Token: 0x04000024 RID: 36
		public int height;

		// Token: 0x04000025 RID: 37
		public int offsetX;

		// Token: 0x04000026 RID: 38
		public int offsetY;

		// Token: 0x04000027 RID: 39
		public int originalWidth;

		// Token: 0x04000028 RID: 40
		public int originalHeight;

		// Token: 0x04000029 RID: 41
		public string originalFilePath;

		// Token: 0x0400002A RID: 42
		public Bitmap bitmap;

		// Token: 0x0400002B RID: 43
		public bool hasNormal;

		// Token: 0x0400002C RID: 44
		public Tile duplicateOf;

		// Token: 0x0400002D RID: 45
		public int atlasIndex;
	}
}
