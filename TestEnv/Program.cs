using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
using WwiseTools;
using WwiseTools.Utils;
using WwiseTools.Audio;

namespace TestEnv
{
    class Program
    {
        static void Main(string[] args)
        {
            WwiseTools.Utils.WwiseUtility.Init(@"D:\UnityProject\project_WwiseProject", @"C:\", false);//初始化Wwise工程路径
            WwiseTools.Utils.WwiseParser parser = new WwiseTools.Utils.WwiseParser();
            parser.Parse(@"Interactive Music Hierarchy\New Work Unit.wwu");
            //WwiseNodeWithName node = new WwiseNodeWithName("Folder", "TestFolder", parser);
            WwiseFolder folder = new WwiseFolder("TestFolder", parser);

            WwiseSwitchContainer c = new WwiseSwitchContainer("TestContainer", parser);
            c.AddChild(new WwiseSound("TestSound", "SFX", "xx.wav", parser));

            folder.AddChild(c);

            WwiseMusicSegment segment = new WwiseMusicSegment("Test", parser);
            segment.AddTrack("Test", "dd.wav", WwiseMusicTrack.TrackType.RandomStep);

            parser.AddChildToWorkUnit(segment);
            //parser.AddChildToWorkUnit(folder);

            //parser.ToFile("test.xml");

            parser.ToFile("test.xml");

            Console.ReadLine();
        }
    }
}
