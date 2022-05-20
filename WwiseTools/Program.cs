using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Utils;
using System.IO;
using System.Xml;
using WwiseTools.Properties;
using WwiseTools.Reference;
using WwiseTools.Objects;

namespace WwiseTools
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var selection = await WwiseUtility.GetWwiseObjectsBySelectionAsync();

            foreach (var wwiseObject in selection)
            {
                Console.WriteLine(wwiseObject.Name);
            }

            var ids = selection.Select(x => x.ID).ToArray();

            await WwiseUtility.ExecuteUICommand("ShowSchematicView", ids);

            await WwiseUtility.DisconnectAsync();

            Console.WriteLine("Press Any Key to Exit ...");
            Console.ReadLine();
        }
    }
}
