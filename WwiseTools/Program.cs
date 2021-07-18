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
            Task<WwiseObject> obj = WwiseUtility.ImportSound(@"D:\\UI_Cancel.wav");//, "SFX", "UI", "<Folder>TestFolder\\<RandomSequenceContainer>TestContainer");
            obj.Wait();
            Console.WriteLine(obj.Result);

            Console.ReadLine();
        }
    }
}
