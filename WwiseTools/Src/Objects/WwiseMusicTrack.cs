using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.References;
using WwiseTools.Objects;
using WwiseTools.Utils;


namespace WwiseTools.Objects
{
    public class WwiseMusicTrack : WwiseActorMixer
    {
        [Obsolete("use GetTrackLengthAsync() instead")]
        public float TrackLenghtMs { 
            get
            {
                var length = GetTrackLengthAsync();
                length.Wait();
                return length.Result * 1000;
            }
        }

        /// <summary>
        /// 创建一个音轨
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent_path"></param>
        [Obsolete("use CreateWwiseMusicTrackAsync() instead")]
        public WwiseMusicTrack(string name, WwiseMusicSegment parent) : base(name, "", ObjectType.MusicTrack.ToString())
        {

            var tempObj = WwiseUtility.Instance.CreateObject(name, ObjectType.MusicTrack, parent.Path);
            WwiseUtility.Instance.ChangeObjectName(tempObj, name);
            ID = tempObj.ID;
            Name = tempObj.Name;
            parent.SetExitCue(TrackLenghtMs);
        }

        public static async Task<WwiseMusicTrack> CreateWwiseMusicTrackAsync(string name, WwiseMusicSegment parent)
        {
            var tempObj = await WwiseUtility.Instance.CreateObjectAsync(name, ObjectType.MusicTrack, await parent.GetPathAsync());
            var musicTrack = new WwiseMusicTrack(tempObj);
            await parent.SetExitCueAsync(await musicTrack.GetTrackLengthAsync());
            return musicTrack;
        }

        /// <summary>
        /// 创建一个音轨，配置导入选项
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filePath"></param>
        /// <param name="subFolder"></param>
        /// <param name="parent"></param>
        [Obsolete("use CreateWwiseMusicTrackAsync() instead")]
        public WwiseMusicTrack(string name, string filePath, WwiseMusicSegment parent, string subFolder = "") : base(name, "", ObjectType.MusicTrack.ToString())
        {
            var tempObj = WwiseUtility.Instance.ImportSound(filePath, "SFX", subFolder, parent.Path);
            WwiseUtility.Instance.ChangeObjectName(tempObj, name);
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
        public async Task<float> GetTrackLengthAsync()
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return 0;


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
                    var func = WaapiFunction.CoreObjectGet;

                    JObject jresult = await WwiseUtility.Instance.Client.Call(func, query, options, WwiseUtility.Instance.TimeOut);
                    
                    if (jresult["return"].Last["audioSource:maxDurationSource"] == null) throw new Exception();

                    float duration = float.Parse(jresult["return"].Last["audioSource:maxDurationSource"]["trimmedDuration"].ToString());

                    WaapiLog.Log($"Duration of WwiseObject {Name} is {duration}s");

                    return duration;

                }
                catch (Exception e)
                {
                    WaapiLog.Log($"Failed to return file path of Object : {Name}! ======> {e.Message}");
                    return -1;
                }
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return file path of Object : {Name}! ======> {e.Message}");
                return -1;
            }
        }

        /// <summary>
        /// 设置流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="nonCachable"></param>
        /// <param name="zeroLatency"></param>
        [Obsolete("Use async version instead")]
        public void SetStream(bool stream, bool nonCachable, bool zeroLatency, uint lookAheadTime = 100, uint prefetchLength = 100)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_IsStreamingEnabled(stream));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_IsNonCachable(nonCachable));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_IsZeroLantency(zeroLatency));

            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_LookAheadTime(lookAheadTime));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_PreFetchLength(prefetchLength));

        }
        
        public async Task SetStreamAsync(bool stream, bool nonCachable, bool zeroLatency, uint lookAheadTime = 100, uint prefetchLength = 100)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_IsStreamingEnabled(stream));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_IsNonCachable(nonCachable));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_IsZeroLantency(zeroLatency));
            
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_LookAheadTime(lookAheadTime));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_PreFetchLength(prefetchLength));

        }

        /// <summary>
        /// 设置音轨类型
        /// </summary>
        /// <param name="type"></param>
        [Obsolete("Use async version instead")]
        public void SetTrackType(WwiseProperty.Option_MusicTrackType type)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_MusicTrackType(type));
        }
        
        public async Task SetTrackTypeAsync(WwiseProperty.Option_MusicTrackType type)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_MusicTrackType(type));
        }

        /// <summary>
        /// 设置Switch Group 或者 State Group 引用
        /// </summary>
        /// <param name="group"></param>
        [Obsolete("Use async version instead")]
        public void SetSwitchGroupOrStateGroup(WwiseReference group)
        {
            WwiseUtility.Instance.SetObjectReference(this, group);
        }
        
        public async Task SetSwitchGroupOrStateGroupAsync(WwiseReference group)
        {
            await WwiseUtility.Instance.SetObjectReferenceAsync(this, group);
        }

        /// <summary>
        /// 设置默认的Switch或者State
        /// </summary>
        /// <param name="switchOrState"></param>
        [Obsolete("Use async version instead")]
        public void SetDefaultSwitchOrState(WwiseReference switchOrState)
        {
            WwiseUtility.Instance.SetObjectReference(this, switchOrState);
        }
        
        public async Task SetDefaultSwitchOrStateAsync(WwiseReference switchOrState)
        {
            await WwiseUtility.Instance.SetObjectReferenceAsync(this, switchOrState);
        }
    }
}
