using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Utils;
using System.IO;

namespace WwiseTools
{
    class Program
    {
        static void Main(string[] args)
        { 
            var obj = WwiseUtility.ImportSound(@"D:\\BGM\\Login\\denglu_bpm120_4_4_1.wav", "SFX", "UI", "<Folder>TEST");
            Console.WriteLine(obj.ToString());

            WwiseUtility.Close().Wait();

            Console.ReadLine();
        }
    }
}
