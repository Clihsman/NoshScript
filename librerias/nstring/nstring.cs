using System;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Collections;

namespace nstring
{
	public class nstring
	{
		public static void main(){
			PackageManager manager = new PackageManager ();
			Package package = manager.Create ("nstring");
			NString nString = new NString (manager, package);
			PackageList.AddPackage (package);
		}
	}
}

