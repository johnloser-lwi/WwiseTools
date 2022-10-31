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

    public async Task SetScopeAsync(WwiseProperty.Option_Scope scope)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_Scope(scope));
    }

    public async Task SetDelayAsync(float value)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_ActionDelay(value));
    }
    
    public async Task SetFadeTimeAsync(float value)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_FadeTime(value));
    }

    public async Task SetFadeInCurveAsync(WwiseProperty.Option_Curve curve)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_FadeInCurve(curve));
    }
    
    public async Task SetFadeOutCurveAsync(WwiseProperty.Option_Curve curve)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_FadeOutCurve(curve));
    }
}