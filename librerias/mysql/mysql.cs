using System;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Collections;

namespace mysql
{
	public class mysql
	{
		public static void main(){
			PackageManager manager = new PackageManager ();
			Package package = manager.Create ("mysql");
			NMYSQL nMysql = new NMYSQL (manager, package);
			NMYSQL_COMMAND NMysqlCommand = new NMYSQL_COMMAND (manager, package);
			PackageList.AddPackage (package);
		}
	}
}

