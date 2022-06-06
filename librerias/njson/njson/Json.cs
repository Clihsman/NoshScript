using System;
using NoshScript.Nosh.Collections;
using NoshScript;
using System.Collections.Generic;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Native.Types;

namespace njson
{
	public class Json
	{
		private Var var;

		public Json(PackageManager manager,Package package)
		{
			NoshType varType = new NoshType ("Json",NoshTypeCode.Target, package);
			var = new Var ("Json", varType, this);

			var.addMethod ("serealizer", new Funtion ("serealizer", new Func<Var,string>(delegate(Var obj) {
				Newtonsoft.Json.Linq.JObject jObject = new Newtonsoft.Json.Linq.JObject();
				if (obj != null)
				{
					if (obj.getValue() != null)
						if (obj.getValue() is Var)
						{
							Var[] variables = obj.getVariables();
							foreach (Var jVar in ((Var)obj.getValue()).getVariables())
							{
								object value = jVar.getValue();

								if(jVar.getName() != "this")
							 	if (value != null)
								{
									if(value is Var)
									{
										Var subVar = (Var)value;
										Newtonsoft.Json.Linq.JObject subObject = new Newtonsoft.Json.Linq.JObject();
										jObject.Add(subVar.getName(), subObject);

										foreach (Var vrs in subVar.getVariables())
										{
											if (value.GetType().IsArray)
											{
												subObject.Add(vrs.getName(), new Newtonsoft.Json.Linq.JArray(vrs.getValue()));
											}
											else
											{
												if(vrs.getName() != "this" && !(vrs.getValue() is Funtion))
												{
													subObject.Add(vrs.getName(), new Newtonsoft.Json.Linq.JValue(vrs.getValue()));
												}
											}
										}
									}
									else
										if (value.GetType().IsArray)
										{
											jObject.Add(jVar.getName(), new Newtonsoft.Json.Linq.JArray(jVar.getValue()));
										}
										else
										{   
											if(jVar.getName() != "this" && !(jVar.getValue() is Funtion))
												jObject.Add(jVar.getName(), new Newtonsoft.Json.Linq.JValue(jVar.getValue()));
										}
								}
							}
						}
						else
						{
							object value = obj.getValue();
							if (value is Array)
								jObject.Add(obj.getName(), new Newtonsoft.Json.Linq.JArray(obj.getValue()));
							else
								if(obj.getName() != "this"){
								  jObject.Add(obj.getName(), new Newtonsoft.Json.Linq.JValue(obj.getValue()));
								}
						}
				}
				return jObject.ToString();
			})));

			manager.AddVar (var);
		}

		public Var getVar(){
			return var;
		}
	}
}

