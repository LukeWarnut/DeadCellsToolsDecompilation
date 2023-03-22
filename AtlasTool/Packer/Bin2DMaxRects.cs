using System;
using System.Collections.Generic;
using System.Drawing;

namespace Packer
{
	internal class Bin2DMaxRects : Bin2D
	{
		public Bin2DMaxRects(Size _startSize, Size _margin, MarginType _marginType) : base(_startSize, _margin, _marginType)
		{
			this.Reset();
		}
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

		protected override void RetrieveSizes(ref List<Size> _sizeList)
		{
			foreach (Bin2DMaxRects.Element element in this.m_UsedAreas)
			{
				_sizeList.Add(element.area.Size - base.margin);
			}
		}

		protected override void RetrieveIDs(ref List<uint> _idList)
		{
			foreach (Bin2DMaxRects.Element element in this.m_UsedAreas)
			{
				_idList.Add(element.id);
			}
		}

		protected override void Reset()
		{
			this.m_FreeAreas = new List<Bin2DMaxRects.Element>();
			this.m_UsedAreas = new List<Bin2DMaxRects.Element>();
			this.m_FreeAreas.Add(new Bin2DMaxRects.Element(new Rectangle(0, 0, base.size.Width, base.size.Height)));
		}

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
		
		private List<Bin2DMaxRects.Element> m_FreeAreas;
		private List<Bin2DMaxRects.Element> m_UsedAreas;

		private class Element
		{
			public Element()
			{
				this.area = default(Rectangle);
				this.id = uint.MaxValue;
			}

			public Element(Rectangle _rectangle)
			{
				this.area = _rectangle;
				this.id = uint.MaxValue;
			}

			public Rectangle area;
			public uint id;
		}
	}
}
