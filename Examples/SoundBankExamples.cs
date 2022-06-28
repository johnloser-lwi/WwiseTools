using WwiseTools.Utils;
using WwiseTools.Utils.SoundBank;

namespace Examples
{
    internal class SoundBankExamples
    {
        public static async Task GetGeneratedSoundBankPaths()
        {
            var paths =  await WwiseUtility.Instance.GetGeneratedSoundBankPaths();
            foreach (var generatedSoundBankPath in paths)
            {
                WaapiLog.Log($"{generatedSoundBankPath.Platform} : {generatedSoundBankPath.Path}");
            }

        }

        public static async Task GetGeneratedSoundBankInfos()
        {
            var result = await WwiseUtility.Instance.GetGeneratedSoundBankInfos();

            var info = result.Where(i => i.Platform == "Android" && (i.Language == "English(US)" || i.Language == "SFX")).ToList();
            info.Sort((a, b) => a.SoundBankSize().CompareTo(b.SoundBankSize()) * -1);

            long total = 0;

            foreach (var generatedSoundBankInfo in info)
            {
                WaapiLog.Log($"{generatedSoundBankInfo.Name} : {generatedSoundBankInfo.SoundBankSize()}");

                total += generatedSoundBankInfo.SoundBankSize();
            }

            WaapiLog.Log($"Total size : {total}");
        }
    }
}
