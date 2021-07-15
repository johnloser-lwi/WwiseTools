using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Utils;
using WwiseTools.Audio;
using WwiseTools.Basics;

namespace WwiseTools
{
    class Program
    {
        static void Main(string[] args)
        {
            WwiseUtility.Init(@"D:\UnityProject\project_WwiseProject", @"D:\", false);//初始化Wwise工程路径

            WwiseMusicPlaylistContainer container = WwiseUtility.GenerateMusicPlaylistFromFolder(@"TestTrapTrack", WwiseMusicPlaylistItem.PlaylistType.SequenceContinous, 0);
            container.SetTempoAndTimeSignature(140, 4, 4);
            var group = container.Playlist.AddGroup(WwiseMusicPlaylistItem.PlaylistType.RandomContinous, 1);
            WwiseMusicSegment track = (WwiseMusicSegment)container.Children.ChildNodes[0];
            group.AddSegment(new WwiseSegmentRef(track.name, track.id));

            Console.WriteLine("id {0}", container.FindSegmentByName("TrapTest").id);

            WwiseParser parser = new WwiseParser();
            parser.Parse(@"Interactive Music Hierarchy/New Work Unit.wwu");
            parser.AddChildToWorkUnit(container);
            //parser.CommitChange();

            WwiseParser eventParser = new WwiseParser();
            eventParser.Parse(@"Events/New Work Unit.wwu");
            var action = WwiseUtility.ToEvent("TrapMusic", container, parser.GetWorkUnit(), WwiseAction.ActionType.Play);
            eventParser.AddChildToWorkUnit(action);
            //eventParser.CommitChange();

            //Print()函数会将这个WorkUnit中的所有内容转换成字符串
            //Console.WriteLine(eventParser.GetSchemaVersion());
            //Console.WriteLine(eventParser.ToString());
            
            Console.ReadLine();
        }
    }
}
