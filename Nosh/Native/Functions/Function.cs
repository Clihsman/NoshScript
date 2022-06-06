using System;
using NoshScript.Types;

namespace NoshScript
{
	public class Function : NoshObject
	{
		private NoshScript script;

		public Function (NoshScript script)
		{
			this.script = script;
		}

		public Function (string varName,NoshScript script) : base(varName)
		{
			this.script = script;
		}

		public void Run(NoshObject[] args)
		{
			script.setVariables (args);
			script.Run ();
		}
	}
}

