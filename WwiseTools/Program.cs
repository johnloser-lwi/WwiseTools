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
            //var folder = WwiseUtility.CreateObject("TestFolder", WwiseObject.ObjectType.Folder);
            
            var rscontainer = WwiseUtility.CreateObject("TestRS", WwiseObject.ObjectType.RandomSequenceContainer);
            var sound = WwiseUtility.CreateObject("TestSound", WwiseObject.ObjectType.Sound, rscontainer.Path);

            WwiseUtility.SetObjectProperty(rscontainer, WwiseProperty.Prop_RandomOrSequence(WwiseProperty.Option_RandomOrSequence.Sequence));

            WwiseUtility.CreatePlayEvent("TestEvent", rscontainer);

            WwiseUtility.Close().Wait();

            Console.ReadLine();
        }
    }
}
