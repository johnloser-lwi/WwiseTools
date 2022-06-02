using WwiseTools.Components;

namespace WwiseTools.Objects
{
    public partial class WwiseObject
    {
        public WwiseSwitchContainerComponent SwitchContainer => new WwiseSwitchContainerComponent(this);
    }
}
