using System;
using System.Collections.Generic;
using System.Drawing;

namespace Packer
{
	internal class Bin2DPacker
	{
		public bool isEmpty
		{
			get
			{
				return this.m_Bins.Count == 0 && this.m_CurrentBin == null;
			}
		}
		
		public List<Bin2D> bins
		{
			get
			{
				return this.m_Bins;
			}
		}

		public Size maximumSize { get; set; }
		public uint maximumBinCount { get; set; }
		public Size margin { get; set; }
		public MarginType marginType { get; set; }
		public Bin2DPacker.Algorithm algorithm { get; set; }

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

		public void Clear()
		{
			this.m_Bins.Clear();
			this.m_CurrentBin = null;
		}

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

		private Size m_StartingSize;
		private List<Bin2D> m_Bins = new List<Bin2D>();
		private Bin2D m_CurrentBin;
		private bool m_bCanIncreaseSize;

		public enum Algorithm
		{
			Guillotine,
			MaxRects
		}
	}
}
