using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Utils;
using System.IO;
using WwiseTools.Properties;
using WwiseTools.Reference;
using WwiseTools.Objects;

namespace WwiseTools
{
    class Program
    {
        static void Main(string[] args)
        {
            WwiseFolder testFolder = new WwiseFolder("TestFolder");
            WwiseSequenceContainer testRandom = new WwiseSequenceContainer("TestRandom");
            testRandom.SetScope(WwiseProperty.Option_GlobalOrPerObject.GameObject);
            testRandom.SetAttenuation("TestAttenuation");
            Console.ReadLine();
        }
    }
}
