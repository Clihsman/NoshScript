using System;
using NoshScript;
using NoshScript.Nosh.Native.Types;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Collections;
using MySql.Data.MySqlClient;


namespace mysql
{
	public class NMYSQL_COMMAND
	{
		private Var var;
		private MySqlCommand command;
		private MySqlDataReader reader;

		public NMYSQL_COMMAND(PackageManager manager,Package package)
		{
			NoshType varType = new NoshType ("MySqlCommand",NoshTypeCode.String, package);
			var = new Var ("MySqlCommand", varType, this);
			var.addMethod("execute", new Funtion("execute", 0, (Delegate)null));
            var.addMethod ("read", new Funtion ("read", 0, (Delegate)null));
			var.addMethod ("getValue", new Funtion ("getValue", 1, (Delegate)null));

			manager.AddFuntion (new Funtion("MySqlCommand",new Func<Var,string,Var>(delegate(Var connection,string commandString) {
				NMYSQL_COMMAND mysql = new NMYSQL_COMMAND();
				mysql.command = new MySqlCommand(commandString,(MySqlConnection)connection.getNative());
				return mysql.getVar();
			})));

			manager.AddVar (var);
		}

		public NMYSQL_COMMAND()
		{
			var = new Var ("MySqlCommand", PackageInfo.getType("MySqlCommand"), this);

			var.addMethod("execute", new Funtion("execute", new Action(delegate {
                reader = command.ExecuteReader();
            })));

            var.addMethod ("read", new Funtion ("read", new Func<bool> (delegate {
				return reader.Read();
			})));

			var.addMethod ("getValue", new Funtion ("getValue", new Func<int,object> (delegate(int i) {
				return reader.GetValue(i);
			})));

		}

		public Var getVar(){
			return var;
		}
	}
}

