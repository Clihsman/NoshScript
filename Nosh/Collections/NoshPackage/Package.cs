using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoshScript.Nosh.Collections
{
    public class Package
    {
        private string name;

        private Dictionary<string, List<Funtion>> functions;
        private Dictionary<string, List<Funtion>> constructors;
        private List<Var> variables;

        public Package(string name, Dictionary<string, List<Funtion>> functions, Dictionary<string, List<Funtion>> constructors, List<Var> variables) {
            this.functions = functions;
            this.variables = variables;
            this.constructors = constructors;
            this.name = name;
        }

        public string getName() {
            return name;
        }

        public Funtion getFuntion(string name,int argCount)
        {
            if (functions == null)
                return null;

            List<Funtion> funs;
            if (functions.TryGetValue(name, out funs))
                foreach (Funtion func in funs)
                    if (func.getArgCount() == argCount)
                        return func;
            return null;
        }

        public Funtion getConstructor(string name, int argCount)
        {
            if (constructors == null)
                return null;

            List<Funtion> funs;
            if (constructors.TryGetValue(name, out funs))
                foreach (Funtion func in funs)
                    if (func.getArgCount() == argCount)
                        return func;
            return null;
        }

        public Var getVariable(string name)
        {
            if (variables == null)
                return null;

            string[] vars = name.Split('.');

            if (vars.Length > 1)
            {
                Var var = getVariable(vars[0]);

                for (int i = 1; i < vars.Length; i++)
                {
                    string varName = vars[i];
                    if (var != null)
                    {
                        Var currentVar = getVariable(varName);

                        if (currentVar != null)
                            var = currentVar;
                        else
                        {
                            object value = var.getValue();

                            if (value != null && value.GetType() == typeof(Var))
                            {
                                return ((Var)value).getVar(varName);
                            }
                        }
                    }
                    else
                        return null;
                }

                return var;
            }

            foreach (Var var in variables)
                if (var.getName() == name)
                    return var;
            return null;
        }
    }
}
