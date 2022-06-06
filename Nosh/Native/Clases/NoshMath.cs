using System;
using NoshScript.Types;

namespace NoshScript
{
	public static class NoshMath
	{
		public static NoshObject Sum(NoshObject value0,NoshObject value1)
		{
			if (!(value0 is INumObject && value1 is INumObject))
				throw new NotSupportedException ();

			return ((INumObject)value0).Sum(value1);
		}

		public static NoshObject Res(NoshObject value0,NoshObject value1)
		{
			if (!(value0 is INumObject && value1 is INumObject))
				throw new NotSupportedException ();
			
			return ((INumObject)value0).Res(value1);
		}

		public static NoshObject Mult(NoshObject value0,NoshObject value1)
		{
			if (!(value0 is INumObject && value1 is INumObject))
				throw new NotSupportedException ();
			
			return ((INumObject)value0).Mult(value1);
		}

		public static NoshObject Div(NoshObject value0,NoshObject value1)
		{
			if (!(value0 is INumObject && value1 is INumObject))
				throw new NotSupportedException ();
			
			return ((INumObject)value0).Div(value1);
		}

		public static NoshObject Mod(NoshObject value0,NoshObject value1)
		{
			if (!(value0 is INumObject && value1 is INumObject))
				throw new NotSupportedException ();
			
			return ((INumObject)value0).Mod(value1);
		}
	}
}

