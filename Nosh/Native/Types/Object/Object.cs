using System;

namespace NoshScript.Types
{
	public class NoshObject
	{ 
		private string name;
		private object @value;

		public NoshObject (string name)
		{
			this.name = name;
		}

		public NoshObject(){
			
		}

		public virtual string toString()
		{
			return "natives [NoshObject]";
		}

		public virtual object getValue()
		{
			return @value;
		}

		public virtual void setValue(object value){
			this.@value = value;
		}

		public string getName(){
			return name;
		}
	}
}

