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
    class WwiseMusicPlaylistItem : WwiseObject
    {
        public Option_PlaylistItemType PlaylistItemType { get; private set; }

        public WwiseMusicPlaylistItem(Option_PlaylistItemType playlist_item_type, string parent_id) : base("", "", "MusicPlaylistItem")
        {
            var tempObj = WwiseUtility.CreateObject("", ObjectType.MusicPlaylistItem, parent_id);
            ID = tempObj.ID;
            Name = tempObj.Name;

            SetPlaylistItemType(playlist_item_type);
        }

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

        public void SetLoopCount(int count = -1)
        {
            WwiseUtility.SetObjectProperty(this, new WwiseProperty("LoopCount", count));
        }

        public enum Option_PlayMode { SequenceContinuous = 0, SequenceStep = 1, RandomContinuous = 2, RandomStep = 3 }
        public void SetPlayMode(Option_PlayMode play_mode)
        {
            WwiseUtility.SetObjectProperty(this, new WwiseProperty("PlayMode", (int)play_mode));
        }

        public enum Option_PlaylistItemType { Group = 0, Segment = 1 }
        public void SetPlaylistItemType(Option_PlaylistItemType type)
        {
            PlaylistItemType = type;
            WwiseUtility.SetObjectProperty(this, new WwiseProperty("PlaylistItemType", (int)type));
        }

        public void SetRandom(bool shuffle = true, uint avoid_repeat_count = 1)
        {
            int standard = 0;
            if (!shuffle) standard = 1;
            WwiseUtility.SetObjectProperty(this, new WwiseProperty("NormalOrShuffle", standard));

            WwiseUtility.SetObjectProperty(this, new WwiseProperty("RandomAvoidRepeatingCount", avoid_repeat_count));
        }

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

        public void SetWeight(float weight)
        {
            WwiseUtility.SetObjectProperty(this, new WwiseProperty("Weight", weight));
        }

        public WwiseMusicPlaylistItem AddChildGroup()
        {
            if (PlaylistItemType == Option_PlaylistItemType.Segment) return null;

            var tempObj = WwiseUtility.CreateObject("", ObjectType.MusicPlaylistItem, ID);
            var item = new WwiseMusicPlaylistItem(tempObj);
            item.SetPlaylistItemType(Option_PlaylistItemType.Group);
            return item;
        }

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
    }
}
