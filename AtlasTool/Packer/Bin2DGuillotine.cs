﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace Packer
{
	internal class Bin2DGuillotine : Bin2D
	{
		public Bin2DGuillotine(Size _startSize, Size _margin, MarginType _marginType)
			: base(_startSize, _margin, _marginType)
		{
			this.Reset();
		}
		protected override bool InsertElement(uint _id, Size _elementSize, out Rectangle _area)
		{
			Bin2DNodeGuillotine bin2DNodeGuillotine = this.m_Root.Insert(_id, _elementSize, base.margin, base.marginType);
			if (bin2DNodeGuillotine == null)
			{
				_area = default(Rectangle);
				return false;
			}
			_area = bin2DNodeGuillotine.GetAreaWithoutMargin(bin2DNodeGuillotine.area.Size, base.marginType);
			return true;
		}
		protected override void RetrieveSizes(ref List<Size> _sizeList)
		{
			this.m_Root.RetrieveSizes(ref _sizeList);
		}
		protected override void RetrieveIDs(ref List<uint> _idList)
		{
			this.m_Root.RetrieveIDs(ref _idList);
		}
		protected override void Reset()
		{
			this.m_Root = new Bin2DNodeGuillotine(this);
			this.m_Root.area = new Rectangle(0, 0, base.size.Width, base.size.Height);
		}
		private Bin2DNodeGuillotine m_Root;
	}
}
