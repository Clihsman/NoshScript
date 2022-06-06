using System;
using NoshScript.Types;

namespace NoshScript
{
	public class Float : NoshObject, INumObject
	{
		private float value;

		public Float (float value)
		{
			this.value = value;
		}

		public Float (string varName,float value) : base(varName)
		{
			this.value = value;
		}

		public Float ()
		{
			
		}

		public NoshObject Sum (NoshObject value)
		{
			float result = (float)(this.value + Convert.ToSingle (value.getValue ()));
			return new Float(result);
		}

		public NoshObject Res (NoshObject value)
		{
			float result = (float)(this.value - Convert.ToSingle (value.getValue ()));
			return new Float (result);
		}

		public NoshObject Mult (NoshObject value)
		{
			float result = (float)(this.value * Convert.ToSingle (value.getValue ()));
			return new Float(result);
		}

		public NoshObject Div (NoshObject value)
		{
			float result = (float)(this.value / Convert.ToSingle (value.getValue ()));
			return new Float(result);
		}

		public NoshObject Mod (NoshObject value)
		{
			float result = (float)(this.value % Convert.ToSingle (value.getValue ()));
			return new Float(result);
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
			this.value = Convert.ToSingle (value);
		}
	}
}

