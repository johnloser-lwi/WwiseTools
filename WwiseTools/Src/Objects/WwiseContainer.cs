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
        [Obsolete("use async version instead")]
        public void SetInitialDelay(float delay)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_InitialDelay(delay));
        }

        public async Task SetInitialDelayAsync(float delay)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_InitialDelay(delay));
        }


        /// <summary>
        /// 添加子对象
        /// </summary>
        /// <param name="wwiseObject"></param>
        [Obsolete("use async version instead")]
        public void AddChild(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return;
            WwiseUtility.Instance.MoveToParent(wwiseObject, this);
        }
        
        public async Task AddChildAsync(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return;
            await WwiseUtility.Instance.MoveToParentAsync(wwiseObject, this);
        }
    }
}
