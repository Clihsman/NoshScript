using System;
using NoshScript.Nosh.Native.Types;

namespace nconvert
{
	public class PackageInfo
	{
		public const string name = "nconvert";

		public static NoshType getType(string name){
			return NoshType.getType (string.Format("{0}.{1}",PackageInfo.name,name));
		}
	}
}

