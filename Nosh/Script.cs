using NoshScript.Nosh;
using NoshScript.Nosh.Collections;
using NoshScript.Nosh.Collections.NoshPackage;
using NoshScript.Nosh.Native.Bucles;
using NoshScript.Nosh.Native.Condicionales;
using NoshScript.Nosh.Native.Types;
using System;
using System.Collections.Generic;
using System.IO;

namespace NoshScript
{
    public class Script
    {
        public List<Var> variables = new List<Var>();
        private List<Var> ins = new List<Var>();
        private Dictionary<string, List<Funtion>> functions = new Dictionary<string, List<Funtion>>();
        private Dictionary<string, Var> constructors = new Dictionary<string, Var>();
        private List<string> source = new List<string>();
        private List<Script> subScripts = new List<Script>();
        private List<Package> packages = new List<Package>();
        private int executeLine = 0;
        private Script parent;
        private string scriptName;
        private Var m_this;

        public Script(StringReader source)
        {
            loadSource(source);
        }

        public Script(StringReader source, Script parent)
        {
            loadSource(source);
            this.parent = parent;
        }

        public Script(StringReader source, Script parent, Dictionary<string, List<Funtion>> functions)
        {
            loadSource(source);
            this.parent = parent;
            this.functions = functions;
        }

        public void setFunctions(Dictionary<string, List<Funtion>> functions)
        {
            this.functions = functions;
        }

        public void loadSource(StringReader src)
        {
            packages.Add(new Package("nosh", null,null, null));

            string line;
            while ((line = src.ReadLine()) != null)
            {
                source.Add(line);
            }
        }

        public object execute()
        {
            string line = "";

            bool comment = false;

            for (int i = 0; i < source.Count; i++)
            {
                try
                {
                    line = source[i];
                    line = line.Trim();

                    if (line.StartsWith("!#"))
                    {
                        comment = !comment;
                        continue;
                    }

                    if (line.StartsWith("#") || comment)
                        continue;

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    if (line.StartsWith("var:"))
                    {
                        variables.Add(createVar(line));
                    }

                    else if (line == "continue")
                        return BucleOption.CONTINUE;
                    else if (line == "break")
                        return BucleOption.BREAK;
                    else
                    if (line.StartsWith("var "))
                    {
                        int index = line.IndexOf(" ");
                        int end = line.IndexOf(":") - index;
                        string name = line.Substring(index, end).Trim();

                        NoshType varType = NoshType.getType("nosh.target");
                        Var var = new Var(name, varType, null);
                        var.setValue(var, varType);
                        do
                        {
                            if (line.StartsWith("var:"))
                            {
                                var.addVar(createVar(line));
                            }
                            else
                            if (line.StartsWith("go "))
                            {
                                Funtion method = loadMethod(source, ref i);
                                method.getMethodScript().variables.AddRange(var.getVariables());

                                if (method.getName() == name)
                                {
                                    addConstructor(name, var);
                                    var.addMethodConstructor(method.getName(), method);
                                }
                                else
                                    var.addMethod(method.getName(), method);

                                method.getMethodScript().setExecuteLine(i - method.getMethodScript().source.Count);
                            }


                            i++;
                            line = source[i];
                            line = line.Trim();

                            if (line == "pass")
                                break;
                        }
                        while (line != "pass");

                        variables.Add(var);
                    }
                    else if (line.StartsWith("go "))
                    {
                        Funtion method = loadMethod(source, ref i);
                        addFuntion(method.getName(), method);
                        method.getMethodScript().setExecuteLine(i - method.getMethodScript().source.Count);
                    }
                    else if (line.StartsWith("include "))
                    {
                        if (line.Contains("<") && line.Contains("\""))
                            throw new Exception();

                        if (line.Contains("\""))
                        {
                            int location = line.IndexOf("\"");
                            string scriptName = line.Substring(location, line.Length - location);
                            scriptName = Utils.getValue(scriptName).ToString();
                            string scriptPath = string.Format("{0}.ns", scriptName);

                            StringReader source = new StringReader(Utils.getScriptSource(scriptPath));
                            Script bscript = new Script(source);
                            bscript.execute();
                            subScripts.Add(bscript);
                        }
                        else if (line.Contains("<"))
                        {
                            line = line.Replace('<', '"').Replace('>', '"');

                            int location = line.IndexOf("\"");
                            string packageName = line.Substring(location, line.Length - location);
                            packageName = Utils.getValue(packageName).ToString();
                            Package package = PackageList.GetPackage(packageName);
                            if (package != null)
                            {
                                packages.Add(package);
                            }
                            else
                                throw new FileNotFoundException(packageName);
                        }
                    }

                    else if (line.StartsWith("import "))
                    {
                        int location = line.IndexOf(" ");
                        int end = line.Length - location;
                        string assembly = "local";

                        if (line.Contains(" in "))
                        {
                            end = line.IndexOf(" in ");
                            assembly = (string)Utils.getValue(line.Substring(end + 3, line.Length - end - 3).Trim());
                            end -= 5;
                        }

                        string className = line.Substring(location, end);
                        className = className.Trim().TrimEnd();
                        Type type = null;

                        if (assembly == "local")
                            type = Type.GetType(className, false);
                        else
                        {
                            System.Reflection.Assembly ams = System.Reflection.Assembly.LoadFrom(Path.GetFullPath(assembly));

                            if (ams != null)
                            {
                                type = ams.GetType(className, false);
                            }
                        }

                        if (type != null)
                        {
                            System.Reflection.MethodInfo[] methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.InvokeMethod);
                            foreach (var method in methods)
                            {
                                Delegate func = new Func<object, object>(delegate (object arg)
                                 {
                                     if (arg is Array)
                                         return method.Invoke(null, (object[])arg);
                                     else
                                     {
                                         if (arg != null)
                                             return method.Invoke(null, new object[] { arg });
                                         else
                                             return method.Invoke(null, null);
                                     }
                                 });
                                addFuntion(method.Name, method.GetParameters().Length, func);
                            }
                        }
                    }
                    else if (line.StartsWith("return "))
                    {
                        line = line.Remove(0, "return".Length).Trim();

                        if (isFuction(line))
                        {
                            return RunFunction(line, ref i);
                        }
                        else
                        {
                            string varName = line.Trim();
                            Var result = getVar(varName);
                            if (result != null)
                            {
                                return result.getValue();
                            }
                        }
                    }
                    else if (isFuction(line))
                    {
                        object result = RunFunction(line, ref i);

                        if (result is BucleOption)
                        {
                            return result;
                        }

                    }
                    else if (line.Contains("=") && !line.Contains("=="))
                    {
                        string[] value = line.Split('=');
                        value = Utils.removeWhiteSpace(value);

                        string varName = value[0].Replace(" ", "");
                        setVarValue(varName, value[1].Trim());
                    }
                }
                catch (Exception ex)
                {
                    throw new NoshException(string.Format("line {0} error : {1}", executeLine + (i + 1), ex), executeLine + (i + 1));
                   // System.Windows.Forms.MessageBox.Show(string.Format("line {0} error : {1}", executeLine + (i + 1), ex), "NoshScript", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                 //   Environment.Exit(-1);
                }
            }
            return null;
        }

