using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Utils;

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
        public WwiseMusicPlaylistItem(PlaylistType playlistType, WwiseParser parser, int loopCount = 1) : base("", "MusicPlaylistItem", parser)
        {
            this.playlistType = playlistType;
            AddProperty(new WwiseProperty("PlayMode", "int16", PlaylistTypeCheck(playlistType).ToString(), parser));
            AddProperty(new WwiseProperty("LoopCount", "int16", loopCount.ToString(), parser));
        }

        /// <summary>
        /// 通过指定的SegmentRef初始化
        /// </summary>
        /// <param name="segment"></param>
        public WwiseMusicPlaylistItem(WwiseSegmentRef segment, WwiseParser parser) : base("", "MusicPlaylistItem", parser)
        {
            AddProperty(new WwiseProperty("PlaylistItemType", "int16", "1", parser));
            AddChildNode(segment);
        }

        /// <summary>
        /// 将SegmentRef其添加至Playlist Item中
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public WwiseSegmentRef AddSegment(WwiseSegmentRef segment)
        {
            AddChild(new WwiseMusicPlaylistItem(segment, parser));
            return segment;
        }

        /// <summary>
        /// 添加Playlist Item，设置类型以及循环并返回该组
        /// </summary>
        /// <param name="playlistType"></param>
        /// <param name="loopCount"></param>
        /// <returns></returns>
        public WwiseMusicPlaylistItem AddGroup(WwiseMusicPlaylistItem.PlaylistType playlistType, int loopCount = 1)
        {
            var group = new WwiseMusicPlaylistItem(playlistType, parser, loopCount);
            AddChild(group);

            return group;
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
