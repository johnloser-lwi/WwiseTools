using System.Collections.Generic;
using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.Src.Models.SoundBank;
using WwiseTools.Utils;
using WwiseTools.Utils.SoundBank;

namespace WwiseTools.WwiseTypes;

public class SoundBank : WwiseTypeBase
{
    public async Task<bool> AddInclusionAsync(SoundBankInclusion inclusion)
    {
        return await WwiseUtility.Instance.AddSoundBankInclusionAsync(WwiseObject, inclusion);
    }

    public async Task<bool> RemoveInclusionAsync(WwiseObject reference)
    {
        return await WwiseUtility.Instance.RemoveSoundBankInclusionAsync(WwiseObject, reference);
    }

    public async Task<List<SoundBankInclusion>> GetInclusionsAsync()
    {
        return await WwiseUtility.Instance.GetSoundBankInclusionAsync(WwiseObject);
    }

    public async Task<bool> CleanInclusionAsync()
    {
        return await WwiseUtility.Instance.CleanSoundBankInclusionAsync(WwiseObject);
    }

    public async Task<bool> Generate()
    {
        return await Generate(new string[] { }, new string[] { });
    }
    
    public async Task<bool> Generate(string[] platforms, string[] languages)
    {
        return await WwiseUtility.Instance.GenerateSelectedSoundBanksAsync(new[] {WwiseObject.Name}, platforms, languages);
    }
    
    public SoundBank(WwiseObject wwiseObject) : base(wwiseObject, nameof(SoundBank))
    {
    }
}