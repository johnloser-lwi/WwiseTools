using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.References;
using WwiseTools.Utils;

namespace WwiseTools.Components
{
    public class SwitchGroup : ComponentBase
    {

        public async Task SetSwitchGroupOrStateGroupAsync(WwiseReference group)
        {
            await WwiseUtility.Instance.SetObjectReferenceAsync(WwiseObject, group);
        }

        public async Task SetDefaultSwitchOrStateAsync(WwiseReference switchOrState)
        {
            await WwiseUtility.Instance.SetObjectReferenceAsync(WwiseObject, switchOrState);
        }

        public SwitchGroup(WwiseObject wwiseObject) : base(wwiseObject, "SwitchGroup")
        {
        }
    }
}
