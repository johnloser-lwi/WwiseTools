using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.References;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public class WwiseSound : WwiseActorMixer
    {
        /// <summary>
        /// 创建一个Wwise Sound对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentPath"></param>
        [Obsolete("use WwiseUtility.CreateObjectAsync instead")]
        public WwiseSound(string name, string parentPath =@"\Actor-Mixer Hierarchy\Default Work Unit") : base(name, "", "Sound")
        {
            var tempObj = WwiseUtility.CreateObject(name, ObjectType.Sound, parentPath);
            ID = tempObj.ID;
            Name = tempObj.Name;
        }

        /// <summary>
        /// 创建一个Wwise Sound对象，设置包含的音频文件信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filePath"></param>
        /// <param name="language"></param>
        /// <param name="subFolder"></param>
        /// <param name="parentPath"></param>
        [Obsolete("Use WwiseUtility.ImportSoundAsync instead")]
        public WwiseSound(string name, string filePath, string language = "SFX", string subFolder = "", string parentPath = @"\Actor-Mixer Hierachy\Default Work Unit") : base(name, "", "Sound")
        {
            var tempObj = WwiseUtility.ImportSound(filePath, language, subFolder, parentPath);
            WwiseUtility.ChangeObjectName(tempObj, name);
            ID = tempObj.ID;
            Name = tempObj.Name;
        }

        public WwiseSound(WwiseObject @object) : base("", "", "")
        {
            if (@object == null) return;
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;
        }

        /// <summary>
        /// 设置播放延迟
        /// </summary>
        /// <param name="delay"></param>
        [Obsolete("use async version instead")]
        public void SetInitialDelay(float delay)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_InitialDelay(delay));
        }

        public async Task SetInitialDelayAsync(float delay)
        {
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_InitialDelay(delay));
        }

        /// <summary>
        /// 设置循环
        /// </summary>
        /// <param name="loop"></param>
        /// <param name="infinite"></param>
        /// <param name="numOfLoop"></param>
        [Obsolete("use async version instead")]
        public void SetLoop(bool loop, bool infinite = true , uint numOfLoop = 2)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_IsLoopingEnabled(loop));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_IsLoopingInfinite(infinite));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_LoopCount(numOfLoop));
        }
        
        public async Task SetLoopAsync(bool loop, bool infinite = true , uint numOfLoop = 2)
        {
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_IsLoopingEnabled(loop));
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_IsLoopingInfinite(infinite));
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_LoopCount(numOfLoop));
        }
        
        /// <summary>
        /// 设置流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="nonCachable"></param>
        /// <param name="zeroLatency"></param>
        [Obsolete("use async version instead")]
        public void SetStream(bool stream, bool nonCachable, bool zeroLatency)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_IsStreamingEnabled(stream));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_IsNonCachable(nonCachable));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_IsZeroLantency(zeroLatency));
        }

        public async Task SetStreamAsync(bool stream, bool nonCachable, bool zeroLatency)
        {
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_IsStreamingEnabled(stream));
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_IsNonCachable(nonCachable));
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_IsZeroLantency(zeroLatency));
        }

        [Obsolete("use async version instead")]
        public string[] GetWavSourceFilePath()
        {
            var r = GetWavFilePathAsync();
            r.Wait();
            List<string> paths = new List<string>();
            foreach (var result in r.Result["return"].Last)
            {
                paths.Add(result.Last.ToString());
            }
            return paths.ToArray();
        }

        public async Task<string[]> GetWavSourceFilePathAsync()
        {
            var r = await GetWavFilePathAsync();
            List<string> paths = new List<string>();
            foreach (var result in r["return"].Last)
            {
                paths.Add(result.Last.ToString());
            }
            return paths.ToArray();
        }

        private  async Task<JObject> GetWavFilePathAsync()
        {
            if (!await WwiseUtility.TryConnectWaapiAsync()) return null;


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

                    @return = new string[] { "sound:originalWavFilePath" }

                };

                try // 尝试返回物体数据
                {
                    var func = WaapiFunction.CoreObjectGet;

                    JObject jresult = await WwiseUtility.Client.Call(func, query, options);

                    return jresult;
                }
                catch
                {
                    WaapiLog.Log($"Failed to return WaveFilePath from ID : {ID}!");
                    return null;
                }
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return WaveFilePath from ID : {ID}! ======> {e.Message}");
                return null;
            }

        }

    }
}
