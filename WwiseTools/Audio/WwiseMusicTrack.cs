using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basics;
using WwiseTools.Properties;

namespace WwiseTools.Audio
{
    /// <summary>
    /// Wwise中的Music Track
    /// </summary>
    public class WwiseMusicTrack : WwiseSound
    {
        public enum TrackType { Normal, RandomStep, SequenceStep, Switch }

        public WwiseMusicTrack(string _name, string file, float endPosition, TrackType trackType) : base(_name, "MusicTrack")
        {
            AddProperty(new WwiseProperty("MusicTrackType", "int16", TrackTypeChecker(trackType).ToString()));
            WwiseAudioFileSource source;
            AddChild(source = new WwiseAudioFileSource(_name, "SFX", file));
            AddChildNode(new WwiseNode("TransitionList", new WwiseMusicTransition()));
            WwiseMusicTrackSequence seq = new WwiseMusicTrackSequence();
            seq.AddChild(new WwiseMusicClip(_name, endPosition, file, source.id));
            AddChildNode(new WwiseNode("SequenceList", seq));
        }

        /// <summary>
        /// 设置Stream模式
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="nonCache"></param>
        /// <param name="zeroLatency"></param>
        /// <param name="preFetchLength"></param>
        /// <param name="lookAheadTime"></param>
        public void SetStream(bool stream, bool nonCache, bool zeroLatency, int preFetchLength = 100, int lookAheadTime = 100)
        {
            base.SetStream(stream, nonCache, zeroLatency, preFetchLength);
            AddProperty(new WwiseProperty("LookAheadTime", "int16", lookAheadTime.ToString()));
        }

        private int TrackTypeChecker(TrackType trackType)
        {
            switch (trackType)
            {
                case TrackType.Normal:
                    return 0;
                case TrackType.RandomStep:
                    return 1;
                case TrackType.SequenceStep:
                    return 2;
                case TrackType.Switch:
                    return 3;
                default:
                    return 0;
            }
        }
    }
}
