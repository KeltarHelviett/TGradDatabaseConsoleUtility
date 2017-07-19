using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGradDatabaseConsoleUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (CommandLine.Parser.Default.ParseArguments(args, Options.Instance))
                {
                    Metabase.ReadFromXmlFile(Options.Instance.MetabaseXml);
                    Database.LoadPackagesFromFolder(Options.Instance.DBPackagesFolder);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n{e.Message}");
            }
        }
    }
}
