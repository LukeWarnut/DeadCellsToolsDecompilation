using System;
using System.Collections.Generic;
using System.Drawing;

namespace Packer
{
	// Token: 0x02000005 RID: 5
	internal abstract class Bin2D
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002C8E File Offset: 0x00000E8E
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002C96 File Offset: 0x00000E96
		public Size size { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002C9F File Offset: 0x00000E9F
		public Dictionary<uint, Rectangle> elements
		{
			get
			{
				return this.m_Elements;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002CA7 File Offset: 0x00000EA7
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002CAF File Offset: 0x00000EAF
		public Size margin { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002CB8 File Offset: 0x00000EB8
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002CC0 File Offset: 0x00000EC0
		public MarginType marginType { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002CCC File Offset: 0x00000ECC
		public Size nextSize
		{
			get
			{
				if (this.currentGrowthState == Bin2D.GrowthState.GrowWidth)
				{
					return new Size(this.size.Width * 2, this.size.Height);
				}
				if (this.currentGrowthState == Bin2D.GrowthState.SwapWidthHeight)
				{
					return new Size(this.size.Height, this.size.Width);
				}
				throw new NotImplementedException();
			}
		}

		public Bin2D(Size _startSize, Size _margin, MarginType _marginType)
		{
			this.size = _startSize;
			this.margin = _margin;
			this.marginType = _marginType;
			this.currentGrowthState = Bin2D.GrowthState.GrowWidth;
			this.startSize = _startSize;
		}

        public void IncreaseSize()
        {
            this.size = this.nextSize;
            this.currentGrowthState = (Bin2D.GrowthState)(((int)this.currentGrowthState + 1) % (int)Bin2D.GrowthState.Count);
        }

        // Token: 0x06000023 RID: 35 RVA: 0x00002D8C File Offset: 0x00000F8C
        public bool InsertElement(uint _id, Size _elementSize)
		{
			Rectangle rectangle;
			if (this.InsertElement(_id, _elementSize, out rectangle))
			{
				this.m_Elements.Add(_id, rectangle);
				return true;
			}
			return false;
		}

		// Token: 0x06000024 RID: 36
		protected abstract bool InsertElement(uint _id, Size _elementSize, out Rectangle _area);

		// Token: 0x06000025 RID: 37
		protected abstract void RetrieveSizes(ref List<Size> _areaList);

		// Token: 0x06000026 RID: 38
		protected abstract void RetrieveIDs(ref List<uint> _idList);

		// Token: 0x06000027 RID: 39
		protected abstract void Reset();

		// Token: 0x06000028 RID: 40 RVA: 0x00002DB8 File Offset: 0x00000FB8
		public void RearrangeBin()
		{
			List<Size> list = new List<Size>();
			List<uint> list2 = new List<uint>();
			this.RetrieveSizes(ref list);
			this.RetrieveIDs(ref list2);
			bool flag;
			do
			{
				flag = true;
				this.m_Elements.Clear();
				this.Reset();
				int count = list.Count;
				int num = 0;
				while (num < count && flag)
				{
					if (!this.InsertElement(list2[num], list[num]))
					{
						flag = false;
						this.IncreaseSize();
					}
					num++;
				}
			}
			while (!flag);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002E34 File Offset: 0x00001034
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002E3C File Offset: 0x0000103C
		private protected Size startSize { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002E45 File Offset: 0x00001045
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00002E4D File Offset: 0x0000104D
		private Bin2D.GrowthState currentGrowthState { get; set; }

		// Token: 0x04000010 RID: 16
		private Dictionary<uint, Rectangle> m_Elements = new Dictionary<uint, Rectangle>();

		// Token: 0x0200000E RID: 14
		private enum GrowthState
		{
			// Token: 0x04000039 RID: 57
			GrowWidth,
			// Token: 0x0400003A RID: 58
			SwapWidthHeight,
			// Token: 0x0400003B RID: 59
			Count
		}
	}
}
