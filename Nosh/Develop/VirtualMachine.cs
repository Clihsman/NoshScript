using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NoshScript
{
	public class VirtualMachine
	{
		private StringReader script;

		public VirtualMachine (string script)
		{
			this.script = new StringReader (script);
		}

		public VirtualMachine (StringReader script)
		{
			this.script = script;
		}

		public void execute()
		{
			string line;

			Regex regex = new Regex ("(var:)(.+?).+?(=).+(.+?)");

			foreach(object data in regex.Match("var:input = loader").Groups){
				Console.WriteLine (data);
			}

			while ((line = script.ReadLine ()) != null) 
			{
				
			}
		}
    }
}

