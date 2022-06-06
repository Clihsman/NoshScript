using System;
using NoshScript.Nosh.Native.Types;

namespace ncollections
{
	public class PackageInfo
	{
		public const string name = "ncollections";

		public static NoshType getType(string name){
			return NoshType.getType (string.Format("{0}.{1}",PackageInfo.name,name));
		}
	}
}

