using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
using WwiseTools;
using WwiseTools.Utils;

namespace TestEnv
{
    class Program
    {
        static void Main(string[] args)
        {
            WwiseTools.Utils.WwiseUtility.Init(@"C:\Users\Loser\Desktop\TestWwise", @"C:\", false);//初始化Wwise工程路径
            WwiseTools.Utils.WwiseParser parser = new WwiseTools.Utils.WwiseParser();
            parser.Parse(@"Actor-Mixer Hierarchy\New Work Unit.wwu");
            WwiseNodeWithName node = new WwiseNodeWithName("Folder", "TestFolder", parser);
            WwiseFolder folder = new WwiseFolder("TestFolder", parser);

            WwiseSwitchContainer c = new WwiseSwitchContainer("Test", "RandomSequenceContainer", parser);
            c.AddChild(new WwiseUnit("testchild", "Sound", parser));

            folder.AddChild(c);

            parser.AddChildToWorkUnit(folder);

            //parser.ToFile("test.xml");

            parser.ToFile("test.xml");

            Console.ReadLine();
        }
    }
}
