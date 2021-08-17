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

            /*
            Console.WriteLine("0");
            WwiseMusicPlaylistContainer container = new WwiseMusicPlaylistContainer("testContainer");

            WwiseMusicSegment seg = new WwiseMusicSegment("TestSegment", container.Path);

            WwiseMusicTrack track = new WwiseMusicTrack("TestTrack", @"D:\BGM\Login\denglu_bpm120_4_4_1.wav", seg, "LoginBGM");
            */
            //Console.WriteLine("1");
            //container.AddPlaylistItem(WwiseMusicPlaylistItem.Option_PlaylistItemType.Group, null);
            WwiseMusicPlaylistContainer container = new WwiseMusicPlaylistContainer("testContainer");
            Console.WriteLine(WwiseUtility.GetPropertyAndReferenceNames(container));

            WwiseUtility.Close().Wait();

            Console.ReadLine();
        }
    }
}
