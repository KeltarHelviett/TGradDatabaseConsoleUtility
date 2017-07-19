using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CommandLine;
using CommandLine.Text;


namespace TGradDatabaseConsoleUtility
{
    class Options
    {
        private Options () { }

        public static Options Instance { get; } = new Options();

        [Option('v', "verbose", DefaultValue = false, HelpText = "Print all information")]
        public bool Verbose { get; set; }

        private string dbPackagesFolder;

        [ValueOption(0)]
        public string  DBPackagesFolder
        {
            get { return dbPackagesFolder; }
            set
            {
                if (!Directory.Exists(value))
                    throw new Exception("ERROR: Database Packages folder doesn't exists");
                dbPackagesFolder = value;
            }
        }

        private string metabasexml;

        [ValueOption(1)]
        public string MetabaseXml
        {
            get { return metabasexml; }
            set
            {
                if (value.IndexOf(".xml") == -1)
                    throw new Exception("Given metabase file isn't an xml document");
                metabasexml = value;
            }
        }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
