using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Utils;
using System.IO;
using WwiseTools.Properties;
using WwiseTools.Reference;

namespace WwiseTools
{
    class Program
    {
        static void Main(string[] args)
        {
            var folder = WwiseUtility.CreateObject("TestFolder", WwiseObject.ObjectType.Folder, @"\Actor-Mixer Hierarchy\Default Work Unit");
            var rscontainer = WwiseUtility.CreateObject("TestRS", WwiseObject.ObjectType.RandomSequenceContainer, @"\Actor-Mixer Hierarchy\Default Work Unit");

            var attenuation = WwiseUtility.GetWwiseObjectByName("Attenuation:TestAttenuation");

            WwiseUtility.SetObjectReference(rscontainer, WwiseReference.Ref_Attenuation(attenuation));

            var conversion = WwiseUtility.GetWwiseObjectByName("Conversion:TestConversion");

            WwiseUtility.SetObjectReference(rscontainer, WwiseReference.Ref_Conversion(conversion));

            WwiseUtility.MoveToParent(rscontainer, folder);

            WwiseUtility.SetObjectProperty(rscontainer, WwiseProperty.Prop_EnableAttenuation(true));

            WwiseUtility.SetObjectProperty(rscontainer, WwiseProperty.Prop_3DPosition(WwiseProperty.Option_3DPosition.EmitterWithAutomation));

            WwiseUtility.ChangeObjectName(folder, "NewFolderName");

            Console.WriteLine(rscontainer.Path);

            WwiseUtility.CreatePlayEvent("TestEvent", rscontainer.Path);

            WwiseUtility.Close().Wait();

            Console.ReadLine();
        }
    }
}
