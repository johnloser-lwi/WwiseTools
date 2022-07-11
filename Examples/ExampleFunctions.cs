using System.Xml;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.References;
using WwiseTools.Utils;
using WwiseTools.Utils.Feature2021;
using WwiseTools.Utils.Feature2022;

namespace Examples
{
    internal class ExampleFunctions
    {
        static WwiseUtility Waapi = WwiseTools.Utils.WwiseUtility.Instance;

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

        public static async Task GetReferencedEventsAsync()
        {
            var selection = await Waapi.GetWwiseObjectsBySelectionAsync();
            foreach (var wwiseObject in selection)
            {
                var references = await Waapi.GetEventReferencesToWwiseObjectAndParentsAsync(wwiseObject);
                foreach (var reference in references)
                {
                    WaapiLog.Log(reference);
                }
            }
        }

        public static async Task GetReferencedSoundBanksAsync()
        {
            var selection = await Waapi.GetWwiseObjectsBySelectionAsync();
            foreach (var wwiseObject in selection)
            {
                var references = await Waapi.GetSoundBankReferencesToWwiseObjectAsync(wwiseObject);
                foreach (var reference in references)
                {
                    WaapiLog.Log(reference);
                }
            }
        }



        public static async Task BatchSetTestAsync()
        {
            var selection = await Waapi.GetWwiseObjectsBySelectionAsync();
            await WwiseUtility.Instance.BatchSetObjectPropertyAsync(selection, 
                WwiseProperty.Prop_Volume(-3),
                WwiseProperty.Prop_OverrideOutput(true),
                WwiseProperty.Prop_OutputBusVolume(-3));

            var bus = await Waapi.GetWwiseObjectByNameAsync("Bus:Test");
            if (bus == null) return;
            await WwiseUtility.Instance.BatchSetObjectReferenceAsync(selection, 
                WwiseReference.Ref_OutputBus(bus));
        }


        public static async Task WaqlTestAsync()
        {
            Waql query = new Waql("where type = \"Sound\"");

            if (await query.RunAsync())
            {
                foreach (var wwieObject in query)
                {
                    WaapiLog.Log(wwieObject.Name);
                }
            }

            foreach (var wwieObject in await WwiseUtility.Instance.Waql("where type = \"RandomSequenceContainer\""))
            {
                WaapiLog.Log(wwieObject.Name);
            }


        }

        public static async Task ParserTestAsync()
        {
            var selection = await Waapi.GetWwiseObjectsBySelectionAsync();

            var obj = selection[0];

            var workUnitPath = await Waapi.GetWorkUnitFilePathAsync(obj);

            WwiseWorkUnitParser parser = new WwiseWorkUnitParser(workUnitPath);
            var objNode = parser.GetNodeByID(obj.ID);

            foreach (XmlElement node in parser.GetChildrenNodeList(objNode))
            {
                WaapiLog.Log(node.Name);
            }

            foreach (var child in await obj.GetHierarchy().GetChildrenAsync())
            {
                WaapiLog.Log(child.Name);
            }

            foreach (var sound in await Waapi.GetWwiseObjectsOfTypeAsync("Sound"))
            {
                WaapiLog.Log(sound.ID);
            }
        }
    }
}
