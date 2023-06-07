using System;
using WwiseTools.WwiseTypes;

namespace WwiseTools.Objects
{
    public static class WwiseObjectStandardExtensions
    {
        [Obsolete("Use AsActorMixer instead")]
        public static Voice AsVoice(this WwiseObject obj) => new Voice(obj);
        
        public static ActorMixer AsActorMixer(this WwiseObject obj) => new ActorMixer(obj);

        public static SwitchContainer AsSwitchContainer(this WwiseObject obj) => new SwitchContainer(obj);

        public static Container AsContainer(this WwiseObject obj) => new Container(obj);

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
        public static WwiseTypes.Action AsAction(this WwiseObject obj) => new WwiseTypes.Action(obj);
    }
}
