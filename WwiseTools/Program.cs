using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Utils;
using System.IO;
using WwiseTools.Properties;

namespace WwiseTools
{
    class Program
    {
        static void Main(string[] args)
        {
            var folder = WwiseUtility.CreateObject("TestFolder", WwiseObject.ObjectType.Folder, @"\Actor-Mixer Hierarchy\Default Work Unit");
            var rscontainer = WwiseUtility.CreateObject("TestRS", WwiseObject.ObjectType.RandomSequenceContainer, @"\Actor-Mixer Hierarchy\Default Work Unit");

            var att = WwiseUtility.GetWwiseObjectByID("{8C2E78FC-4DC6-4643-A7E5-001309D94932}");
            att.Wait();
            WwiseObject attenuation = att.Result;

            WwiseUtility.SetObjectReference(rscontainer, new Reference.WwiseReference("Attenuation", attenuation));

            WwiseUtility.MoveToParent(rscontainer, folder);

            WwiseUtility.SetObjectProperty(rscontainer, WwiseSoundProperties.P_EnableAttenuation(false));

            WwiseUtility.ChangeObjectName(folder, "NewFolderName");
            

            Console.WriteLine(rscontainer.Path);

            WwiseUtility.CreatePlayEvent("TestEvent", rscontainer.Path);

            //var obj = WwiseUtility.ImportSoundFromFolder(@"D:\\BGM\\Login", "SFX", "BGM", rscontainer.Path);

            WwiseUtility.Close().Wait();

            Console.ReadLine();
        }
    }
}
