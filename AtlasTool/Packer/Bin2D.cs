using System;
using System.Collections.Generic;
using System.Drawing;

namespace Packer
{
	internal abstract class Bin2D
	{
		public Size size { get; private set; }

		public Dictionary<uint, Rectangle> elements
		{
			get
			{
				return this.m_Elements;
			}
		}
		public Size margin { get; private set; }
		public MarginType marginType { get; private set; }

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

		protected abstract bool InsertElement(uint _id, Size _elementSize, out Rectangle _area);
		protected abstract void RetrieveSizes(ref List<Size> _areaList);
		protected abstract void RetrieveIDs(ref List<uint> _idList);
		protected abstract void Reset();

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

		private protected Size startSize { get; set; }
		private Bin2D.GrowthState currentGrowthState { get; set; }
		private Dictionary<uint, Rectangle> m_Elements = new Dictionary<uint, Rectangle>();

		private enum GrowthState
		{
			GrowWidth,
			SwapWidthHeight,
			Count
		}
	}
}
