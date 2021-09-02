using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

using AK.Wwise.Waapi;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Reference;
using WwiseTools.Objects;
using WwiseTools.Utils;


namespace WwiseTools.Objects
{
    public class WwiseMusicTrack : WwiseActorMixer
    {
        public float TrackLenghtMs { 
            get
            {
                var length = GetTrackLength();
                length.Wait();
                return length.Result * 1000;
            }
        }

        /// <summary>
        /// 创建一个音轨
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent_path"></param>
        public WwiseMusicTrack(string name, WwiseMusicSegment parent) : base(name, "", ObjectType.MusicTrack.ToString())
        {

            var tempObj = WwiseUtility.CreateObject(name, ObjectType.MusicTrack, parent.Path);
            WwiseUtility.ChangeObjectName(tempObj, name);
            ID = tempObj.ID;
            Name = tempObj.Name;
            parent.SetExitCue(TrackLenghtMs);
        }

        /// <summary>
        /// 创建一个音轨，配置导入选项
        /// </summary>
        /// <param name="name"></param>
        /// <param name="file_path"></param>
        /// <param name="sub_folder"></param>
        /// <param name="parent"></param>
        public WwiseMusicTrack(string name, string file_path, WwiseMusicSegment parent, string sub_folder = "") : base(name, "", ObjectType.MusicTrack.ToString())
        {
            var tempObj = WwiseUtility.ImportSound(file_path, "SFX", sub_folder, parent.Path);
            WwiseUtility.ChangeObjectName(tempObj, name);
            ID = tempObj.ID;
            Name = tempObj.Name;
            parent.SetExitCue(TrackLenghtMs);
        }

        public WwiseMusicTrack(WwiseObject @object) : base("", "", "")
        {
            if (@object == null) return;
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;
        }

        /// <summary>
        /// 获取轨道长度，同步执行
        /// </summary>
        /// <returns></returns>
        private async Task<float> GetTrackLength()
        {
            try
            {
                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        id = new string[] { ID }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {
                    @return = new string[] { "audioSource:maxDurationSource" }
                };



                try // 尝试返回物体数据
                {

                    JObject jresult = await WwiseUtility.Client.Call(ak.wwise.core.@object.get, query, options);
                    
                    if (jresult["return"].Last["audioSource:maxDurationSource"] == null) throw new Exception();

                    float duration = float.Parse(jresult["return"].Last["audioSource:maxDurationSource"]["trimmedDuration"].ToString());

                    Console.WriteLine($"Duration of WwiseObject {Name} is {duration}s");

                    return duration;

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to return file path of Object : {Name}! ======> {e.Message}");
                    return -1;
                }
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to return file path of Object : {Name}! ======> {e.Message}");
                return -1;
            }
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
