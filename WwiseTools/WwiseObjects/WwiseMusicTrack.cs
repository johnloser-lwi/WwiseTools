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
    public class WwiseMusicTrack : WwiseActorMixer
    {
        /// <summary>
        /// 创建一个音轨
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent_path"></param>
        public WwiseMusicTrack(string name, string parent_path = @"\Interactive Music Hierarchy\Default Work Unit\") : base(name, "", ObjectType.MusicTrack.ToString())
        {
            string path = @"\Interactive Music Hierarchy\Default Work Unit\";
            if (parent_path == path)
            {
                var segment = WwiseUtility.CreateObject("New Music Segment", ObjectType.MusicSegment, path);
                parent_path = segment.Path;
            }

            var tempObj = WwiseUtility.CreateObject(name, ObjectType.MusicTrack, parent_path);
            ID = tempObj.ID;
            Name = tempObj.Name;
        }

        /// <summary>
        /// 创建一个音轨，配置导入选项
        /// </summary>
        /// <param name="name"></param>
        /// <param name="file_path"></param>
        /// <param name="sub_folder"></param>
        /// <param name="parent_path"></param>
        public WwiseMusicTrack(string name, string file_path, string sub_folder = "", string parent_path = @"\Interactive Music Hierarchy\Default Work Unit") : base(name, "", ObjectType.MusicTrack.ToString())
        {
            string path = @"\Interactive Music Hierarchy\Default Work Unit\";
            if (parent_path == path)
            {
                var segment = WwiseUtility.CreateObject("New Music Segment", ObjectType.MusicSegment, path);
                parent_path = segment.Path;
            }

            var tempObj = WwiseUtility.ImportSound(file_path, "SFX", sub_folder, parent_path);
            ID = tempObj.ID;
            Name = tempObj.Name;
        }

        public WwiseMusicTrack(WwiseObject @object) : base("", "", "")
        {
            if (@object == null) return;
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;
        }

        /// <summary>
        /// 设置流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="non_cachable"></param>
        /// <param name="zero_latency"></param>
        public void SetStream(bool stream, bool non_cachable, bool zero_latency, uint look_ahead_time = 100, uint prefetch_length = 100)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_IsStreamingEnabled(stream));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_IsNonCachable(non_cachable));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_IsZeroLantency(zero_latency));

            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_LookAheadTime(look_ahead_time));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PreFetchLength(prefetch_length));

        }

        /// <summary>
        /// 设置音轨类型
        /// </summary>
        /// <param name="type"></param>
        public void SetTrackType(WwiseProperty.Option_MusicTrackType type)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_MusicTrackType(type));
        }

        /// <summary>
        /// 设置Switch Group 或者 State Group 引用
        /// </summary>
        /// <param name="group"></param>
        public void SetSwitchGroupOrStateGroup(WwiseReference group)
        {
            WwiseUtility.SetObjectReference(this, group);
        }

        /// <summary>
        /// 设置默认的Switch或者State
        /// </summary>
        /// <param name="switch_or_state"></param>
        public void SetDefaultSwitchOrState(WwiseReference switch_or_state)
        {
            WwiseUtility.SetObjectReference(this, switch_or_state);
        }
    }
}
