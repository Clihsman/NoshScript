using System;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Collections;
using NoshScript;

using System.Collections.Generic;
using NoshScript.Nosh.Native.Types;

namespace ncollections
{
	public class NList
	{
		private Var var;
		private List<object> value;

		public NList(PackageManager manager,Package package)
		{
			NoshType type = new NoshType ("list",NoshTypeCode.Target,package);
			var = new Var ("list", type, this);
			var.addMethod ("set", new Funtion ("set", 2, (Delegate)null));
			var.addMethod ("add", new Funtion ("add", 1, (Delegate)null));
			var.addMethod ("remove", new Funtion ("remove", 1, (Delegate)null));
			var.addMethod ("get", new Funtion ("get", 1, (Delegate)null));
			var.addMethod ("contains", new Funtion ("contains", 1, (Delegate)null));
			var.addMethod ("indexOf", new Funtion ("indexOf", 1, (Delegate)null));
			var.addMethod ("count", new Funtion ("count", (Delegate)null));
			var.addMethod ("toArray", new Funtion ("toArray", (Delegate)null));

			manager.AddFuntion (new Funtion("list",new Func<Var>(delegate {
				NList nlist = new NList();
				nlist.init();
				return nlist.getVar();
			})));

			manager.AddFuntion (new Funtion("list",new Func<int,Var>(delegate(int capacity) {
				NList nlist = new NList();
				nlist.init(capacity);
				return nlist.getVar();
			})));

			manager.AddVar (var);
		}

		public NList()
		{
			var = new Var ("list", PackageInfo.getType("list"), this);

			var.addMethod ("set", new Funtion ("set", new Action<object,int> (delegate(object value,int index) 
			{
					this.value[index] = value;
			})));

			var.addMethod ("add", new Funtion ("add", new Action<object> (delegate(object value) {
				   this.value.Add(value);
			})));

			var.addMethod ("remove", new Funtion ("remove", new Action<object> (delegate(object value) {
				  this.value.Remove(value);
			})));

			var.addMethod ("get", new Funtion ("get", new Func<int,object> (delegate(int index) {
				return this.value[index];
			})));

			var.addMethod ("contains", new Funtion ("contains", new Func<object,bool> (delegate(object value) {
				return this.value.Contains(value);
			})));

			var.addMethod ("indexOf", new Funtion ("indexOf", new Func<object,int> (delegate(object value) {
				return this.value.IndexOf(value);
			})));

			var.addMethod ("count", new Funtion ("count", new Func<int> (delegate() {
				return value.Count;
			})));

			var.addMethod ("toArray", new Funtion ("toArray", new Func<object[]> (delegate() {
				return value.ToArray();
			})));


		}

		public void init(){
			value = new List<object> ();
		}

		public void init(int capacity){
			value = new List<object> (capacity);
		}

		public Var getVar(){
			return var;
		}
	}
}

