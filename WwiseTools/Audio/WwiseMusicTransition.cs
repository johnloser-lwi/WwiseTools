using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
using WwiseTools.Properties;

namespace WwiseTools.Audio
{
    class WwiseMusicTransition : WwiseContainer
    {
        public WwiseNode TransitionInfo { get => transitionInfo; }
        WwiseNode transitionInfo;

        public WwiseMusicTransition() : base("Root", "MusicTransition")
        {
            SetDefaultProperty();
            AddChildrenList();
            AddChildNode(transitionInfo = new WwiseNode("TransitionInfo"));
        }

        public WwiseMusicTransition(string name) : base(name, "MusicTransition")
        {
            //SetDefaultProperty();
            //AddChildrenList();
            AddChildNode(transitionInfo = new WwiseNode("TransitionInfo"));
        }

        public void AddTransitionInfo(IWwisePrintable info)
        {
            transitionInfo.AddChildNode(info);
        }

        private void SetDefaultProperty()
        {
            AddProperty(new WwiseProperty("IsFolder", "bool", "True"));
        }

        protected override void Init(string _name, string u_type, string audioSourceID)
        {
            this.unit_name = _name;
            this.guid = Guid.NewGuid().ToString().ToUpper().Trim();
            xml_head = String.Format("<{0} Name=\"{1}\" ID=\"{{{2}}}\">", u_type, unit_name, guid);
            xml_tail = String.Format("</{0}>", u_type);
        }
    }
}
