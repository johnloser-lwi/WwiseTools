using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public class WwiseContainer : WwiseActorMixer
    {
        public WwiseContainer(string name, string id, string type) : base(name, id, type)
        {
        }

        /// <summary>
        /// 设置播放延迟
        /// </summary>
        /// <param name="delay"></param>
        public void SetInitialDelay(float delay)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_InitialDelay(delay));
        }


        /// <summary>
        /// 添加子对象
        /// </summary>
        /// <param name="wwiseObject"></param>
        public void AddChild(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return;
            WwiseUtility.MoveToParent(wwiseObject, this);
        }
    }
}
