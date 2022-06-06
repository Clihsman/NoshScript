using NoshScript.Nosh.Native.Bucles;
using NoshScript.Nosh.Native.Types;
using System;

namespace NoshScript
{
	public class While
	{
        private Script script;
        private Var value = null;
        private string arg;

        public While(Script script, string arg)
        {
            this.script = script;
            this.arg = arg;
            load(arg);
        }

        private void load(string arg)
        {
            string valueName = arg.Trim();

            value = new Var(valueName, NoshType.getType("nosh.bool"), null);

            if (script.isFuction(valueName))
            {
                object result = script.getVarValue(value, valueName);
                if(result is bool)
                  value.setValue(result, NoshType.getType("nosh.bool"));
            }
            else
            {
                object result = script.getVar(valueName).getValue();
                if (result is bool)
                    value.setValue(result, NoshType.getType("nosh.bool"));
            }
        }

        public object execute()
        {
            while ((bool)value.getValue())
            {
                script.addVar(value);
                object result = script.execute();
                if (result is BucleOption)
                {
                    if (((BucleOption)result) == BucleOption.BREAK)
                    {
                        script.finish();
                        break;
                    }

                    if (((BucleOption)result) == BucleOption.CONTINUE)
                    {
                        script.finish();
                        continue;
                    }
                }         
                script.finish();
                load(arg);
            }
            script.removeVar(value);
            dispose();
            return null;
        }

        private void dispose()
        {
            script = null;
            value = null;
        }
    }
}

