using System;
using NoshScript.Types;

namespace NoshScript
{
	public class Double : NoshObject, INumObject
	{
		private double value;

		public Double (double value)
		{
			this.value = value;
		}

		public Double()
		{

		}

		public NoshObject Sum (NoshObject value)
		{
			double result = (double)(this.value + Convert.ToDouble (value.getValue ()));
			return new Double(result);
		}

		public NoshObject Res (NoshObject value)
		{
			double result = (double)(this.value - Convert.ToDouble (value.getValue ()));
			return new Double (result);
		}

		public NoshObject Mult (NoshObject value)
		{
			double result = (double)(this.value * Convert.ToDouble (value.getValue ()));
			return new Double(result);
		}

		public NoshObject Div (NoshObject value)
		{
			double result = (double)(this.value / Convert.ToDouble (value.getValue ()));
			return new Double(result);
		}

		public NoshObject Mod (NoshObject value)
		{
			double result = (double)(this.value % Convert.ToDouble (value.getValue ()));
			return new Double(result);
		}

		public void parse(string value)
		{
			setValue (value.Replace('.',','));
		}

		public override object getValue ()
		{
			return value;
		}

		public override void setValue (object value)
		{
			this.value = Convert.ToDouble (value);
		}
	}
}

