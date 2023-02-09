using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.WwiseTypes
{
    public class Sound : WwiseTypeBase
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
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_IsZeroLatency(zeroLatency));
        }

        public async Task<List<AudioFileSource>> GetAudioFileSourcesAsync()
        {
            var children = await WwiseUtility.Instance.GetWwiseObjectChildrenAsync(WwiseObject);

            var ret = children
                .Select(c => c.AsAudioFileSource())
                .Where(c => c.Valid)
                .ToList();

            return ret;
        }

        public async Task<string[]> GetOriginalFilePathsAsync()
        {
            var fileSources = await GetAudioFileSourcesAsync();

            var result = new List<string>();
            
            foreach (var audioFileSource in fileSources)
            {
                var source = await audioFileSource.GetAudioFilePathAsync();
                if (source is not null) result.Add(source);
            }

            return result.ToArray();
        }

        public async Task<bool> IsVoiceAsync()
        {
            var sources = await GetAudioFileSourcesAsync();

            bool isValid = true;
            
            foreach (var audioFileSource in sources)
            {
                var lan = await audioFileSource.GetLanguageAsync();
                if (lan == "SFX")
                {
                    isValid = false;
                    continue;
                }
            }

            return isValid;
        }

        public Sound(WwiseObject wwiseObject) : base(wwiseObject, "Sound, MusicTrack")
        {
        }
    }
}
