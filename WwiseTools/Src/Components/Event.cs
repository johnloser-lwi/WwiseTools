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
    
    public async Task<Components.Action> AddActionAsync(WwiseObject target, Action.ActionType type)
    {
        var action = await WwiseUtility.Instance.CreateObjectAsync("", WwiseObject.ObjectType.Action, WwiseObject);

        if (action == null) return null;

        var comp = action.AsAction();
        await comp.SetTargetAsync(target);
        await comp.SetActionTypeAsync(type);

        return comp;
    }
}