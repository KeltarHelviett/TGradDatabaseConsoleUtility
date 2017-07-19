using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;

namespace TGradDatabaseConsoleUtility
{
    class Metabase
    {
        private Metabase() { }

        public static Metabase Instance { get; } = new Metabase();

        #region PublicProperties

        public List<Command> Commands { get; } = new List<Command>();

        #endregion

        public void ReadFromXmlFile(string fileName)
        {             
            var xdoc = XDocument.Load(fileName);
            var commands = xdoc.Descendants("Command");
            Commands.Clear();
            Commands.Capacity = commands.Count();
            foreach (var command in commands)
            {
                try
                {
                    
                    var com = new Command(command);
                    var parameters = command.Descendants("Parameters").First().Descendants();
                    foreach (var parameter in parameters)
                    {
                        
                        com.Parameters.Add(new Parameter(parameter));

                    }
                    Commands.Add(com);
                }
                catch (Exception e)
                {
                    Log.Error($"Entity { command.Parent.Parent.Attribute("Name")?.Value ?? "Unknown"}, Command {command.Attribute("Name")?.Value ?? "Unknown"}. ", e.Message);
                }
            }
        }
    }
}
