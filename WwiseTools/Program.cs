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

            Console.WriteLine(switchContainer.ToString());
            
            WwiseSwitchGroup testSwitchGroup = new WwiseSwitchGroup (WwiseUtility.GetWwiseObjectByName("SwitchGroup:TestSwitchGroup"));

           

            switchContainer.SetSwitchGroupOrStateGroup(WwiseReference.Ref_SwitchGroupOrStateGroup(testSwitchGroup));
            switchContainer.SetDefaultSwitchOrState(WwiseReference.Ref_DefaultSwitchOrState(testSwitchGroup.GetSwitches()[0]));

            Console.WriteLine(switchContainer.GetAssignments());

            switchContainer.AssignChildToStateOrSwitch(WwiseUtility.GetWwiseObjectsByNameAndType("TestSound", "Sound")[0], testSwitchGroup.GetSwitches()[0]);

            WwiseSequenceContainer sc = new WwiseSequenceContainer("Test");
            Console.WriteLine(sc.GetPropertyAndReferenceNames());

            WwiseUtility.Close().Wait();

            Console.ReadLine();
        }
    }
}
