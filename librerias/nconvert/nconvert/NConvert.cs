using System;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript;
using NoshScript.Nosh.Collections;
using NoshScript.Nosh.Native.Types;

namespace nconvert
{
	public class NConvert
	{
		public NConvert(PackageManager manager,Package package)
		{
			NoshType type = new NoshType ("Convert",NoshTypeCode.Target,package);
			Var var = new Var ("Convert", type, this);

			var.addMethod ("parseInt", new Funtion ("parseInt", new Func<object,int> (delegate(object value) 
				{
					return Convert.ToInt32(value);
				})));

			var.addMethod ("parseFloat", new Funtion ("parseFloat", new Func<object,float> (delegate(object value) 
				{
					if(value is string)
						value = ((string)value).Replace(".",",");
					
					return Convert.ToSingle(value);
				})));

			var.addMethod ("parseString", new Funtion ("parseString", new Func<object,string> (delegate(object value) 
				{
					return Convert.ToString(value);
				})));

			manager.AddVar (var);
		}
	}
}

