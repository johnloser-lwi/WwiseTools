#nullable enable
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Serialization;
using WwiseTools.Utils;

namespace WwiseTools.WwiseTypes;

public class AudioFileSource : WwiseTypeBase
{
    public async Task<string> GetLanguageAsync()
    {
        if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return "";

        try
        {
          
            var query = new
            {
                from = new
                {
                    id = new string[] {WwiseObject.ID}
                }
            };
            

          
            var options = new
            {

                @return = new string[] {"audioSourceLanguage"}

            };

            var func = WaapiFunctionList.CoreObjectGet;

            var jresult =
                await WwiseUtility.Instance.CallAsync(func, query, options, WwiseUtility.Instance.TimeOut);
            
            var returnData = WaapiSerializer.Deserialize<ReturnData<ObjectReturnData>>(jresult.ToString());
            if (returnData.Return.Length == 0) return "";
            
            var name = returnData.Return[0].AudioSourceLanguage.Name;
            return name;
        }
        catch (Exception e)
        {
            WaapiLog.InternalLog($"Failed to return Language from ID : {WwiseObject.ID}! ======> {e.Message}");
            return "";
        }
    }

    public async Task<string?> GetAudioFilePathAsync()
    {
        var r = await GetWavFilePathAsync();

        if (r is null) return null;
        
        return r["return"]?.Last?.Last?.Last?.ToString();
    }

    public async Task<string?> GetAudioFileRelativePathAsync()
    {
        var projectFolder = WwiseUtility.Instance.ConnectionInfo.ProjectFolder;
        var language = await GetLanguageAsync();
        var voiceOriginalPath = Path.Combine(projectFolder, $"Originals\\Voices\\{language}");

        var filePath = await GetAudioFilePathAsync();

        return filePath?.Replace(voiceOriginalPath, "").Trim('\\');
    }

    private async Task<JObject?> GetWavFilePathAsync()
    {
        if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return null;

        try
        {
          
            var query = new
            {
                from = new
                {
                    id = new string[] { WwiseObject.ID }
                }
            };

            
          
            var originalWavFilePath = "sound:originalWavFilePath";
            
            
          
            var options = new
            {

                @return = new string[] { originalWavFilePath }

            };

            var func = WaapiFunctionList.CoreObjectGet;

            var jresult = await WwiseUtility.Instance.CallAsync(func, query, options, WwiseUtility.Instance.TimeOut);

            return jresult;
        }
        catch (Exception e)
        {
            WaapiLog.InternalLog($"Failed to return WaveFilePath from ID : {WwiseObject.ID}! ======> {e.Message}");
            return null;
        }

    }

    public async Task SetChannelConfigOverrideAsync(int config)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject,
            WwiseProperty.Prop_ChannelConfigOverride(config));
    }

    public async Task SetMakeUpGainAsync(float gain)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_VolumeOffset(gain));
    }

    public async Task SetTrimStartAsync(float trim)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_TrimBegin(trim));
    }

    public async Task SetTrimEndAsync(float trim)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_TrimEnd(trim));
    }

    public async Task SetOverrideWavLoopPointsAsync(bool option)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverrideWavLoop(option));
    }

    public async Task SetLoopStartAsync(float position)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_LoopBegin(position));
    }

    public async Task SetLoopEndAsync(float position)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_LoopEnd(position));
    }

    public async Task SetCrossfadeDurationAsync(float duration)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_CrossfadeDuration(duration));
    }

    public async Task SetCrossfadeShapeAsync(WwiseProperty.Option_Curve shape)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_CrossfadeShape(shape));
    }

    public async Task SetMarkerInputModeAsync(WwiseProperty.Option_MarkerInputMode mode)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_MarkerInputMode(mode));
    }

    public async Task SetMarkerSensitivityAsync(float sensitivity)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject,
            WwiseProperty.Prop_MarkerDetectionSensitivity(sensitivity));
    }

    public async Task SetFadeInDurationAsync(float duration)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_FadeInDuration(duration));
    }

    public async Task SetFadeInCurveAsync(WwiseProperty.Option_Curve curve)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_FadeInCurve(curve));
    }
    
    public async Task SetFadeOutDurationAsync(float duration)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_FadeOutDuration(duration));
    }

    public async Task SetFadeOutCurveAsync(WwiseProperty.Option_Curve curve)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_FadeOutCurve(curve));
    }

    public async Task SetOverrideConversionSettingsAsync(bool option)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverrideConversion(option));
    }

    public async Task SetConversionSettingAsync(WwiseObject conversion)
    {
        await WwiseUtility.Instance.SetObjectPropertiesAsync(WwiseObject, WwiseProperty.Prop_Conversion(conversion));
    }
    
    public AudioFileSource(WwiseObject wwiseObject) : base(wwiseObject, nameof(AudioFileSource))
    {
    }
}