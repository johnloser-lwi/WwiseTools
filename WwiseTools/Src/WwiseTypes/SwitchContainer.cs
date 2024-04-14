using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Serialization;
using WwiseTools.Utils;

namespace WwiseTools.WwiseTypes
{
    public class SwitchContainer : SwitchContainerBase
    {
        public async Task SetPlayModeAsync(WwiseProperty.Option_SwitchBehavior behavior)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_SwitchBehavior(behavior));
        }

        public async Task<List<SwitchAssignment>> GetSwitchAssignmentsAsync()
        {
            var result = new List<SwitchAssignment>();

            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return result;


            try
            {
                var func = WwiseUtility.Instance.Function.Verify("ak.wwise.core.switchContainer.getAssignments");

              
                var jresult = await WwiseUtility.Instance.CallAsync
                (
                    func,
                    new JObject
                    {
                        new JProperty("id", WwiseObject.ID),
                    },
                    null
                );

                var returnData = WaapiSerializer.Deserialize<ReturnData<GetSwitchAssingmentsData>>(jresult.ToString());
                if (returnData.Return.Length == 0) return result;
                foreach (var token in returnData.Return)
                {
                    var childID = token.Child;
                    var switchID = token.StateOrSwitch;
                    if (string.IsNullOrEmpty(childID) ||
                        string.IsNullOrEmpty(switchID)) continue;

                    result.Add(new SwitchAssignment()
                    {
                        Child = await WwiseUtility.Instance.GetWwiseObjectByIDAsync(childID),
                        AssignedSwitch = await WwiseUtility.Instance.GetWwiseObjectByIDAsync(switchID)
                    });
                }

            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get assignment of {WwiseObject.Name}! ======> {e.Message}");

            }

            return result;
        }

        public async Task RemoveAssignedChildFromStateOrSwitchAsync(WwiseObject child, WwiseObject stateOrSwitch)
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return;


            if (child == null || stateOrSwitch == null) return;

            try
            {
                var func = WwiseUtility.Instance.Function.Verify("ak.wwise.core.switchContainer.removeAssignment");

              
                var result = await WwiseUtility.Instance.CallAsync
                (
                    func,
                    new JObject
                    {
                        new JProperty("child", child.ID),
                        new JProperty("stateOrSwitch", stateOrSwitch.ID),
                    },
                    null
                );
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to assign {child.Name} to {stateOrSwitch}! ======> {e.Message}");
            }
        }

      
      
      
      
      
      
        public async Task AssignChildToStateOrSwitchAsync(WwiseObject child, WwiseObject stateOrSwitch)
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return;


            if (child == null || stateOrSwitch == null) return;



            foreach (var assignment in await GetSwitchAssignmentsAsync())
            {
                if (assignment.AssignedSwitch == stateOrSwitch && assignment.Child == child)
                {
                    WaapiLog.InternalLog($"Child {child.Name} has already been assigned to {stateOrSwitch.Type} : {stateOrSwitch.Name}!");
                    return;
                }
            }

            try
            {
                var func = WwiseUtility.Instance.Function.Verify("ak.wwise.core.switchContainer.addAssignment");

              
                var result = await WwiseUtility.Instance.CallAsync
                (
                    func,
                    new JObject
                    {
                        new JProperty("child", child.ID),
                        new JProperty("stateOrSwitch", stateOrSwitch.ID),
                    },
                    null
                );
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to assign {child.Name} to {stateOrSwitch}! ======> {e.Message}");
            }
        }

        public SwitchContainer(WwiseObject wwiseObject) : base(wwiseObject, nameof(SwitchContainer))
        {
        }

    }
    public class SwitchAssignment
    {
        public WwiseObject Child { get; set; }
        public WwiseObject AssignedSwitch { get; set; }

        public async Task<WwiseObject> GetSwitchGroupAsync()
        {
            return await AssignedSwitch.GetParentAsync();
        }
    }
}
