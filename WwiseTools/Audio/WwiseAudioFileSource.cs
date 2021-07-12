using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools
{
    class WwiseAudioFileSource : WwiseUnit
    {
        public string Language => language;

        protected string language;

        public WwiseAudioFileSource(string _name, string language, string file) : base(_name, "AudioFileSource")
        {
            this.language = language;
            AddChildNode(new WwiseProperty("Language", language));
            WwiseUtility.CopyFile(file, language);
            AddChildNode(new WwiseProperty("AudioFile", file));
        }

        public WwiseAudioFileSource(string _name, string language, string file, string guid) : base(_name, "AudioFileSource", guid)
        {
            this.language = language;
            AddChildNode(new WwiseProperty("Language", language));
            WwiseUtility.CopyFile(file, language);
            AddChildNode(new WwiseProperty("AudioFile", file));
        }
    }
}