        public object executeNoTry()
        {
            string line = "";

            bool comment = false;

            for (int i = 0; i < source.Count; i++)
            {
                line = source[i];
                line = line.Trim();

                if (line.StartsWith("#!"))
                {
                    comment = true;
                    continue;
                }

                if (line.StartsWith("!#"))
                {
                    comment = false;
                    continue;
                }

                if (line.StartsWith("#") || comment)
                    continue;

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("var:"))
                {
                    variables.Add(createVar(line));
                }
                else if (line == "continue")
                    return BucleOption.CONTINUE;
                else if (line == "break")
                    return BucleOption.BREAK;
                else
                if (line.StartsWith("var "))
                {
                    int index = line.IndexOf(" ");
                    int end = line.IndexOf(":") - index;
                    string name = line.Substring(index, end).Trim();

                    NoshType varType = NoshType.getType("nosh.target");
                    Var var = new Var(name, varType, null);
                    var.setValue(var, varType);
                    do
                    {
                        if (line.StartsWith("var:"))
                        {
                            var.addVar(createVar(line));
                        }
                        else
                        if (line.StartsWith("go "))
                        {
                            Funtion method = loadMethod(source, ref i);
                            method.getMethodScript().variables.AddRange(var.getVariables());
                            var.addMethod(method.getName(), method);
                            method.getMethodScript().setExecuteLine(i - method.getMethodScript().source.Count);
                        }

                        i++;
                        line = source[i];
                        line = line.Trim();

                        if (line == "pass")
                            break;
                    }
                    while (line != "pass");

                    variables.Add(var);
                }
                else if (line.StartsWith("return "))
                {
                    line = line.Remove(0, "return".Length).Trim();

                    if (isFuction(line))
                    {
                        return RunFunction(line, ref i);
                    }
                    else
                    {
                        string varName = line.Trim().TrimEnd();
                        return getVar(varName).getValue();
                    }
                }
                else if (line.StartsWith("go "))
                {
                    Funtion method = loadMethod(source, ref i);
                    addFuntion(method.getName(), method);
                    method.getMethodScript().setExecuteLine(i - method.getMethodScript().source.Count);
                }
                else if (line.StartsWith("include "))
                {
                    if (line.Contains("<") && line.Contains("\""))
                        throw new Exception();

                    if (line.Contains("\""))
                    {
                        int location = line.IndexOf("\"");
                        string scriptName = line.Substring(location, line.Length - location);
                        scriptName = Utils.getValue(scriptName).ToString();
                        string scriptPath = string.Format("{0}.ns", scriptName);

                        StringReader source = new StringReader(Utils.getScriptSource(scriptPath));
                        Script bscript = new Script(source);
                        bscript.execute();
                        subScripts.Add(bscript);
                    }
                    else if (line.Contains("<"))
                    {
                        line = line.Replace('<', '"').Replace('>', '"');

                        int location = line.IndexOf("\"");
                        string packageName = line.Substring(location, line.Length - location);
                        packageName = Utils.getValue(packageName).ToString();
                        Package package = PackageList.GetPackage(packageName);
                        if (package != null)
                        {
                            packages.Add(package);
                        }
                    }
                }

