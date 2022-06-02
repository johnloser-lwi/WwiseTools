using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Objects;

namespace WwiseTools.Src.Components
{
    public abstract class ComponentBase
    {
        protected WwiseObject _wwiseObject;

        public ComponentBase(WwiseObject wwiseObject)
        {
            _wwiseObject = wwiseObject;
        }
    }
}
