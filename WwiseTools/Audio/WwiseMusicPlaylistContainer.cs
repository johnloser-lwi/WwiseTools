using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
using WwiseTools.Properties;

namespace WwiseTools.Audio
{
    /// <summary>
    /// Wwise中的Music Playlist Container
    /// </summary>
    public class WwiseMusicPlaylistContainer : WwiseContainer
    {

        public WwiseMusicPlaylistItem Playlist => playlistItem;
        WwiseMusicPlaylistItem playlistItem;

        public List<WwiseMusicSegment> MusicSegments => segments;
        List<WwiseMusicSegment> segments;

        /// <summary>
        /// 初始化名称、类型以及循环(默认为1)
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="playlistType"></param>
        /// <param name="loopCount"></param>
        public WwiseMusicPlaylistContainer(string _name, WwiseMusicPlaylistItem.PlaylistType playlistType, int loopCount = 1) : base(_name, "MusicPlaylistContainer")
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
            AddProperty(new WwiseProperty("Tempo", "Real64", tempo.ToString()));
            AddProperty(new WwiseProperty("TimeSignatureLower", "int16", timeSigLower.ToString()));
            AddProperty(new WwiseProperty("TimeSignatureUpper", "int16", timeSigUpper.ToString()));
        }

        
        public WwiseMusicSegment AddSegment(WwiseMusicSegment segment)
        {
            if (segments == null) segments = new List<WwiseMusicSegment>();
            AddChild(segment);
            playlistItem.AddChild(new WwiseMusicPlaylistItem(new WwiseSegmentRef(segment.name, segment.id)));


            foreach (var c in Children.ChildNodes)
            {
                try
                {
                    WwiseMusicSegment s = (WwiseMusicSegment)c;
                    if (s != null)
                    {
                        segments.Add(s);
                    }
                }
                catch
                {
                    continue;
                }
            }

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
            var group = new WwiseMusicPlaylistItem(playlistType, loopCount);
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
            foreach (var s in MusicSegments)
            {
                if (s.name == name)
                {
                    return s;
                }
            }

            return null;
        }


        private void InitPlaylist(WwiseMusicPlaylistItem.PlaylistType playlistType, int loopCount = 1)
        {
            WwiseNode playlist = (WwiseNode)AddChildNode(new WwiseNode("Playlist"));
            playlistItem = (WwiseMusicPlaylistItem)playlist.AddChildNode(new WwiseMusicPlaylistItem(playlistType, loopCount));
            
        }

        private void AddTransitionList()
        {
            WwiseNode transitionList = (WwiseNode)AddChildNode(new WwiseNode("TransitionList"));
            WwiseMusicTransition transition = (WwiseMusicTransition)transitionList.AddChildNode(new WwiseMusicTransition());
            transition.AddChild(new WwiseMusicTransition("Transition"));
        }
    }
}
