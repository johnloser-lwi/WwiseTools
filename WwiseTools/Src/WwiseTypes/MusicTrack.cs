using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.WwiseTypes
{
    public class MusicTrack : WwiseTypeBase
    {

        /// <summary>
        /// 获取轨道长度，同步执行
        /// </summary>
        /// <returns></returns>
        public async Task<float> GetTrackLengthAsync()
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return 0;


            try
            {
                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        id = new string[] { WwiseObject.ID }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {
                    @return = new string[] { "audioSource:maxDurationSource" }
                };



                var func = WaapiFunctionList.CoreObjectGet;

                JObject jresult = await WwiseUtility.Instance.CallAsync(func, query, options, WwiseUtility.Instance.TimeOut);

                if (jresult["return"]?.Last == null) throw new Exception();
                if (jresult["return"].Last["audioSource:maxDurationSource"] == null) throw new Exception();

                float.TryParse(jresult["return"].Last["audioSource:maxDurationSource"]["trimmedDuration"]?.ToString(),
                    out float duration);

                WaapiLog.InternalLog($"Duration of WwiseObject {WwiseObject.Name} is {duration}s");

                return duration;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return file path of Object : {WwiseObject.Name}! ======> {e.Message}");
                return -1;
            }
        }


        public async Task SetStreamAsync(bool stream, bool nonCachable, bool zeroLatency, uint lookAheadTime = 100, uint prefetchLength = 100)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_IsStreamingEnabled(stream));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_IsNonCachable(nonCachable));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_IsZeroLatency(zeroLatency));

            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_LookAheadTime(lookAheadTime));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_PreFetchLength(prefetchLength));

        }

        public async Task SetSwitchGroupOrStateGroupAsync(WwiseProperty group)
        {
            await WwiseUtility.Instance.SetObjectPropertiesAsync(WwiseObject, group);
        }

        public async Task SetDefaultSwitchOrStateAsync(WwiseProperty switchOrState)
        {
            await WwiseUtility.Instance.SetObjectPropertiesAsync(WwiseObject, switchOrState);
        }

        public MusicTrack(WwiseObject wwiseObject) : base(wwiseObject, nameof(MusicTrack))
        {
        }
    }
}
