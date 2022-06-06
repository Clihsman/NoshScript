using System;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Collections;

namespace nmath
{
	public class nmath
	{
		public static void main(){
			PackageManager manager = new PackageManager ();
			Package package = manager.Create ("nmath");
			NMath nMath = new NMath (manager,package);
			PackageList.AddPackage (package);
		}
	}
}