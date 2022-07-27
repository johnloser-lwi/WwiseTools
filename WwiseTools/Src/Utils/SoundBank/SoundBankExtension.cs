using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Src.Models.SoundBank;

namespace WwiseTools.Utils.SoundBank;

public static class SoundBankExtension
{
    public static async Task<bool> AddSoundBankInclusion(this WwiseUtility util, WwiseObject soundBank, SoundBankInclusion inclusion)
    {
        if (!await util.TryConnectWaapiAsync() || soundBank.Type != "SoundBank") return false;

        var filter = inclusion.Filter;

        JArray inclusionFilterArray = new JArray();
        
        if (filter.HasFlag(SoundBankInclusionFilter.Events))
            inclusionFilterArray.Add("events");
        if (filter.HasFlag(SoundBankInclusionFilter.Media))
            inclusionFilterArray.Add("media");
        if (filter.HasFlag(SoundBankInclusionFilter.Structures))
            inclusionFilterArray.Add("structures");
        
        
        try
        {
            var func = util.Function.Verify("ak.wwise.core.soundbank.setInclusions");
            await util.CallAsync
            (
                func,
                new JObject
                {
                    new JProperty("soundbank", soundBank.ID),
                    new JProperty("operation", "add"),
                    new JProperty("inclusions", new JArray
                    {
                        new JObject
                        {
                            new JProperty("object", inclusion.Object.ID),
                            new JProperty("filter", inclusionFilterArray)
                        }
                    })
                },
                null,
                util.TimeOut
            );

            return true;
        }
        catch (Exception e)
        {
            WaapiLog.InternalLog($"Failed to Add Event to Bank ======> {e.Message}");
        }

        return false;
    }
    
    
    public static async Task<List<SoundBankInclusion>> GetSoundBankInclusion(this WwiseUtility util, WwiseObject soundBank)
    {
        var result = new List<SoundBankInclusion>();
        if (!await util.TryConnectWaapiAsync() || soundBank.Type != "SoundBank") return result;

        try
        {
            var func = util.Function.Verify("ak.wwise.core.soundbank.getInclusions");
            var args = new
            {
                soundbank = soundBank.ID
            };

            var jresult = await util.CallAsync(func, args, null, util.TimeOut);
            if (jresult == null || jresult["inclusions"] == null) return result;
            foreach (var inclusion in jresult["inclusions"])
            {
                var id = inclusion["object"]?.ToString();
                if (string.IsNullOrEmpty(id)) continue;

                var filter = inclusion["filter"]?.ToString();
                if (filter == null) continue;

                var soundBankInclusion = new SoundBankInclusion();
                soundBankInclusion.Object = await WwiseUtility.Instance.GetWwiseObjectByIDAsync(id);

                if (filter.Contains("events")) soundBankInclusion.Filter |= SoundBankInclusionFilter.Events;
                if (filter.Contains("structures")) soundBankInclusion.Filter |= SoundBankInclusionFilter.Structures;
                if (filter.Contains("media")) soundBankInclusion.Filter |= SoundBankInclusionFilter.Media;


                result.Add(soundBankInclusion);
            }
        }
        catch (Exception e)
        {
            WaapiLog.InternalLog($"Failed to retrieve soundbank inclusions! ======> {e.Message}");
        }

        return result;
    }
}