using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WwiseTools.Basics;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Audio
{
    /// <summary>
    /// Wwise中的Music Playlist Container
    /// </summary>
    public class WwiseMusicPlaylistContainer : WwiseContainer
    {

        public WwiseMusicPlaylistItem Playlist => playlistItem;
        WwiseMusicPlaylistItem playlistItem;

        //public WwiseNode MusicSegments => segments;

        /// <summary>
        /// 初始化名称、类型以及循环(默认为1)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="playlistType"></param>
        /// <param name="parser"></param>
        /// <param name="loopCount"></param>
        public WwiseMusicPlaylistContainer(string name, WwiseMusicPlaylistItem.PlaylistType playlistType, WwiseParser parser, int loopCount = 1) : base(name, "MusicPlaylistContainer", parser)
        {
            AddChildrenList();
            AddTransitionList();
            InitPlaylist(playlistType, loopCount);
        }

        /// <summary>
        /// 设置速度与拍号信息
        /// </summary>
        /// <param name="tempo"></param>
        /// <param name="timeSigUpper"></param>
        /// <param name="timeSigLower"></param>
        public void SetTempoAndTimeSignature(float tempo, int timeSigUpper, int timeSigLower)
        {
            AddProperty(new WwiseProperty("Tempo", "Real64", tempo.ToString(), parser));
            AddProperty(new WwiseProperty("TimeSignatureLower", "int16", timeSigLower.ToString(), parser));
            AddProperty(new WwiseProperty("TimeSignatureUpper", "int16", timeSigUpper.ToString(), parser));
        }

        /// <summary>
        /// 添加Music Segment，返回Segenmt，如果Segment为null则返回null
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public WwiseMusicSegment AddSegment(WwiseMusicSegment segment)
        {
            if (segment == null)
            {
                Console.WriteLine("Music Segment is null!");
                return null;
            }
            AddChild(segment);
            playlistItem.AddChild(new WwiseMusicPlaylistItem(new WwiseSegmentRef(segment.Name, segment.ID, parser), parser));

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
            playlistItem.AddChild(group);

            return group;
        }

        /// <summary>
        /// 通过名称查找包含的MusicSegment
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public WwiseMusicSegment FindSegmentByName(string name)
        {
            
            foreach (XmlElement s in Node.GetElementsByTagName("MusicSegment"))
            {
                if (s.GetAttribute("Name") == name)
                {
                    return new WwiseMusicSegment(name, parser);
                }
            }

            return null;
        }


        private void InitPlaylist(WwiseMusicPlaylistItem.PlaylistType playlistType, int loopCount = 1)
        {
            WwiseNode playlist = (WwiseNode)AddChildNode(new WwiseNode("Playlist", parser));
            playlistItem = (WwiseMusicPlaylistItem)playlist.AddChildNode(new WwiseMusicPlaylistItem(playlistType, parser, loopCount));
            
        }

        private void AddTransitionList()
        {
            WwiseNode transitionList = (WwiseNode)AddChildNode(new WwiseNode("TransitionList", parser));
            WwiseMusicTransition transition = (WwiseMusicTransition)transitionList.AddChildNode(new WwiseMusicTransition(parser));
            transition.AddChild(new WwiseMusicTransition("Transition", parser));
        }
    }
}
