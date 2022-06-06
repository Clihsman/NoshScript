using System;
using NoshScript.Types;

namespace NoshScript
{
	public class Long : NoshObject,INumObject
	{
		private long value;

		public Long (long value)
		{
			this.value = value;
		}

		public Long ()
		{
			
		}

		public NoshObject Sum (NoshObject value)
		{
			long result = (long)(this.value + Convert.ToInt64 (value.getValue ()));
			return new Long(result);
		}

		public NoshObject Res (NoshObject value)
		{
			long result = (long)(this.value - Convert.ToInt64 (value.getValue ()));
			return new Long (result);
		}

		public NoshObject Mult (NoshObject value)
		{
			long result = (long)(this.value * Convert.ToInt64 (value.getValue ()));
			return new Long(result);
		}

		public NoshObject Div (NoshObject value)
		{
			long result = (long)(this.value / Convert.ToInt64 (value.getValue ()));
			return new Long(result);
		}

		public NoshObject Mod (NoshObject value)
		{
			long result = (long)(this.value % Convert.ToInt64 (value.getValue ()));
			return new Long(result);
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
			this.value = Convert.ToInt64 (value);
		}
	}
}

