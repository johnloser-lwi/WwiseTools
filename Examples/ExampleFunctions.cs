using System.Diagnostics;
using System.Xml;
using WwiseTools.WwiseTypes;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Utils;
using WwiseTools.Utils.Feature2021;
using WwiseTools.Utils.Feature2022;

namespace Examples
{
    internal class ExampleFunctions
    {
        static WwiseUtility Waapi = WwiseTools.Utils.WwiseUtility.Instance;

        public static void CustomLogger(object message)
        {
            string msg = DateTime.Now.ToString() + " => " + message.ToString();

            using (var writer = new StreamWriter("log", true))
            {
                writer.WriteLine(msg);
            }
        }

        public static async Task GetSourceLanguageAsync()
        {
            var selection = await Waapi.GetWwiseObjectsBySelectionAsync();
            foreach (var wwiseObject in selection)
            {
                var sound = wwiseObject.AsContainer();
                var sources = await sound.GetChildrenAsync();

                foreach (var source in sources)
                {
                    var language = await (new AudioFileSource(source)).GetLanguageAsync();
                    
                    Console.WriteLine(language);
                }
            }
        }

        public static async Task HierarchyCastAsync()
        {
            var selection = await Waapi.GetWwiseObjectsBySelectionAsync();
            foreach (var wwiseObject in selection)
            {
                wwiseObject.AsContainer();
            }
        }
        

        public static async Task GetReferencedEventsAsync()
        {
            var selection = await Waapi.GetWwiseObjectsBySelectionAsync();
            foreach (var wwiseObject in selection)
            {
                var references = await Waapi.GetEventsReferencingWwiseObjectAndParentsAsync(wwiseObject);
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
                var references = await Waapi.GetSoundBanksReferencingWwiseObjectAsync(wwiseObject);
                foreach (var reference in references)
                {
                    WaapiLog.Log(reference);
                }
            }
        }



        public static async Task BatchSetTestAsync()
        {
            var selection = await Waapi.GetWwiseObjectsBySelectionAsync();
            await WwiseUtility.Instance.BatchSetObjectPropertyAsync(selection.ToArray(), 
                WwiseProperty.Prop_Volume(-3),
                WwiseProperty.Prop_OverrideOutput(true),
                WwiseProperty.Prop_OutputBusVolume(-3));

            var bus = await Waapi.GetWwiseObjectByNameAsync("Bus:Test");
            if (bus == null) return;
            await WwiseUtility.Instance.BatchSetObjectPropertyAsync(selection.ToArray(), 
                WwiseProperty.Prop_OutputBus(bus));
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

            foreach (var child in await obj.AsContainer().GetChildrenAsync())
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
