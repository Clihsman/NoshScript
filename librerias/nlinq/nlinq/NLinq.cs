using System;
using NoshScript;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Collections;
using NoshScript.Nosh.Native.Types;
using System.Linq;

namespace nlinq
{
	public class NLinq
	{
		private Var var;

		public NLinq(PackageManager manager,Package package)
		{
			NoshType varType = new NoshType ("Linq",NoshTypeCode.String, package);

			var = new Var ("Linq", varType, this);

			var.addMethod ("orderBy", new Funtion ("orderBy", new Func<object[],Funtion,object[]>(delegate(object[] array,Funtion fun) {
				return array.OrderBy(delegate(object value) 
				{
						if(fun != null && fun.getArgCount() == 1)
						{
							object result;
							if(fun.Invoke(new object[]{value},out result))
								return result;
						}
						return value;
					}).ToArray();
			})));

			var.addMethod ("orderBy", new Funtion ("orderBy", new Func<object[],object[]>(delegate(object[] array) {
				return array.OrderBy(delegate(object value) 
					{
						return value;
					}).ToArray();
			})));
		
			var.addMethod ("orderByDesc", new Funtion ("orderByDesc", new Func<object[],Funtion,object[]>(delegate(object[] array,Funtion fun) {
				return array.OrderByDescending(delegate(object value) 
					{
						if(fun != null && fun.getArgCount() == 1)
						{
							object result;
							if(fun.Invoke(new object[]{value},out result))
								return result;
						}
						return value;
					}).ToArray();
			})));

			var.addMethod ("orderByDesc", new Funtion ("orderByDesc", new Func<object[],object[]>(delegate(object[] array) {
				return array.OrderByDescending(delegate(object value) 
					{
						return value;
					}).ToArray();
			})));

			manager.AddVar (var);
		}
	}
}

