using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.WwiseTypes;

public class Action : WwiseTypeBase
{
    public Action(WwiseObject wwiseObject) : base(wwiseObject, "Action")
    {
    }

    public async Task<WwiseProperty.Option_ActionType> GetActionTypeAsync()
    {
        var prop = await WwiseUtility.Instance.GetWwiseObjectPropertyAsync(WwiseObject, "ActionType");

        if (prop == null) return 0;
        
        var res = Enum.TryParse(prop.Value.ToString(), out WwiseProperty.Option_ActionType type);

        if (!res) return 0;
        
        return type;
    }

    public async Task<WwiseObject> GetTargetAsync()
    {
        var reference = await WwiseUtility.Instance.GetWwiseObjectPropertyAsync(WwiseObject, "Target");

        if (reference == null) return null;

        var ret = await WwiseUtility.Instance.GetWwiseObjectByIDAsync(reference.Value.ToString());

        return ret;
    }
    
    public async Task SetTargetAsync(WwiseObject target)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_Target(target));
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