using System;
using System.IO;
using Newtonsoft.Json;

namespace ModTools
{
	public class CastleJsonTextWriter : JsonTextWriter
	{
		public CastleJsonTextWriter(TextWriter _writer)
			: base(_writer)
		{
		}

		public override void WriteValue(float _value)
		{
			if (_value == (float)((int)_value))
			{
				this.WriteValue((int)_value);
				return;
			}
			
			base.WriteValue(_value);
		}

		public override void WriteValue(float? _value)
		{
			if (_value != null)
			{
				float? num = _value;
				float num2 = (float)((int)_value.Value);

				if ((num.GetValueOrDefault() == num2) & (num != null))
				{
					this.WriteValue((int)_value.Value);
					return;
				}
			}

			base.WriteValue(_value);
		}
	}
}
