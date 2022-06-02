using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    [Obsolete]
    public class WwiseRandomContainer : WwiseRandomSequenceContainer
    {
        /// <summary>
        /// 创建一个随机容器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentPath"></param>
        [Obsolete("use WwiseUtility.Instance.CreateObjectAsync instead")]
        public WwiseRandomContainer(string name, string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit") : base(name, "", WwiseObject.ObjectType.RandomSequenceContainer.ToString())
        {
            var tempObj = WwiseUtility.Instance.CreateObject(name, ObjectType.RandomSequenceContainer, parentPath);
            ID = tempObj.ID;
            Name = tempObj.Name;
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_RandomOrSequence(WwiseProperty.Option_RandomOrSequence.Random));
        }

        public WwiseRandomContainer(WwiseObject @object) : base("", "", "")
        {
            if (@object == null) return;
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;
        }


        /// <summary>
        /// 设置随机模式
        /// </summary>
        /// <param name="option"></param>
        [Obsolete("use async version instead")]
        public void SetRandomMode(WwiseProperty.Option_NormalOrShuffle option)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_NormalOrShuffle(option));
        }

        public async  Task SetRandomModeAsync(WwiseProperty.Option_NormalOrShuffle option)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_NormalOrShuffle(option));
        }
    }
}