                else if (line.StartsWith("import "))
                {
                    int location = line.IndexOf(" ");
                    int end = line.Length - location;
                    string assembly = "local";

                    if (line.Contains(" in "))
                    {
                        end = line.IndexOf(" in ");
                        assembly = (string)Utils.getValue(line.Substring(end + 3, line.Length - end - 3).Trim());
                        end -= 5;
                    }

                    string className = line.Substring(location, end);
                    className = className.Trim().TrimEnd();
                    Type type = null;

                    if (assembly == "local")
                        type = Type.GetType(className, false);
                    else
                    {
                        System.Reflection.Assembly ams = System.Reflection.Assembly.LoadFrom(Path.GetFullPath(assembly));

                        if (ams != null)
                        {
                            type = ams.GetType(className, false);
                        }
                    }

                    if (type != null)
                    {
                        System.Reflection.MethodInfo[] methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.InvokeMethod);
                        foreach (var method in methods)
                        {
                            Delegate func = new Func<object, object>(delegate (object arg)
                            {
                                if (arg is Array)
                                    return method.Invoke(null, (object[])arg);
                                else
                                {
                                    if (arg != null)
                                        return method.Invoke(null, new object[] { arg });
                                    else
                                        return method.Invoke(null, null);
                                }
                            });
                            addFuntion(method.Name, method.GetParameters().Length, func);
                        }
                    }
                }

