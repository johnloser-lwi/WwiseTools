using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Utils;

namespace WwiseTools
{
    class Program
    {
        static void Main(string[] args)
        {
            WwiseUtility.Init(@"../../../TestProject", @"C:\", false).Wait();
            WwiseUtility.ImportFile("D:\\UI_Cancel.wav", "Chinese(CN)").Wait();

            Console.ReadLine();
        }
    }
}
