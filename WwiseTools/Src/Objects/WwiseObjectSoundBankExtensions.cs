using WwiseTools.Components;

namespace WwiseTools.Objects;

public static class WwiseObjectSoundBankExtensions
{
    public static SoundBankComponent GetSoundBank(this WwiseObject obj) => new SoundBankComponent(obj);
}