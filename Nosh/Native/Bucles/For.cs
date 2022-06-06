using NoshScript.Nosh.Native.Types;
using System;

namespace NoshScript
{
	public class For
	{
        private Var varInit;
        private Var varCompareA;
        private Var varCompareB;
        private Script script;
        private string condition;
        private string loop;

		public For (Script script,string arg)
		{
            this.script = script;
            string[] args = arg.Split(';');

            loadInit(args[0]);
            loadCond(args[1]);
            loop = args[2];
		}

        private void loadInit(string arg)
        {
            if(arg.StartsWith("var:") && (arg.Contains("=") && !arg.Contains("==")))
            {
                string[] currentLine = arg.Split(' ');
                string varName = currentLine[1].Trim();
                string varTypeString = currentLine[0].Split(':')[1].Trim();

                string[] value = arg.Split('=');
                value = Utils.removeWhiteSpace(value);
                NoshType varType = NoshType.getType(varTypeString, script.getPackages());
                varInit = new Var(varName, varType);
                script.addVar(varInit);

                string valueStr = value[1].Trim();

                if (script.isFuction(valueStr)) {
                    PatternInput pattern = new PatternInput('(', ')');
                    string functionName = valueStr.Split('(')[0];
                    string[] args = pattern.compile(valueStr).Split(',');
                    args = Utils.removeWhiteSpace(args);


                    if (args.Length > 0)
                    {
                        varInit.setValue(script.invokeFunc(functionName, args), varType);
                    }
                    else
                        varInit.setValue(script.invokeFunc(functionName), varType);
                }
                else
                {
                    varInit.setValue(Utils.getValue(varInit, valueStr, script), varType);
                }
            }
        }

        private void loadCond(string arg)
        {
            string[] separ = new string[] { "<=", ">=", "==", "!=","<", ">" };

            string[] currentArg = arg.Split(separ, StringSplitOptions.RemoveEmptyEntries);

            string varNameA = currentArg[0].Trim();
            string varNameB = currentArg[1].Trim();
            condition = arg.Substring(varNameA.Length, (arg.Length -1) - varNameB.Length).Trim();

            if (script.isFuction(varNameA))
            {
                NoshType varType = NoshType.getType("nosh.bool");
                varCompareA = new Var("varCompareA", varType, false);
                object value = script.getVarValue(varCompareA, varNameA);
                varCompareA.setValue(value, varType);
            }
            else
           if (varNameA == varInit.getName())
                varCompareA = varInit;
            else
                varCompareA = script.getVar(varNameA);


            if (script.isFuction(varNameB))
            {
                NoshType varType = NoshType.getType("nosh.bool");
                varCompareB = new Var("varCompareB", varType, false);
                object value = script.getVarValue(varCompareB, varNameB);
                varCompareB.setValue(value,varType);
            }
            else
             if (Utils.isText(varNameB[0]))
                varCompareB = script.getVar(varNameB);
            else
            if (char.IsNumber(varNameB[0]))
            {
                NoshType varType = NoshType.getType("nosh.int");
                varCompareB = new Var("local", varType);
                varCompareB.setValue(Convert.ToInt32(varNameB), varType);
            }

        }

        private void loadLoop()
        {
            if (!script.isFuction(loop))
            {
                script.addVar(varInit);
                if (Utils.condition(varCompareA, varCompareB, condition))
                {
                    if (loop.Contains("++"))
                    {
                        int pointer = loop.IndexOf("++");
                        string varName = loop.Substring(0,pointer).Trim();

                        if (varInit.getName() == varName)
                        {
                            int sum = Convert.ToInt32(varInit.getValue()) + 1;
                            varInit.setValue(sum,NoshType.getType("nosh.int"));

                        }
                    }
                    if (loop.Contains("--"))
                    {
                        int pointer = loop.IndexOf("--");
                        string varName = loop.Substring(0, pointer).Trim();
                        int sum = (int)varInit.getValue() - 1;
                        varInit.setValue(sum, NoshType.getType("nosh.int"));
                    }
                }
            }
        }

        public object execute()
        {
            while (Utils.condition(varCompareA, varCompareB, condition))
            {
                script.execute();
                script.finish();
                loadLoop();
            }
            script.removeVar(varInit);
            dispose();
            return null;
        }

        private void dispose() {
            varInit = null;
            varCompareA = null;
            varCompareB = null;
            script = null;
            condition = null;
            loop = null;
        }
	}
}

