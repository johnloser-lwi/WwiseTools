using System.Collections.Generic;
using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.WwiseTypes
{
    public class RandomSequenceContainer : WwiseTypeBase
    {
        // RandomSequenceContainer

        public async Task<WwiseProperty.Option_RandomOrSequence> GetPlayTypeAsync()
        {
            var result = (await WwiseUtility.Instance.GetWwiseObjectPropertyAsync(WwiseObject, "RandomOrSequence")).Value.ToString();

            return (WwiseProperty.Option_RandomOrSequence)int.Parse(result);
        }
        public async Task SetScopeAsync(WwiseProperty.Option_GlobalOrPerObject option = WwiseProperty.Option_GlobalOrPerObject.Global)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_GlobalOrPerObject(option));
        }
        public async Task SetPlayModeAsync(WwiseProperty.Option_PlayMechanismStepOrContinuous mode)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_PlayMechanismStepOrContinuous(mode));
        }


        public async Task SetLoopAsync(bool loop, uint loopCount, WwiseProperty.Option_PlayMechanismInfiniteOrNumberOfLoops mode)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_PlayMechanismLoop(loop));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_PlayMechanismInfiniteOrNumberOfLoops(mode));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_PlayMechanismLoopCount(loopCount));

        }

        public async Task SetTransitionsAsync(bool transitions, WwiseProperty.Option_PlayMechanismSpecialTransitionsType type, float duration)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_PlayMechanismSpecialTransitions(transitions));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_PlayMechanismSpecialTransitionsType(type));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_PlayMechanismSpecialTransitionsValue(duration));
        }


        // Sequence Container


        public async Task SetSequenceEndBehaviorAsync(WwiseProperty.Option_RestartBeginningOrBackward option)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_RestartBeginningOrBackward(option));
        }


        public async Task SetAlwaysResetPlaylistAsync(bool reset)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_PlayMechanismResetPlaylistEachPlay(reset));
        }


        public async Task SetPlaylistAsync(List<WwiseObject> items, bool autoReload = false)
        {
            foreach (var item in items)
            {
                if (!(await item.GetPathAsync()).Contains(await WwiseObject.GetPathAsync())) return;
            }


            await WwiseUtility.Instance.SaveWwiseProjectAsync();
            WwiseWorkUnitParser parser = new WwiseWorkUnitParser(await WwiseUtility.Instance.GetWorkUnitFilePathAsync((WwiseObject)));

            var xpath = "//*[@ID='" + WwiseObject.ID + "']/Playlist";
            var playlistNode = parser.XML.SelectSingleNode(xpath);


            var containerNode = parser.GetNodeByID(WwiseObject.ID);

            if (playlistNode != null)
            {

                containerNode.RemoveChild(playlistNode);
                //parser.SaveFile();
            }


            var newPlaylist = parser.XML.CreateElement("Playlist");


            foreach (var item in items)
            {
                var node = parser.XML.CreateElement("ItemRef");
                node.SetAttribute("Name", item.Name);
                node.SetAttribute("ID", item.ID);
                newPlaylist.AppendChild(node);
            }

            containerNode.AppendChild(parser.XML.ImportNode(newPlaylist, true));

            parser.SaveFile();

            if (autoReload) await WwiseUtility.Instance.ReloadWwiseProjectAsync();

        }


        // Random Container

        public async Task SetRandomModeAsync(WwiseProperty.Option_NormalOrShuffle option)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_NormalOrShuffle(option));
        }

        public RandomSequenceContainer(WwiseObject wwiseObject) : base(wwiseObject, nameof(RandomSequenceContainer))
        {
        }
    }
}
