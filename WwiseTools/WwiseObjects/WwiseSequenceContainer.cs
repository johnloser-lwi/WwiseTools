using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public class WwiseSequenceContainer : WwiseRandomSequenceContainer
    {
        public WwiseSequenceContainer(string name, string parent_path = @"\Actor-Mixer Hierarchy\Default Work Unit") : base(name, "", WwiseObject.ObjectType.RandomSequenceContainer.ToString())
        {
            var tempObj = WwiseUtility.CreateObject(name, ObjectType.RandomSequenceContainer, parent_path);
            ID = tempObj.ID;
            Name = tempObj.Name;
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_RandomOrSequence(WwiseProperty.Option_RandomOrSequence.Sequence));
        }

        public WwiseSequenceContainer(WwiseObject @object) : base("", "", "")
        {
            if (@object == null) return;
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;
        }

        /// <summary>
        /// 设置序列结束后的行为
        /// </summary>
        /// <param name="option"></param>
        public void SetSequenceEndBehavior(WwiseProperty.Option_RestartBeginningOrBackward option)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_RestartBeginningOrBackward(option));
        }

        /// <summary>
        /// 设置连续模式下是否重置播放列表
        /// </summary>
        /// <param name="reset"></param>
        public void SetAlwaysResetPlaylist(bool reset)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismResetPlaylistEachPlay(reset));
        }
    }
}
