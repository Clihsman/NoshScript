using System;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Collections;
using NoshScript.Nosh.Native.Types;
using NoshScript;
using System.IO;
using System.Collections.Generic;

namespace stdio
{
	public class NBuffer
	{
		private Var var;
		private Stream stream;

		public NBuffer (PackageManager manager,Package package)
		{
			NoshType type = new NoshType ("Buffer",NoshTypeCode.Target, package);

			var = new Var ("Buffer", type, this);

			var.addMethod ("readLine", new Funtion ("readLine", (Delegate)null));
			var.addMethod ("readLines", new Funtion ("readLines", (Delegate)null));

			manager.AddVar (var);
		}

		public NBuffer ()
		{
			NoshType type = NoshType.getType("stdio.Buffer");

			var = new Var ("Buffer", type, this);

			var.addMethod ("close", new Funtion ("close", new Action (delegate() 
			{
					close();
			})));

			var.addMethod ("readLine", new Funtion ("readLine", new Func<string> (delegate() 
			{
				StreamReader reader = new StreamReader(stream);
				return reader.ReadLine();
			})));

			var.addMethod ("readToEnd", new Funtion ("readToEnd", new Func<string> (delegate() 
				{
					StreamReader reader = new StreamReader(stream);
					return reader.ReadToEnd();
				})));

			var.addMethod ("readLines", new Funtion ("readLines", new Func<string[]> (delegate() 
			{
					StreamReader reader = new StreamReader(stream);
					List<string> lines = new List<string>();
					string line;
					while((line = reader.ReadLine()) != null)
					{
						lines.Add(line);
					}
					return lines.ToArray();
			})));
			
		}

		public void close(){
			if(stream != null)
				stream.Close ();
		}

		public void init(Stream stream)
		{
			this.stream = stream;
		}

		public Var getVar ()
		{
			return var;
		}
	}
}

