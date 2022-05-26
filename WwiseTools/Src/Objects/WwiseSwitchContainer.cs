using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.References;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
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

        public WwiseSwitchContainer(WwiseObject @object) : base("", "", "")
        {
            if (@object == null) return;
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;
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
        /// 分配子对象至State或者Switch，后台运行
        /// </summary>
        /// <param name="child"></param>
        /// <param name="stateOrSwitch"></param>
        /// <returns></returns>
        public async Task AssignChildToStateOrSwitchAsync(WwiseObject child, WwiseObject stateOrSwitch)
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return;


            if (child == null || stateOrSwitch == null) return;



            foreach (var assignment in (await GetAssignmentsAsync())["return"])
            {
                if (assignment["stateOrSwitch"].ToString() == stateOrSwitch.ID && assignment["child"].ToString() == child.ID)
                {
                    WaapiLog.Log($"Child {child.Name} has already been assigned to {stateOrSwitch.Type} : {stateOrSwitch.Name}!");
                    return;
                }
            }

            try
            {
                var func = WwiseUtility.Instance.Function.Verify("ak.wwise.core.switchContainer.addAssignment");

                // 创建物体
                var result = await WwiseUtility.Instance.Client.Call
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
        /// 从State或者Switch删除分配的子对象，后台运行
        /// </summary>
        /// <param name="child"></param>
        /// <param name="stateOrSwitch"></param>
        public async Task RemoveAssignedChildFromStateOrSwitchAsync(WwiseObject child, WwiseObject stateOrSwitch)
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return;


            if (child == null || stateOrSwitch == null) return;

            try
            {
                var func = WwiseUtility.Instance.Function.Verify("ak.wwise.core.switchContainer.removeAssignment");

                // 创建物体
                var result = await WwiseUtility.Instance.Client.Call
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
        /// 获取分配信息，后台运行
        /// </summary>
        /// <returns></returns>
        public async Task<JObject> GetAssignmentsAsync()
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return null;


            try
            {
                var func = WwiseUtility.Instance.Function.Verify("ak.wwise.core.switchContainer.getAssignments");

                // 获取信息
                var result = await WwiseUtility.Instance.Client.Call
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
    }
}
