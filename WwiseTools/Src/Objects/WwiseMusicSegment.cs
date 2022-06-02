using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public class WwiseMusicSegment : WwiseActorMixer
    {
        public float EntryCuePos { get; private set; }
        public float ExitCuePos { get; private set; }

        /// <summary>
        /// 创建一个音乐片段
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentPath"></param>
        [Obsolete("use WwiseUtility.Instance.CreateObjectAsync instead")]
        public WwiseMusicSegment(string name, string parentPath = @"\Interactive Music Hierarchy\Default Work Unit\") : base(name, "", ObjectType.MusicSegment.ToString())
        {
            var segment = WwiseUtility.Instance.CreateObject(name, ObjectType.MusicSegment, parentPath);
            ID = segment.ID;
            Name = segment.Name;

            EntryCuePos = 0;
            ExitCuePos = 0;
        }

        /// <summary>
        /// 创建一个音乐片段，配置导入选项
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filePath"></param>
        /// <param name="subFolder"></param>
        /// <param name="parentPath"></param>
        [Obsolete("use CreateMusicSegmentAsync instead")]
        public WwiseMusicSegment(string name, string filePath, string subFolder = "", string parentPath = @"\Interactive Music Hierarchy\Default Work Unit") : base(name, "", ObjectType.MusicSegment.ToString())
        {
            var segment = WwiseUtility.Instance.CreateObject(name, ObjectType.MusicSegment, parentPath);
            parentPath = System.IO.Path.Combine(parentPath, name);
            ID = segment.ID;
            Name = name;

            var tempObj = WwiseUtility.Instance.ImportSound(filePath, "SFX", subFolder, parentPath);

            EntryCuePos = 0;
            ExitCuePos = 0;

        }

        public static async Task<WwiseMusicSegment> CreateMusicSegmentAsync(string name, string filePath,
            string subFolder = "", string parentPath = @"\Interactive Music Hierarchy\Default Work Unit")
        {
            var segment = await WwiseUtility.Instance.CreateObjectAsync(name, ObjectType.MusicSegment, parentPath);
            parentPath = System.IO.Path.Combine(parentPath, name);
            await WwiseUtility.Instance.ImportSoundAsync(filePath, "SFX", subFolder, parentPath);

            return new WwiseMusicSegment(segment) {EntryCuePos = 0, ExitCuePos = 0};
        }
        
        
        public WwiseMusicSegment(WwiseObject @object) : base("", "", "")
        {
            if (@object == null) return;
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;

            EntryCuePos = 0;
            ExitCuePos = 0;
        }


        /// <summary>
        /// 设置BPM和拍号
        /// </summary>
        /// <param name="tempo"></param>
        /// <param name="timeSignatureLower"></param>
        /// <param name="timeSignatureUpper"></param>
        [Obsolete("use async version instead")]
        public void SetTempoAndTimeSignature(float tempo, WwiseProperty.Option_TimeSignatureLower timeSignatureLower, uint timeSignatureUpper)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_Tempo(tempo));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_TimeSignatureLower(timeSignatureLower));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_TimeSignatureUpper(timeSignatureUpper));
        }

        public async Task SetTempoAndTimeSignatureAsync(float tempo, WwiseProperty.Option_TimeSignatureLower timeSignatureLower, uint timeSignatureUpper)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_Tempo(tempo));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_TimeSignatureLower(timeSignatureLower));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_TimeSignatureUpper(timeSignatureUpper));
        }

        /// <summary>
        /// 设置Entry Cue位置
        /// </summary>
        /// <param name="timeMs"></param>
        [Obsolete("Use async version instead")]
        public void SetEntryCue(float timeMs)
        {
            var cues = WwiseUtility.Instance.GetWwiseObjectsOfType("MusicCue");
            WwiseObject entryCue = null;
            foreach (var cue in cues)
            {
                if (cue.Path.Contains(Path) && cue.Name == "Entry Cue")
                {
                    entryCue = cue;
                    break;
                }
            }

            if (entryCue != null)
            {
                WwiseUtility.Instance.SetObjectProperty(entryCue, new WwiseProperty("TimeMs", timeMs));
                EntryCuePos = timeMs;
            }
        }
        
        public async Task SetEntryCueAsync(float timeMs)
        {
            var cues = await WwiseUtility.Instance.GetWwiseObjectsOfTypeAsync("MusicCue");
            WwiseObject entryCue = null;
            foreach (var cue in cues)
            {
                if ((await cue.GetPathAsync()).Contains(await GetPathAsync()) && cue.Name == "Entry Cue")
                {
                    entryCue = cue;
                    break;
                }
            }

            if (entryCue != null)
            {
                await WwiseUtility.Instance.SetObjectPropertyAsync(entryCue, new WwiseProperty("TimeMs", timeMs));
                EntryCuePos = timeMs;
            }
        }

        /// <summary>
        /// 设置Exit Cue位置
        /// </summary>
        /// <param name="timeMs"></param>
        /// <param name="ignoreSmallerValue"></param>
        [Obsolete("Use async version instead")]
        public void SetExitCue(float timeMs, bool ignoreSmallerValue = true)
        {
            if (ignoreSmallerValue && timeMs <= ExitCuePos) return; // 如果新的位置参数小于当前位置，则无视该参数

                var cues = WwiseUtility.Instance.GetWwiseObjectsOfType("MusicCue");
            WwiseObject exitCue = null;
            foreach (var cue in cues)
            {
                if (cue.Path.Contains(Path) && cue.Name == "Exit Cue")
                {
                    exitCue = cue;
                    break;
                }
            }

            

            if (exitCue != null)
            {
                WwiseUtility.Instance.SetObjectProperty(exitCue, new WwiseProperty("TimeMs", timeMs));
                ExitCuePos = timeMs;
            }
        }
        
        public async Task SetExitCueAsync(float timeMs, bool ignoreSmallerValue = true)
        {
            if (ignoreSmallerValue && timeMs <= ExitCuePos) return; // 如果新的位置参数小于当前位置，则无视该参数

            var cues = await WwiseUtility.Instance.GetWwiseObjectsOfTypeAsync("MusicCue");
            WwiseObject exitCue = null;
            foreach (var cue in cues)
            {
                if ((await cue.GetPathAsync()).Contains(await GetPathAsync()) && cue.Name == "Exit Cue")
                {
                    exitCue = cue;
                    break;
                }
            }

            

            if (exitCue != null)
            {
                await WwiseUtility.Instance.SetObjectPropertyAsync(exitCue, new WwiseProperty("TimeMs", timeMs));
                ExitCuePos = timeMs;
            }
        }

        /// <summary>
        /// 创建新的Cue
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timeMs"></param>
        [Obsolete("Use async version instead")]
        public void CreateCue(string name, float timeMs)
        {
            CreateCueAsync(name, timeMs).Wait();
        }

        /// <summary>
        /// 创建新的Cue，异步执行
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timeMs"></param>
        /// <returns></returns>
        public async Task CreateCueAsync(string name, float timeMs)
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return;

            try
            {
                var func = WwiseUtility.Instance.Function.Verify("ak.wwise.core.object.create");

                // 创建物体
                var result = await WwiseUtility.Instance.Client.Call
                    (
                        func,
                        new JObject
                        {
                            new JProperty("name", name),
                            new JProperty("type", "MusicCue"),
                            new JProperty("parent", await GetPathAsync()),
                            new JProperty("onNameConflict", "replace"),
                            new JProperty("list", "Cues"),
                            new JProperty("@TimeMs", timeMs),
                            new JProperty("@CueType", 2)
                        },
                        null,
                        WwiseUtility.Instance.TimeOut
                    );

                WaapiLog.Log($"Music Cue {name} created successfully!");
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to create Cue : {name}! ======> {e.Message}");
            }
        }
    }
}
