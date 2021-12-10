using System;
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
        /// <param name="parent_path"></param>
        public WwiseRandomSequenceContainer(string name, WwiseProperty.Option_RandomOrSequence option, string parent_path = @"\Actor-Mixer Hierarchy\Default Work Unit") : base(name, "", WwiseObject.ObjectType.RandomSequenceContainer.ToString())
        {
            var tempObj = WwiseUtility.CreateObject(name, ObjectType.RandomSequenceContainer, parent_path);
            ID = tempObj.ID;
            Name = tempObj.Name;
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_RandomOrSequence(option));
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

        public WwiseProperty.Option_RandomOrSequence GetPlayType()
        {
            var result = WwiseUtility.GetWwiseObjectProperty(ID, "RandomOrSequence").ToString();

            return (WwiseProperty.Option_RandomOrSequence)int.Parse(result);
        }


        /// <summary>
        /// 设置序列的范围为全局或者对象
        /// </summary>
        /// <param name="option"></param>
        public void SetScope(WwiseProperty.Option_GlobalOrPerObject option = WwiseProperty.Option_GlobalOrPerObject.Global)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_GlobalOrPerObject(option));
        }

        /// <summary>
        /// 设置播放模式连续或者步进
        /// </summary>
        /// <param name="mode"></param>
        public void SetPlayMode(WwiseProperty.Option_PlayMechanismStepOrContinuous mode)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismStepOrContinuous(mode));
        }

        /// <summary>
        /// 设置循环
        /// </summary>
        /// <param name="loop"></param>
        /// <param name="loop_count"></param>
        /// <param name="mode"></param>
        public void SetLoop(bool loop, uint loop_count, WwiseProperty.Option_PlayMechanismInfiniteOrNumberOfLoops mode)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismLoop(loop));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismInfiniteOrNumberOfLoops(mode));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismLoopCount(loop_count));

        }

        /// <summary>
        /// 设置衔接模式
        /// </summary>
        /// <param name="transitions"></param>
        /// <param name="type"></param>
        /// <param name="duration"></param>
        public void SetTransitions(bool transitions, WwiseProperty.Option_PlayMechanismSpecialTransitionsType type, float duration)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismSpecialTransitions(transitions));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismSpecialTransitionsType(type));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismSpecialTransitionsValue(duration));
        }
    }
}
