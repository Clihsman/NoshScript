using System;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Collections;

namespace ncollections
{
	public class ncollections
	{
		public static void main()
		{
			PackageManager manager = new PackageManager ();
			Package package = manager.Create (PackageInfo.name);
			NList nList = new NList (manager,package);
			PackageList.AddPackage (package);
		}
	}
}

