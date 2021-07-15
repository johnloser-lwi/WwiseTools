using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Audio
{
    /// <summary>
    /// Wwise中的Music Track
    /// </summary>
    public class WwiseMusicTrack : WwiseSound
    {
        public enum TrackType { Normal, RandomStep, SequenceStep, Switch }

        public WwiseMusicTrack(string name, string file, float endPosition, TrackType trackType, WwiseParser parser) : base(name, "MusicTrack", parser)
        {
            AddProperty(new WwiseProperty("MusicTrackType", "int16", TrackTypeChecker(trackType).ToString(), parser));
            WwiseAudioFileSource source;
            AddChild(source = new WwiseAudioFileSource(name, "SFX", file, parser));
            AddChildNode(new WwiseNode("TransitionList", parser, new WwiseMusicTransition(parser)));
            WwiseMusicTrackSequence seq = new WwiseMusicTrackSequence(parser);
            seq.AddChild(new WwiseMusicClip(name, endPosition, file, source.ID, parser));
            AddChildNode(new WwiseNode("SequenceList", parser, seq));
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
            AddProperty(new WwiseProperty("LookAheadTime", "int16", lookAheadTime.ToString(), parser));
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
