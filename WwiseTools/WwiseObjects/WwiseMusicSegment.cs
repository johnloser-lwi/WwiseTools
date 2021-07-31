using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public class WwiseMusicSegment : WwiseActorMixer
    {
        /// <summary>
        /// 创建一个音乐片段
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent_path"></param>
        public WwiseMusicSegment(string name, string parent_path = @"\Interactive Music Hierarchy\Default Work Unit\") : base(name, "", ObjectType.MusicSegment.ToString())
        {
            var segment = WwiseUtility.CreateObject(name, ObjectType.MusicSegment, parent_path);
            ID = segment.ID;
            Name = segment.Name;
        }

        /// <summary>
        /// 创建一个音乐片段，配置导入选项
        /// </summary>
        /// <param name="name"></param>
        /// <param name="file_path"></param>
        /// <param name="sub_folder"></param>
        /// <param name="parent_path"></param>
        public WwiseMusicSegment(string name, string file_path, string sub_folder = "", string parent_path = @"\Interactive Music Hierarchy\Default Work Unit") : base(name, "", ObjectType.MusicSegment.ToString())
        {
            var segment = WwiseUtility.CreateObject(name, ObjectType.MusicSegment, parent_path);
            parent_path = segment.Path;
            ID = segment.ID;
            Name = name;

            var tempObj = WwiseUtility.ImportSound(file_path, "SFX", sub_folder, segment.Path);
            
        }
        public WwiseMusicSegment(WwiseObject @object) : base("", "", "")
        {
            if (@object == null) return;
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;
        }


        /// <summary>
        /// 设置BPM和拍号
        /// </summary>
        /// <param name="tempo"></param>
        /// <param name="time_signature_lower"></param>
        /// <param name="time_signature_upper"></param>
        public void SetTempoAndTimeSignature(float tempo, WwiseProperty.Option_TimeSignatureLower time_signature_lower, uint time_signature_upper)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_Tempo(tempo));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_TimeSignatureLower(time_signature_lower));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_TimeSignatureUpper(time_signature_upper));
        }


    }
}
