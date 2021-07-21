using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Properties
{
    public class WwiseProperty
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public WwiseProperty(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
