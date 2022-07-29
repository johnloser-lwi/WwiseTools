using WwiseTools.Components;

namespace WwiseTools.Objects;

public static class WwiseObjectSoundBankExtensions
{
    public static SoundBank AsSoundBank(this WwiseObject obj) => new SoundBank(obj);
}