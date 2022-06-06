using System;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Collections;

namespace nconvert
{
	public class nconvert
	{
		public static void main(){
			PackageManager manager = new PackageManager ();
			Package package = manager.Create (PackageInfo.name);
			NConvert nConvert = new NConvert (manager,package);
			PackageList.AddPackage (package);
		}
	}
}

