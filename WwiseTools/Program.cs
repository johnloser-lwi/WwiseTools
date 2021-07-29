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

            WwiseSwitchContainer switchContainer = new WwiseSwitchContainer(WwiseUtility.GetWwiseObjectsByTypeAndParent("SwitchContainer", @"\Actor-Mixer Hierarchy\Default Work Unit").Where(obj => obj.Name == "TestSwitchContainer").First());

            Console.WriteLine(switchContainer.GetAssignments());

            WwiseUtility.Close().Wait();

            Console.ReadLine();
        }
    }
}
