using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
using WwiseTools.Properties;

namespace WwiseTools.Audio
{
    public class WwiseMusicPlaylistContainer : WwiseContainer
    {
        WwiseMusicPlaylistItem playlistItem;

        public WwiseMusicPlaylistContainer(string _name, WwiseMusicPlaylistItem.PlaylistType playlistType, int loopCount = 1) : base(_name, "MusicPlaylistContainer")
        {
            AddChildrenList();
            AddTransitionList();
            InitPlaylist(playlistType, loopCount);
        }

        public void SetTempoAndTimeSignature(float tempo, int timeSigUpper, int timeSigLower)
        {
            AddProperty(new WwiseProperty("Tempo", "Real64", tempo.ToString()));
            AddProperty(new WwiseProperty("TimeSignatureLower", "int16", timeSigLower.ToString()));
            AddProperty(new WwiseProperty("TimeSignatureUpper", "int16", timeSigUpper.ToString()));
        }

        public WwiseMusicSegment AddSegment(WwiseMusicSegment segment)
        {
            AddChild(segment);
            playlistItem.AddChild(new WwiseMusicPlaylistItem(new WwiseSegmentRef(segment.name, segment.id)));
            return segment;
        }

        public WwiseMusicPlaylistItem AddGroup(WwiseMusicPlaylistItem.PlaylistType playlistType, int loopCount = 1)
        {
            var group = new WwiseMusicPlaylistItem(playlistType, loopCount);
            playlistItem.AddChild(group);

            return group;
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
