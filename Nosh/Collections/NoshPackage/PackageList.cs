using System;
using System.Collections.Generic;
using System.IO;

namespace NoshScript.Nosh.Collections.NoshPackage
{
    public static class PackageList
    {
        private static List<Package> packages = new List<Package>();
        private static List<string> libs = new List<string>();
        private static string libPath = null;

        public static Package GetPackage(string name)
        {
            foreach (Package package in packages)
                if (package.getName() == name)
                    return package;

            return null;
        }

        public static void AddPackage(Package package)
        {
            packages.Add(package);
        }

        public static void LoadPackages()
        {
            foreach (string pkgFile in GetPackagePaths())
            {
                FileInfo fileInfo = new FileInfo(pkgFile);
                string pkgName = GetPackageName(pkgFile);

                Package package = GetPackage(pkgName);

                if (package != null)
                    LoadPackageData(package, pkgFile);
                else
                {
                    if (!libs.Contains(pkgName))
                    {
                    #if DEBUG
                        if (LoadLib(pkgName))
                        {
                         //   Console.WriteLine("loaded Package {0}", pkgName);
                        }
                        //  LoadPackages();
                        else
                            Console.WriteLine("Package {0} no found!", pkgName);
                    #else
                        LoadLib(pkgName);
                  #endif
                    }
                }
            }
        }

        public static void LoadPackages(string libPath)
        {
            PackageList.libPath = libPath;
            foreach (string pkgFile in GetPackagePaths())
            {
                FileInfo fileInfo = new FileInfo(pkgFile);
                string pkgName = GetPackageName(pkgFile);

                Package package = GetPackage(pkgName);

                if (package != null)
                    LoadPackageData(package, pkgFile);
                else
                {
                    if (!libs.Contains(pkgName))
                    {
#if DEBUG
                        if (LoadLib(pkgName))
                        {
                            //   Console.WriteLine("loaded Package {0}", pkgName);
                        }
                        //  LoadPackages();
                        else
                            Console.WriteLine("Package {0} no found!", pkgName);
#else
                        LoadLib(pkgName);
#endif
                    }
                }
            }
        }

        private static bool LoadLib(string name)
        {
            foreach (string libFile in GetLibsPaths())
            {
                FileInfo fileInfo = new FileInfo(libFile);
                string libName = fileInfo.Name.Replace(fileInfo.Extension,"");

                if (libName == name)
                {
                    Type type = System.Reflection.Assembly.LoadFrom(libFile).GetType(string.Format("{0}.{0}", libName));
                    if (type != null)
                    {
                        System.Reflection.MethodInfo method = type.GetMethod("main",System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.InvokeMethod);
                        method.Invoke(null,null);
                        libs.Add(name);
                       return true;
                    }
                    return false;
                }
            }
            return false;
        }

        private static void LoadPackageData(Package package, string filename)
        {
            StreamReader reader = new StreamReader(filename);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();

                if (line.StartsWith("link:"))
                {
                    if (line.StartsWith("link:method "))
                    {
                        ContainsMethod(package, line);
                    }
                    else if (line.StartsWith("link:var "))
                    {
                        ContainsVar(package, reader, line);
                    }
                }
            }
        }
    
        private static void ContainsMethod(Package package, string line)
        {
            string name = GetMethodName(line);
            int argCount = GetMethodArgCount(line);

            Funtion fun = package.getFuntion(name, argCount);
            if (fun == null)
            {
                Console.WriteLine(name);
                throw new MissingMethodException(name);
            }
        }

        private static string GetMethodName(string line) {
            PatternInput pattern = new PatternInput('(', ')');
            return line.Replace("link:method", "").Split('(')[0].Trim();
        }

        private static int GetMethodArgCount(string line) {
            PatternInput pattern = new PatternInput('(', ')');
            string[] args = pattern.compile(line).Split(',');
            args = Utils.removeWhiteSpace(args);
            return args.Length;
    }

        private static string[] GetPackagePaths()
        {
            List<string> libs = new List<string>();

            foreach (string path in GetSearchPath())
            {
                if (Directory.Exists(path + "/npkgs"))
                {
                   string[] files = System.IO.Directory.GetFiles(path + "/npkgs/", "*.nlib", System.IO.SearchOption.AllDirectories);
                   libs.AddRange(files);
                }
            }
            return libs.ToArray();
        }

        private static string[] GetLibsPaths()
        {
            List<string> libs = new List<string>();

            foreach (string path in GetSearchPath())
            {
                 if (Directory.Exists(path + "/libs"))
                 {
                    string[] files = System.IO.Directory.GetFiles(path + "/libs/", "*.dll", System.IO.SearchOption.AllDirectories);
                    libs.AddRange(files);
                 }
            }
            return libs.ToArray();
        }

        private static string GetMachinePath()
        {
            if (libPath != null)
                return libPath;

            string path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            return Path.GetDirectoryName(path);
        }

        private static string GetAppPath()
        {
            if (libPath != null)
                return libPath;

            string[] args = Environment.GetCommandLineArgs();
            if (args != null && args.Length > 1)
                if(File.Exists(args[1]))
                return Path.GetDirectoryName(args[1]);
            return null;
        }

        private static string[] GetSearchPath() {
            List<string> paths = new List<string>();
            string appPath = GetAppPath();
            string machinePath = GetMachinePath();
            if (appPath == machinePath)
                paths.Add(machinePath);
            else
                paths.AddRange(new string[] {appPath,machinePath});
            return paths.ToArray();
        }

        private static void ContainsVar(Package package, StreamReader reader, string line)
        {
            string varName = line.Replace("link:var","").Trim();
            Var var = package.getVariable(varName);
            if (var != null)
            {
                while ((line = reader.ReadLine().Trim()) != "pass")
                {
                    string[] args = line.Split(';');
                    args = Utils.removeWhiteSpace(args);
                    foreach (string arg in args)
                    {
                        if (arg.Contains("(") && arg.Contains(")"))
                        {
                            string methodName = GetMethodName(arg);
                            int argsCount = GetMethodArgCount(arg);

                            Funtion fun = var.getMethod(methodName,argsCount);

                            if (fun == null || fun.getArgCount() != argsCount)
                            {
                                throw new MissingMethodException(methodName);
                            }
                        }
                        else
                        {
                            string sub_varName = arg.Trim();
                            Var sub_var = var.getVar(sub_varName);

                            if (sub_var == null)
                            {
                                throw new MissingFieldException(sub_varName);
                            }
                        }
                    }
                }
            }
            else throw new MissingFieldException(varName);
        }

        private static string GetPackageName(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("package "))
                {
                    reader.Close();
                    int index = line.IndexOf(' ');
                    int end = line.Length - index;
                    return line.Substring(index, end).Trim();
                }
            }
            reader.Close();
            throw new Exception(filename);
        }
    }
}
