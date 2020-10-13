using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyLocatorTestHarness
{
    class Program
    {
        private const string TestDirectory = "C:\\TestBin";

        static void Main(string[] args)
        {
            var foundAssemblies = new List<string>();
            Console.WriteLine("Starting to Search");
            var files = Directory.GetFiles(TestDirectory, "*.dll", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var references = Assembly.LoadFile(file).GetReferencedAssemblies();
                var foundTelerik =
                    references.Any(r => r.FullName.StartsWith("Telerik", true, CultureInfo.InvariantCulture));
                if (!foundTelerik)
                    continue;

                foundAssemblies.Add(Path.GetFileName(file));
            }

            Console.WriteLine($"Found {foundAssemblies.Count} References");
            Console.ReadLine();
        }
    }
}
