using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Basic
{
    public class WwiseNodeWithName :WwiseNode
    {
        public WwiseNodeWithName(string u_type, string name) : base (u_type)
        {
            this.u_type = u_type;
            xml_head = String.Format("<{0} Name=\"{1}\">", u_type, name);
            xml_tail = String.Format("</{0}>", u_type);
            xml_body = new List<IWwisePrintable>();
        }

        public WwiseNodeWithName(string u_type, string name, IWwisePrintable child) : base(u_type)
        {
            this.u_type = u_type;
            xml_head = String.Format("<{0} Name=\"{1}\">", u_type, name);
            xml_tail = String.Format("</{0}>", u_type);
            xml_body = new List<IWwisePrintable>();

            AddChildNode(child);
        }

        public WwiseNodeWithName(string u_type, string name, List<IWwisePrintable> children) : base(u_type)
        {
            this.u_type = u_type;
            xml_head = String.Format("<{0} Name=\"{1}\">", u_type, name);
            xml_tail = String.Format("</{0}>", u_type);
            xml_body = new List<IWwisePrintable>();

            foreach (var c in children)
            {
                AddChildNode(c);
            }
        }
    }
}
