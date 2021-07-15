using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basics;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Audio
{

    /// <summary>
    /// Wwise中的Music Segment
    /// </summary>
    public class WwiseMusicSegment : WwiseContainer
    {
        List<WwiseMusicCue> cueList = new List<WwiseMusicCue>();

        WwiseNode cues;

        /// <summary>
        /// 初始化名称
        /// </summary>
        /// <param name="_name"></param>
        public WwiseMusicSegment(string _name) : base(_name, "MusicSegment")
        {
            AddChildrenList();
            AddDefaultCue(new WwiseMusicCue("Entry Cue", WwiseMusicCue.CueType.Entry));
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

        /// <summary>
        /// 添加轨道，需要轨道名、文件以及轨道类型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="file"></param>
        /// <param name="trackType"></param>
        /// <returns></returns>
        public WwiseMusicTrack AddTrack(string name, string file, WwiseMusicTrack.TrackType trackType)
        {
            float length = 0;
            length = SoundInfo.GetSoundLength(Path.Combine(WwiseUtility.FilePath,file));
            WwiseMusicTrack track;
            AddChild(track = new WwiseMusicTrack(name, file, length, trackType));
            WwiseMusicCue exitCue = new WwiseMusicCue("Exit Cue", WwiseMusicCue.CueType.Exit);
            exitCue.AddProperty(new WwiseProperty("TimeMs", "Real64", length.ToString()));
            AddProperty(new WwiseProperty("EndPosition", "Real64", length.ToString()));
            AddDefaultCue(exitCue, true);
            //RefreshCue();

            return track;
        }

        private void AddDefaultCue(WwiseMusicCue newCue, bool replace = false)
        {
            if (cues == null)
            {
                cues = new WwiseNode("CueList");
                AddChildNode(cues);
            }

            if (replace)
            {
                WwiseMusicCue remove = null;
                foreach (var cue in cueList)
                {
                    if (cue.name == newCue.name)
                    {
                        //cueList.Remove(cue);
                        cues.ChildNodes.Remove(cue);
                    }
                }
                if (remove != null) cueList.Remove(remove);
            }

            
            cueList.Add(newCue);
            cues.AddChildNode(newCue);
        }

        /// <summary>
        /// 添加自定义Cue
        /// </summary>
        /// <param name="name"></param>
        /// <param name="time"></param>
        public void AddCue(string name, float time)
        {
            WwiseMusicCue newCue = new WwiseMusicCue(name, WwiseMusicCue.CueType.Custom);
            newCue.AddProperty(new WwiseProperty("TimeMs", "Real64", time.ToString()));
            AddDefaultCue(newCue);
            //RefreshCue();
        }

        /// <summary>
        /// 移除自定义Cue
        /// </summary>
        /// <param name="name"></param>
        public void RemoveCue(string name)
        {
            foreach (var cue in cueList)
            {
                if (cue.name == name)
                {
                    cues.ChildNodes.Remove(cue);
                    cueList.Remove(cue);
                }
            }

            
            //RefreshCue();
        }
    }
}
