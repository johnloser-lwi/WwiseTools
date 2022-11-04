using System;
using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.WwiseTypes
{
    public class MusicPlaylistItem : WwiseTypeBase
    {

        public async Task SetLoopCountAsync(int count = -1)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, new WwiseProperty("LoopCount", count));
        }

        
        public async Task SetPlayModeAsync(WwiseProperty.Option_PlayMode playMode)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, new WwiseProperty("PlayMode", (int)playMode));
        }

        
        public async Task SetPlaylistItemTypeAsync(WwiseProperty.Option_PlaylistItemType type)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, new WwiseProperty("PlaylistItemType", (int)type));
        }

        public async Task<WwiseProperty.Option_PlaylistItemType> GetPlaylistItemTypeAsync()
        {
            var property = await WwiseUtility.Instance.GetWwiseObjectPropertyAsync(WwiseObject, "PlaylistItemType");

            int.TryParse(property.Value.ToString(), out int type);
            return (WwiseProperty.Option_PlaylistItemType)type;
        }


        public async Task SetRandomAsync(bool shuffle = true, uint avoidRepeatCount = 1)
        {
            int standard = 0;
            if (!shuffle) standard = 1;
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, new WwiseProperty("NormalOrShuffle", standard));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, new WwiseProperty("RandomAvoidRepeatingCount", avoidRepeatCount));
        }


        public async Task SetSegmentRefAsync(WwiseObject segment)
        {
            await WwiseUtility.Instance.SaveWwiseProjectAsync();
            WwiseWorkUnitParser parser = new WwiseWorkUnitParser(await WwiseUtility.Instance.GetWorkUnitFilePathAsync(WwiseObject));

            var node = parser.XML.CreateElement("SegmentRef");
            node.SetAttribute("Name", segment.Name);
            node.SetAttribute("ID", segment.ID);

            parser.AddToUnit(WwiseObject, node);
            parser.SaveFile();
        }


        public async Task SetWeightAsync(float weight)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, new WwiseProperty("Weight", weight));
        }

        public async Task<WwiseObject> AddChildGroupAsync()
        {
            if (await GetPlaylistItemTypeAsync() == WwiseProperty.Option_PlaylistItemType.Segment) return null;

            var playlistItem
                = await WwiseUtility.Instance.CreateObjectAtPathAsync("", 
                    WwiseObject.ObjectType.MusicPlaylistItem, 
                    WwiseObject.ID);
            await playlistItem.AsMusicPlaylistItem().SetPlaylistItemTypeAsync(WwiseProperty.Option_PlaylistItemType.Group);
            return playlistItem;
        }

        public async Task<WwiseObject> AddChildSegmentAsync(WwiseObject segment)
        {
            if (await GetPlaylistItemTypeAsync() == WwiseProperty.Option_PlaylistItemType.Segment) return null;

            var item = await WwiseUtility.Instance.CreateObjectAtPathAsync("", WwiseObject.ObjectType.MusicPlaylistItem, WwiseObject.ID);
            //var item = new WwiseMusicPlaylistItem(tempObj);
            await item.AsMusicPlaylistItem().SetPlaylistItemTypeAsync(WwiseProperty.Option_PlaylistItemType.Segment);
            await item.AsMusicPlaylistItem().SetSegmentRefAsync(segment);

            await WwiseUtility.Instance.ReloadWwiseProjectAsync();

            return item;
        }

        public MusicPlaylistItem(WwiseObject wwiseObject) : base(wwiseObject, nameof(MusicPlaylistItem))
        {
        }
    }
}
