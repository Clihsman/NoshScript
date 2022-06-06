using System;
using NoshScript.Types;

namespace NoshScript
{
	public class Short : NoshObject, INumObject
	{
		private short value;

		public Short (short value)
		{
			this.value = value;
		}

		public Short (string varName,short value) : base(varName)
		{
			this.value = value;
		}

		public Short ()
		{
			
		}

		public NoshObject Sum (NoshObject value)
		{
			short result = (short)(this.value + Convert.ToInt16 (value.getValue ()));
			return new Short(result);
		}

		public NoshObject Res (NoshObject value)
		{
			short result = (short)(this.value - Convert.ToInt16 (value.getValue ()));
			return new Short(result);
		}

		public NoshObject Mult (NoshObject value)
		{
			short result = (short)(this.value * Convert.ToInt16 (value.getValue ()));
			return new Short(result);
		}

		public NoshObject Div (NoshObject value)
		{
			short result = (short)(this.value / Convert.ToInt16 (value.getValue ()));
			return new Short(result);
		}

		public NoshObject Mod (NoshObject value)
		{
			short result = (short)(this.value % Convert.ToInt16 (value.getValue ()));
			return new Short(result);
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
			this.value = Convert.ToInt16 (value);
		}
	}
}

