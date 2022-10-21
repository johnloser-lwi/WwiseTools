using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Utils;

namespace WwiseTools.Components;

public class AudioFileSource : ComponentBase
{
    public async Task<string> GetLanguageAsync()
    {
        if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return null;

        try
        {
            // ak.wwise.core.@object.get 指令
            var query = new
            {
                from = new
                {
                    id = new string[] {WwiseObject.ID}
                }
            };
            

            // ak.wwise.core.@object.get 返回参数设置
            var options = new
            {

                @return = new string[] {"audioSource:language"}

            };

            var func = WaapiFunction.CoreObjectGet;

            JObject jresult =
                await WwiseUtility.Instance.CallAsync(func, query, options, WwiseUtility.Instance.TimeOut);

            if (jresult["return"] == null) return "";

            var sourceLanguage = jresult["return"].First;
            if (sourceLanguage == null) return "";

            var name = sourceLanguage["audioSource:language"]?["name"];
            return name == null ? "" : name.ToString();
        }
        catch (Exception e)
        {
            WaapiLog.InternalLog($"Failed to return Language from ID : {WwiseObject.ID}! ======> {e.Message}");
            return null;
        }
    }


    public AudioFileSource(WwiseObject wwiseObject) : base(wwiseObject, nameof(AudioFileSource))
    {
    }
}