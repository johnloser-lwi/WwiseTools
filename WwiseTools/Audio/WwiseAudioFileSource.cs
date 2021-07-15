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

        public WwiseAudioFileSource(string name, string language, string file, WwiseParser parser) : base(name, "AudioFileSource", parser)
        {
            this.language = language;
            //AddChildNode(new WwiseProperty("Language", language, parser));
            var lan = XML.CreateElement("Language");
            lan.InnerText = language;
            Node.AppendChild(lan);
            WwiseUtility.CopyFile(file, language);
            //AddChildNode(new WwiseProperty("AudioFile", file, parser));
            var au = XML.CreateElement("AudioFile");
            au.InnerText = file;
            Node.AppendChild(au);
        }

        public WwiseAudioFileSource(string name, string language, string file, string guid, WwiseParser parser) : base(name, "AudioFileSource", guid, parser)
        {
            this.language = language;
            //AddChildNode(new WwiseProperty("Language", language, parser));
            var lan = XML.CreateElement("Language");
            lan.InnerText = language;
            Node.AppendChild(lan);
            WwiseUtility.CopyFile(file, language);
            //AddChildNode(new WwiseProperty("AudioFile", file, parser));
            var au = XML.CreateElement("AudioFile");
            au.InnerText = file;
            Node.AppendChild(au);
        }
    }
}
