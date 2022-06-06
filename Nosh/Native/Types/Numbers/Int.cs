using System;
using NoshScript.Types;

namespace NoshScript
{
	public class Int : NoshObject, INumObject
	{
		private int value;

		public Int (int value)
		{
			this.value = value;
		}

		public Int (string varName,int value) : base(varName)
		{
			this.value = value;
		}

		public Int ()
		{
			
		}

		public NoshObject Sum (NoshObject value)
		{
			int result = (int)(this.value + Convert.ToInt32 (value.getValue ()));
			return new Int(result);
		}

		public NoshObject Res (NoshObject value)
		{
			int result = (int)(this.value - Convert.ToInt32 (value.getValue ()));
			return new Int (result);
		}

		public NoshObject Mult (NoshObject value)
		{
			int result = (int)(this.value * Convert.ToInt32 (value.getValue ()));
			return new Int(result);
		}

		public NoshObject Div (NoshObject value)
		{
			int result = (int)(this.value / Convert.ToInt32 (value.getValue ()));
			return new Int(result);
		}

		public NoshObject Mod (NoshObject value)
		{
			int result = (int)(this.value % Convert.ToInt32 (value.getValue ()));
			return new Int(result);
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
			this.value = Convert.ToInt32 (value);
		}
	}
}

