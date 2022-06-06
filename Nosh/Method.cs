using System;

namespace NoshScript
{
	public class Method
	{
		public Method (string name,object action)
		{
			((Delegate)action).DynamicInvoke ();
		}
	}
}

