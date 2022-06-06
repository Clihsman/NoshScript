using System;
using NoshScript.Types;
using NoshScript.Nosh.Native.Types;

namespace NoshScript
{
	public class Foreach
	{
        private Script script;
        private Var value = null;
        private object[] array = null;

        public Foreach(Script script, string arg)
        {
            this.script = script;
            string[] args = arg.Split(new string[] { "in" },StringSplitOptions.None);
            args = Utils.removeWhiteSpace(args);
            load(args);
        }

        private void load(string[] args)
        {
            string valueName = args[0].Trim();
            string arrayName = args[1].Trim();

            value = new Var(valueName, NoshType.getType("nosh.object"), null);

            if (script.isFuction(arrayName))
            {
                object result = script.getVarValue(this.value, arrayName);
                if (result is object[])
                    array = (object[])result;
            }
            else
            {
                object result = script.getVar(arrayName).getValue();
                if (result is object[])
                    array = (object[])result;
            }
        }

        public object execute()
        {
            foreach (object obj in array)
            {
                if (obj != null)
                {
                    script.addVar(value);
                    value.setValue(obj, NoshType.getType("nosh.object"));
                    script.execute();
                    script.finish();
                }
            }
            script.removeVar(value);
            dispose();
            return null;
        }

        private void dispose()
        {
            script = null;
            value = null;
            array = null;
        }
    }
}

