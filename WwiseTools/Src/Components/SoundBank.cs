using System.Collections.Generic;
using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.Src.Models.SoundBank;
using WwiseTools.Utils;
using WwiseTools.Utils.SoundBank;

namespace WwiseTools.Components;

public class SoundBank : ComponentBase
{
    public async Task<bool> AddInclutionAsync(SoundBankInclusion inclusion)
    {
        return await WwiseUtility.Instance.AddSoundBankInclusionAsync(WwiseObject, inclusion);
    }

    public async Task<List<SoundBankInclusion>> GetInclusionsAsync()
    {
        return await WwiseUtility.Instance.GetSoundBankInclusionAsync(WwiseObject);
    }

    public async Task<bool> CleanInclusionAsync()
    {
        return await WwiseUtility.Instance.CleanSoundBankInclusionAsync(WwiseObject);
    }
    
    public SoundBank(WwiseObject wwiseObject) : base(wwiseObject, nameof(SoundBank))
    {
    }
}