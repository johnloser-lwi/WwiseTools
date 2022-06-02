using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Components
{
    public class WwiseMusicSwitchContainerComponent : ComponentBase
    {
        public async Task SetContinuePlayAsync(bool value)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_ContinuePlay(value));
        }

        public WwiseMusicSwitchContainerComponent(WwiseObject wwiseObject) : base(wwiseObject)
        {
        }
    }
}
