using System;
using NoshScript;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Collections;
using NoshScript.Nosh.Native.Types;

namespace nmath
{
	public sealed class NMath
	{
		public NMath(PackageManager manager,Package package)
		{
			NoshType type = new NoshType ("Math",NoshTypeCode.Target,package);
			Var var = new Var ("Math", type, this);

			var.addMethod ("abs", new Funtion ("abs", new Func<object,double> (delegate(object value) 
			{
					double db = Convert.ToDouble(value);
					return Math.Abs(db);
			})));

			var.addMethod ("acos", new Funtion ("acos", new Func<object,double> (delegate(object value) 
				{
					double db = Convert.ToDouble(value);
					return Math.Acos(db);
				})));

			var.addMethod ("asin", new Funtion ("asin", new Func<object,double> (delegate(object value) 
				{
					double db = Convert.ToDouble(value);
					return Math.Asin(db);
				})));

			var.addMethod ("atan", new Funtion ("atan", new Func<object,double> (delegate(object value) 
				{
					double db = Convert.ToDouble(value);
					return Math.Atan(db);
				})));

			var.addMethod ("atan2", new Funtion ("atan2", new Func<object,object,double> (delegate(object valuex,object valuey) 
				{
					double x = Convert.ToDouble(valuex);
					double y = Convert.ToDouble(valuey);
					return Math.Atan2(x,y);
				})));
			
			var.addMethod ("bigMul", new Funtion ("bigMul", new Func<object,object,long> (delegate(object valuea,object valueb) 
				{
					int a = Convert.ToInt32(valuea);
					int b = Convert.ToInt32(valueb);
					return Math.BigMul(a,b);
				})));

			var.addMethod ("ceiling", new Funtion ("ceiling", new Func<object,double> (delegate(object value) 
				{
					double d = Convert.ToDouble(value);
					return Math.Ceiling(d);
				})));

			var.addMethod ("cos", new Funtion ("cos", new Func<object,double> (delegate(object value) 
				{
					double d = Convert.ToDouble(value);
					return Math.Cos(d);
				})));

			var.addMethod ("cosh", new Funtion ("cosh", new Func<object,double> (delegate(object value) 
				{
					double d = Convert.ToDouble(value);
					return Math.Cosh(d);
				})));

			var.addMethod ("exp", new Funtion ("exp", new Func<object,double> (delegate(object value) 
				{
					double d = Convert.ToDouble(value);
					return Math.Exp(d);
				})));

			var.addMethod ("floor", new Funtion ("floor", new Func<object,double> (delegate(object value) 
				{
					double d = Convert.ToDouble(value);
					return Math.Floor(d);
				})));
			
			var.addMethod ("ieeeRemainder", new Funtion ("ieeeRemainder", new Func<object,object,double> (delegate(object valuex,object valuey) 
				{
					double x = Convert.ToDouble(valuex);
					double y = Convert.ToDouble(valuey);
					return Math.IEEERemainder(x,y);
				})));

			var.addMethod ("log", new Funtion ("log", new Func<object,double> (delegate(object value) 
				{
					double d = Convert.ToDouble(value);
					return Math.Log(d);
				})));

			var.addMethod ("log10", new Funtion ("log10", new Func<object,double> (delegate(object value) 
				{
					double d = Convert.ToDouble(value);
					return Math.Log10(d);
				})));

			var.addMethod ("max", new Funtion ("max", new Func<object,object,double> (delegate(object valuex,object valuey) 
				{
					double x = Convert.ToDouble(valuex);
					double y = Convert.ToDouble(valuey);
					return Math.Max(x,y);
				})));

			var.addMethod ("max", new Funtion ("max", new Func<object,object,double> (delegate(object valuex,object valuey) 
				{
					double val1 = Convert.ToDouble(valuex);
					double val2 = Convert.ToDouble(valuey);
					return Math.Max(val1,val2);
				})));

			var.addMethod ("min", new Funtion ("min", new Func<object,object,double> (delegate(object valuex,object valuey) 
				{
					double val1 = Convert.ToDouble(valuex);
					double val2 = Convert.ToDouble(valuey);
					return Math.Min(val1,val2);
				})));

			var.addMethod ("pow", new Funtion ("pow", new Func<object,object,double> (delegate(object valuex,object valuey) 
				{
					double x = Convert.ToDouble(valuex);
					double y = Convert.ToDouble(valuey);
					return Math.Pow(x,y);
				})));

			var.addMethod ("round", new Funtion ("round", new Func<object,double> (delegate(object value) {
				double d = Convert.ToDouble (value);
				return Math.Round (d);
			})));

			var.addMethod ("sign", new Funtion ("sign", new Func<object,double> (delegate(object value) {
				double d = Convert.ToDouble (value);
				return Math.Sign (d);
			})));

			var.addMethod ("sin", new Funtion ("sin", new Func<object,double> (delegate(object value) {
				double d = Convert.ToDouble (value);
				return Math.Sin (d);
			})));

			var.addMethod ("sinh", new Funtion ("sinh", new Func<object,double> (delegate(object value) {
				double d = Convert.ToDouble (value);
				return Math.Sinh (d);
			})));

			var.addMethod ("sqrt", new Funtion ("sqrt", new Func<object,double> (delegate(object value) {
				double d = Convert.ToDouble (value);
				return Math.Sqrt (d);
			})));

			var.addMethod ("tan", new Funtion ("tan", new Func<object,double> (delegate(object value) {
				double d = Convert.ToDouble (value);
				return Math.Tan (d);
			})));

			var.addMethod ("tanh", new Funtion ("tanh", new Func<object,double> (delegate(object value) {
				double d = Convert.ToDouble (value);
				return Math.Tanh (d);
			})));

			var.addMethod ("truncate", new Funtion ("truncate", new Func<object,double> (delegate(object value) {
				double d = Convert.ToDouble (value);
				return Math.Truncate (d);
			})));

			#region EXTRAS

			var.addMethod ("sum", new Funtion ("sum", new Func<object,object,int> (delegate(object valuex,object valuey) 
				{
					int x = Convert.ToInt32(valuex);
					int y = Convert.ToInt32(valuey);
					return x + y;
				})));

			var.addMethod ("res", new Funtion ("res", new Func<object,object,int> (delegate(object valuex,object valuey) 
				{
					int x = Convert.ToInt32(valuex);
					int y = Convert.ToInt32(valuey);
					return x - y;
				})));

			var.addMethod ("mult", new Funtion ("mult", new Func<object,object,double> (delegate(object valuex,object valuey) 
				{
					double x = Convert.ToDouble(valuex);
					double y = Convert.ToDouble(valuey);
					return x * y;
				})));

			var.addMethod ("div", new Funtion ("div", new Func<object,object,double> (delegate(object valuex,object valuey) 
				{
					double x = Convert.ToDouble(valuex);
					double y = Convert.ToDouble(valuey);
					return x / y;
				})));

			var.addMethod ("mod", new Funtion ("mod", new Func<object,object,double> (delegate(object valuex,object valuey) 
				{
					double x = Convert.ToDouble(valuex);
					double y = Convert.ToDouble(valuey);
					return x % y;
				})));

			#endregion

			var.addVar ("E",NoshType.getType("nosh.double"),Math.E);
			var.addVar ("PI",NoshType.getType("nosh.double"),Math.PI);

			manager.AddVar (var);
		}
	}
}

