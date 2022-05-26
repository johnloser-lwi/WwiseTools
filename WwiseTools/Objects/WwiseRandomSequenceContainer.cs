﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public class WwiseRandomSequenceContainer : WwiseContainer
    {
        /// <summary>
        /// 创建一个随机步进容器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="option"></param>
        /// <param name="parentPath"></param>
        [Obsolete("use WwiseUtility.CreateObjectAsync instead")]
        public WwiseRandomSequenceContainer(string name, WwiseProperty.Option_RandomOrSequence option, string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit") : base(name, "", WwiseObject.ObjectType.RandomSequenceContainer.ToString())
        {
            var tempObj = WwiseUtility.CreateObject(name, ObjectType.RandomSequenceContainer, parentPath);
            ID = tempObj.ID;
            Name = tempObj.Name;
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_RandomOrSequence(option));
        }

        public static async Task<WwiseRandomSequenceContainer> CreateWwiseRandomSequenceContainer(string name, WwiseProperty.Option_RandomOrSequence option, string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit")
        {
            var tempObj = await WwiseUtility.CreateObjectAsync(name, ObjectType.RandomSequenceContainer, parentPath);
            await WwiseUtility.SetObjectPropertyAsync(tempObj, WwiseProperty.Prop_RandomOrSequence(option));

            return new WwiseRandomSequenceContainer(tempObj);
        }

        public static async Task<WwiseRandomContainer> CreateWwiseRandomContainer(string name, string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit")
        {
            var tempObj = await CreateWwiseRandomSequenceContainer(name, WwiseProperty.Option_RandomOrSequence.Random, parentPath);
            return new WwiseRandomContainer(tempObj);
        }

        public static async Task<WwiseSequenceContainer> CreateWwiseSequenceContainer(string name, string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit")
        {
            var tempObj = await CreateWwiseRandomSequenceContainer(name, WwiseProperty.Option_RandomOrSequence.Sequence, parentPath);
            return new WwiseSequenceContainer(tempObj);
        }

        internal WwiseRandomSequenceContainer(string name, string id, string type) : base(name, id, type)
        { 
        }

        public WwiseRandomSequenceContainer(WwiseObject @object) : base("", "", "")
        {
            if (@object == null) return;
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;
        }

        [Obsolete("use async version instead")]
        public WwiseProperty.Option_RandomOrSequence GetPlayType()
        {
            var result = WwiseUtility.GetWwiseObjectProperty(ID, "RandomOrSequence").ToString();

            return (WwiseProperty.Option_RandomOrSequence)int.Parse(result);
        }

        public async Task< WwiseProperty.Option_RandomOrSequence> GetPlayTypeAsync()
        {
            var result = (await WwiseUtility.GetWwiseObjectPropertyAsync(ID, "RandomOrSequence")).ToString();

            return (WwiseProperty.Option_RandomOrSequence)int.Parse(result);
        }


        /// <summary>
        /// 设置序列的范围为全局或者对象
        /// </summary>
        /// <param name="option"></param>
        [Obsolete("use async version instead")]
        public void SetScope(WwiseProperty.Option_GlobalOrPerObject option = WwiseProperty.Option_GlobalOrPerObject.Global)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_GlobalOrPerObject(option));
        }

        public async Task SetScopeAsync(WwiseProperty.Option_GlobalOrPerObject option = WwiseProperty.Option_GlobalOrPerObject.Global)
        {
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_GlobalOrPerObject(option));
        }

        /// <summary>
        /// 设置播放模式连续或者步进
        /// </summary>
        /// <param name="mode"></param>
        [Obsolete("use async version instead")]
        public void SetPlayMode(WwiseProperty.Option_PlayMechanismStepOrContinuous mode)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismStepOrContinuous(mode));
        }

        public async Task SetPlayModeAsync(WwiseProperty.Option_PlayMechanismStepOrContinuous mode)
        {
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_PlayMechanismStepOrContinuous(mode));
        }

        /// <summary>
        /// 设置循环
        /// </summary>
        /// <param name="loop"></param>
        /// <param name="loopCount"></param>
        /// <param name="mode"></param>
        [Obsolete("use async version instead")]
        public void SetLoop(bool loop, uint loopCount, WwiseProperty.Option_PlayMechanismInfiniteOrNumberOfLoops mode)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismLoop(loop));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismInfiniteOrNumberOfLoops(mode));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismLoopCount(loopCount));

        }

        public async Task SetLoopAsync(bool loop, uint loopCount, WwiseProperty.Option_PlayMechanismInfiniteOrNumberOfLoops mode)
        {
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_PlayMechanismLoop(loop));
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_PlayMechanismInfiniteOrNumberOfLoops(mode));
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_PlayMechanismLoopCount(loopCount));

        }

        /// <summary>
        /// 设置衔接模式
        /// </summary>
        /// <param name="transitions"></param>
        /// <param name="type"></param>
        /// <param name="duration"></param>
        [Obsolete("use async version instead")]
        public void SetTransitions(bool transitions, WwiseProperty.Option_PlayMechanismSpecialTransitionsType type, float duration)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismSpecialTransitions(transitions));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismSpecialTransitionsType(type));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismSpecialTransitionsValue(duration));
        }

        public async Task SetTransitionsAsync(bool transitions, WwiseProperty.Option_PlayMechanismSpecialTransitionsType type, float duration)
        {
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_PlayMechanismSpecialTransitions(transitions));
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_PlayMechanismSpecialTransitionsType(type));
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_PlayMechanismSpecialTransitionsValue(duration));
        }
    }
}
