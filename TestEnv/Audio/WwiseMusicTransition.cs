using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basics;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Audio
{
    class WwiseMusicTransition : WwiseContainer
    {
        public WwiseNode TransitionInfo { get => transitionInfo; }
        WwiseNode transitionInfo;

        public WwiseMusicTransition(WwiseParser parser) : base("Root", "MusicTransition", parser)
        {
            SetDefaultProperty();
            AddChildrenList();
            AddChildNode(transitionInfo = new WwiseNode("TransitionInfo", parser));
            Node.RemoveChild(Node.GetElementsByTagName("ReferenceList")[0]);
        }

        public WwiseMusicTransition(string name, WwiseParser parser) : base(name, "MusicTransition", parser)
        {
            //SetDefaultProperty();
            //AddChildrenList();
            AddChildNode(transitionInfo = new WwiseNode("TransitionInfo", parser));
        }

        public void AddTransitionInfo(WwiseNode info)
        {
            transitionInfo.AddChildNode(info);
        }

        private void SetDefaultProperty()
        {
            AddProperty(new WwiseProperty("IsFolder", "bool", "True", parser));
        }
    }
}
