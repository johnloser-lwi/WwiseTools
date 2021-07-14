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
            //WwiseTools.Utils.WwiseUtility.Init(@"D:\UnityProject\project_WwiseProject", @"D:\", false);//初始化Wwise工程路径
            //WwiseTools.Utils.WwiseParser parser = new WwiseTools.Utils.WwiseParser();
            //parser.Parse(@"Interactive Music Hierarchy/New Work Unit.wwu");
            //WwiseNodeWithName node = new WwiseNodeWithName("Folder", "TestFolder", parser);
            //WwiseFolder folder = new WwiseFolder("TestFolder", parser);

            //WwiseContainer c = new WwiseContainer("Test", "RandomSequenceContainer", parser);

            //folder.AddChild(c);

            //parser.AddChildToWorkUnit(folder);

            //parser.ToFile("test.xml");

            WwiseWorkUnit wu = new WwiseWorkUnit("Test", "Events");
            var parser = WwiseParser.CreateParserFromNode(wu);

            parser.ToFile("test.xml");

            Console.ReadLine();
        }
    }
}
