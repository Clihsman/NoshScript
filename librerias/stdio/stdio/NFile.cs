using System;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript;
using NoshScript.Nosh.Native.Types;
using NoshScript.Nosh.Collections;
using System.IO;

namespace stdio
{
	public class NFile
	{
		private Var var;

		public NFile (PackageManager manager, Package package)
		{
			NoshType type = new NoshType ("File", NoshTypeCode.Target, package);

			var = new Var ("File", type, this);

			var.addMethod ("open", new Funtion ("open", new Func<string,string,Var> (delegate(string filename,string mode) {
				if (!File.Exists (filename))
					throw new FileNotFoundException (filename);	

				FileStream fileStream = null;
				if(mode == "r")
					fileStream = new FileStream(filename,FileMode.Open);
				else if(mode == "w")
					fileStream = new FileStream(filename,FileMode.Append);
				
				NBuffer buffer = new NBuffer ();
				buffer.init (fileStream);

				return buffer.getVar();
			})));


			manager.AddVar (var);
		}

		public Var getVar ()
		{
			return var;
		}
	}
}

