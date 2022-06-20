using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Components;
using WwiseTools.Properties;
using WwiseTools.References;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    [Obsolete]
    public class WwiseSwitchContainer : WwiseContainer
    {

        /// <summary>
        /// 创建一个转变容器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentPath"></param>
        [Obsolete("use WwiseUtility.Instance.CreateObjectAsync instead")]
        public WwiseSwitchContainer(string name, string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit") : base(name, "", WwiseObject.ObjectType.SwitchContainer.ToString())
        {
            var tempObj = WwiseUtility.Instance.CreateObject(name, ObjectType.SwitchContainer, parentPath);
            ID = tempObj.ID;
            Name = tempObj.Name;
        }

        [Obsolete]

        public WwiseSwitchContainer(WwiseObject wwiseObject) : base("", "", "")
        {
            if (wwiseObject == null) return;
            ID = wwiseObject.ID;
            Name = wwiseObject.Name;
            Type = wwiseObject.Type;
        }


        /// <summary>
        /// 设置播放模式
        /// </summary>
        /// <param name="behavior"></param>
        [Obsolete("use async version instead")]
        public void SetPlayMode(WwiseProperty.Option_SwitchBehavior behavior)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_SwitchBehavior(behavior));
        }

        [Obsolete]
        public async Task SetPlayModeAsync(WwiseProperty.Option_SwitchBehavior behavior)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_SwitchBehavior(behavior));
        }

        /// <summary>
        /// 设置Switch Group 或者 State Group 引用
        /// </summary>
        /// <param name="group"></param>
        [Obsolete("use async version instead")]
        public void SetSwitchGroupOrStateGroup(WwiseReference group)
        {
            WwiseUtility.Instance.SetObjectReference(this, group);
        }

        [Obsolete]
        public async Task SetSwitchGroupOrStateGroupAsync(WwiseReference group)
        {
            await WwiseUtility.Instance.SetObjectReferenceAsync(this, group);
        }

        /// <summary>
        /// 设置默认的Switch或者State
        /// </summary>
        /// <param name="switchOrState"></param>
        
        [Obsolete("use async version instead")]
        public void SetDefaultSwitchOrState(WwiseReference switchOrState)
        {
            WwiseUtility.Instance.SetObjectReference(this, switchOrState);
        }

        [Obsolete]
        public async Task SetDefaultSwitchOrStateAsync(WwiseReference switchOrState)
        {
            await WwiseUtility.Instance.SetObjectReferenceAsync(this, switchOrState);
        }

        /// <summary>
        /// 分配子对象至State或者Switch
        /// </summary>
        /// <param name="child"></param>
        /// <param name="stateOrSwitch"></param>
        [Obsolete("use async version instead")]
        public void AssignChildToStateOrSwitch(WwiseObject child, WwiseObject stateOrSwitch)
        {
            var temp = AssignChildToStateOrSwitchAsync(child, stateOrSwitch);
            temp.Wait();
        }

        /// <summary>
        /// 分配子对象至State或者Switch，异步执行
        /// </summary>
        /// <param name="child"></param>
        /// <param name="stateOrSwitch"></param>
        /// <returns></returns>
        [Obsolete]
        public async Task AssignChildToStateOrSwitchAsync(WwiseObject child, WwiseObject stateOrSwitch)
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return;


            if (child == null || stateOrSwitch == null) return;



            foreach (var assignment in await GetSwitchAssignmentsAsync())
            {
                if (assignment.AssignedSwitch == stateOrSwitch && assignment.Child == child)
                {
                    WaapiLog.Log($"Child {child.Name} has already been assigned to {stateOrSwitch.Type} : {stateOrSwitch.Name}!");
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
                WaapiLog.Log($"Failed to assign {child.Name} to {stateOrSwitch}! ======> {e.Message}");
            }
        }

        /// <summary>
        /// 从State或者Switch删除分配的子对象
        /// </summary>
        /// <param name="child"></param>
        /// <param name="stateOrSwitch"></param>
        [Obsolete("Use async version instead")]
        public void RemoveAssignedChildFromStateOrSwitch(WwiseObject child, WwiseObject stateOrSwitch)
        {
            var temp = RemoveAssignedChildFromStateOrSwitchAsync(child, stateOrSwitch);
            temp.Wait();
        }

        /// <summary>
        /// 从State或者Switch删除分配的子对象，异步执行
        /// </summary>
        /// <param name="child"></param>
        /// <param name="stateOrSwitch"></param>
        [Obsolete]
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
                WaapiLog.Log($"Failed to assign {child.Name} to {stateOrSwitch}! ======> {e.Message}");
            }
        }

        /// <summary>
        /// 获取分配信息
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public JObject GetAssignments()
        {
            var temp = GetAssignmentsAsync();
            temp.Wait();
            return temp.Result;
        }

        /// <summary>
        /// 获取分配信息，异步执行
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use GetSwitchAssignmentAsync instead")]
        public async Task<JObject> GetAssignmentsAsync()
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync() ||
                Type != "SwitchContainer") return null;


            try
            {
                var func = WwiseUtility.Instance.Function.Verify("ak.wwise.core.switchContainer.getAssignments");

                // 获取信息
                var result = await WwiseUtility.Instance.CallAsync
                    (
                    func,
                    new JObject
                    {
                        new JProperty("id", ID),
                    },
                    null
                    );
                return result;
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to get assignment of {Name}! ======> {e.Message}");
                return null;
            }
        }

        [Obsolete]
        public async Task<List<SwitchAssignment>> GetSwitchAssignmentsAsync()
        {
            var result = new List<SwitchAssignment>();

            if (!await WwiseUtility.Instance.TryConnectWaapiAsync() || 
                Type != "SwitchContainer") return result;


            try
            {
                var func = WwiseUtility.Instance.Function.Verify("ak.wwise.core.switchContainer.getAssignments");

                // 获取信息
                var jresult = await WwiseUtility.Instance.CallAsync
                (
                    func,
                    new JObject
                    {
                        new JProperty("id", ID),
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
                WaapiLog.Log($"Failed to get assignment of {Name}! ======> {e.Message}");

            }

            return result;
        }
    }

    
}
