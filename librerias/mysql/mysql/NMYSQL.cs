using System;
using NoshScript;
using NoshScript.Nosh.Native.Types;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Collections;
using MySql.Data.MySqlClient;

namespace mysql
{
	public class NMYSQL
	{
		private Var var;
		private MySqlConnection connection;

		public NMYSQL(PackageManager manager,Package package)
		{
			NoshType varType = new NoshType ("MySqlConnection",NoshTypeCode.String, package);
			var = new Var ("MySqlConnection", varType, this);
			var.addMethod ("open", new Funtion ("open", 0, (Delegate)null));
			var.addMethod ("close", new Funtion ("close", 0, (Delegate)null));

			manager.AddFuntion (new Funtion("MySqlConnection",new Func<string,Var>(delegate(string connectionString) {
				NMYSQL mysql = new NMYSQL();
				mysql.connection = new MySqlConnection(connectionString);
				mysql.var.setNative(mysql.connection);
				return mysql.getVar();
			})));

			manager.AddVar (var);
		}

		public NMYSQL()
		{
			var = new Var ("MySqlConnection", PackageInfo.getType("MySqlConnection"), this);

			var.addMethod ("open", new Funtion ("open", new Action (delegate {
				connection.Open();
			})));

			var.addMethod ("close", new Funtion ("close", new Action (delegate {
				connection.Close();
			})));
				
		}

		public Var getVar(){
			return var;
		}
	}
}

