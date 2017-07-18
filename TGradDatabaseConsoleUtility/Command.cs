using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace TGradDatabaseConsoleUtility
{
    class Command
    {
        #region Ctor

        public Command(string name, string package, string serverName)
        {
            Name = name;
            Package = Package;
            ServerName = serverName;
        }

        #endregion

        #region PublicProperties

        public string Name { set; get; }

        public string Package { set; get; }

        public string ServerName { set; get; }

        public List<Parameter> Parameters { set; get; } = new List<Parameter>();

        #endregion
    }
}
