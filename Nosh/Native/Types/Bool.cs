using System;
using NoshScript.Types;

namespace NoshScript
{
	public class Bool : NoshObject
	{
		private bool value;

		public Bool (string varName) : base(varName){}

		public Bool (string varName,bool value) : base(varName)
		{
			this.value = value;
		}

		public override object getValue ()
		{
			return value;	
		}

		public override void setValue (object value)
		{
			value = Convert.ChangeType (value,typeof(bool));
		}
			
		public override string toString ()
		{
			return value.ToString ();
		}
	}
}

