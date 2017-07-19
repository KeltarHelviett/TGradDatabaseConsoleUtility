using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TGradDatabaseConsoleUtility
{
    static class Database
    {
        public static  List<Package> Packages { get; } = new List<Package>();

        public static void LoadPackagesFromFolder(string folder)
        {
            var files = Directory.GetFiles(folder).Select(s => s).Where(s => s.EndsWith(".pck"));
            Packages.Capacity = files.Count();
            foreach (var file in files)
            {
                Packages.Add(new Package(file));
            }
        }
    }
}
