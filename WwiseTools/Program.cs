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
            

            Console.WriteLine("Closing ...");
            WwiseUtility.Close().Wait();
        }

        static void Adjust2D3DVolume()
        {
            Console.WriteLine("Set Volume:");
            var volume = Console.ReadLine();

            try
            {
                var newVolume = float.Parse(volume);

                var li = WwiseUtility.GetWwiseObjectsBySelection();
                if (li != null)
                {
                    foreach (var wo in li)
                    {
                        if (wo == null) continue;
                        (new WwiseActorMixer(wo)).SetVolume(newVolume);
                        string counterPartName = wo.Path;
                        if (counterPartName.Contains("Character 2D"))
                        {
                            counterPartName = counterPartName.Replace("Character 2D", "Character 3D");
                        }
                        else if (counterPartName.Contains("Character 3D"))
                        {
                            counterPartName = counterPartName.Replace("Character 3D", "Character 2D");
                        }
                        (new WwiseActorMixer(WwiseUtility.GetWwiseObjectByPath(counterPartName))).SetVolume(newVolume);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error running! ======> {e.Message}");
            }
        }
    }
}
