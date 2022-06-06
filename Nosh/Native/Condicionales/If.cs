using NoshScript.Nosh.Collections;
using NoshScript.Nosh.Native.Types;
using System;
using System.Collections.Generic;

namespace NoshScript
{
    public class If : IDisposable
	{
		private Script script;
        private Script m_else;
        private string m_args;
        private Var valueA;
        private Var valueB;
        private string condition;

        public If (Script script,string m_args,Script m_else)
		{
			this.script = script;
			this.m_args = m_args;
            this.m_else = m_else;
        }

        private void loadCond()
        {
            string[] separ = new string[] { "<=", ">=", "==", "!=", "<", ">" };

            string[] currentArg = m_args.Split(separ, StringSplitOptions.RemoveEmptyEntries);

            if (currentArg.Length > 1)
            {
                string varNameA = currentArg[0].Trim();
                string varNameB = currentArg[1].Trim();

                condition = m_args.Substring(varNameA.Length, m_args.Length - (varNameA.Length + varNameB.Length)).Trim();

                if (script.isFuction(varNameA))
                {
                    valueA = new Var("varCompareA", NoshType.getType("nosh.bool"), false);
                    object value = script.getVarValue(valueA, varNameA);
                    valueA.setValue(value, NoshType.getType("nosh.bool"));
                }
                else
                if (Utils.isText(varNameA[0]))
                {
                    valueA = script.getVar(varNameA);
                }
                else
                if (char.IsNumber(varNameA[0]))
                {
                    NoshType varType = NoshType.getType("nosh.int");
                    valueA = new Var("local", varType);
                    valueA.setValue(Convert.ToInt32 (varNameA), varType);
                }
                else if (varNameA[0] == '"')
                {
                    NoshType varType = NoshType.getType("nosh.string");
                    valueA = new Var("local", varType);
                    valueA.setValue(Utils.getValue(varNameA), varType);
                }

                if (script.isFuction(varNameB))
                {
                    NoshType varType = NoshType.getType("nosh.bool");
                    valueB = new Var("varCompareA", varType, false);
                    object value = script.getVarValue(valueB, varNameB);
                    valueB.setValue(value, varType);
                }
                else
                if (Utils.isText(varNameB[0]))
                {
                    valueB = script.getVar(varNameB);
                }
                else if (char.IsNumber(varNameB[0]))
                {
                    NoshType varType = NoshType.getType("nosh.int");
                    valueB = new Var("local", varType);
                    valueB.setValue(Convert.ToInt32(varNameB), varType);
                }
                else if (varNameB[0] == '"')
                {
                    NoshType varType = NoshType.getType("nosh.string");
                    valueB = new Var("local", varType);
                    valueB.setValue(Utils.getValue(varNameB), varType);
                }
            }
            else
            {
                string varNameA = currentArg[0].Trim();
                bool compare = true;

                if (varNameA.StartsWith("!"))
                {
                    varNameA = varNameA.Remove(0,1);
                    compare = false;
                }

                if (script.isFuction(varNameA))
                {
                    NoshType varType = NoshType.getType("nosh.bool");
                    valueA = new Var("varCompareA", varType, false);
                    object value = script.getVarValue(valueA, varNameA);
                    valueA.setValue(value, varType);
                }
                else
                if (Utils.isText(varNameA[0]))
                {
                    valueA = script.getVar(varNameA);
                    if(valueA == null)
                        throw new Exception(varNameA);
                }

                valueB = new Var("local", NoshType.getType("nosh.bool"),compare);
                condition = "==";
            }
        }

        private object invokeFunction(string line){
			PatternInput pattern = new PatternInput ('(',')');
			string functionName = line.Split ('(')[0];
			string[] args = pattern.compile (line).Split(',');
			args = Utils.removeWhiteSpace (args);

			if (args.Length > 0) {
				return script.invokeFunc (functionName, args);
			} else
				return script.invokeFunc (functionName);
		}

		public object execute()
		{
            object result = null;
            loadCond();

            if (Utils.condition(valueA, valueB, condition))
            {
                result = script.execute();
            }
            else
            {
                if (m_else != null)
                    result =  m_else.execute();
            }
            script.removeVar(valueA);
            script.removeVar(valueB);

            script.finish();

            if(m_else != null)
               m_else.finish();
            Dispose();

            return result;
		}

        public void Dispose() {
            script = null;
            valueA = null;
            valueB = null;
            condition = null;
        }

		class IfObject
		{
			public object value0,value1;
			public Exprecion exprecion;

			public IfObject(object value0,object value1,Exprecion exprecion)
			{
				this.value0 = value0;
				this.value1 = value1;
				this.exprecion = exprecion;
			}

			public bool run(){
				switch(exprecion)
				{
				case Exprecion.AND:
					return AND (value0,value0);
				case Exprecion.OR:
					return OR ((bool)value0,(bool)value0);
				case Exprecion.NOT:
					return NOT (value0,value1);
				}
				throw new Exception ();
			}

			public static bool AND(object a,object b){
				return object.Equals (a, b);
			}

			public static bool OR(bool a,bool b){
				return (a | b);
			}

			public static bool NOT(object a,object b){
				return !object.Equals (a, b);
			}
		}

		enum Exprecion
		{
			AND,
			OR,
			NOT,
			NULL
		}
	}
	
}

