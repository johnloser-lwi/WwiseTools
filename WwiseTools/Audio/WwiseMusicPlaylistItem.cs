using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;

namespace WwiseTools.Audio
{
    public class WwiseMusicPlaylistItem  : WwiseUnit
    {
        public enum PlaylistType { SequenceContinous, SequenceStep,  RandomContinous, RandomStep }
        private PlaylistType playlistType;

        public WwiseMusicPlaylistItem(PlaylistType playlistType, int loopCount = 1) : base("", "MusicPlaylistItem")
        {
            this.playlistType = playlistType;
            AddProperty(new WwiseProperty("PlayMode", "int16", PlaylistTypeCheck(playlistType).ToString()));
            AddProperty(new WwiseProperty("LoopCount", "int16", loopCount.ToString()));
        }

        public WwiseMusicPlaylistItem(WwiseSegmentRef segment) : base("", "MusicPlaylistItem")
        {
            AddProperty(new WwiseProperty("PlaylistItemType", "int16", "1"));
            AddChildNode(segment);
        }

        private int PlaylistTypeCheck(PlaylistType type)
        {
            switch (type)
            {
                case PlaylistType.SequenceContinous:
                    return 0;
                case PlaylistType.SequenceStep:
                    return 1;
                case PlaylistType.RandomContinous:
                    return 2;
                case PlaylistType.RandomStep:
                    return 3;
                default:
                    return 0;
            }
        }
    }
}
