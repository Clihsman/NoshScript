using System;
using NoshScript.Types;

namespace NoshScript
{
	public class NoshArray : NoshObject
	{
		private NoshObject[] values;

		public NoshArray ()
		{
			
		}

		public NoshArray (string varName) : base(varName){}

		public NoshArray (NoshObject[] values)
		{
			this.values = values;
		}

		public NoshArray (string varName,NoshObject[] values) : base(varName)
		{
			this.values = values;
		}

		public NoshObject getValue(int index)
		{
			if (index > values.Length - 1 && index < 0)
				throw new IndexOutOfRangeException ();
			return values [index];
		}

		public void setValue(int index,NoshObject value)
		{
			if (index > values.Length - 1 && index < 0)
				throw new IndexOutOfRangeException ();
			values [index] = value;
		}

		public override void setValue (object value)
		{
			if (value is NoshObject[])
				values = value as NoshObject[];
			else
				throw new NotSupportedException ();
		}

		public override object getValue ()
		{
			return values;
		}

		public int length(){
			return values.Length;
		}
	}
}

