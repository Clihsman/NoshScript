using System;
using NoshScript.Types;

namespace NoshScript
{
	public interface INumObject
	{
		NoshObject Sum(NoshObject value);
		NoshObject Res(NoshObject value);
		NoshObject Mult(NoshObject value);
		NoshObject Div(NoshObject value);
		NoshObject Mod(NoshObject value);
	
		void parse(string value);
	}
}

