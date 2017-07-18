using System;
using System.Collections.Generic;
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
    static class Metabase
    {
        #region PublicProperties

        public static List<Command> Commands { get; } = new List<Command>();

        #endregion

        public static void ReadFromXmlFile(string fileName)
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
                    Log.Error($"{command.Attribute("Name").Value ?? "Unknown"}. ", e.Message);
                }
            }
        }
    }
}
