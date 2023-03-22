using System;
using System.Collections.Generic;
using System.Drawing;

namespace Packer
{
	// Token: 0x02000006 RID: 6
	internal class Bin2DPacker
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002E56 File Offset: 0x00001056
		public bool isEmpty
		{
			get
			{
				return this.m_Bins.Count == 0 && this.m_CurrentBin == null;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002E70 File Offset: 0x00001070
		public List<Bin2D> bins
		{
			get
			{
				return this.m_Bins;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002E78 File Offset: 0x00001078
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002E80 File Offset: 0x00001080
		public Size maximumSize { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002E89 File Offset: 0x00001089
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002E91 File Offset: 0x00001091
		public uint maximumBinCount { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002E9A File Offset: 0x0000109A
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002EA2 File Offset: 0x000010A2
		public Size margin { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002EAB File Offset: 0x000010AB
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00002EB3 File Offset: 0x000010B3
		public MarginType marginType { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002EBC File Offset: 0x000010BC
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002EC4 File Offset: 0x000010C4
		public Bin2DPacker.Algorithm algorithm { get; set; }

		// Token: 0x06000039 RID: 57 RVA: 0x00002ED0 File Offset: 0x000010D0
		public Bin2DPacker(Size _startingSize, Size _maximumSize, Bin2DPacker.Algorithm _algorithm)
		{
			this.m_StartingSize = _startingSize;
			this.maximumSize = _maximumSize;
			this.m_bCanIncreaseSize = true;
			this.marginType = MarginType.None;
			this.margin = new Size(0, 0);
			this.m_CurrentBin = null;
			this.maximumBinCount = uint.MaxValue;
			this.algorithm = _algorithm;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002F2C File Offset: 0x0000112C
		public void Clear()
		{
			this.m_Bins.Clear();
			this.m_CurrentBin = null;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002F40 File Offset: 0x00001140
		public bool InsertElement(uint _id, Size _size, out bool _newBinCreated)
		{
			_newBinCreated = false;
			if (this.m_CurrentBin == null)
			{
				this.m_CurrentBin = this.CreateBin();
				this.m_Bins.Add(this.m_CurrentBin);
				_newBinCreated = true;
			}
			if (!this.m_CurrentBin.size.CanFit(_size + this.margin) && ((this.m_bCanIncreaseSize && !this.maximumSize.CanFit(_size + this.margin)) || !this.m_bCanIncreaseSize))
			{
				throw new Exception("This element will never fit in an atlas with the given parameters");
			}
			bool flag = false;
			for (int i = 0; i < this.m_Bins.Count; i++)
			{
				if (flag)
				{
					break;
				}
				flag = this.m_Bins[i].InsertElement(_id, _size);
			}
			while (!flag && (ulong)this.maximumBinCount > (ulong)((long)this.m_Bins.Count))
			{
				if (this.m_bCanIncreaseSize && this.maximumSize.CanFit(this.m_CurrentBin.nextSize))
				{
					this.m_CurrentBin.IncreaseSize();
					this.m_CurrentBin.RearrangeBin();
				}
				else if ((ulong)this.maximumBinCount > (ulong)((long)this.m_Bins.Count))
				{
					this.m_CurrentBin = this.CreateBin();
					this.m_Bins.Add(this.m_CurrentBin);
					_newBinCreated = true;
				}
				flag = this.m_CurrentBin.InsertElement(_id, _size);
			}
			return flag;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003098 File Offset: 0x00001298
		private Bin2D CreateBin()
		{
			if (this.algorithm == Bin2DPacker.Algorithm.Guillotine)
			{
				return new Bin2DGuillotine(this.m_StartingSize, this.margin, this.marginType);
			}
			if (this.algorithm == Bin2DPacker.Algorithm.MaxRects)
			{
				return new Bin2DMaxRects(this.m_StartingSize, this.margin, this.marginType);
			}
			throw new NotImplementedException();
		}

		// Token: 0x04000016 RID: 22
		private Size m_StartingSize;

		// Token: 0x04000017 RID: 23
		private List<Bin2D> m_Bins = new List<Bin2D>();

		// Token: 0x04000018 RID: 24
		private Bin2D m_CurrentBin;

		// Token: 0x04000019 RID: 25
		private bool m_bCanIncreaseSize;

		// Token: 0x0200000F RID: 15
		public enum Algorithm
		{
			// Token: 0x0400003D RID: 61
			Guillotine,
			// Token: 0x0400003E RID: 62
			MaxRects
		}
	}
}
