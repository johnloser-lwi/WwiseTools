using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basics;
using WwiseTools.Properties;
using WwiseTools.Reference;
using WwiseTools.Utils;

namespace WwiseTools
{
    /// <summary>
    /// Wwise中的Sequence Contianer
    /// </summary>
    public class WwiseSequenceContainer : WwiseRandomContainer
    {
        /// <summary>
        /// 获取播放列表(Playlist)
        /// </summary>
        public WwiseNode PlayList => playlist;
        WwiseNode playlist;


        public WwiseSequenceContainer(string name, WwiseParser parser) : base(name, parser)
        {
            AddProperty(new WwiseProperty("RandomOrSequence", "int16", "0", parser));
        }

        public WwiseSequenceContainer(string name, string guid, WwiseParser parser) : base(name, guid, parser)
        {
            AddProperty(new WwiseProperty("RandomOrSequence", "int16", "0", parser));
        }

        
        /// <summary>
        /// 设置重新开始或者倒序播放
        /// </summary>
        /// <param name="restart"></param>
        public void SetEndRestartOrBackward(bool restart)
        {
            int s = 0;
            if (restart) s = 1;
            AddProperty(new WwiseProperty("RestartBeginningOrBackward", "int16", String.Format("{0}", s.ToString()), parser));
        }

        public override WwiseUnit AddChild(WwiseUnit child)
        {
            base.AddChild(child);


            try
            {
                if (playlist == null)
                {
                    playlist = new WwiseNode("Playlist", parser);
                    AddChildNode(playlist);
                }
                playlist.AddChildNode(new WwiseItemRef(child.Name, child.ID, parser));
            }
            catch
            {
                Console.WriteLine("Failed to add ItemRef!");
            }
            

            return child;
        }
    }
}