                else if (isFuction(line))
                {
                    object result = RunFunction(line, ref i);
                    /*
                    if (result != null)
                    {
                        return result;
                    }
                    */
                }
                else if (line.Contains("=") && !line.Contains("=="))
                {
                    string[] value = line.Split('=');
                    value = Utils.removeWhiteSpace(value);

                    string varName = value[0].Replace(" ", "");
                    setVarValue(varName, value[1].Trim());
                }
            }
            return null;
        }

        public void setExecuteLine(int executeLine)
        {
            this.executeLine = executeLine;
        }

        public void addFuntion(string name, int argCount, Delegate method)
        {
            List<Funtion> funcs;
            if (functions.TryGetValue(name, out funcs))
            {
                Funtion func = new Funtion(name, argCount, method);
                if (!funcs.Contains(func))
                {
                    funcs.Add(func);
                }
            }
            else
            {
                funcs = new List<Funtion>();
                Funtion func = new Funtion(name, argCount, method);
                funcs.Add(func);
                functions.Add(name, funcs);
            }
        }

        public void addFuntion(string name, Funtion func)
        {
            List<Funtion> funcs;
            if (functions.TryGetValue(name, out funcs))
            {
                if (!funcs.Contains(func))
                {
                    Var method = new Var(name, NoshType.getType("nosh.method"), func);
                    variables.Add(method);

                    funcs.Add(func);
                }
            }
            else
            {
                Var method = new Var(name, NoshType.getType("nosh.method"), func);
                variables.Add(method);

                funcs = new List<Funtion>();
                funcs.Add(func);
                functions.Add(name, funcs);
            }
        }

        public void addConstructor(string name, Var m_class)
        {
            constructors.Add(name, m_class);
        }

        private object RunFunction(string line, ref int lineNumber)
        {
            PatternInput pattern = new PatternInput('(', ')');
            string inArgs = pattern.compile(line);
            string[] argsFun = Utils.split(inArgs, ',', '"');
            argsFun = Utils.removeWhiteSpace(argsFun);

            //            object result = null;

            if (/*inArgs.Contains(".")*/false)
            {
                /*
                for (int i = 0;i < subs.Length;i++)
                {
                    Console.WriteLine("sub {0}", subs[i]);

                    if (isFuction(subs[i]))
                    {
                        if (result == null)
                            result = RunFunction(subs[i], ref lineNumber);
                        else
                        {
                            string functionName = subs[i].Split('(')[0].Trim();
                            string[] args = getArgs(subs[i]);

                            Funtion fun = ((Var)result).getMethod(functionName);

                            if (fun != null && fun.getArgCount() == args.Length)
                            {
                                if (!fun.Invoke(getArgsToObject(args), out result))
                                    throw new System.MissingMethodException(functionName);
                            }
                        }
                    }
                    else
                    {
                        string varName = subs[i].Trim();
                        if (result == null)
                        {
                            Var var = getVar(varName);
                            if (var != null)
                            {
                                result = var.getValue();
                            }
                        }          
                        else
                        {
                            if (result is Var)
                            {
                                Var var = (Var)result;
                                var = var.getVar(varName);
                                if (var != null)
                                {
                                    result = var.getValue();
                                }
                            }
                        }
                    }
                }
                */
            }
            else
            {
                string functionName = line.Split('(')[0];

                if (isNativeFunc(functionName))
                {
                    return loadNativeFunction(ref lineNumber, functionName, pattern.compile(line));
                }

                string[] args = getArgs(line);

                if (args.Length > 0)
                    return invokeFunc(functionName, args);
                else
                    return invokeFunc(functionName);
            }
            //  return result;
        }

        private string[] getArgs(string line)
        {
            PatternInput pattern = new PatternInput('(', ')');
            string[] args = pattern.compile(line).Split(',');
            args = Utils.removeWhiteSpace(args);
            return args;
        }

        private Funtion loadMethod(List<string> source, ref int i)
        {
            string head = source[i];
            string name = head.Substring(0, head.IndexOf("("));
            string[] currentName = name.Split(' ');
            name = currentName[currentName.Length - 1];

            if (isNativeFunc(name))
                return null;

            string sourceScript = getSource(source, ref i);

            int indexStart = head.IndexOf("(");
            int length = head.IndexOf("):") - indexStart;
            string[] m_args = getArgs(head.Substring(indexStart, length));

            StringReader src = new StringReader(sourceScript);
            Script script = new Script(src, this);
            script.setFunctions(functions);

            foreach (string varName in m_args)
                script.addIn(varName, null);

            script.scriptName = name;

            return new Funtion(name, m_args.Length, script);
        }

        private string getSource(List<string> source, ref int point)
        {
            int end = 1;
            string sourceScript = string.Empty;
            int pointer = 0;

            for (int i = point + 1; i < source.Count; i++)
            {
                string line = source[i].TrimEnd();
                pointer = i;

                if (line.EndsWith("):"))
                    end++;

                if (line.Trim().Split(' ')[0] == "pass")
                {
                    end--;

                    if (end == 0)
                        break;
                }
                sourceScript += line + "\n";
            }

            point = pointer;
            return sourceScript;
        }

        private object loadNativeFunction(ref int lineP, string funcName, string args)
        {
            string sourceScript = "";
            string line;
            int ultPos = 0;
            int end = 0;

            for (int i = lineP + 1; i < source.Count; i++)
            {
                line = source[i];
                ultPos = i;

                if (isFuction(line))
                {
                    PatternInput pattern = new PatternInput('(', ')');
                    string functionName = line.Split('(')[0].Trim();

                    if (line.Trim().EndsWith("):"))
                    {
                        end++;
                    }
                }

                if (line.Trim().Split(' ')[0] == "pass")
                {
                    if (end == 0)
                        break;
                    end--;
                }

                sourceScript += line + "\n";
            }

            StringReader reader = new StringReader(sourceScript);
            Script script = new Script(reader, this);
            script.setFunctions(functions);
            script.packages = packages;
            script.setThis(m_this);

            lineP = ultPos;

            if (funcName == "if")
            {
                Script m_else = null;

                if (lineP + 1 < source.Count)
                {
                    if (source[lineP + 1].Trim().StartsWith("else("))
                    {
                        lineP++;
                        m_else = loadCatch(ref lineP);
                    }
                }

                If m_if = new If(script, args, m_else);

                object value = m_if.execute();
                if (value != null)
                    return value;
            }
            else if (funcName == "for")
            {
                For m_for = new For(script, args);
                object value = m_for.execute();
                if (value != null)
                    return value;
            }
            else if (funcName == "foreach")
            {
                Foreach m_foreach = new Foreach(script, args);
                object value = m_foreach.execute();
                if (value != null)
                    return value;
            }
            else if (funcName == "while")
            {
                While m_while = new While(script, args);
                object value = m_while.execute();

                if (value != null)
                    return value;
            }
            else if (funcName == "try")
            {
                if (args.Length > 0)
                    throw new MissingMethodException(funcName);

                Script m_catch = null;
                Script m_finally = null;

                if (lineP + 1 < source.Count)
                    if (source[lineP + 1].Trim().StartsWith("catch("))
                    {
                        PatternInput pattern = new PatternInput('(', ')');
                        string exName = pattern.compile(source[ultPos + 1]).Trim();

                        lineP++;
                        m_catch = loadCatch(ref lineP);

                        if (!string.IsNullOrWhiteSpace(exName))
                        {
                            m_catch.addIn(exName, null);
                        }
                    }
                if (lineP + 1 < source.Count)
                    if (source[lineP + 1].Trim().StartsWith("finally("))
                    {
                        lineP++;
                        m_finally = loadCatch(ref lineP);
                    }

                Try m_try = new Try(script, m_catch, m_finally);
                object value = m_try.execute();
                if (value != null)
                    return value;
            }
            return null;
        }

        private Script loadCatch(ref int lineP)
        {
            string sourceScript = "";
            string line;
            int ultPos = 0;
            int end = 0;

            for (int i = lineP + 1; i < source.Count; i++)
            {
                line = source[i];
                ultPos = i;

                if (isFuction(line))
                {
                    PatternInput pattern = new PatternInput('(', ')');
                    string functionName = line.Split('(')[0].Trim();

                    if (isNativeFunc(functionName))
                    {
                        end++;
                    }
                }

                if (line.Trim().Split(' ')[0] == "pass")
                {
                    if (end == 0)
                        break;
                    end--;
                }

                sourceScript += line + "\n";
            }

            StringReader reader = new StringReader(sourceScript);
            Script script = new Script(reader, this);
            script.setFunctions(functions);
            script.packages = packages;
            lineP = ultPos;

            return script;
        }

        private bool isNativeFunc(string funcName)
        {
            if (funcName == "if")
                return true;
            else
            if (funcName == "for")
                return true;
            else
            if (funcName == "foreach")
                return true;
            if (funcName == "while")
                return true;
            if (funcName == "try")
                return true;
            return false;
        }

        private Var createVar(string line)
        {
            string[] currentLine = line.Split(' ');
            string varName = currentLine[1].Trim();
            string varTypeString = currentLine[0].Split(':')[1].Trim();
            NoshType varType = NoshType.getType(varTypeString, packages.ToArray());

            Var var = null;
           
            if (line.Contains("="))
            {
                string[] value = line.Split('=');
                value = Utils.removeWhiteSpace(value);
                var = new Var(varName, varType);

                int indexOf = line.IndexOf("=", 0);
                string data = line.Remove(0, indexOf + 1).Trim();

                if (isFuction(data))
                {
                    string methodName = data;

                    methodName = methodName.Substring(0, methodName.LastIndexOf("(")).Trim();

                    if (methodName == varTypeString)
                    {
                        string[] argsString = getArgs(data);
                        object[] args = getArgsToObject(argsString);

                        Var instace = invokeConstructor(methodName, args, varName);

                        if (instace != null)
                        {
                            var = instace;
                            var.setValue(instace,instace.getType());
                            return var;
                        }
                    }
                }
                
                var.setValue(getVarValue(var, data), varType);
            }
            else
            {
                var = new Var(varName, varType);
            }
            return var;
        }

        public void addIn(string name, object value)
        {
            Var var = new Var(name, NoshType.getType("nosh.object"));
            var.setValue(value, NoshType.getType("nosh.object"));
            ins.Add(var);
        }

        private void addVar(string name, string type)
        {
            Var var = new Var(name, NoshType.getType(type, packages.ToArray()));
            variables.Add(var);
        }

        public void setVarValue(string name, string value)
        {
            Var var = getVar(name);

            if (var != null)
            {
                if (isFuction(value))
                {
                    PatternInput pattern = new PatternInput('(', ')');
                    string functionName = value.Split('(')[0];
                    string[] args = pattern.compile(value).Split(',');
                    args = Utils.removeWhiteSpace(args);

                    if (args.Length > 0)
                    {
                        var.setValue(invokeFunc(functionName, args),null);
                    }
                    else
                        var.setValue(invokeFunc(functionName), null);
                }
                else
                {
                    var.setValue(Utils.getValue(var, value, this, parent), null);
                }
            }

            else throw new MissingFieldException(name);

        }

        public void setVarValue(Var var, string value)
        {
            if (var != null)
            {
                if (isFuction(value))
                {
                    PatternInput pattern = new PatternInput('(', ')');
                    string functionName = value.Split('(')[0];
                    string[] args = pattern.compile(value).Split(',');
                    args = Utils.removeWhiteSpace(args);

                    if (args.Length > 0)
                    {
                        var.setValue(invokeFunc(functionName, args), null);
                    }
                    else
                        var.setValue(invokeFunc(functionName), null);
                }
                else
                {
                    var.setValue(Utils.getValue(var, value, this, parent), null);
                }
            }

        }

        public object getVarValue(Var var, string value)
        {
            if (var != null)
            {
                if (value[0] != '"' && !char.IsNumber(value[0]))
                {
                    string[] methdos = splitMethos(value);

                    if (methdos.Length > 2)
                    {
                        object result = null;

                        for (int i = 0; i < methdos.Length; i++)
                        {
                            string methodName = methdos[i].Trim();

                            if (isFuction(methodName))
                            {
                                PatternInput pattern = new PatternInput('(', ')');
                                string functionName = methodName.Split('(')[0];
                                string[] argsString = getArgs(methodName);
                                argsString = Utils.removeWhiteSpace(argsString);
                                object[] args = getArgsToObject(argsString);

                                if (result != null && result is Var)
                                {
                                    if (!((Var)result).getMethod(functionName, args.Length).Invoke(args, out result))
                                        throw new MissingMethodException(functionName);
                                }
                                else
                                {
                                    Funtion fun;
                                    if (args.Length > 0)
                                    {
                                        if (getFunction(functionName, args.Length, out fun))
                                        {
                                            if (!fun.Invoke(args, out result))
                                                throw new MissingMethodException(functionName);
                                        }
                                    }
                                    else
                                    {
                                        if (getFunction(functionName, 0, out fun))
                                        {
                                            if (!fun.Invoke(null, out result))
                                                throw new MissingMethodException(functionName);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (methodName[0] != '"' && !char.IsNumber(methodName[0]))
                                {
                                    if (result == null)
                                        result = getVar(methodName);
                                    else
                                        if (result is Var)
                                        result = ((Var)result).getVar(methodName);
                                }
                            }
                        }
                        return result;
                    }
                    else
                    {
                        if (isFuction(value))
                        {

                            PatternInput pattern = new PatternInput('(', ')');
                            string functionName = value.Split('(')[0];
                            string[] args = pattern.compile(value).Split(',');
                            args = Utils.removeWhiteSpace(args);

                            if (args.Length > 0)
                            {
                                return invokeFunc(functionName, args);
                            }
                            else
                            {
                                return invokeFunc(functionName);
                            }
                        }
                        else
                        {
                            return Utils.getValue(var, value, this, parent);
                        }
                    }
                }
                else
                    return Utils.getValue(value);
            }
            return null;
        }

        private static string[] splitMethos(string line)
        {
            List<string> methods = new List<string>();

            string data = "";

            for (int i = 0; i < line.Length; i++)
            {

                if (line[i] == '(')
                {
                    data += line[i];

                    i++;
                    int count = 1;
                    while (line[i] != ')' || count <= 0)
                    {
                        if (line[i] == '"')
                        {
                            data += line[i];
                            i++;
                            while (line[i] != '"')
                            {
                                data += line[i];
                                i++;
                            }
                        }

                        if (line[i] == ')')
                            count--;

                        if (line[i] == '(')
                            count++;

                        data += line[i];
                        i++;
                    }
                }

                if (line[i] == '.')
                {
                    methods.Add(data);
                    data = "";
                }
                else
                    data += line[i];
            }

            methods.Add(data);

            return methods.ToArray();
        }

        public bool setVarValue(string name, object value)
        {
            Var var = getVar(name);
            if (var != null)
            {
                var.setValue(value, null);
                return true;
            }
            return false;
        }

        public Var getVar(string name)
        {
            if (name == "true")
                return new Var("TRUE", NoshType.getType("nosh.bool"), true);

            if (name == "false")
                return new Var("FALSE", NoshType.getType("nosh.bool"), false);

            if (name == "this" && m_this != null)
            {
                return m_this;
            }

            Var arg = getIn(name);
            if (arg != null)
                return arg;

            string[] vars = name.Split('.');

            if (vars.Length > 1)
            {
                Var var = getVar(vars[0]);

                for (int i = 1; i < vars.Length; i++)
                {

                    string varName = vars[i];

                    if (var != null)
                    {
                        Var currentVar = var.getVar(varName);

                        if (currentVar != null)
                            var = currentVar;
                        else
                        {
                            object value = var.getValue();

                            if (value != null && value.GetType() == typeof(Var))
                            {
                                var = ((Var)value).getVar(varName);
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                return var;
            }

            foreach (Package package in packages)
            {
                Var var = package.getVariable(name);
                if (var != null)
                {
                    return var;
                }
            }

            for (int i = 0; i < variables.Count; i++)
            {
                if (variables[i].getName() == name)
                {
                    return variables[i];
                }
            }

            foreach (Script sub in subScripts)
            {
                Var var = sub.getVar(name);
                if (var != null)
                    return var;
            }

            Script currentScript = parent;

            while (true)
            {

                if (currentScript != null)
                {
                    for (int i = 0; i < currentScript.variables.Count; i++)
                    {
                        Var var = currentScript.getVar(name);
                        if (var != null)
                            return var;
                    }
                    currentScript = currentScript.parent;
                }
                else
                    break;
            }
            return null;
        }

        public Var getIn(string name)
        {
            for (int i = 0; i < ins.Count; i++)
            {
                if (ins[i].getName() == name)
                {
                    return ins[i];
                }
            }

            Script currentScript = parent;

            while (true)
            {

                if (currentScript != null)
                {
                    for (int i = 0; i < currentScript.ins.Count; i++)
                    {
                        Var var = currentScript.getIn(name);
                        if (var != null)
                            return var;
                    }
                    currentScript = currentScript.parent;
                }
                else
                    break;
            }

            return null;
        }

        private Var invokeConstructor(string name, object[] args, string varName)
        {
            Var instace = null;
            InvokeConstructor(name, varName, args, out instace);
            return instace;
        }

        public object invokeFunc(string name)
        {
            object result = null;
            if (!InvokeInternalFunc(name, null, out result))
                if (!isNativeFunc(name))
                    throw new System.MissingMethodException(name);

            return result;
        }

        public object invokeFunc(string name, string[] m_args)
        {
            object result = null;

            object[] args = getArgsToObject(m_args);

            if (getConstructor(name, args.Length))
            {
                Var instace;
                if (InvokeConstructor(name, name, args, out instace))
                {
                    return instace;
                }
            }

            if (!InvokeInternalFunc(name, args, out result))
                if (!isNativeFunc(name))
                    throw new System.MissingMethodException(name);

            return result;
        }

        private object[] getArgsToObject(string[] argsString)
        {
            List<Object> args = new List<Object>();

            for (int i = 0; i < argsString.Length; i++)
            {
                if (isFuction(argsString[i]))
                {
                    string in_function = argsString[i];

                    while (!argsString[i].Contains(")"))
                    {
                        i++;
                        if (argsString[i] == ")")
                            in_function += argsString[i];
                        else in_function += "," + argsString[i];
                    }

                    PatternInput pattern = new PatternInput('(', ')');
                    string functionName = in_function.Split('(')[0];
                    string[] f_args = pattern.compile(in_function).Split(',');
                    f_args = Utils.removeWhiteSpace(f_args);

                    if (f_args.Length > 0)
                    {
                        object arg = invokeFunc(functionName, f_args);
                        args.Add(arg);
                    }
                    else
                        args.Add(invokeFunc(functionName));
                }
                else
                {
                    object arg = Utils.getValue(argsString[i], this, parent);
                    args.Add(arg);
                }
            }
            return args.ToArray();
        }

        private bool InvokeInternalFunc(string name, object[] args, out object result)
        {
            Funtion func;

            if (args == null)
            {
                if (getFunction(name, 0, out func))
                {
                    if (func != null)
                        return func.Invoke(null, out result);
                }
            }
            else if (getFunction(name, args.Length, out func))
            {
                if (func != null)
                    return func.Invoke(args, out result);
            }
            result = null;
            return false;
        }

        private bool InvokeConstructor(string name, string varName, object[] args, out Var result)
        {
            Var instance = null;
            Var m_class = null;

            if (args == null)
            {
                if (getConstructor(name, 0, out m_class))
                {
                    instance = Var.instace(m_class, varName);
                    result = instance;
                    Funtion constructor = instance.getConstructor(name, 0);
                    constructor.Invoke(null);
                    return true;
                }
            }
            else if (getConstructor(name, args.Length, out m_class))
            {
                instance = Var.instace(m_class, varName);
                result = instance;
                Funtion constructor = instance.getConstructor(name, args.Length);
                if (constructor != null)
                    constructor.Invoke(args);
                else
                    throw new MissingMethodException(name);
                return true;
            }

            result = null;
            return false;
        }

        public bool getFunction(string name, int argCount, out Funtion func)
        {

            string[] vars = name.Split('.');

            if (vars.Length > 1)
            {
                Var var = getVar(vars[0]);

                for (int i = 1; i < vars.Length - 1; i++)
                {
                    string varName = vars[i];
                    if (var != null)
                        var = var.getVar(varName);
                }
                if (var != null)
                {
                    string funcName = vars[vars.Length - 1];
                    func = var.getMethod(funcName, argCount);
                    return true;
                }
            }

            foreach (Package package in packages)
            {
                Funtion fun = package.getFuntion(name, argCount);
                if (fun != null)
                {
                    func = fun;
                    return true;
                }
            }

            List<Funtion> funcs;
            if (functions.TryGetValue(name, out funcs))
            {
                foreach (Funtion funtion in funcs)
                {
                    if (funtion.getName() == name)
                    {
                        if (funtion.getArgCount() == argCount)
                        {
                            func = funtion;
                            return true;
                        }
                    }
                }
            }
            else
            {
                foreach (Script script in subScripts)
                {
                    if (script.functions.TryGetValue(name, out funcs))
                    {
                        foreach (Funtion funtion in funcs)
                        {
                            if (funtion.getName() == name)
                            {
                                if (funtion.getArgCount() == argCount)
                                {
                                    func = funtion;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            Script currentScript = parent;

            while (true)
            {
                if (currentScript != null)
                {
                    if (currentScript.getFunction(name, argCount, out func))
                        return true;
                    currentScript = currentScript.parent;
                }
                else
                    break;
            }

            func = null;
            return false;
        }

        public bool getConstructor(string name, int argCount, out Var m_class)
        {
            if (constructors.TryGetValue(name, out m_class))
            {
                return true;
            }
            else
            {
                foreach (Script script in subScripts)
                {
                    if (script.constructors.TryGetValue(name, out m_class))
                    {
                        return true;
                    }
                }
            }

            Script currentScript = parent;

            while (true)
            {
                if (currentScript != null)
                {
                    if (currentScript.getConstructor(name, argCount, out m_class))
                        return true;
                    currentScript = currentScript.parent;
                }
                else
                    break;
            }

            m_class = null;
            return false;
        }

        public bool getConstructor(string name, int argCount)
        {
            Var m_class;
            return getConstructor(name, argCount, out m_class);
        }

        public bool isFuction(string line)
        {
            bool ignore = false;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '=')
                    return false;

                if (ignore)
                {
                    if (line[i] == '"')
                    {
                        ignore = false;
                        continue;
                    }
                    continue;
                }
                else
                if (line[i] == '"')
                {
                    ignore = true;
                    continue;
                }

                if (line[i] == '(')
                    return true;
            }
            return false;
        }

        public void removeVar(Var var)
        {
            this.variables.Remove(var);
        }

        public void addVar(Var var)
        {
            this.variables.Add(var);
        }

        public void setIn(int index, object value)
        {
            ins[index].setValue(value, null);
        }

        public void dispose()
        {
            variables.Clear();
            functions.Clear();
            source.Clear();
            subScripts.Clear();
            packages.Clear();
            parent = null;
        }

        public void finish()
        {
            variables.Clear();
        }

        public void cleanIns() {
            ins.Clear();
        }

        public void setThis(Var value) {
            m_this = value;
        }

        public Script instace()
        {
            StringReader reader = new StringReader(string.Join("\n",source.ToArray()));
            Script script = new Script(reader,parent,functions);
            script.scriptName = scriptName;
            script.subScripts = subScripts;
            script.packages = packages;
            script.constructors = constructors;
            return script;
        }

        public string getName() {
            return scriptName;
        }

        public int argsCount()
        {
            return ins.Count;
        }

        public Script getParent()
        {
            return parent;
        }

        public Package[] getPackages() {
            return packages.ToArray();
        }

        public string[] getSource() {
            return source.ToArray();
        }

        public override string ToString()
        {
            return string.Join("\n", source.ToArray());
        }
    }
}