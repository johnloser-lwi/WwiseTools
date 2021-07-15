using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basics;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools
{
    /// <summary>
    /// WWise中的Sound单元，当语言为SFX时为音效模式
    /// </summary>
    public class WwiseSound : WwiseContainer
    {
        /// <summary>
        /// 获取语言
        /// </summary>
        public string Language => language;

        protected string language;

        /// <summary>
        /// 初始化名称、语言以及源文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="language"></param>
        /// <param name="file"></param>
        /// <param name="parser"></param>
        public WwiseSound(string name, string language, string file, WwiseParser parser) : base(name, "Sound", parser)
        {
            this.language = language;
            AddFile(file);
        }

        /// <summary>
        /// 初始化名称、语言、源文件以及GUID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="language"></param>
        /// <param name="file"></param>
        /// <param name="guid"></param>
        /// <param name="parser"></param>
        public WwiseSound(string name, string language, string file, string guid, WwiseParser parser) : base(name, "Sound", guid, parser)
        {
            this.language = language;
            AddFile(file);
        }


        public WwiseSound(string name, string u_type, WwiseParser parser) : base(name, u_type, parser)
        {
        }
        

        /// <summary>
        /// 设置Stream模式
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="nonCache"></param>
        /// <param name="zeroLatency"></param>
        /// <param name="preFetchLength"></param>
        public virtual void SetStream(bool stream, bool nonCache, bool zeroLatency, int preFetchLength = 100)
        {
            string isStream = "False";
            string isNonCache = "False";
            string isZeroLatency = "False";
            if (stream) isStream = "True";
            if (nonCache) isNonCache = "True";
            if (zeroLatency) isZeroLatency = "True";

            AddProperty(new WwiseProperty("IsNonCachable", "bool", isNonCache, parser));
            AddProperty(new WwiseProperty("IsStreamingEnabled", "bool", isStream, parser));
            AddProperty(new WwiseProperty("IsZeroLantency", "bool", isZeroLatency, parser));
            AddProperty(new WwiseProperty("PreFetchLength", "int16", preFetchLength.ToString(), parser));
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
                AddProperty(new WwiseProperty("IsVoice", "bool", "True", parser));
            }

            WwiseAudioFileSource audioFileSource = new WwiseAudioFileSource(Name, language, file, parser);

            AddChild(audioFileSource);
        }
    }

}
