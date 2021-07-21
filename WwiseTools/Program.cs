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

            WwiseUtility.SetObjectProperty(rscontainer, "Volume", -9);

            WwiseUtility.MoveToParent(rscontainer, folder);

            WwiseUtility.ChangeObjectName(folder, "NewFolderName");
            

            Console.WriteLine(rscontainer.Path);

            WwiseUtility.CreatePlayEvent("TestEvent", rscontainer.Path);

            //var obj = WwiseUtility.ImportSoundFromFolder(@"D:\\BGM\\Login", "SFX", "BGM", rscontainer.Path);

            WwiseUtility.Close().Wait();

            Console.ReadLine();
        }
    }
}
