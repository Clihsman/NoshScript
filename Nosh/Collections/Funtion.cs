using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoshScript.Nosh.Collections
{
    public class Funtion
    {
        private string name;
        private Delegate method_delegate = null;
        private Script method_script = null;
        private int argCount;
        
        public Funtion(string name,Delegate method)
        {
            this.name = name;
            this.method_delegate = method;
            if (method != null)
                argCount = method.Method.GetParameters().Length;
            else
                argCount = 0;
        }

        public Funtion(string name, int argCount, Delegate method)
        {
            this.name = name;
            this.method_delegate = method;
            this.argCount = argCount;
        }

        public Funtion(string name, int argCount, Script method)
        {
            this.name = name;
            this.method_script = method;
            this.argCount = argCount;
        }

        public Funtion(string name, Script method)
        {
            this.name = name;
            method_script = method;
            argCount = method.argsCount();
        }

        public string getName() {
            return name;
        }

        public Delegate getMethodDelegate() {
            return method_delegate;
        }

        public Script getMethodScript()
        {
            return method_script;
        }

        public int getArgCount()
        {
            return argCount;
        }

        public bool Invoke(object[] args,out object result)
        {
            if (method_delegate != null)
            {
                return InvokeDelegate(args, out result);
            }
            else if (method_script != null)
                return InvokeScript(args, out result);

            result = null;
            return false;
        }

        public bool Invoke(object[] args)
        {
            object result;
            if (method_delegate != null)
            {
                return InvokeDelegate(args, out result);
            }
            else if (method_script != null)
                return InvokeScript(args, out result);

            result = null;
            return false;
        }

        private bool InvokeScript(object[] args,out object result)
        {
            if (args == null)
            {
                result = method_script.execute();
                method_script.finish();
                return true;
            }
            else if (argCount == args.Length)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    method_script.setIn(i,args[i]);
                }
                result = method_script.execute();

                method_script.finish();
                return true;
            }
            result = null;
            return false;
        }

        private bool InvokeDelegate(object[] args,out object result)
        {
            int argCount = 0;

            if (args != null)
                argCount = args.Length;

                System.Reflection.ParameterInfo[] m_params = method_delegate.Method.GetParameters();

                if (args == null)
                {
                    if (method_delegate.Method.GetParameters().Length == 0)
                        result = method_delegate.DynamicInvoke();
                    else
                        result = method_delegate.DynamicInvoke(new object[] { null });
                    return true;
                }
                else
                if (m_params.Length == args.Length || argCount == args.Length)
                {
                  if (method_delegate.Method.GetParameters().Length != argCount)
                    result = method_delegate.DynamicInvoke((object)args.ToArray());
                  else
                    result = method_delegate.DynamicInvoke(args.ToArray());
                   return true;
                }
                else
                {
                    string msg = string.Empty;
                    foreach (System.Reflection.ParameterInfo paramInfo in m_params)
                    {
                        msg += paramInfo.ParameterType.ToString() + " ";
                    }
                    Console.WriteLine("Los parametros no son validos {1} {0}", msg, name);
                }
            result = null;
            return false;
        }
    }
}
