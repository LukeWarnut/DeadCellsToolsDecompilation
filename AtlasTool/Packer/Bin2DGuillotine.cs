using System;
using System.Collections.Generic;
using System.Drawing;

namespace Packer
{
	// Token: 0x02000002 RID: 2
	internal class Bin2DGuillotine : Bin2D
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public Bin2DGuillotine(Size _startSize, Size _margin, MarginType _marginType)
			: base(_startSize, _margin, _marginType)
		{
			this.Reset();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000205C File Offset: 0x0000025C
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

		// Token: 0x06000003 RID: 3 RVA: 0x000020B2 File Offset: 0x000002B2
		protected override void RetrieveSizes(ref List<Size> _sizeList)
		{
			this.m_Root.RetrieveSizes(ref _sizeList);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020C0 File Offset: 0x000002C0
		protected override void RetrieveIDs(ref List<uint> _idList)
		{
			this.m_Root.RetrieveIDs(ref _idList);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020D0 File Offset: 0x000002D0
		protected override void Reset()
		{
			this.m_Root = new Bin2DNodeGuillotine(this);
			this.m_Root.area = new Rectangle(0, 0, base.size.Width, base.size.Height);
		}

		// Token: 0x04000001 RID: 1
		private Bin2DNodeGuillotine m_Root;
	}
}
