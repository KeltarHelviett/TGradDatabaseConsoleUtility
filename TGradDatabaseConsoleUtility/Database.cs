using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TGradDatabaseConsoleUtility
{
    class Database
    {
        private Database() { }

        public static Database Instance { get; } = new Database();

        public Dictionary<string, Package> Packages { get; } = new Dictionary<string, Package>();

        public void LoadPackagesFromFolder(string folder)
        {
            var files = Directory.GetFiles(folder).Select(s => s).Where(s => s.EndsWith(".pck"));
            foreach (var file in files)
            {
                var pck = new Package(file);
                Packages.Add(pck.Name, pck);
            }
        }
    }
}
