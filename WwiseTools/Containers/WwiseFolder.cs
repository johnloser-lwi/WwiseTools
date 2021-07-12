using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools
{
    public class WwiseFolder : WwiseContainer
    {
        public WwiseFolder(string _name) : base(_name, "Folder")
        {
        }

        public WwiseFolder(string _name, string guid) : base(_name, "Folder", guid)
        {
        }
    }
}
