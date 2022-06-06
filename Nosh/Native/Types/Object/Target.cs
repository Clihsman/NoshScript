using System;
using NoshScript.Types;
using System.Collections.Generic;

namespace NoshScript
{
	public class Target : NoshObject
	{
		private Dictionary<string ,NoshObject> variables = new Dictionary<string ,NoshObject> ();

		public Target (string varName) : base(varName)
		{
			
		}

		public void addVar(string name)
		{
			variables.Add (name,new NoshObject(name));
		}

		public void addVar(NoshObject value)
		{
			variables.Add (value.getName(),value);
		}

		public void setVar(NoshObject value)
		{
			if (!variables.ContainsKey (value.getName ()))
				throw new KeyNotFoundException ();

			variables [value.getName ()] = value;
		}

		public NoshObject getVar(string name)
		{
			if (!variables.ContainsKey (name))
				throw new KeyNotFoundException ();

			return variables [name];
		}
	}
}

