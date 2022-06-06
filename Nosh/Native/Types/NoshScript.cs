using System;
using NoshScript.Types;

namespace NoshScript
{
	public class NoshScript
	{
		private NoshObject[] variables;

		public NoshScript (NoshObject[] variables)
		{
			this.variables = variables;
		}

		public NoshScript(){
			
		}

		public void setVariables(NoshObject[] variables)
		{
			this.variables = variables;
		}

		public void Run()
		{
			Console.WriteLine (variables[0].getValue());
		}
	}
}

