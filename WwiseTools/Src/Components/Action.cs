using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.References;
using WwiseTools.Utils;

namespace WwiseTools.Components;

public class Action : ComponentBase
{
    public Action(WwiseObject wwiseObject) : base(wwiseObject, "Action")
    {
    }
    
    public async Task SetTargetAsync(WwiseObject target)
    {
        await WwiseUtility.Instance.SetObjectReferenceAsync(WwiseObject, WwiseReference.Ref_Target(target));
    }

    public async Task SetActionTypeAsync(WwiseProperty.Option_ActionType type)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_ActionType(type));
    }
}