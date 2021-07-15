using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basics;
using WwiseTools.Properties;

namespace WwiseTools.Audio
{
    class WwiseMusicCue : WwiseUnit
    {
        public CueType Type => cueType;

        public enum CueType { Entry, Exit, Custom }
        private CueType cueType;

        public WwiseMusicCue(string _name, CueType cueType) : base(_name, "MusicCue")
        {
            this.cueType = cueType;
            AddProperty(new WwiseProperty("CueType", "int16", CueTypeChecker(cueType).ToString()));
        }

        private int CueTypeChecker(CueType trackType)
        {
            switch (trackType)
            {
                case CueType.Entry:
                    return 0;
                case CueType.Exit:
                    return 1;
                case CueType.Custom:
                    return 2;
                default:
                    return 0;
            }
        }
    }
}
