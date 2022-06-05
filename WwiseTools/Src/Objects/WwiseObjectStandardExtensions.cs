using WwiseTools.Components;

namespace WwiseTools.Objects
{
    public static class WwiseObjectStandardExtensions
    {
        public static VoiceComponent GetVoice(this WwiseObject obj) => new VoiceComponent(obj);

        public static WwiseSwitchContainerComponent GetSwitchContainer(this WwiseObject obj) => new WwiseSwitchContainerComponent(obj);
        public static HierarchyComponent GetHierarchy(this WwiseObject obj) => new HierarchyComponent(obj);

        public static WwiseSoundComponent GetSound(this WwiseObject obj) => new WwiseSoundComponent(obj);

        public static WwiseRandomSequenceContainerComponent GetRandomSequenceContainer(this WwiseObject obj) =>
            new WwiseRandomSequenceContainerComponent(obj);

        public static WwiseMusicTrackComponent GetMusicTrack(this WwiseObject obj) => new WwiseMusicTrackComponent(obj);

        public static WwiseMusicSegmentComponent GetMusicSegment(this WwiseObject obj) => new WwiseMusicSegmentComponent(obj);

        public static WwiseMusicPlaylistItemComponent GetMusicPlaylistItem(this WwiseObject obj) => new WwiseMusicPlaylistItemComponent(obj);

        public static  WwiseMusicPlaylistContainerComponent GetMusicPlaylistContainer(this WwiseObject obj) => new WwiseMusicPlaylistContainerComponent(obj);

        public static WwiseMusicSwitchContainerComponent GetMusicSwitchContainer(this WwiseObject obj) => new WwiseMusicSwitchContainerComponent(obj);
    }
}
