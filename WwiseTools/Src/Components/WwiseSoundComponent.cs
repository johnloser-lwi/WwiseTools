using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Components
{
    public class WwiseSoundComponent : ComponentBase
    {
        public async Task SetInitialDelayAsync(float delay)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_InitialDelay(delay));
        }

        public async Task SetLoopAsync(bool loop, bool infinite = true, uint numOfLoop = 2)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_IsLoopingEnabled(loop));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_IsLoopingInfinite(infinite));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_LoopCount(numOfLoop));
        }

        public async Task SetStreamAsync(bool stream, bool nonCachable, bool zeroLatency)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_IsStreamingEnabled(stream));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_IsNonCachable(nonCachable));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_IsZeroLantency(zeroLatency));
        }

        public async Task<string[]> GetWavSourceFilePathAsync()
        {
            var r = await GetWavFilePathAsync();
            List<string> paths = new List<string>();
            if (r["return"]?.Last == null) return paths.ToArray();
            foreach (var result in r["return"].Last)
            {
                if (result.Last == null) continue;
                paths.Add(result.Last?.ToString());
            }
            return paths.ToArray();
        }

        private async Task<JObject> GetWavFilePathAsync()
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return null;

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

                    @return = new string[] { "sound:originalWavFilePath" }

                };

                var func = WaapiFunction.CoreObjectGet;

                JObject jresult = await WwiseUtility.Instance.CallAsync(func, query, options, WwiseUtility.Instance.TimeOut);

                return jresult;
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return WaveFilePath from ID : {WwiseObject.ID}! ======> {e.Message}");
                return null;
            }

        }


        public WwiseSoundComponent(WwiseObject wwiseObject) : base(wwiseObject)
        {
        }
    }
}
