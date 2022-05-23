using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WwiseTools.Objects;
using WwiseTools.Utils;

namespace Examples
{
    internal class ExampleFunctions
    {
        public static void CustomLogger(object message, bool firstLog)
        {
            string msg = DateTime.Now.ToString() + " => " + message.ToString();


            if (firstLog)
                msg = "\n\n\n\n" + $"Session Started On {DateTime.Now.ToString()}>\n" +
                      msg;

            using (var writer = new StreamWriter("log", true))
            {
                writer.WriteLine(msg);
            }
        }


        public static async Task WaqlTestAsync()
        {
            await WwiseUtility.ConnectAsync();
            Waql query = new Waql("where type = \"Sound\"");

            if (await query.RunAsync())
            {
                foreach (var wwieObject in query)
                {
                    WaapiLog.Log(wwieObject.Name);
                }
            }

            if (await query.RunAsync("where type = \"RandomSequenceContainer\""))
            {
                foreach (var wwieObject in query)
                {
                    WaapiLog.Log(wwieObject.Name);
                }
            }


        }

        public static async Task ParserTestAsync()
        {
            var selection = await WwiseUtility.GetWwiseObjectsBySelectionAsync();

            var obj = new WwiseActorMixer(selection[0]);

            var workUnitPath = await WwiseUtility.GetWorkUnitFilePathAsync(obj);

            WwiseWorkUnitParser parser = new WwiseWorkUnitParser(workUnitPath);
            var objNode = parser.GetNodeByID(obj.ID);

            foreach (XmlElement node in parser.GetChildrenNodeList(objNode))
            {
                WaapiLog.Log(node.Name);
            }

            foreach (var child in await obj.GetChildrenAsync())
            {
                WaapiLog.Log(child.Name);
            }

            foreach (var sound in await WwiseUtility.GetWwiseObjectsOfTypeAsync("Sound"))
            {
                WaapiLog.Log(sound.ID);
            }
        }
    }
}
