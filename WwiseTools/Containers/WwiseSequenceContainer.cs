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
    /// Wwise中的Sequence Contianer
    /// </summary>
    public class WwiseSequenceContainer : WwiseRandomContainer
    {
        /// <summary>
        /// 获取播放列表(Playlist)
        /// </summary>
        public WwiseNode PlayList => playlist;
        WwiseNode playlist;


        public WwiseSequenceContainer(string _name) : base(_name)
        {
        }

        public WwiseSequenceContainer(string _name, string guid) : base(_name, guid)
        {
        }

        protected override void Init(string _name, string u_type, string guid)
        {
            base.Init(_name, u_type, guid);

            AddChildNodeAtFront(properties = WwiseNode.NewPropertyList(new List<IWwisePrintable>()
            {
                new WwiseProperty("RandomOrSequence", "int16", "0")
            })); ;
        }

        
        /// <summary>
        /// 设置重新开始或者倒序播放
        /// </summary>
        /// <param name="restart"></param>
        public void SetEndRestartOrBackward(bool restart)
        {
            int s = 0;
            if (restart) s = 1;
            AddProperty(new WwiseProperty("RestartBeginningOrBackward", "int16", String.Format("{0}", s.ToString())));
        }

        public override WwiseUnit AddChild(WwiseUnit child)
        {
            base.AddChild(child);

            if (playlist == null)
            {
                playlist = new WwiseNode("Playlist");
                AddChildNode(playlist);
            }
            playlist.AddChildNode(new WwiseItemRef(child.name, child.id));

            return child;
        }
    }
}
