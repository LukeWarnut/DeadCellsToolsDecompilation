using System;
using System.Collections.Generic;
using System.Drawing;

namespace Packer
{
	// Token: 0x02000004 RID: 4
	internal class Bin2DMaxRects : Bin2D
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002048 File Offset: 0x00000248
		public Bin2DMaxRects(Size _startSize, Size _margin, MarginType _marginType)
			: base(_startSize, _margin, _marginType)
		{
			this.Reset();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000270C File Offset: 0x0000090C
		protected override bool InsertElement(uint _id, Size _elementSize, out Rectangle _area)
		{
			_area = default(Rectangle);
			Size size = _elementSize + new Size(1, 1);
			int bestIndexForElement = this.GetBestIndexForElement(size);
			if (bestIndexForElement == -1)
			{
				return false;
			}
			_area.Size = size;
			_area.Location = this.m_FreeAreas[bestIndexForElement].area.Location;
			Bin2DMaxRects.Element element = new Bin2DMaxRects.Element();
			element.area = _area;
			element.id = _id;
			this.m_UsedAreas.Add(element);
			Rectangle area = this.m_FreeAreas[bestIndexForElement].area;
			Rectangle rectangle = new Rectangle(_area.X + _area.Width, _area.Y, area.Width - _area.Width, area.Height);
			if (rectangle.GetArea() > 0)
			{
				this.m_FreeAreas.Add(new Bin2DMaxRects.Element(rectangle));
			}
			rectangle = new Rectangle(_area.X, _area.Y + _area.Height, area.Width, area.Height - _area.Height);
			if (rectangle.GetArea() > 0)
			{
				this.m_FreeAreas.Add(new Bin2DMaxRects.Element(rectangle));
			}
			this.m_FreeAreas.RemoveAt(bestIndexForElement);
			List<Rectangle> list = new List<Rectangle>();
			int i = 0;
			while (i < this.m_FreeAreas.Count)
			{
				Rectangle rectangle2 = Rectangle.Intersect(this.m_FreeAreas[i].area, _area);
				if (rectangle2.GetArea() > 0)
				{
					Rectangle area2 = this.m_FreeAreas[i].area;
					Rectangle rectangle3 = new Rectangle(area2.X, area2.Y, rectangle2.X - area2.X, area2.Height);
					if (rectangle3.GetArea() > 0)
					{
						list.Add(rectangle3);
					}
					rectangle3 = new Rectangle(area2.X, rectangle2.Y + rectangle2.Height, area2.Width, area2.Height - (rectangle2.Y - area2.Y + rectangle2.Height));
					if (rectangle3.GetArea() > 0)
					{
						list.Add(rectangle3);
					}
					rectangle3 = new Rectangle(area2.X, area2.Y, area2.Width, rectangle2.Y - area2.Y);
					if (rectangle3.GetArea() > 0)
					{
						list.Add(rectangle3);
					}
					rectangle3 = new Rectangle(rectangle2.X + rectangle2.Width, area2.Y, area2.Width - (rectangle2.X - area2.X + rectangle2.Width), area2.Height);
					if (rectangle3.GetArea() > 0)
					{
						list.Add(rectangle3);
					}
					this.m_FreeAreas.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				this.m_FreeAreas.Add(new Bin2DMaxRects.Element(list[j]));
			}
			int k = 0;
			while (k < this.m_FreeAreas.Count - 1)
			{
				bool flag = false;
				int num = 1;
				while (num < this.m_FreeAreas.Count && k < this.m_FreeAreas.Count - 1)
				{
					if (k == num)
					{
						num++;
					}
					else if (this.m_FreeAreas[k].area.Contains(this.m_FreeAreas[num].area))
					{
						this.m_FreeAreas.RemoveAt(num);
					}
					else
					{
						if (this.m_FreeAreas[num].area.Contains(this.m_FreeAreas[k].area))
						{
							this.m_FreeAreas.RemoveAt(k);
							flag = true;
							break;
						}
						num++;
					}
				}
				if (!flag)
				{
					k++;
				}
			}
			return true;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002AE0 File Offset: 0x00000CE0
		protected override void RetrieveSizes(ref List<Size> _sizeList)
		{
			foreach (Bin2DMaxRects.Element element in this.m_UsedAreas)
			{
				_sizeList.Add(element.area.Size - base.margin);
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002B4C File Offset: 0x00000D4C
		protected override void RetrieveIDs(ref List<uint> _idList)
		{
			foreach (Bin2DMaxRects.Element element in this.m_UsedAreas)
			{
				_idList.Add(element.id);
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002BA8 File Offset: 0x00000DA8
		protected override void Reset()
		{
			this.m_FreeAreas = new List<Bin2DMaxRects.Element>();
			this.m_UsedAreas = new List<Bin2DMaxRects.Element>();
			this.m_FreeAreas.Add(new Bin2DMaxRects.Element(new Rectangle(0, 0, base.size.Width, base.size.Height)));
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002C00 File Offset: 0x00000E00
		private int GetBestIndexForElement(Size _elementSize)
		{
			int num = int.MaxValue;
			int num2 = -1;
			for (int i = 0; i < this.m_FreeAreas.Count; i++)
			{
				Rectangle area = this.m_FreeAreas[i].area;
				if (area.Size.CanFit(_elementSize))
				{
					int num3 = Math.Min(area.Size.Width - _elementSize.Width, area.Size.Height - _elementSize.Height);
					if (num3 < num)
					{
						num2 = i;
						num = num3;
					}
				}
			}
			return num2;
		}

		// Token: 0x04000009 RID: 9
		private List<Bin2DMaxRects.Element> m_FreeAreas;

		// Token: 0x0400000A RID: 10
		private List<Bin2DMaxRects.Element> m_UsedAreas;

		// Token: 0x0200000D RID: 13
		private class Element
		{
			// Token: 0x0600004E RID: 78 RVA: 0x00004A64 File Offset: 0x00002C64
			public Element()
			{
				this.area = default(Rectangle);
				this.id = uint.MaxValue;
			}

			// Token: 0x0600004F RID: 79 RVA: 0x00004A7F File Offset: 0x00002C7F
			public Element(Rectangle _rectangle)
			{
				this.area = _rectangle;
				this.id = uint.MaxValue;
			}

			// Token: 0x04000036 RID: 54
			public Rectangle area;

			// Token: 0x04000037 RID: 55
			public uint id;
		}
	}
}
