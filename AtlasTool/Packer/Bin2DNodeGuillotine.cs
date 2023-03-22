using System;
using System.Collections.Generic;
using System.Drawing;

namespace Packer
{
	// Token: 0x02000003 RID: 3
	internal class Bin2DNodeGuillotine
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002117 File Offset: 0x00000317
		// (set) Token: 0x06000007 RID: 7 RVA: 0x0000211F File Offset: 0x0000031F
		public Rectangle area { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002128 File Offset: 0x00000328
		public bool isLeaf
		{
			get
			{
				return this.m_LeftChild == null && this.m_RightChild == null;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000213D File Offset: 0x0000033D
		public Bin2DNodeGuillotine(Bin2DGuillotine _bin)
		{
			this.id = uint.MaxValue;
			this.m_Bin = _bin;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002154 File Offset: 0x00000354
		public Bin2DNodeGuillotine Insert(uint _id, Size _size, Size _margins, MarginType _marginType)
		{
			if (!this.isLeaf)
			{
				Bin2DNodeGuillotine bin2DNodeGuillotine = this.m_LeftChild.Insert(_id, _size, _margins, _marginType);
				if (bin2DNodeGuillotine != null)
				{
					return bin2DNodeGuillotine;
				}
				return this.m_RightChild.Insert(_id, _size, _margins, _marginType);
			}
			else
			{
				if (this.id != 4294967295U)
				{
					return null;
				}
				Size size = this.GetSizeWithMargin(_size, _margins, _marginType);
				if (size.Width > this.area.Width || size.Height > this.area.Height)
				{
					return null;
				}
				if (size.Width == this.area.Width && size.Height == this.area.Height)
				{
					this.id = _id;
					return this;
				}
				this.m_LeftChild = new Bin2DNodeGuillotine(this.m_Bin);
				this.m_RightChild = new Bin2DNodeGuillotine(this.m_Bin);
				this.m_LeftChild.m_Border = Bin2DNodeGuillotine.BorderType.None;
				this.m_RightChild.m_Border = Bin2DNodeGuillotine.BorderType.None;
				int num = this.area.Width - size.Width;
				int num2 = this.area.Height - size.Height;
				if (num > num2)
				{
					this.m_LeftChild.m_Border = (this.m_Border & Bin2DNodeGuillotine.BorderType.Left) | (this.m_Border & Bin2DNodeGuillotine.BorderType.Top) | (this.m_Border & Bin2DNodeGuillotine.BorderType.Bottom);
					size = this.GetSizeWithMargin(_size, _margins, _marginType);
					this.m_LeftChild.area = new Rectangle(this.area.Location, new Size(size.Width, this.area.Height));
					this.m_RightChild.m_Border = (this.m_Border & Bin2DNodeGuillotine.BorderType.Right) | (this.m_Border & Bin2DNodeGuillotine.BorderType.Top) | (this.m_Border & Bin2DNodeGuillotine.BorderType.Bottom);
					this.m_RightChild.area = new Rectangle(this.area.Left + size.Width, this.area.Top, this.area.Width - size.Width, this.area.Height);
				}
				else
				{
					this.m_LeftChild.m_Border = (this.m_Border & Bin2DNodeGuillotine.BorderType.Left) | (this.m_Border & Bin2DNodeGuillotine.BorderType.Top) | (this.m_Border & Bin2DNodeGuillotine.BorderType.Right);
					size = this.GetSizeWithMargin(_size, _margins, _marginType);
					this.m_LeftChild.area = new Rectangle(this.area.Location, new Size(this.area.Width, size.Height));
					this.m_RightChild.m_Border = (this.m_Border & Bin2DNodeGuillotine.BorderType.Left) | (this.m_Border & Bin2DNodeGuillotine.BorderType.Bottom) | (this.m_Border & Bin2DNodeGuillotine.BorderType.Right);
					this.m_RightChild.area = new Rectangle(this.area.Left, this.area.Top + size.Height, this.area.Width, this.area.Height - size.Height);
				}
				return this.m_LeftChild.Insert(_id, _size, _margins, _marginType);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000245C File Offset: 0x0000065C
		public void RetrieveSizes(ref List<Size> _sizeList)
		{
			if (this.isLeaf)
			{
				if (this.id != 4294967295U)
				{
					_sizeList.Add(this.GetAreaWithoutMargin(this.m_Bin.margin, this.m_Bin.marginType).Size);
					return;
				}
			}
			else
			{
				this.m_LeftChild.RetrieveSizes(ref _sizeList);
				this.m_RightChild.RetrieveSizes(ref _sizeList);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000024C0 File Offset: 0x000006C0
		private Size GetSizeWithMargin(Size _sizeWithoutMargins, Size _margin, MarginType _marginType)
		{
			Size size = new Size(_sizeWithoutMargins.Width, _sizeWithoutMargins.Height);
			if ((_marginType == MarginType.OnlyBorder || _marginType == MarginType.All) && (this.m_Border & Bin2DNodeGuillotine.BorderType.Left) != Bin2DNodeGuillotine.BorderType.None)
			{
				size.Width += _margin.Width;
			}
			if (_marginType == MarginType.All || (_marginType == MarginType.OnlyBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Right) != Bin2DNodeGuillotine.BorderType.None) || (_marginType == MarginType.NoBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Right) == Bin2DNodeGuillotine.BorderType.None))
			{
				size.Width += _margin.Width;
			}
			if ((_marginType == MarginType.OnlyBorder || _marginType == MarginType.All) && (this.m_Border & Bin2DNodeGuillotine.BorderType.Top) != Bin2DNodeGuillotine.BorderType.None)
			{
				size.Height += _margin.Height;
			}
			if (_marginType == MarginType.All || (_marginType == MarginType.OnlyBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Bottom) != Bin2DNodeGuillotine.BorderType.None) || (_marginType == MarginType.NoBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Bottom) == Bin2DNodeGuillotine.BorderType.None))
			{
				size.Height += _margin.Height;
			}
			return size;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000259C File Offset: 0x0000079C
		public Rectangle GetAreaWithoutMargin(Size _margin, MarginType _marginType)
		{
			Rectangle rectangle = new Rectangle(this.area.Location, this.area.Size);
			if ((_marginType == MarginType.OnlyBorder || _marginType == MarginType.All) && (this.m_Border & Bin2DNodeGuillotine.BorderType.Left) != Bin2DNodeGuillotine.BorderType.None)
			{
				rectangle.X += _margin.Width;
				rectangle.Width -= _margin.Width;
			}
			if (_marginType == MarginType.All || (_marginType == MarginType.OnlyBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Right) != Bin2DNodeGuillotine.BorderType.None) || (_marginType == MarginType.NoBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Right) == Bin2DNodeGuillotine.BorderType.None))
			{
				rectangle.Width -= _margin.Width;
			}
			if ((_marginType == MarginType.OnlyBorder || _marginType == MarginType.All) && (this.m_Border & Bin2DNodeGuillotine.BorderType.Top) != Bin2DNodeGuillotine.BorderType.None)
			{
				rectangle.Y += _margin.Height;
				rectangle.Height -= _margin.Height;
			}
			if (_marginType == MarginType.All || (_marginType == MarginType.OnlyBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Bottom) != Bin2DNodeGuillotine.BorderType.None) || (_marginType == MarginType.NoBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Bottom) == Bin2DNodeGuillotine.BorderType.None))
			{
				rectangle.Height -= _margin.Height;
			}
			return rectangle;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000026AF File Offset: 0x000008AF
		public void RetrieveIDs(ref List<uint> _idList)
		{
			if (this.isLeaf)
			{
				if (this.id != 4294967295U)
				{
					_idList.Add(this.id);
					return;
				}
			}
			else
			{
				this.m_LeftChild.RetrieveIDs(ref _idList);
				this.m_RightChild.RetrieveIDs(ref _idList);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000026E8 File Offset: 0x000008E8
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000026F0 File Offset: 0x000008F0
		private uint id { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000026F9 File Offset: 0x000008F9
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002701 File Offset: 0x00000901
		private Bin2DNodeGuillotine.BorderType m_Border { get; set; }

		// Token: 0x04000003 RID: 3
		private const uint invalidID = 4294967295U;

		// Token: 0x04000006 RID: 6
		private Bin2DNodeGuillotine m_LeftChild;

		// Token: 0x04000007 RID: 7
		private Bin2DNodeGuillotine m_RightChild;

		// Token: 0x04000008 RID: 8
		private Bin2DGuillotine m_Bin;

		// Token: 0x0200000C RID: 12
		private enum BorderType
		{
			// Token: 0x04000031 RID: 49
			None,
			// Token: 0x04000032 RID: 50
			Left,
			// Token: 0x04000033 RID: 51
			Top,
			// Token: 0x04000034 RID: 52
			Right = 4,
			// Token: 0x04000035 RID: 53
			Bottom = 8
		}
	}
}
