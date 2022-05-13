using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Reference;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public class WwiseMusicPlaylistItem : WwiseObject
    {
        public Option_PlaylistItemType PlaylistItemType { get; private set; }

        [Obsolete("use WwiseUtility.CreateObjectAsync instead")]
        public WwiseMusicPlaylistItem(Option_PlaylistItemType playlist_item_type, string parent_id) : base("", "", "MusicPlaylistItem")
        {
            var tempObj = WwiseUtility.CreateObject("", ObjectType.MusicPlaylistItem, parent_id);
            ID = tempObj.ID;
            Name = tempObj.Name;

            SetPlaylistItemType(playlist_item_type);
        }

        public static async Task<WwiseMusicPlaylistItem> CreateWwiseMusicPlaylistItem(
            Option_PlaylistItemType playlist_item_type, string parent_id)
        {
            var tempObj = await WwiseUtility.CreateObjectAsync("", ObjectType.MusicPlaylistItem, parent_id);
            var playlistItem = new WwiseMusicPlaylistItem(tempObj);
            await playlistItem.SetPlaylistItemTypeAsync(playlist_item_type);
            return playlistItem;
        }
        
        
        public static async Task<WwiseMusicPlaylistItem> CreateWwiseMusicPlaylistItem(
            WwiseMusicSegment segment, string parent_id)
        {
            var tempObj = await WwiseUtility.CreateObjectAsync("", ObjectType.MusicPlaylistItem, parent_id);
            var playlistItem = new WwiseMusicPlaylistItem(tempObj);
            await playlistItem.SetPlaylistItemTypeAsync(Option_PlaylistItemType.Segment);
            await playlistItem.SetSegmentRefAsync(segment);
            return playlistItem;
        }

        [Obsolete("use WwiseUtility.CreateObjectAsync instead")]
        public WwiseMusicPlaylistItem(WwiseMusicSegment segment, string parent_id) : base("", "", "MusicPlaylistItem")
        {
            var tempObj = WwiseUtility.CreateObject("", ObjectType.MusicPlaylistItem, parent_id);
            ID = tempObj.ID;
            Name = tempObj.Name;

            SetPlaylistItemType(Option_PlaylistItemType.Segment);
            SetSegmentRef(segment);
        }
        

        public WwiseMusicPlaylistItem(WwiseObject @object) : base("", "", "")
        {
            if (@object == null) return;
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;
        }


        /// <summary>
        /// 设置循环数
        /// </summary>
        /// <param name="count"></param>
        public void SetLoopCount(int count = -1)
        {
            WwiseUtility.SetObjectProperty(this, new WwiseProperty("LoopCount", count));
        }

        public enum Option_PlayMode { SequenceContinuous = 0, SequenceStep = 1, RandomContinuous = 2, RandomStep = 3 }
        [Obsolete("use async version instead")]
        public void SetPlayMode(Option_PlayMode play_mode)
        {
            WwiseUtility.SetObjectProperty(this, new WwiseProperty("PlayMode", (int)play_mode));
        }

        public async Task SetPlayModeAsync(Option_PlayMode play_mode)
        {
            await WwiseUtility.SetObjectPropertyAsync(this, new WwiseProperty("PlayMode", (int)play_mode));
        }

        public enum Option_PlaylistItemType { Group = 0, Segment = 1 }
        [Obsolete("use async version instead")]
        public void SetPlaylistItemType(Option_PlaylistItemType type)
        {
            PlaylistItemType = type;
            WwiseUtility.SetObjectProperty(this, new WwiseProperty("PlaylistItemType", (int)type));
        }

        public async Task SetPlaylistItemTypeAsync(Option_PlaylistItemType type)
        {
            PlaylistItemType = type;
            await WwiseUtility.SetObjectPropertyAsync(this, new WwiseProperty("PlaylistItemType", (int)type));
        }

        /// <summary>
        /// 设置随机
        /// </summary>
        /// <param name="shuffle"></param>
        /// <param name="avoid_repeat_count"></param>
        [Obsolete("use async version instead")]
        public void SetRandom(bool shuffle = true, uint avoid_repeat_count = 1)
        {
            int standard = 0;
            if (!shuffle) standard = 1;
            WwiseUtility.SetObjectProperty(this, new WwiseProperty("NormalOrShuffle", standard));

            WwiseUtility.SetObjectProperty(this, new WwiseProperty("RandomAvoidRepeatingCount", avoid_repeat_count));
        }

        public async Task SetRandomAsync(bool shuffle = true, uint avoid_repeat_count = 1)
        {
            int standard = 0;
            if (!shuffle) standard = 1;
            await WwiseUtility.SetObjectPropertyAsync(this, new WwiseProperty("NormalOrShuffle", standard));
            await WwiseUtility.SetObjectPropertyAsync(this, new WwiseProperty("RandomAvoidRepeatingCount", avoid_repeat_count));
        }

        /// <summary>
        /// 设置片段引用
        /// </summary>
        /// <param name="segment"></param>
        [Obsolete("Use async version instead")]
        public void SetSegmentRef(WwiseMusicSegment segment)
        {
            WwiseUtility.SaveWwiseProject();
            WwiseWorkUnitParser parser = new WwiseWorkUnitParser(WwiseUtility.GetWorkUnitFilePath(this));

            var node = parser.XML.CreateElement("SegmentRef");
            node.SetAttribute("Name", segment.Name);
            node.SetAttribute("ID", segment.ID);

            parser.AddToUnit(this, node);
            parser.SaveFile();
        }
        
        public async Task SetSegmentRefAsync(WwiseMusicSegment segment)
        {
            await WwiseUtility.SaveWwiseProjectAsync();
            WwiseWorkUnitParser parser = new WwiseWorkUnitParser( await  WwiseUtility.GetWorkUnitFilePathAsync(this));

            var node = parser.XML.CreateElement("SegmentRef");
            node.SetAttribute("Name", segment.Name);
            node.SetAttribute("ID", segment.ID);

            parser.AddToUnit(this, node);
            parser.SaveFile();
        }

        /// <summary>
        /// 设置权重
        /// </summary>
        /// <param name="weight"></param>
        [Obsolete("use async version instead")]
        public void SetWeight(float weight)
        {
            WwiseUtility.SetObjectProperty(this, new WwiseProperty("Weight", weight));
        }

        public async Task SetWeightAsync(float weight)
        {
            await WwiseUtility.SetObjectPropertyAsync(this, new WwiseProperty("Weight", weight));
        }


        /// <summary>
        /// 增加组
        /// </summary>
        /// <returns></returns>
        [Obsolete("use async version instead")]
        public WwiseMusicPlaylistItem AddChildGroup()
        {
            if (PlaylistItemType == Option_PlaylistItemType.Segment) return null;

            var tempObj = WwiseUtility.CreateObject("", ObjectType.MusicPlaylistItem, ID);
            var item = new WwiseMusicPlaylistItem(tempObj);
            item.SetPlaylistItemType(Option_PlaylistItemType.Group);
            return item;
        }

        public async Task<WwiseMusicPlaylistItem> AddChildGroupAsync()
        {
            if (PlaylistItemType == Option_PlaylistItemType.Segment) return null;

            var tempObj = await WwiseUtility.CreateObjectAsync("", ObjectType.MusicPlaylistItem, ID);
            var item = new WwiseMusicPlaylistItem(tempObj);
            await item.SetPlaylistItemTypeAsync(Option_PlaylistItemType.Group);
            return item;
        }

        /// <summary>
        /// 增加片段
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        [Obsolete("use async version instead")]
        public WwiseMusicPlaylistItem AddChildSegment(WwiseMusicSegment segment)
        {
            if (PlaylistItemType == Option_PlaylistItemType.Segment) return null;

            var tempObj = WwiseUtility.CreateObject("", ObjectType.MusicPlaylistItem, ID);
            var item = new WwiseMusicPlaylistItem(tempObj);
            item.SetPlaylistItemType(Option_PlaylistItemType.Segment);
            item.SetSegmentRef(segment);

            WwiseUtility.ReloadWwiseProject();

            return item;
        }

        public async Task<WwiseMusicPlaylistItem> AddChildSegmentAsync(WwiseMusicSegment segment)
        {
            if (PlaylistItemType == Option_PlaylistItemType.Segment) return null;

            var tempObj = await WwiseUtility.CreateObjectAsync("", ObjectType.MusicPlaylistItem, ID);
            var item = new WwiseMusicPlaylistItem(tempObj);
            await item.SetPlaylistItemTypeAsync(Option_PlaylistItemType.Segment);
            await item.SetSegmentRefAsync(segment);

            await WwiseUtility.ReloadWwiseProjectAsync();

            return item;
        }
    }
}
