using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.WwiseTypes
{
    public class MusicSwitchContainer : SwitchContainerBase
    {
        public async Task SetContinuePlayAsync(bool value)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_ContinuePlay(value));
        }

        public MusicSwitchContainer(WwiseObject wwiseObject) : base(wwiseObject, nameof(MusicSwitchContainer))
        {
        }
    }
}
