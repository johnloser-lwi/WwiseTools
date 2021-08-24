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
            try
            {
                /*Console.WriteLine("0");
                WwiseMusicPlaylistContainer container = new WwiseMusicPlaylistContainer("testContainer");

                WwiseMusicSegment seg = new WwiseMusicSegment("TestSegment", container.Path);

                WwiseMusicTrack track = new WwiseMusicTrack("TestTrack", @"D:\BGM\Login\denglu_bpm120_4_4_1.wav", seg, "LoginBGM");
                Console.WriteLine("1");
                var subGroup = container.AddPlaylistItemGroup();
                
                subGroup.AddChildGroup().AddChildSegment(seg);
                subGroup.AddChildGroup();


                Console.WriteLine("2");*/

                //WwiseMusicSwitchContainer container = new WwiseMusicSwitchContainer("Test");
                //container.SetContinuePlay(false);

                WwiseSequenceContainer container = new WwiseSequenceContainer("Test");

                WwiseSound sound = new WwiseSound("TestSound");
                Console.WriteLine("Adding child");
                container.AddChild(sound);

                Console.WriteLine("Setting up playlist");
                container.SetPlaylist(sound);

                Console.WriteLine("Reloading...");
                WwiseUtility.ReloadWwiseProject();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error running! ======> {e.Message}");
            }

            Console.WriteLine("Closing ...");
            WwiseUtility.Close().Wait();

            Console.ReadLine();
        }
    }
}
