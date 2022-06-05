using WwiseTools.Objects;

namespace WwiseTools.Components
{
    public abstract class ComponentBase
    {
        public WwiseObject WwiseObject { get; protected set; }

        protected ComponentBase(WwiseObject wwiseObject)
        {
            WwiseObject = wwiseObject;
        }
    }
}
