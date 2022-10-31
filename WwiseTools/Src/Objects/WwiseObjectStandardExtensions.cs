using WwiseTools.Components;

namespace WwiseTools.Objects
{
    public static class WwiseObjectStandardExtensions
    {
        public static Voice AsVoice(this WwiseObject obj) => new Voice(obj);

        public static SwitchContainer AsSwitchContainer(this WwiseObject obj) => new SwitchContainer(obj);

        public static Hierarchy AsHierarchy(this WwiseObject obj) => new Hierarchy(obj);

        public static Sound AsSound(this WwiseObject obj) => new Sound(obj);

        public static AudioFileSource AsAudioFileSource(this WwiseObject obj) => new AudioFileSource(obj);

        public static RandomSequenceContainer AsRandomSequenceContainer(this WwiseObject obj) =>
            new RandomSequenceContainer(obj);

        public static MusicTrack AsMusicTrack(this WwiseObject obj) => new MusicTrack(obj);

        public static MusicSegment AsMusicSegment(this WwiseObject obj) => new MusicSegment(obj);

        public static MusicPlaylistItem AsMusicPlaylistItem(this WwiseObject obj) => new MusicPlaylistItem(obj);

        public static  MusicPlaylistContainer AsMusicPlaylistContainer(this WwiseObject obj) => new MusicPlaylistContainer(obj);

        public static MusicSwitchContainer AsMusicSwitchContainer(this WwiseObject obj) => new MusicSwitchContainer(obj);

        public static Event AsEvent(this WwiseObject obj) => new Event(obj);
        public static Components.Action AsAction(this WwiseObject obj) => new Components.Action(obj);
    }
}
