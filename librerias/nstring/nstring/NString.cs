using System;
using NoshScript;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Collections;
using System.Collections.Generic;
using NoshScript.Nosh.Native.Types;

namespace nstring
{
	public class NString
	{
		private Var var;
		private string value;

		public NString(PackageManager manager,Package package)
		{
			NoshType varType = new NoshType ("string",NoshTypeCode.String, package);

			var = new Var ("string", varType, this);
			var.addMethod ("set", new Funtion ("set", 1, (Delegate)null));
			var.addMethod ("sum", new Funtion ("sum", 1, (Delegate)null));
			var.addMethod ("get", new Funtion ("get", 0, (Delegate)null));
			var.addMethod ("contains", new Funtion ("contains", 1, (Delegate)null));
			var.addMethod ("indexOf", new Funtion ("indexOf", 1, (Delegate)null));
			var.addMethod ("startsWith", new Funtion ("startsWith", 1, (Delegate)null));
			var.addMethod ("endsWith", new Funtion ("endsWith", 1, (Delegate)null));
			var.addMethod ("length", new Funtion ("length", (Delegate)null));
			var.addMethod ("split", new Funtion ("split", 1, (Delegate)null));
			var.addMethod ("remove", new Funtion ("remove", 1, (Delegate)null));
			var.addMethod ("remove", new Funtion ("remove", 2, (Delegate)null));
			var.addMethod ("toString", new Funtion ("toString", 0, (Delegate)null));
			var.addMethod ("replace", new Funtion ("replace", 2, (Delegate)null));

			manager.AddFuntion (new Funtion("string",new Func<Var>(delegate {
				NString nString = new NString();
				nString.init("");
				return nString.getVar();
			})));

			manager.AddFuntion (new Funtion("string",new Func<string,Var>(delegate(string value) {
				NString nString = new NString();
				nString.init(value);
				return nString.getVar();
			})));

			manager.AddVar (var);
		}

		public NString()
		{
			var = new Var ("string", PackageInfo.getType("string"), this);

			var.addMethod ("set", new Funtion ("set", new Action<string> (delegate(string value) {
				this.value = value;
			})));

			var.addMethod ("sum", new Funtion ("sum", new Action<string> (delegate(string value) {
				 this.value += value;
			})));


			var.addMethod ("get", new Funtion ("get", new Func<string> (delegate() {
				return value;
			})));

			var.addMethod ("contains", new Funtion ("contains", new Func<string,bool> (delegate(string value) {
				return this.value.Contains(value);
			})));

			var.addMethod ("indexOf", new Funtion ("indexOf", new Func<string,int> (delegate(string value) {
				return this.value.IndexOf(value);
			})));

			var.addMethod ("startsWith", new Funtion ("startsWith", new Func<string,bool> (delegate(string value) {
				return this.value.StartsWith(value);
			})));

			var.addMethod ("endsWith", new Funtion ("endsWith", new Func<string,bool> (delegate(string value) {
				return this.value.EndsWith(value);
			})));

			var.addMethod ("replace", new Funtion ("replace", new Func<string,string,string> (delegate(string oldChar,string newChar) {
				return this.value.Replace(oldChar, newChar);
			})));

			var.addMethod ("split", new Funtion ("split", new Func<string,object[]> (delegate(string separador)
			{
					string[] data = value.Split(new string[]{separador},StringSplitOptions.None);
					List<Var> values = new List<Var>();
					foreach(string str in data)
					{
						NString nString = new NString();
						nString.init(str);
						values.Add(nString.getVar());
					}
					return values.ToArray();
			})));

			var.addMethod ("length", new Funtion ("length", new Func<int> (delegate() {
				return value.Length;
			})));

			var.addMethod ("remove", new Funtion ("remove", new Action<int> (delegate(int startIndex) {
				value = value.Remove(startIndex);
			})));

			var.addMethod ("remove", new Funtion ("remove", new Action<int,int> (delegate(int startIndex,int count) {
				value = value.Remove(startIndex,count);
			})));

			var.addMethod ("toString", new Funtion ("toString", new Func<string> (delegate() {
				return value;
			})));


		}

		public void init(string value){
			this.value = value;
		}

		public Var getVar(){
			return var;
		}
	}
}

