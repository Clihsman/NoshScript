using NoshScript.Nosh.Native.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoshScript.Nosh.Collections.NoshPackage
{
    public class PackageManager : IDisposable
    {
        private Dictionary<string, List<Funtion>> functions;
        private Dictionary<string, List<Funtion>> constructors;
        private List<Var> variables;

        public PackageManager() {
            variables = new List<Var>();
            functions = new Dictionary<string, List<Funtion>>();
            constructors = new Dictionary<string, List<Funtion>>();
        }

        public void AddConstructor(Funtion fun)
        {
            string name = fun.getName();

            List<Funtion> funs;
            if (constructors.TryGetValue(name, out funs))
                funs.Add(fun);
            else
            {
                funs = new List<Funtion>();
                funs.Add(fun);
                constructors.Add(name, funs);
            }
        }

        public void AddFuntion(Funtion fun)
        {
            string name = fun.getName();

            List<Funtion> funs;
            if (functions.TryGetValue(name, out funs))
                funs.Add(fun);
            else
            {
                funs = new List<Funtion>();
                funs.Add(fun);
                functions.Add(name, funs);
            }
        }

        public void AddVar(string name,NoshType type,object value)
        {
            variables.Add(new Var(name,type, value));    
        }

        public void AddVar(Var var)
        {
            variables.Add(var);
        }

        public Package Create(string name)
        {
            return new Package(name,functions, constructors, variables);
        }

        public void Dispose()
        {
            variables = null;
            functions = null;
        }
    }
}
