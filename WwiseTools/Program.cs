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
            //var folder = WwiseUtility.CreateObject("TestFolder", WwiseObject.ObjectType.Folder);

            WwiseSound sound = new WwiseSound("TestSound");
            sound.SetAuxilaryBus0("TestAux", -9, 5, 5);
            sound.SetOutputBus("TestBus", -9, 10, 10);
            sound.SetEarlyReflections("TestReflectionBus", -9);
            sound.SetPitch(-15);
            sound.SetVolume(-10);
            sound.SetFilter(10, 10);
            sound.SetStream(true, true, true);
            sound.SetInitialDelay(0.5f);
            sound.SetLoop(true, false, 5);
            sound.SetConversion("TestConversion");

            Console.ReadLine();
        }
    }
}
