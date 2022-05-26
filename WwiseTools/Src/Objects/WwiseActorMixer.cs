using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.References;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public class WwiseActorMixer : WwiseObject
    {
        /// <summary>
        /// 创建一个Wwise Actor-Mixer对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentPath"></param>
        [Obsolete("use WwiseUtility.Instance.CreateObjectAsync instead")]
        public WwiseActorMixer(string name, string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit") : base(name, "", "ActorMixer")
        {
            var tempObj = WwiseUtility.Instance.CreateObject(name, ObjectType.Sound, parentPath);
            ID = tempObj.ID;
            Name = tempObj.Name;
        }

        public WwiseActorMixer(WwiseObject @object) : base("", "", "")
        {
            if (@object == null) return;
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;
        }

        internal WwiseActorMixer(string name, string id, string type) : base(name, id, type)
        { 
        }

        /// <summary>
        /// 创建一个播放事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        [Obsolete("Use WwiseUtiliy.CreatPlayEventAsync instead")]
        public WwiseObject CreatePlayEvent(string name = "", string parentPath = @"\Events\Default Work Unit")
        {
            if (String.IsNullOrEmpty(name)) name = Name;

            return WwiseUtility.Instance.CreatePlayEvent(name, Path, parentPath);
        }

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="value"></param>
        [Obsolete("use async version instead")]
        public void SetVolume(float value)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_Volume(value));
        }
        
        public async Task SetVolumeAsync(float value)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_Volume(value));
        }

        /// <summary>
        /// 设置音高
        /// </summary>
        /// <param name="value"></param>
        [Obsolete("use async version instead")]
        public void SetPitch(int value)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_Pitch(value));
        }
        
        public async Task SetPitchAsync(int value)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_Pitch(value));
        }

        /// <summary>
        /// 设置滤波器
        /// </summary>
        /// <param name="highPass"></param>
        /// <param name="lowPass"></param>
        [Obsolete("use async version instead")]
        public void SetFilter(uint highPass, uint lowPass)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_Lowpass(lowPass));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_Highpass(highPass));
        }
        
        public async Task SetFilterAsync(uint highPass, uint lowPass)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_Lowpass(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_Highpass(highPass));
        }

        /// <summary>
        /// 设置游戏Aux发送
        /// </summary>
        /// <param name="busName"></param>
        /// <param name="volume"></param>
        /// <param name="lowPass"></param>
        /// <param name="highPass"></param>
        [Obsolete("use async version instead")]
        public void SetGameAuxSend(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OverrideGameAuxSends(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_UseGameAuxSends(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_GameAuxSendVolume(volume));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_GameAuxSendLPF(lowPass));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_GameAuxSendHPF(highPass));
        }
        
        public async Task SetGameAuxSendAsync(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OverrideGameAuxSends(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_UseGameAuxSends(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_GameAuxSendVolume(volume));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_GameAuxSendLPF(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_GameAuxSendHPF(highPass));
        }

        /// <summary>
        /// 设置用户Aux发送0
        /// </summary>
        /// <param name="busName"></param>
        /// <param name="volume"></param>
        /// <param name="lowPass"></param>
        /// <param name="highPass"></param>
        [Obsolete("use async version instead")]
        public void SetAuxilaryBus0(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OverrideUserAuxSends(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendVolume0(volume));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendLPF0(lowPass));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendHPF0(highPass));
            WwiseUtility.Instance.SetObjectReference(this, WwiseReference.Ref_UserAuxSend0(WwiseUtility.Instance.GetWwiseObjectByName($"AuxBus:{busName}")));
        }

        public async Task SetAuxilaryBus0Async(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OverrideUserAuxSends(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_UserAuxSendVolume0(volume));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_UserAuxSendLPF0(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_UserAuxSendHPF0(highPass));
            await WwiseUtility.Instance.SetObjectReferenceAsync(this, WwiseReference.Ref_UserAuxSend0(await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"AuxBus:{busName}")));
        }

        /// <summary>
        /// 设置用户Aux发送1
        /// </summary>
        /// <param name="busName"></param>
        /// <param name="volume"></param>
        /// <param name="lowPass"></param>
        /// <param name="highPass"></param>
        [Obsolete("use async version instead")]
        public void SetAuxilaryBus1(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OverrideUserAuxSends(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendVolume1(volume));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendLPF1(lowPass));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendHPF1(highPass));
            WwiseUtility.Instance.SetObjectReference(this, WwiseReference.Ref_UserAuxSend1(WwiseUtility.Instance.GetWwiseObjectByName($"AuxBus:{busName}")));
        }
        public async Task SetAuxilaryBus1Async(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OverrideUserAuxSends(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_UserAuxSendVolume1(volume));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_UserAuxSendLPF1(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_UserAuxSendHPF1(highPass));
            await WwiseUtility.Instance.SetObjectReferenceAsync(this, WwiseReference.Ref_UserAuxSend1(await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"AuxBus:{busName}")));
        }

        /// <summary>
        /// 设置用户Aux发送2
        /// </summary>
        /// <param name="busName"></param>
        /// <param name="volume"></param>
        /// <param name="lowPass"></param>
        /// <param name="highPass"></param>
        [Obsolete("use async version instead")]
        public void SetAuxilaryBus2(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OverrideUserAuxSends(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendVolume2(volume));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendLPF2(lowPass));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendHPF2(highPass));
            WwiseUtility.Instance.SetObjectReference(this, WwiseReference.Ref_UserAuxSend2(WwiseUtility.Instance.GetWwiseObjectByName($"AuxBus:{busName}")));
        }

        public async Task SetAuxilaryBus2Async(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OverrideUserAuxSends(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_UserAuxSendVolume2(volume));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_UserAuxSendLPF2(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_UserAuxSendHPF2(highPass));
            await WwiseUtility.Instance.SetObjectReferenceAsync(this, WwiseReference.Ref_UserAuxSend2(await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"AuxBus:{busName}")));
        }

        /// <summary>
        /// 设置用户Aux发送3
        /// </summary>
        /// <param name="busName"></param>
        /// <param name="volume"></param>
        /// <param name="lowPass"></param>
        /// <param name="highPass"></param>
        [Obsolete("use async version instead")]
        public void SetAuxilaryBus3(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OverrideUserAuxSends(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendVolume3(volume));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendLPF3(lowPass));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendHPF3(highPass));
            WwiseUtility.Instance.SetObjectReference(this, WwiseReference.Ref_UserAuxSend3(WwiseUtility.Instance.GetWwiseObjectByName($"AuxBus:{busName}")));
        }

        public async Task SetAuxilaryBus3Async(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OverrideUserAuxSends(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_UserAuxSendVolume3(volume));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_UserAuxSendLPF3(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_UserAuxSendHPF3(highPass));
            await WwiseUtility.Instance.SetObjectReferenceAsync(this, WwiseReference.Ref_UserAuxSend3(await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"AuxBus:{busName}")));
        }

        /// <summary>
        /// 设置反射发送
        /// </summary>
        /// <param name="busName"></param>
        /// <param name="volume"></param>
        [Obsolete("use async version instead")]
        public void SetEarlyReflections(string busName, float volume = 0)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OverrideEarlyReflections(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_ReflectionsVolume(volume));
            WwiseUtility.Instance.SetObjectReference(this, WwiseReference.Ref_ReflectionsAuxSend(WwiseUtility.Instance.GetWwiseObjectByName($"AuxBus:{busName}")));
        }

        public async Task SetEarlyReflectionsAsync(string busName, float volume = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OverrideEarlyReflections(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_ReflectionsVolume(volume));
            await WwiseUtility.Instance.SetObjectReferenceAsync(this, WwiseReference.Ref_ReflectionsAuxSend(await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"AuxBus:{busName}")));
        }

        /// <summary>
        /// 设置输出总线
        /// </summary>
        /// <param name="busName"></param>
        /// <param name="volume"></param>
        /// <param name="lowPass"></param>
        /// <param name="highPass"></param>
        [Obsolete("use async version instead")]
        public void SetOutputBus(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OverrideOutput(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OutputBusVolume(volume));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OutputBusLowpass(lowPass));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OutputBusHighpass(highPass));
            WwiseUtility.Instance.SetObjectReference(this, WwiseReference.Ref_OutputBus(WwiseUtility.Instance.GetWwiseObjectByName($"Bus:{busName}")));
        }
        
        public async Task SetOutputBusAsync(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OverrideOutput(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OutputBusVolume(volume));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OutputBusLowpass(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OutputBusHighpass(highPass));
            await WwiseUtility.Instance.SetObjectReferenceAsync(this, WwiseReference.Ref_OutputBus(await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"Bus:{busName}")));
        }
        
        /// <summary>
        /// 设置输出总线
        /// </summary>
        /// <param name="bus_name"></param>
        /// <param name="volume"></param>
        /// <param name="lowPass"></param>
        /// <param name="highPass"></param>
        [Obsolete("use async version instead")]
        public void SetOutputBus(WwiseObject bus, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OverrideOutput(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OutputBusVolume(volume));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OutputBusLowpass(lowPass));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OutputBusHighpass(highPass));
            WwiseUtility.Instance.SetObjectReference(this, WwiseReference.Ref_OutputBus(bus));
        }

        public async Task SetOutputBusAsync(WwiseObject bus, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OverrideOutput(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OutputBusVolume(volume));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OutputBusLowpass(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OutputBusHighpass(highPass));
            await WwiseUtility.Instance.SetObjectReferenceAsync(this, WwiseReference.Ref_OutputBus(bus));
        }

        /// <summary>
        /// 设置转码预制
        /// </summary>
        /// <param name="conversionName"></param>
        [Obsolete("use async version instead")]
        public void SetConversion(string conversionName)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OverrideConversion(true));
            WwiseUtility.Instance.SetObjectReference(this, WwiseReference.Ref_Conversion(WwiseUtility.Instance.GetWwiseObjectByName($"Conversion:{conversionName}")));
        }

        public async Task SetConversionAsync(string conversionName)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OverrideConversion(true));
            await WwiseUtility.Instance.SetObjectReferenceAsync(this, WwiseReference.Ref_Conversion(await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"Conversion:{conversionName}")));
        }

        /// <summary>
        /// 设置3D位置
        /// </summary>
        /// <param name="_3d_position"></param>
        [Obsolete("use async version instead")]
        private void Set3DPosition(WwiseProperty.Option_3DPosition _3d_position)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OverridePositioning(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_ListenerRelativeRouting(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_3DPosition(_3d_position));
        }

        private async Task Set3DPositionAsync(WwiseProperty.Option_3DPosition _3d_position)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OverridePositioning(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_ListenerRelativeRouting(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_3DPosition(_3d_position));
        }

        /// <summary>
        /// 设置衰减
        /// </summary>
        /// <param name="attenuationName"></param>
        [Obsolete("use async version instead")]
        public void SetAttenuation(string attenuationName)
        {
            var att = WwiseUtility.Instance.GetWwiseObjectByName($"Attenuation:{attenuationName}");
            if (att == null)
            {
                att = WwiseUtility.Instance.CreateObject(attenuationName, ObjectType.Attenuation, @"\Attenuations\Default Work Unit");
            }
            if (att == null) return;
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OverridePositioning(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_ListenerRelativeRouting(true));
            if (!String.IsNullOrEmpty(attenuationName))
            {
                WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_EnableAttenuation(true));
                WwiseUtility.Instance.SetObjectReference(this, WwiseReference.Ref_Attenuation(
                    att));
            }
        }

        public async Task SetAttenuationAsync(string attenuationName)
        {
            var att = await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"Attenuation:{attenuationName}");
            if (att == null)
            {
                att = await WwiseUtility.Instance.CreateObjectAsync(attenuationName, ObjectType.Attenuation, @"\Attenuations\Default Work Unit");
            }
            if (att == null) return;
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OverridePositioning(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_ListenerRelativeRouting(true));
            if (!String.IsNullOrEmpty(attenuationName))
            {
                await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_EnableAttenuation(true));
                await WwiseUtility.Instance.SetObjectReferenceAsync(this, WwiseReference.Ref_Attenuation(
                    att));
            }
        }

        /// <summary>
        /// 设置空间音频
        /// </summary>
        /// <param name="spatialization"></param>
        /// <param name="speakerPanningSpacializationMix"></param>
        [Obsolete("use async version instead")]
        public void SetSpatialization(WwiseProperty.Option_3DSpatialization spatialization, uint speakerPanningSpacializationMix)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OverridePositioning(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_ListenerRelativeRouting(true));

            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_3DSpatialization(spatialization));

            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_SpeakerPanning_3DSpatializationMix(speakerPanningSpacializationMix));
        }

        public async Task SetSpatializationAsync(WwiseProperty.Option_3DSpatialization spatialization, uint speakerPanningSpacializationMix)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OverridePositioning(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_ListenerRelativeRouting(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_3DSpatialization(spatialization));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_SpeakerPanning_3DSpatializationMix(speakerPanningSpacializationMix));
        }

        /// <summary>
        /// 设置位置中心
        /// </summary>
        /// <param name="center"></param>
        [Obsolete("use async version instead")]
        public void SetPostitioningCenter(uint center)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OverridePositioning(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_CenterPercentage(center));
        }

        public async Task SetPostitioningCenterAsync(uint center)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OverridePositioning(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_CenterPercentage(center));
        }

        /// <summary>
        /// 设置音响方向模式
        /// </summary>
        /// <param name="speakerPanning"></param>
        [Obsolete("use async version instead")]
        public void SetSpeakerPanningMode(WwiseProperty.Option_SpeakerPanning speakerPanning)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OverridePositioning(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_SpeakerPanning(speakerPanning));
        }

        public async Task SetSpeakerPanningModeAsync(WwiseProperty.Option_SpeakerPanning speakerPanning)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OverridePositioning(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_SpeakerPanning(speakerPanning));
        }

        /// <summary>
        /// 设置播放限制
        /// </summary>
        /// <param name="ignoreParent"></param>
        /// <param name="option"></param>
        /// <param name="soundInstanceLimit"></param>
        [Obsolete("use async version instead")]
        public void SetPlaybackLimit(bool ignoreParent = false, WwiseProperty.Option_IsGlobalLimit option = WwiseProperty.Option_IsGlobalLimit.PerGameObject, uint soundInstanceLimit = 50)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_IgnoreParentMaxSoundInstance(ignoreParent));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_UseMaxSoundPerInstance(true));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_MaxSoundPerInstance(soundInstanceLimit));
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_IsGlobalLimit(option));
        }

        public async Task SetPlaybackLimitAsync(bool ignoreParent = false, WwiseProperty.Option_IsGlobalLimit option = WwiseProperty.Option_IsGlobalLimit.PerGameObject, uint soundInstanceLimit = 50)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_IgnoreParentMaxSoundInstance(ignoreParent));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_UseMaxSoundPerInstance(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_MaxSoundPerInstance(soundInstanceLimit));
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_IsGlobalLimit(option));
        }

        /// <summary>
        /// 设置播放限制表现
        /// </summary>
        /// <param name="behavior"></param>
        [Obsolete("use async version instead")]
        public void SetLimitReachedBehavior(WwiseProperty.Option_OverLimitBehavior behavior = WwiseProperty.Option_OverLimitBehavior.KillVoice)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_OverLimitBehavior(behavior));
        }

        public async Task SetLimitReachedBehaviorAsync(WwiseProperty.Option_OverLimitBehavior behavior = WwiseProperty.Option_OverLimitBehavior.KillVoice)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_OverLimitBehavior(behavior));
        }


        [Obsolete("use async version instead")]
        public List<WwiseObject> GetChildren()
        {
            List<WwiseObject> result = new List<WwiseObject>();

            WwiseWorkUnitParser parser = new WwiseWorkUnitParser(WwiseUtility.Instance.GetWorkUnitFilePath(this));
            var folders = parser.XML.GetElementsByTagName("Folder");
            foreach (XmlElement folder in folders)
            {
                if (folder.GetAttribute("ID") == ID)
                {
                    var children = (folder.GetElementsByTagName("ChildrenList")[0] as XmlElement).ChildNodes;
                    foreach (XmlElement child in children)
                    {
                        result.Add(WwiseUtility.Instance.GetWwiseObjectByID(child.GetAttribute("ID")));
                    }
                    break;
                }
            }

            return result;
        }

        public async Task<List<WwiseObject>> GetChildrenAsync()
        {
            return await WwiseUtility.Instance.GetWwiseObjectChildrenAsync(this);
        }
    }
}
