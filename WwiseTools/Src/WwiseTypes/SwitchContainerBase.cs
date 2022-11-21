using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.WwiseTypes
{
    public abstract class SwitchContainerBase : WwiseTypeBase
    {

        public async Task SetSwitchGroupOrStateGroupAsync(WwiseProperty group)
        {
            await WwiseUtility.Instance.SetObjectPropertiesAsync(WwiseObject, group);
        }

        public async Task SetDefaultSwitchOrStateAsync(WwiseProperty switchOrState)
        {
            await WwiseUtility.Instance.SetObjectPropertiesAsync(WwiseObject, switchOrState);
        }

        public SwitchContainerBase(WwiseObject wwiseObject, string typeFilter) : base(wwiseObject, typeFilter)
        {
        }
    }
}
