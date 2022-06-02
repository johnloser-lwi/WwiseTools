using WwiseTools.Components;

namespace WwiseTools.Objects
{
    public partial class WwiseObject
    {
        public VoiceComponent Voice => new VoiceComponent(this);
        public WwiseSwitchContainerComponent SwitchContainer => new WwiseSwitchContainerComponent(this);

        public HierarchyComponent Hierarchy => new HierarchyComponent(this);

        public WwiseSoundComponent Sound => new WwiseSoundComponent(this);

        public WwiseRandomSequenceContainerComponent RandomSequenceContainer =>
            new WwiseRandomSequenceContainerComponent(this);

        public WwiseMusicTrackComponent MusicTrack => new WwiseMusicTrackComponent(this);

        public WwiseMusicSegmentComponent MusicSegment => new WwiseMusicSegmentComponent(this);

        public WwiseMusicPlaylistItemComponent MusicPlaylistItem => new WwiseMusicPlaylistItemComponent(this);

        public WwiseMusicPlaylistContainerComponent MusicPlaylistContainer => new WwiseMusicPlaylistContainerComponent(this);

        public WwiseMusicSwitchContainerComponent MusicSwitchContainer => new WwiseMusicSwitchContainerComponent(this);
    }
}
