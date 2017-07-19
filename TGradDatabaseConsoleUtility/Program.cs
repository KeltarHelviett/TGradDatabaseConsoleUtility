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
            Console.WriteLine();
            try
            {
                if (CommandLine.Parser.Default.ParseArguments(args, Options.Instance))
                {
                    Metabase.Instance.ReadFromXmlFile(Options.Instance.MetabaseXml);
                    Database.Instance.LoadPackagesFromFolder(Options.Instance.DBPackagesFolder);
                    Comparator.Compare(Metabase.Instance, Database.Instance);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n{e.Message}");
            }
        }
    }
}
