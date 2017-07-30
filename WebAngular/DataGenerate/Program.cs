using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenerate.Generator;

namespace DataGenerate
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lCity = new List<string>();
            List<string> lStreet = new List<string>();

            GeneratorData gd = new GeneratorData();

                Console.WriteLine("{0}", gd.getRecords(3));
            Console.Read();
        }
    }
}
