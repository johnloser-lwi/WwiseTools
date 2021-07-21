using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Utils;

namespace WwiseTools.Reference
{
    public class WwiseReference
    {
        public string Name { get; set; }
        public WwiseObject Object { get; set; }

        public WwiseReference(string name, WwiseObject @object)
        {
            Name = name;
            Object = @object;
        }
    }
}
