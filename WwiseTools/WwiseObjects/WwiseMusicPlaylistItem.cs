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
        public WwiseMusicPlaylistItem(Option_PlaylistItemType playlist_item_type, string parent = @"\Actor-Mixer Hierarchy\Default Work Unit") : base("", "", "MusicPlaylistItem")
        {
            var tempObj = WwiseUtility.CreateObject("", ObjectType.MusicPlaylistItem, parent);
            ID = tempObj.ID;
            Name = tempObj.Name;

            SetPlaylistItemType(playlist_item_type);
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
            WwiseUtility.SetObjectReference(this, new WwiseReference("ObjectRef", segment));
        }

        public void SetWeight(float weight)
        {
            WwiseUtility.SetObjectProperty(this, new WwiseProperty("Weight", weight));
        }
    }
}
