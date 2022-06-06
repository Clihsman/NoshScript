using System;
using NoshScript.Types;

namespace NoshScript
{
	public class Byte : NoshObject, INumObject
	{
		private byte value;

		public Byte (byte value)
		{
			this.value = value;
		}

		public Byte()
		{
			
		}

		public NoshObject Sum (NoshObject value)
		{
			byte result = (byte)(this.value + Convert.ToByte (value.getValue ()));
			return new Byte(result);
		}

		public NoshObject Res (NoshObject value)
		{
			byte result = (byte)(this.value - Convert.ToByte (value.getValue ()));
			return new Byte (result);
		}

		public NoshObject Mult (NoshObject value)
		{
			byte result = (byte)(this.value * Convert.ToByte (value.getValue ()));
			return new Byte(result);
		}

		public NoshObject Div (NoshObject value)
		{
			byte result = (byte)(this.value / Convert.ToByte (value.getValue ()));
			return new Byte(result);
		}

		public NoshObject Mod (NoshObject value)
		{
			byte result = (byte)(this.value % Convert.ToByte (value.getValue ()));
			return new Byte(result);
		}

		public void parse(string value)
		{
			setValue (value);
		}

		public override object getValue ()
		{
			return value;
		}

		public override void setValue (object value)
		{
			this.value = Convert.ToByte (value);
		}
	}
}

