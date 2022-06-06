using NoshScript;
using NoshScript.Nosh.Collections;
using NoshScript.Nosh.Collections.NoshPackage;
using System;

namespace stdio
{
    public class stdio
    {
        public static void main()
        {
            Package stdio = null;
            PackageManager manager = new PackageManager();

            loadPrint(manager);

            manager.AddFuntion(new Funtion("read", new Func<string>(delegate () {
                return Console.ReadLine();
            })));

            manager.AddFuntion(new Funtion("pause", new Action(delegate () {
                Console.ReadKey(true);
            })));

            manager.AddFuntion(new Funtion("exit", new Action(delegate () {
                Environment.Exit(0);
            })));

            manager.AddFuntion(new Funtion("exit", new Action<int>(delegate (int exitCode) {
                Environment.Exit(exitCode);
            })));

            stdio = manager.Create("stdio");

			NFile nFile = new NFile(manager, stdio);
			NBuffer nBuffer = new NBuffer(manager, stdio);

            manager.Dispose();
            PackageList.AddPackage(stdio);
        }

        private static void loadPrint(PackageManager manager) {
            manager.AddFuntion(new Funtion("print", new Action<object>(delegate (object msg) {
                Console.WriteLine(msg);
            })));

            manager.AddFuntion(new Funtion("print", new Action<string,object>(delegate (string msg,object arg) {
                Console.WriteLine(msg,arg);
            })));

            manager.AddFuntion(new Funtion("print", new Action<string, object,object>(delegate (string msg, object arg0,object arg1) {
                Console.WriteLine(msg, arg0,arg1);
            })));

            manager.AddFuntion(new Funtion("print", new Action<string, object, object,object>(delegate (string msg, object arg0, object arg1, object arg2) {
                Console.WriteLine(msg, arg0, arg1, arg2);
            })));

            manager.AddFuntion(new Funtion("print", new Action<string, object, object, object,object>(delegate (string msg, object arg0, object arg1, object arg2,object arg3) {
                Console.WriteLine(msg, arg0, arg1, arg2, arg3);
            })));

            manager.AddFuntion(new Funtion("print", new Action<string, object, object, object, object,object>(delegate (string msg, object arg0, object arg1, object arg2, object arg3, object arg4) {
                Console.WriteLine(msg, arg0, arg1, arg2, arg3, arg4);
            })));
        }
    }
}
