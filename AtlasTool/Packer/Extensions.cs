using System;
using System.Drawing;

namespace Packer
{
	// Token: 0x02000007 RID: 7
	internal static class Extensions
	{
		// Token: 0x0600003D RID: 61 RVA: 0x000030EB File Offset: 0x000012EB
		public static bool CanFit(this Size _this, Size _sizeToFit)
		{
			return _this.Width >= _sizeToFit.Width && _this.Height >= _sizeToFit.Height;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003112 File Offset: 0x00001312
		public static bool DoesIntersect(this Rectangle _this, Rectangle _rectangle)
		{
			return Rectangle.Intersect(_this, _rectangle).GetArea() > 0;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003123 File Offset: 0x00001323
		public static int GetArea(this Rectangle _this)
		{
			return _this.Width * _this.Height;
		}
	}
}
