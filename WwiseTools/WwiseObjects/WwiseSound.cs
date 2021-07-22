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
    public class WwiseSound : WwiseObject
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

        /// <summary>
        /// 创建一个播放事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent_path"></param>
        /// <returns></returns>
        public WwiseObject CreatePlayEvent(string name = "", string parent_path = @"\Events\Default Work Unit")
        {
            if (String.IsNullOrEmpty(name)) name = Name;

            return WwiseUtility.CreatePlayEvent(name, Path, parent_path);
        }

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="value"></param>
        public void SetVolume(float value)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_Volume(value));
        }

        /// <summary>
        /// 设置音高
        /// </summary>
        /// <param name="value"></param>
        public void SetPitch(int value)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_Pitch(value));
        }

        /// <summary>
        /// 设置滤波器
        /// </summary>
        /// <param name="high_pass"></param>
        /// <param name="low_pass"></param>
        public void SetFilter(uint high_pass, uint low_pass)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_Lowpass(low_pass));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_Highpass(high_pass));
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
        /// 设置游戏Aux发送
        /// </summary>
        /// <param name="bus_name"></param>
        /// <param name="volume"></param>
        /// <param name="low_pass"></param>
        /// <param name="high_pass"></param>
        public void SetGameAuxSend(string bus_name, float volume = 0, uint low_pass = 0, uint high_pass = 0)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OverrideGameAuxSends(true));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_UseGameAuxSends(true));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_GameAuxSendVolume(volume));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_GameAuxSendLPF(low_pass));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_GameAuxSendHPF(high_pass));
        }

        /// <summary>
        /// 设置用户Aux发送0
        /// </summary>
        /// <param name="bus_name"></param>
        /// <param name="volume"></param>
        /// <param name="low_pass"></param>
        /// <param name="high_pass"></param>
        public void SetAuxilaryBus0(string bus_name, float volume = 0, uint low_pass = 0, uint high_pass = 0)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OverrideUserAuxSends(true));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendVolume0(volume));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendLPF0(low_pass));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendHPF0(high_pass));
            WwiseUtility.SetObjectReference(this, WwiseReference.Ref_UserAuxSend0(WwiseUtility.GetWwiseObjectByName($"AuxBus:{bus_name}")));
        }

        /// <summary>
        /// 设置用户Aux发送1
        /// </summary>
        /// <param name="bus_name"></param>
        /// <param name="volume"></param>
        /// <param name="low_pass"></param>
        /// <param name="high_pass"></param>
        public void SetAuxilaryBus1(string bus_name, float volume = 0, uint low_pass = 0, uint high_pass = 0)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OverrideUserAuxSends(true));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendVolume1(volume));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendLPF1(low_pass));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendHPF1(high_pass));
            WwiseUtility.SetObjectReference(this, WwiseReference.Ref_UserAuxSend1(WwiseUtility.GetWwiseObjectByName($"AuxBus:{bus_name}")));
        }

        /// <summary>
        /// 设置用户Aux发送2
        /// </summary>
        /// <param name="bus_name"></param>
        /// <param name="volume"></param>
        /// <param name="low_pass"></param>
        /// <param name="high_pass"></param>
        public void SetAuxilaryBus2(string bus_name, float volume = 0, uint low_pass = 0, uint high_pass = 0)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OverrideUserAuxSends(true));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendVolume2(volume));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendLPF2(low_pass));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendHPF2(high_pass));
            WwiseUtility.SetObjectReference(this, WwiseReference.Ref_UserAuxSend2(WwiseUtility.GetWwiseObjectByName($"AuxBus:{bus_name}")));
        }

        /// <summary>
        /// 设置用户Aux发送3
        /// </summary>
        /// <param name="bus_name"></param>
        /// <param name="volume"></param>
        /// <param name="low_pass"></param>
        /// <param name="high_pass"></param>
        public void SetAuxilaryBus3(string bus_name, float volume = 0, uint low_pass = 0, uint high_pass = 0)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OverrideUserAuxSends(true));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendVolume3(volume));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendLPF3(low_pass));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_UserAuxSendHPF3(high_pass));
            WwiseUtility.SetObjectReference(this, WwiseReference.Ref_UserAuxSend3(WwiseUtility.GetWwiseObjectByName($"AuxBus:{bus_name}")));
        }

        /// <summary>
        /// 设置反射发送
        /// </summary>
        /// <param name="bus_name"></param>
        /// <param name="volume"></param>
        public void SetEarlyReflections(string bus_name, float volume = 0)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OverrideEarlyReflections(true));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_ReflectionsVolume(volume));
            WwiseUtility.SetObjectReference(this, WwiseReference.Ref_ReflectionsAuxSend(WwiseUtility.GetWwiseObjectByName($"AuxBus:{bus_name}")));
        }

        /// <summary>
        /// 设置输出总线
        /// </summary>
        /// <param name="bus_name"></param>
        /// <param name="volume"></param>
        /// <param name="low_pass"></param>
        /// <param name="high_pass"></param>
        public void SetOutputBus(string bus_name, float volume = 0, uint low_pass = 0, uint high_pass = 0)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OverrideOutput(true));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OutputBusVolume(volume));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OutputBusLowpass(low_pass));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OutputBusHighpass(high_pass));
            WwiseUtility.SetObjectReference(this, WwiseReference.Ref_OutputBus(WwiseUtility.GetWwiseObjectByName($"Bus:{bus_name}")));
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

        /// <summary>
        /// 设置转码预制
        /// </summary>
        /// <param name="conversion_name"></param>
        public void SetConversion(string conversion_name)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OverrideConversion(true));
            WwiseUtility.SetObjectReference(this, WwiseReference.Ref_Conversion(WwiseUtility.GetWwiseObjectByName($"Conversion:{conversion_name}")));
        }

        /// <summary>
        /// 设置3D位置
        /// </summary>
        /// <param name="_3d_position"></param>
        private void Set3DPosition(WwiseProperty.Option_3DPosition _3d_position)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OverridePositioning(true));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_ListenerRelativeRouting(true));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_3DPosition(_3d_position));
        }

        /// <summary>
        /// 设置衰减
        /// </summary>
        /// <param name="attenuation_name"></param>
        public  void SetAttenuation(string attenuation_name)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OverridePositioning(true));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_ListenerRelativeRouting(true));
            if (!String.IsNullOrEmpty(attenuation_name))
            {
                WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_EnableAttenuation(true));
                WwiseUtility.SetObjectReference(this, WwiseReference.Ref_Attenuation(
                    WwiseUtility.GetWwiseObjectByName($"Attenuation:{attenuation_name}")));

            }
        }

        /// <summary>
        /// 设置空间音频
        /// </summary>
        /// <param name="spatialization"></param>
        /// <param name="speaker_panning_spacialization_mix"></param>
        public  void SetSpatialization( WwiseProperty.Option_3DSpatialization spatialization, uint speaker_panning_spacialization_mix)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OverridePositioning(true));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_ListenerRelativeRouting(true));

            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_3DSpatialization(spatialization));

            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_SpeakerPanning_3DSpatializationMix(speaker_panning_spacialization_mix));
        }

        /// <summary>
        /// 设置位置中心
        /// </summary>
        /// <param name="center"></param>
        public void SetPostitioningCenter(uint center)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OverridePositioning(true));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_CenterPercentage(center));
        }

        /// <summary>
        /// 设置音响方向模式
        /// </summary>
        /// <param name="speaker_panning"></param>
        public void SetSpeakerPanningMode(WwiseProperty.Option_SpeakerPanning speaker_panning)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_OverridePositioning(true));
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_SpeakerPanning(speaker_panning));
        }


    }
}
