using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.References;
using WwiseTools.Utils;

namespace WwiseTools.Components;

public class Event : ComponentBase
{
    public Event(WwiseObject wwiseObject) : base(wwiseObject, "Event")
    {
    }

    public async Task<Components.Action> AddSetSwitchActionAsync(WwiseObject target,
        float delay = 0f)
    {
        if (target.Type != WwiseObject.ObjectType.Switch.ToString()) return null;
        
        var properties = new WwiseProperty[]
        {
            WwiseProperty.Prop_ActionDelay(delay)
        };

        return await AddActionAsync(target, WwiseProperty.Option_ActionType.SetSwitch, properties);
    }
    
    public async Task<Components.Action> AddSetStateActionAsync(WwiseObject target,
        float delay = 0f)
    {
        if (target.Type != WwiseObject.ObjectType.State.ToString()) return null;
        
        var properties = new WwiseProperty[]
        {
            WwiseProperty.Prop_ActionDelay(delay)
        };

        return await AddActionAsync(target, WwiseProperty.Option_ActionType.SetState, properties);
    }

    public async Task<Components.Action> AddPlayActionAsync(WwiseObject target, 
        float delay = 0f, 
        float probability = 100f, 
        float fadeTime = 0f, 
        WwiseProperty.Option_Curve fadeInCurve = WwiseProperty.Option_Curve.Linear)
    {
        var properties = new WwiseProperty[]
        {
            WwiseProperty.Prop_ActionDelay(delay),
            WwiseProperty.Prop_Probability(probability),
            WwiseProperty.Prop_FadeTime(fadeTime),
            WwiseProperty.Prop_FadeInCurve(fadeInCurve) 
        };

        return await AddActionAsync(target, WwiseProperty.Option_ActionType.Play, properties);
    }
    
    public async Task<Components.Action> AddStopActionAsync(WwiseObject target, 
        WwiseProperty.Option_Scope scope = WwiseProperty.Option_Scope.GameObject,
        float delay = 0f, 
        float probability = 100f, 
        float fadeTime = 0f, 
        WwiseProperty.Option_Curve fadeOutCurve = WwiseProperty.Option_Curve.Linear)
    {
        var properties = new WwiseProperty[]
        {
            WwiseProperty.Prop_Scope(scope),
            WwiseProperty.Prop_ActionDelay(delay),
            WwiseProperty.Prop_Probability(probability),
            WwiseProperty.Prop_FadeTime(fadeTime),
            WwiseProperty.Prop_FadeOutCurve(fadeOutCurve) 
        };

        return await AddActionAsync(target, WwiseProperty.Option_ActionType.Stop, properties);
    }

    
    public async Task<Components.Action> AddActionAsync(WwiseObject target, WwiseProperty.Option_ActionType type, params WwiseProperty[] properties)
    {
        var action = await WwiseUtility.Instance.CreateObjectAsync("", WwiseObject.ObjectType.Action, WwiseObject);

        if (action == null) return null;

        var comp = action.AsAction();
        await comp.SetTargetAsync(target);
        await comp.SetActionTypeAsync(type);

        foreach (var wwiseProperty in properties)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(action, wwiseProperty);
        }
        
        return comp;
    }

    public async Task<List<Action>> GetActionsAsync()
    {
        return (await WwiseObject.AsHierarchy().GetChildrenAsync()).Select(a => a.AsAction()).ToList();
    }

    public async Task<List<Action>> FindActionsByTarget(WwiseObject target)
    {
        return (await GetActionsAsync()).Where(a => a.WwiseObject == target).ToList();
    }
    
    public async Task<List<Action>> FindActionsByActionType(WwiseProperty.Option_ActionType type)
    {
        var actions = await GetActionsAsync();


        var res = new List<Action>();
        foreach (var action in actions)
        {
            if (await action.GetActionTypeAsync() == type)
                res.Add(action);
        }

        return res;
    }
}