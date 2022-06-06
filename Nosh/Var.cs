using NoshScript.Nosh.Collections;
using NoshScript.Nosh.Native.Types;
using System;
using System.Collections.Generic;

namespace NoshScript
{
	public class Var
	{
		private string name;
		private object value;
		private NoshType type;
        private object native;

        private List<Var> variables;
        private List<Function> funtions;
        private Dictionary<string,List<Funtion>> methods;
        private Dictionary<string, List<Funtion>> methodsConstructor;
        public Action<object> setValueEvent;
        public Func<object> getValueEvent;

        public Var(string name, NoshType type)
        {
            variables = new List<Var>();
            funtions = new List<Function>();
            methods = new Dictionary<string, List<Funtion>>();
            methodsConstructor = new Dictionary<string, List<Funtion>>();
            setValueEvent = null;
            value = null;

			this.name = name;
			this.type = type;
            loadMethods();
        }

		public Var (string name, NoshType type, object value)
		{
            variables = new List<Var>();
            funtions = new List<Function>();
            methods = new Dictionary<string, List<Funtion>>();
            methodsConstructor = new Dictionary<string, List<Funtion>>();
            setValueEvent = null;

            this.name = name;
			this.type = type;
			this.value = value;
            loadMethods();
        }

        public Var(string name)
        {
            variables = new List<Var>();
            funtions = new List<Function>();
            methods = new Dictionary<string, List<Funtion>>();
            methodsConstructor = new Dictionary<string, List<Funtion>>();
            setValueEvent = null;
            this.name = name;
            type = NoshType.getType("nosh.null");
            value = null;
            addVar("object", NoshType.getType("nosh.target"), this);
            loadMethods();
        }

        public void setNative(object native) {
            this.native = native;
        }

        public object getNative() {
            return native;
        }

        private void loadMethods() {
            addMethod("getType",new Funtion("getType",new Func<string>(delegate {
                return type.getName();
            })));
        }

		public string getName ()
		{
			return name;
		}

		public object getValue ()
		{
            if (getValueEvent != null)
                return getValueEvent();
			return value;
		}

        public NoshType getType()
        {
            return type;
        }

		public void setValue(object value,NoshType type)
        {
            if (setValueEvent != null)
                setValueEvent.Invoke(value);
            this.value = value;
		}

        public void setValue(object value)
        {
            if (setValueEvent != null)
                setValueEvent.Invoke(value);
            this.value = value;
        }

        public Var getVar(string name)
        {
            if (name == "object")
                return this;

            if (this.name == "this")
            {
                Var var = ((Var)value).getVar(name);
                  if (var != null)
                        return var;
            }

            foreach (Var var in variables)
                if (var.name == name)
                    return var;

            return null;
        }

        public Var addVar(string name, NoshType type,object value) {
            if (this.name != name)
            {
                Var var = new Var(name, type, value);
                variables.Add(var);
                return var;
            }
            return null;
        }

        public void addVar(Var var)
        {
            variables.Add(var);
        }

        public Funtion getMethod(string name,int argsCount)
        {
            List<Funtion> result = null;
            if (!methods.TryGetValue(name, out result))
            {
                if (value is Var)
                    ((Var)value).methods.TryGetValue(name, out result);
            }

            if (result != null)
            {
                foreach (Funtion fun in result)
                    if (fun.getName() == name && fun.getArgCount() == argsCount)
                        return fun;
            }

            return null;
        }

        public Funtion getConstructor(string name,int argsCount)
        {
            List<Funtion> result = null;
            if (!methodsConstructor.TryGetValue(name, out result))
            {
                if (value is Var)
                    ((Var)value).methodsConstructor.TryGetValue(name, out result);
            }

            if (result != null)
            {
                foreach (Funtion fun in result)
                    if (fun.getName() == name && fun.getArgCount() == argsCount)
                        return fun;
            }

            return null;
        }

        public void addMethod(string name, Funtion func)
        {
            addVar(name, NoshType.getType("nosh.method"), func);

            List<Funtion> result;
            if (methods.TryGetValue(name, out result))
            {
                result.Add(func);
            }
            else
            {
                result = new List<Funtion>();
                result.Add(func);
                methods.Add(name, result);
            }

        }

        public void addMethodConstructor(string name, Funtion func)
        {
            if (this.name == name)
            {
                List<Funtion> result;
                if (methodsConstructor.TryGetValue(name, out result))
                {
                    result.Add(func);
                }
                else
                {
                    result = new List<Funtion>();
                    result.Add(func);
                    methodsConstructor.Add(name, result);
                }
            }
            else
                throw new MissingMethodException(name);
        }

        public Var[] getVariables() {
            return variables.ToArray();
        }

        private bool existVariable(string name)
        {
            foreach (Var var in variables) {
                if (var.name == name)
                    return true;
            }
            return false;
        }

        public static Var instace(Var m_class, string name)
        {
            Var instace = new Var(name, m_class.type);
            instace.methodsConstructor = m_class.methodsConstructor;
            instace.setValue(instace,m_class.type);

            foreach (KeyValuePair<string, List<Funtion>> methods in m_class.methods)
            {
                foreach (Funtion method in methods.Value)
                {
                    if (method.getMethodScript() != null)
                    {
                        Script script = method.getMethodScript().instace();
                        Funtion fun = new Funtion(method.getName(), script);
                        script.addVar(new Var("this", m_class.type, instace));
                        script.setThis(instace);
                        instace.addMethod(method.getName(), fun);
                    }
                }
            }

            foreach (KeyValuePair<string, List<Funtion>> methods in instace.methodsConstructor)
            {
                foreach (Funtion method in methods.Value)
                {
                    if (method.getMethodScript() != null)
                    {
                        if (!method.getMethodScript().setVarValue("this", instace))
                               method.getMethodScript().addVar(new Var("this",m_class.type, instace));
                    }
                }
            }

            instace.addVar("this", m_class.type, instace);



            foreach (Var var in m_class.variables)
            {
                instace.addVar(var.name,var.type,null);
            }

            return instace;
        }

        public override string ToString()
        {
            Funtion fun = null;
            if (value is Var)
               fun = getMethod("toString", 0);
            else
               fun = getMethod("toString", 0);

            if (fun != null)
            {
                object result;
                fun.Invoke(null,out result);
                return result.ToString();
            }

            return string.Format("{0} {1}",name ,type.getName());
        }
    }
}

