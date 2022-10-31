using WwiseTools.Objects;
using WwiseTools.Src.Models.SoundBank;
using WwiseTools.Utils;
using WwiseTools.Utils.SoundBank;

namespace Examples
{
    internal class SoundBankExamples
    {
        public static async Task GetGeneratedSoundBankPaths()
        {
            var paths =  await WwiseUtility.Instance.GetGeneratedSoundBankPathsAsync();
            foreach (var generatedSoundBankPath in paths)
            {
                WaapiLog.Log($"{generatedSoundBankPath.Platform} : {generatedSoundBankPath.Path}");
            }

        }

        public static async Task GetGeneratedSoundBankInfos()
        {
            var result = await WwiseUtility.Instance.GetGeneratedSoundBankInfosAsync();

            var info = result.Where(i => i.Platform == "Android" && (i.Language == "English(US)" || i.Language == "SFX")).ToList();
            info.Sort((a, b) => a.TotalSoundBankSize.CompareTo(b.TotalSoundBankSize) * -1);

            long total = 0;

            foreach (var generatedSoundBankInfo in info)
            {
                WaapiLog.Log($"{generatedSoundBankInfo.Name} : {generatedSoundBankInfo.TotalSoundBankSize}");

                total += generatedSoundBankInfo.TotalSoundBankSize;
            }

            WaapiLog.Log($"Total size : {total}");
        }

        public static async Task GetSoundBankInclusions()
        {
            var selection = await WwiseUtility.Instance.GetWwiseObjectsBySelectionAsync();
            foreach (var wwiseObject in selection)
            {
                if (wwiseObject.Type != WwiseObject.ObjectType.SoundBank.ToString()) continue;

                var inclusionObjects = await WwiseUtility.Instance.GetSoundBankInclusionAsync(wwiseObject);
                foreach (var inclusionObject in inclusionObjects)
                {
                    WaapiLog.Log(inclusionObject);
                }
            }
        }

        public static async Task SetSoundBankInclusion()
        {
            var selection = await WwiseUtility.Instance.GetWwiseObjectsBySelectionAsync();

            var soundBanks = await WwiseUtility.Instance.GetWwiseObjectsOfTypeAsync("SoundBank");
            
            foreach (var s in selection)
            {
                SoundBankInclusion inclusion = new SoundBankInclusion()
                {
                    Filter = SoundBankInclusionFilter.Events,
                    Object = s
                };

                foreach (var wwiseObject in soundBanks)
                {
                    await wwiseObject.AsSoundBank().AddInclusionAsync(inclusion);
                }
            }
        }
        
        public static async Task RemoveSoundBankInclusion()
        {
            var selection = await WwiseUtility.Instance.GetWwiseObjectsBySelectionAsync();

            var soundBanks = await WwiseUtility.Instance.GetWwiseObjectsOfTypeAsync("SoundBank");
            
            foreach (var s in selection)
            {
                foreach (var wwiseObject in soundBanks)
                {
                    await wwiseObject.AsSoundBank().RemoveInclusionAsync(s);
                }
            }
        }
    }
}
