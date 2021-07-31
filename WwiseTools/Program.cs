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

            WwiseMusicSegment seg = new WwiseMusicSegment("TestSegment");

            WwiseMusicTrack track = new WwiseMusicTrack("TestTrack", @"D:\BGM\Login\denglu_bpm120_4_4_1.wav", seg, "LoginBGM");
            WwiseMusicTrack track2 = new WwiseMusicTrack("track2", seg);


            seg.SetTempoAndTimeSignature(132, WwiseProperty.Option_TimeSignatureLower._32, 4);

            WwiseUtility.Close().Wait();

            Console.ReadLine();
        }
    }
}
