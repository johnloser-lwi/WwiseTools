using System.Diagnostics.SymbolStore;
using System.Threading.Tasks;
using WwiseTools.Components;
using WwiseTools.Objects;
using WwiseTools.Properties;

namespace WwiseTools.Utils
{
    public class WwiseFactory
    {
        // Actor Mixer Hierarchy
        public static async Task<WwiseObject> CreateRandomSequenceContainer(string objectName, bool isRandomContainer,
            WwiseObject parent)
        {
            var result = await WwiseUtility.Instance.CreateObjectAsync(objectName, WwiseObject.ObjectType.RandomSequenceContainer,
                parent);

            if (result == null) return result;

            WwiseProperty.Option_RandomOrSequence type = isRandomContainer
                ? WwiseProperty.Option_RandomOrSequence.Random
                : WwiseProperty.Option_RandomOrSequence.Sequence;

            await WwiseUtility.Instance.SetObjectPropertyAsync(result, WwiseProperty.Prop_RandomOrSequence(type));
            return result;
        }

        // Interactive Music
        public static async Task<WwiseObject> CreateMusicSegmentAsync(string name, string filePath,
            string subFolder, WwiseObject parent)
        {
            var segment = await WwiseUtility.Instance.CreateObjectAsync(name, WwiseObject.ObjectType.MusicSegment, parent);
            var parentPath = System.IO.Path.Combine(await parent.GetPathAsync(), name);
            await WwiseUtility.Instance.ImportSoundAsync(filePath, "SFX", subFolder, parentPath);

            return segment;
        }

        public static async Task<WwiseObject> CreateWwiseMusicPlaylistItem(
            WwiseProperty.Option_PlaylistItemType playlistItemType, WwiseObject parentPlaylistItem)
        {
            var playlistItem = await WwiseUtility.Instance.CreateObjectAsync("", 
                WwiseObject.ObjectType.MusicPlaylistItem, 
                parentPlaylistItem);

            await playlistItem.GetMusicPlaylistItem().SetPlaylistItemTypeAsync(playlistItemType);
            return playlistItem;
        }


        public static async Task<WwiseObject> CreateWwiseMusicPlaylistItem(
            WwiseObject segment, WwiseObject parentPlaylistItem)
        {
            var playlistItem = await WwiseUtility.Instance.CreateObjectAsync("", WwiseObject.ObjectType.MusicPlaylistItem, parentPlaylistItem);
            await playlistItem.GetMusicPlaylistItem().SetPlaylistItemTypeAsync(WwiseProperty.Option_PlaylistItemType.Segment);
            await playlistItem.GetMusicPlaylistItem().SetSegmentRefAsync(segment);
            return playlistItem;
        }
    }
}
