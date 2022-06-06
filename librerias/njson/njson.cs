using System;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Collections;

namespace njson
{
	public class njson
	{
		public static void main(){
			Package nlinq = null;
			PackageManager manager = new PackageManager();
			nlinq = manager.Create("njson");
			Json nJson = new Json (manager,nlinq);
			manager.Dispose();
			PackageList.AddPackage(nlinq);
		}
	}
}

