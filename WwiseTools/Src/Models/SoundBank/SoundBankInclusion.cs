using System;
using WwiseTools.Objects;

namespace WwiseTools.Src.Models.SoundBank
{
    [Flags]
    public enum SoundBankInclusionFilter
    {
        None = 0,
        Events = 1,
        Structures = 2,
        Media = 4,
        Everything = ~0
    }

    public class SoundBankInclusion
    {
        public SoundBankInclusionFilter Filter { get; set; } = SoundBankInclusionFilter.None;
        public WwiseObject Object { get; set; }

        public override string ToString()
        {
            return Object.ToString() + ", Filter: " + Filter.ToString();
        }
    }
}
