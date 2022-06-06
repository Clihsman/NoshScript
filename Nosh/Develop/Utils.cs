using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace NoshScript
{
    public class Utils
    {
        
        public static string[] split(string data, char separator,char ignore) {
            List<string> result = new List<string>();

            string currentLine = "";
            bool add = false;


            for (int i = 0; i < data.Length; i++) {
                if (!add)
                {
                    if (data[i] == ignore) {
                        add = true;
                        currentLine += data[i];
                        continue;
                    }

                    if (data[i] == separator)
                    {
                        result.Add(currentLine);
                        currentLine = "";
                        continue;
                    }

                    currentLine += data[i];
                }
                else
                {
                    if (data[i] == ignore)
                    {
                        add = false;
                        currentLine += data[i];
                        continue;
                    }
                    currentLine += data[i];
                }
            }
            result.Add(currentLine);

            return result.ToArray();
        }
        
        public static string[] removeWhiteSpace(string[] data) {
            List<string> result = new List<string>();
            foreach (string s in data) {
                if (!string.IsNullOrWhiteSpace(s))
                    result.Add(s);
            }
            return result.ToArray();
        }

        public static object getValue(string value) {
            value = value.Trim();

            if (value.StartsWith("\"")) {
                value = value.Replace("\"", "");
                return value;
            }
            if (char.IsNumber(value[0]) || value[0] == '-')
            {

                value = value.Replace(" ", "");
                if (value.Contains("."))
                    return float.Parse(value.Replace('.', ','));
                else {
                    return int.Parse(value);
                }
            }

            return null;
        }

        public static object getValue(string varName, Script script, Script parent) {
            varName = varName.Trim();

            if (isText(varName[0]))
            {
                varName = varName.Trim();

                if (varName == "null")
                    return null;
                if (varName == "true")
                    return true;
                if (varName == "false")
                    return false;

                Var var = null;

                if(script != null)
                 var = script.getVar(varName);

                if (var != null)
                    return var.getValue();

                if (parent != null)
                    var = parent.getVar(varName);

                if (var != null)
                    return var.getValue();

                Console.WriteLine(script.variables.Count);
                Console.WriteLine("script : {0} varName : {1}",script.getName(),varName);

                throw new MissingFieldException(varName);
            }
            else
            {
                if (varName[0] == '-')
                {
                    if (isText(varName[1]))
                    {
                        object value = getValue(varName.Remove(0,1),script,parent);
                        Console.WriteLine(value);
                        if (value is float)
                            return -(float)value;
                        if (value is int)
                            return -(int)value;
                    }
                    else
                        return getValue(varName);
                }
                 return getValue(varName);
            }
        }

        public static object getValue(Var variable, string value, Script script,Script parent)
        {
            return getValue(value, script ,parent);
        }
        
        public static object getValue(Var variable, string value, Script script)
        {
            return getValue(value, script, null);
        }
        
        public static bool isText(char m_c) {
            char[] data = {
                'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o',
                'p','q','r','s','t','u','v','w','x','y','z','_',
            };

            foreach (char i_c in data) {
                if (i_c == char.ToLower(m_c)) {
                    return true;
                }
            }
            return false;
        }

        public static string[] split(string data, char[] separador)
        {
            List<string> result = new List<string>();

            Func<char, char[], bool> contains = delegate (char caracter, char[] in_data) {
                foreach (char m_c in in_data)
                    if (caracter == m_c)
                        return true;
                return false;
            };

            string line = "";

            for (int i = 0; i < data.Length; i++) {

                if (contains(data[i], separador)) {
                    result.Add(line);
                    result.Add(data[i].ToString());
                    line = "";
                }
                else
                    line += data[i];
            }
            result.Add(line);

            return result.ToArray();
        }

        public static bool condition(Var a,Var b,string condition)
        {
            switch (condition)
            {
                case ">":
                    double valueA, valueB;
                    valueA = Convert.ToDouble(a.getValue());
                    valueB = Convert.ToDouble(b.getValue());
                    return valueA > valueB;
                case "<":
                    valueA = Convert.ToDouble(a.getValue());
                    valueB = Convert.ToDouble(b.getValue());
                    return valueA < valueB;
                case ">=":
                    valueA = Convert.ToDouble(a.getValue());
                    valueB = Convert.ToDouble(b.getValue());
                    return valueA >= valueB;
                case "<=":
                    valueA = Convert.ToDouble(a.getValue());
                    valueB = Convert.ToDouble(b.getValue());
                    return valueA <= valueB;
                case "==":
                        return Equals(a.getValue(), Convert.ChangeType(b.getValue(),a.getValue().GetType()));
                case "!=":
                    return !Equals(a.getValue(), Convert.ChangeType(b.getValue(), a.getValue().GetType()));
            }
            return false;
        }

        public static string getScriptSource(string filename)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args != null && args.Length > 1)
            {
                if (System.IO.File.Exists(args[1]))
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(args[1]);
                    if (fileInfo.Extension.ToUpper() != ".NS")
                    {
                        ZipArchive zipFile = ZipFile.Open(fileInfo.FullName,ZipArchiveMode.Read);
                        ZipArchiveEntry entry = zipFile.GetEntry(filename);

                        if (entry != null)
                        {
                            StreamReader reader = new StreamReader(entry.Open());
                            string script = reader.ReadToEnd();
                            reader.Close();
                            return script;
                        }
                        else
                        {
                            zipFile.Dispose();
                            throw new FileNotFoundException(filename);
                        }
                    }
                    else
                    {
                        string path = Path.GetDirectoryName(fileInfo.FullName);
                        string fullPath = path + "/" + filename;
                        if (File.Exists(fullPath))
                            return File.ReadAllText(fullPath);
                        else
                            throw new FileNotFoundException(filename);
                    }
                }
            }
            if(File.Exists(filename))
                return File.ReadAllText(filename);

            throw new FileNotFoundException(filename);
        }
	}
}

