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
            var folder = WwiseUtility.CreateObject("TestFolder", WwiseObject.ObjectType.Folder, @"\Actor-Mixer Hierarchy\Default Work Unit");
            var rscontainer = WwiseUtility.CreateObject("TestRS", WwiseObject.ObjectType.RandomSequenceContainer, @"\Actor-Mixer Hierarchy\Default Work Unit");
            WwiseUtility.MoveToParent(rscontainer, folder);
            Console.WriteLine(folder.ToString());
            Console.WriteLine(rscontainer.ToString());
            var obj = WwiseUtility.ImportSoundFromFolder(@"D:\\BGM\\Login", "SFX", "BGM", rscontainer.Path);

            WwiseUtility.Close().Wait();

            Console.ReadLine();
        }
    }
}
