using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.WwiseTypes
{
    public class MusicSegment : WwiseTypeBase
    {

        public async Task SetTempoAndTimeSignatureAsync(float tempo, WwiseProperty.Option_TimeSignatureLower timeSignatureLower, uint timeSignatureUpper)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_Tempo(tempo));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_TimeSignatureLower(timeSignatureLower));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_TimeSignatureUpper(timeSignatureUpper));
        }


        public async Task SetEntryCueAsync(float timeMs)
        {
            var cues = await WwiseUtility.Instance.GetWwiseObjectsOfTypeAsync("MusicCue");
            WwiseObject entryCue = null;
            foreach (var cue in cues)
            {
                if (Path.GetDirectoryName(await cue.GetPathAsync())!.Contains(await WwiseObject.GetPathAsync()) && cue.Name == "Entry Cue")
                {
                    entryCue = cue;
                    break;
                }
            }

            if (entryCue != null)
            {
                await WwiseUtility.Instance.SetObjectPropertyAsync(entryCue, new WwiseProperty("TimeMs", timeMs));
            }
        }


        public async Task SetExitCueAsync(float timeMs)
        {
            var cues = await WwiseUtility.Instance.GetWwiseObjectsOfTypeAsync("MusicCue");
            WwiseObject exitCue = null;
            foreach (var cue in cues)
            {
                if (Path.GetDirectoryName(await cue.GetPathAsync())!.Contains(await WwiseObject.GetPathAsync()) && cue.Name == "Exit Cue")
                {
                    exitCue = cue;
                    break;
                }
            }



            if (exitCue != null)
            {
                await WwiseUtility.Instance.SetObjectPropertyAsync(exitCue, new WwiseProperty("TimeMs", timeMs));
            }
        }

        /// <summary>
        /// 创建新的Cue，异步执行
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timeMs"></param>
        /// <returns></returns>
        public async Task CreateCueAsync(string name, float timeMs, NameConflictBehaviour conflictBehaviour = NameConflictBehaviour.replace)
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return;

            try
            {
                var func = WwiseUtility.Instance.Function.Verify("ak.wwise.core.object.create");

                // 创建物体
                var result = await WwiseUtility.Instance.CallAsync
                    (
                        func,
                        new JObject
                        {
                            new JProperty("name", name),
                            new JProperty("type", "MusicCue"),
                            new JProperty("parent", await WwiseObject.GetPathAsync()),
                            new JProperty("onNameConflict", conflictBehaviour.ToString()),
                            new JProperty("list", "Cues"),
                            new JProperty("@TimeMs", timeMs),
                            new JProperty("@CueType", 2)
                        },
                        null,
                        WwiseUtility.Instance.TimeOut
                    );

                WaapiLog.InternalLog($"Music Cue {name} created successfully!");
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to create Cue : {name}! ======> {e.Message}");
            }
        }

        public MusicSegment(WwiseObject wwiseObject) : base(wwiseObject, nameof(MusicSegment))
        {
        }
    }
}
