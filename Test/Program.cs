using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WwiseTools.Objects;
using WwiseTools.Utils;

namespace Test
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await ParserTestAsync();
        }

        static async Task ParserTestAsync()
        {
            var selection = await WwiseUtility.GetWwiseObjectsBySelectionAsync();

            var obj = new WwiseActorMixer(selection[0]);

            var workUnitPath = await WwiseUtility.GetWorkUnitFilePathAsync(obj);

            WwiseWorkUnitParser parser = new WwiseWorkUnitParser(workUnitPath);
            var objNode = parser.GetNodeByID(obj.ID);

            foreach (XmlElement node in parser.GetChildrenNodeList(objNode))
            {
                Console.WriteLine(node.Name);
            }

            foreach (var child in await obj.GetChildrenAsync())
            {
                Console.WriteLine(child.Name);
            }

            foreach (var sound in await WwiseUtility.GetWwiseObjectsOfTypeAsync("Sound"))
            {
                Console.WriteLine(sound.ID);
            }

            await WwiseUtility.DisconnectAsync();

            Console.ReadLine();
        }
    }
}
