using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Properties
{
    public class WwiseAudioSourceRef : WwiseProperty
    {
        public WwiseAudioSourceRef(string name, string id)
        {
            body = String.Format("<AudioSourceRef Name=\"{0}\" ID=\"{{{1}}}\"/>", name, id);
        }
    }
}
