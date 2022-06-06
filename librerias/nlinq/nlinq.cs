using System;
using System.Linq;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Collections;

namespace nlinq
{
	public class nlinq
	{
		public static void main()
		{
			Package nlinq = null;
			PackageManager manager = new PackageManager();
			nlinq = manager.Create("nlinq");
			NLinq nLinq = new NLinq (manager,nlinq);
			manager.Dispose();
			PackageList.AddPackage(nlinq);
		}
	}
}

