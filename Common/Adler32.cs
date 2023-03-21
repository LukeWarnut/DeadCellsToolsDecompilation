using System;
using System.IO;

namespace ModTools
{
	public class Adler32
	{
		public int value
		{
			get
			{
				return (this.m_A2 << 16) | this.m_A1;
			}
		}

		public Adler32()
		{
			this.m_A1 = 1;
			this.m_A2 = 0;
		}

		public void Update(byte[] _bytes, int _position, int _length)
		{
			int num = _position + _length;
			for (int i = _position; i < num; i++)
			{
				this.m_A1 = (this.m_A1 + (int)_bytes[i]) % 65521;
				this.m_A2 = (this.m_A2 + this.m_A1) % 65521;
			}
		}

		public int Make(Stream _stream)
		{
			BinaryReader binaryReader = new BinaryReader(_stream);
			return this.Make(binaryReader.ReadBytes((int)_stream.Length));
		}

		public int Make(byte[] _bytes)
		{
			this.m_A1 = 1;
			this.m_A2 = 0;
			this.Update(_bytes, 0, _bytes.Length);
			return this.value;
		}

		private int m_A1;

		private int m_A2;
	}
}
