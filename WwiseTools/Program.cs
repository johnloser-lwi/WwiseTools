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

            WwiseMusicTrack track = new WwiseMusicTrack("Test", @"D:\BGM\Login\denglu_bpm120_4_4_1.wav", "LoginBGM");

            WwiseUtility.Close().Wait();

            Console.ReadLine();
        }
    }
}
