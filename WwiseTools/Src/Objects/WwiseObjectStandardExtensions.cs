using WwiseTools.Components;

namespace WwiseTools.Objects
{
    public static class WwiseObjectStandardExtensions
    {
        public static Voice AsVoice(this WwiseObject obj) => new Voice(obj);

        public static SwitchContainer AsSwitchContainer(this WwiseObject obj) => new SwitchContainer(obj);
        public static Hierarchy AsHierarchy(this WwiseObject obj) => new Hierarchy(obj);

        public static Sound AsSound(this WwiseObject obj) => new Sound(obj);

        public static RandomSequenceContainer AsRandomSequenceContainer(this WwiseObject obj) =>
            new RandomSequenceContainer(obj);

        public static MusicTrack AsMusicTrack(this WwiseObject obj) => new MusicTrack(obj);

        public static MusicSegment AsMusicSegment(this WwiseObject obj) => new MusicSegment(obj);

        public static MusicPlaylistItem AsMusicPlaylistItem(this WwiseObject obj) => new MusicPlaylistItem(obj);

        public static  MusicPlaylistContainer AsMusicPlaylistContainer(this WwiseObject obj) => new MusicPlaylistContainer(obj);

        public static MusicSwitchContainer AsMusicSwitchContainer(this WwiseObject obj) => new MusicSwitchContainer(obj);
    }
}
