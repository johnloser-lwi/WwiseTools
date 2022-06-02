using WwiseTools.Objects;

namespace WwiseTools.Components
{
    public abstract class ComponentBase
    {
        protected WwiseObject WwiseObject;

        public ComponentBase(WwiseObject wwiseObject)
        {
            WwiseObject = wwiseObject;
        }
    }
}
