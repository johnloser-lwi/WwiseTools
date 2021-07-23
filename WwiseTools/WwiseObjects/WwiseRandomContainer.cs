using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public class WwiseRandomContainer : WwiseRandomSequenceContainer
    {
        public WwiseRandomContainer(string name, string parent_path = @"\Actor-Mixer Hierarchy\Default Work Unit") : base(name, "", "RandomSequenceContainer")
        {
            var tempObj = WwiseUtility.CreateObject(name, ObjectType.RandomSequenceContainer, parent_path);
            ID = tempObj.ID;
            Name = tempObj.Name;
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_RandomOrSequence(WwiseProperty.Option_RandomOrSequence.Random));
        }


        /// <summary>
        /// 设置随机模式
        /// </summary>
        /// <param name="option"></param>
        public void SetRandomMode(WwiseProperty.Option_NormalOrShuffle option)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_NormalOrShuffle(option));
        }
    }
}
