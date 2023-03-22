using System;
using System.Collections.Generic;
using System.Drawing;

namespace Packer
{
	internal class Bin2DNodeGuillotine
	{
		public Rectangle area { get; set; }

		public bool isLeaf
		{
			get
			{
				return this.m_LeftChild == null && this.m_RightChild == null;
			}
		}

		public Bin2DNodeGuillotine(Bin2DGuillotine _bin)
		{
			this.id = uint.MaxValue;
			this.m_Bin = _bin;
		}

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

		private uint id { get; set; }
		private Bin2DNodeGuillotine.BorderType m_Border { get; set; }
		private const uint invalidID = 4294967295U;
		private Bin2DNodeGuillotine m_LeftChild;
		private Bin2DNodeGuillotine m_RightChild;
		private Bin2DGuillotine m_Bin;
		
		private enum BorderType
		{
			None,
			Left,
			Top,
			Right = 4,
			Bottom = 8
		}
	}
}
