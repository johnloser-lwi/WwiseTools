using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;

namespace WwiseTools.Audio
{
    /// <summary>
    /// Music Playlist Container中的组(Group)
    /// </summary>
    public class WwiseMusicPlaylistItem  : WwiseUnit
    {
        /// <summary>
        /// 组的播放类型
        /// </summary>
        public enum PlaylistType { SequenceContinous, SequenceStep,  RandomContinous, RandomStep }
        private PlaylistType playlistType;

        /// <summary>
        /// 初始化类型以及循环(默认为1)
        /// </summary>
        /// <param name="playlistType"></param>
        /// <param name="loopCount"></param>
        public WwiseMusicPlaylistItem(PlaylistType playlistType, int loopCount = 1) : base("", "MusicPlaylistItem")
        {
            this.playlistType = playlistType;
            AddProperty(new WwiseProperty("PlayMode", "int16", PlaylistTypeCheck(playlistType).ToString()));
            AddProperty(new WwiseProperty("LoopCount", "int16", loopCount.ToString()));
        }

        /// <summary>
        /// 通过指定的SegmentRef初始化
        /// </summary>
        /// <param name="segment"></param>
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
