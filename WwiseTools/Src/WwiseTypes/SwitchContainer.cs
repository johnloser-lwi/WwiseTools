using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Properties;
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

                // 获取信息
                var jresult = await WwiseUtility.Instance.CallAsync
                (
                    func,
                    new JObject
                    {
                        new JProperty("id", WwiseObject.ID),
                    },
                    null
                );

                var results = jresult["return"];
                if (results == null) return result;
                foreach (var token in results)
                {
                    string childID = token["child"]?.ToString();
                    string switchID = token["stateOrSwitch"]?.ToString();
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

                // 创建物体
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

        /// <summary>
        /// 分配子对象至State或者Switch，异步执行
        /// </summary>
        /// <param name="child"></param>
        /// <param name="stateOrSwitch"></param>
        /// <returns></returns>
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

                // 创建物体
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
