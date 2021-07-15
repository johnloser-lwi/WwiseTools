using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Utils;
using WwiseTools.Audio;
using WwiseTools.Basics;
using WwiseTools.Reference;

namespace WwiseTools
{
    class Program
    {
        static void Main(string[] args)
        {
            WwiseTools.Utils.WwiseUtility.Init(@"D:\UnityProject\project_WwiseProject", @"C:\", false);//初始化Wwise工程路径
            WwiseTools.Utils.WwiseParser parser = new WwiseTools.Utils.WwiseParser();
            parser.Parse(@"Events\New Work Unit.wwu");
            //WwiseNodeWithName node = new WwiseNodeWithName("Folder", "TestFolder", parser);

            WwiseEvent ev = new WwiseEvent("PlayShit", parser);
            WwiseSound sound = new WwiseSound("Shit", "SFX", "test.wav", parser);
            WwiseObjectRef reference = new WwiseObjectRef("Shit", sound.ID, parser);
            ev.AddChild
                (
                    new WwiseAction(WwiseAction.ActionType.Play, reference, parser)
                );

            parser.AddChildToWorkUnit(ev);

            parser.ToFile("test.xml");

            Console.ReadLine();
        }
    }
}
