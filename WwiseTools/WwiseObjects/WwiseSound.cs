using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Reference;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public class WwiseSound : WwiseActorMixer
    {
        /// <summary>
        /// 创建一个Wwise Sound对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent_path"></param>
        public WwiseSound(string name, string parent_path =@"\Actor-Mixer Hierarchy\Default Work Unit") : base(name, "", "Sound")
        {
            var tempObj = WwiseUtility.CreateObject(name, ObjectType.Sound, parent_path);
            ID = tempObj.ID;
            Name = tempObj.Name;
        }

        internal WwiseSound(string name, string id, string type) : base(name, id, type)
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
        /// 设置循环
        /// </summary>
        /// <param name="loop"></param>
        /// <param name="infinite"></param>
        /// <param name="numOfLoop"></param>
        public void SetLoop(bool loop, bool infinite = true , uint numOfLoop = 2)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_IsLoopingEnabled(loop));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_IsLoopingInfinite(infinite));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_LoopCount(numOfLoop));
        }
        
        /// <summary>
        /// 设置流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="non_cachable"></param>
        /// <param name="zero_latency"></param>
        public void SetStream(bool stream, bool non_cachable, bool zero_latency)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_IsStreamingEnabled(stream));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_IsNonCachable(non_cachable));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_IsZeroLantency(zero_latency));
        }

    }
}
