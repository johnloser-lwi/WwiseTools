using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools
{
    public class WwiseSound : WwiseContainer
    {
        public string Language => language;

        protected string language;

        public WwiseSound(string _name, string language, string file) : base(_name, "Sound")
        {
            this.language = language;
            AddFile(file);
        }

        public WwiseSound(string _name, string language, string file, string guid) : base(_name, "Sound", guid)
        {
            this.language = language;
            AddFile(file);
        }

        public WwiseSound(string _name, string u_type) : base(_name, u_type)
        {
        }

        public virtual void SetStream(bool stream, bool nonCache, bool zeroLatency, int preFetchLength = 100)
        {
            string isStream = "False";
            string isNonCache = "False";
            string isZeroLatency = "False";
            if (stream) isStream = "True";
            if (nonCache) isNonCache = "True";
            if (zeroLatency) isZeroLatency = "True";

            AddProperty(new WwiseProperty("IsNonCachable", "bool", isNonCache));
            AddProperty(new WwiseProperty("IsStreamingEnabled", "bool", isStream));
            AddProperty(new WwiseProperty("IsZeroLantency", "bool", isZeroLatency));
            AddProperty(new WwiseProperty("PreFetchLength", "int16", preFetchLength.ToString()));
        }

        protected virtual void AddFile(string file)
        {
            if (WwiseUtility.ProjectPath == null)
            {
                Console.WriteLine("WwiseUtility not initialized!");
                return;
            }
            if (language != "SFX")
            {
                AddChildNode(WwiseNode.NewPropertyList(new List<IWwisePrintable>()
                {
                    new WwiseProperty("IsVoice", "bool", "True")
                }));
            }

            WwiseAudioFileSource audioFileSource = new WwiseAudioFileSource(name, language, file);

            AddChild(audioFileSource);
        }
    }

}
