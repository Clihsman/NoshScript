using System;
using System.IO;
using NoshScript.Nosh.Collections.NoshPackage;

namespace NoshScript
{
	public class MainClass
	{
        [STAThread]
        public static void Main(string[] args)
        {
          //  Console.Title = "NoshScript";

            if (args.Length > 0)
            {
                PackageList.LoadPackages();
                execute(args[0]);
            }
            #if DEBUG
            else
            {
                PackageList.LoadPackages();
                execute("Program/script.ns");     
            }
            #endif
        }

        private static void execute(string filename)
        {
            if (File.Exists(filename))
            {
                FileInfo fileInfo = new FileInfo(filename);
                    if (fileInfo.Extension.ToUpper() == ".NS")
                        executeScript(filename);
                    else
                        executeApp(filename);
            }
            else throw new FileNotFoundException(filename);
        }

        private static void executeApp(string filename)
        {
            StringReader source = new StringReader(Utils.getScriptSource("main.ns"));
            Script script = new Script(source);
            try
            {
                script.execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void executeScript(string filename)
        {
            StringReader source = new StringReader(File.ReadAllText(filename));
            Script script = new Script(source);
            try
            {
                script.execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }
	}
}
