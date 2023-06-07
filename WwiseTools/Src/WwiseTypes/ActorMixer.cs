using System;
using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.WwiseTypes
{
    public class ActorMixer : WwiseTypeBase
    {
        public async Task SetVolumeAsync(float value)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_Volume(value));
        }


        public async Task SetPitchAsync(int value)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_Pitch(value));
        }


        public async Task SetFilterAsync(uint highPass, uint lowPass)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_Lowpass(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_Highpass(highPass));
        }


        public async Task SetGameAuxSendAsync(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverrideGameAuxSends(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UseGameAuxSends(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_GameAuxSendVolume(volume));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_GameAuxSendLPF(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_GameAuxSendHPF(highPass));
        }
        public async Task SetAuxilaryBus0Async(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverrideUserAuxSends(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UserAuxSendVolume0(volume));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UserAuxSendLPF0(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UserAuxSendHPF0(highPass));
            await WwiseUtility.Instance.SetObjectPropertiesAsync(WwiseObject, WwiseProperty.Prop_UserAuxSend0(await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"AuxBus:{busName}")));
        }
        public async Task SetAuxilaryBus1Async(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverrideUserAuxSends(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UserAuxSendVolume1(volume));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UserAuxSendLPF1(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UserAuxSendHPF1(highPass));
            await WwiseUtility.Instance.SetObjectPropertiesAsync(WwiseObject, WwiseProperty.Prop_UserAuxSend1(await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"AuxBus:{busName}")));
        }

        public async Task SetAuxilaryBus2Async(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverrideUserAuxSends(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UserAuxSendVolume2(volume));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UserAuxSendLPF2(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UserAuxSendHPF2(highPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UserAuxSend2(await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"AuxBus:{busName}")));
        }
        public async Task SetAuxilaryBus3Async(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverrideUserAuxSends(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UserAuxSendVolume3(volume));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UserAuxSendLPF3(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UserAuxSendHPF3(highPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UserAuxSend3(await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"AuxBus:{busName}")));
        }

        public async Task SetEarlyReflectionsAsync(string busName, float volume = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverrideEarlyReflections(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_ReflectionsVolume(volume));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_ReflectionsAuxSend(await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"AuxBus:{busName}")));
        }

        public async Task SetOutputBusAsync(string busName, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverrideOutput(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OutputBusVolume(volume));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OutputBusLowpass(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OutputBusHighpass(highPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OutputBus(await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"Bus:{busName}")));
        }

        public async Task SetOutputBusAsync(WwiseObject bus, float volume = 0, uint lowPass = 0, uint highPass = 0)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverrideOutput(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OutputBusVolume(volume));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OutputBusLowpass(lowPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OutputBusHighpass(highPass));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OutputBus(bus));
        }

        public async Task SetConversionAsync(string conversionName)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverrideConversion(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_Conversion(await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"Conversion:{conversionName}")));
        }

        private async Task Set3DPositionAsync(WwiseProperty.Option_3DPosition _3d_position)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverridePositioning(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_ListenerRelativeRouting(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_3DPosition(_3d_position));
        }


        public async Task SetAttenuationAsync(string attenuationName)
        {
            var att = await WwiseUtility.Instance.GetWwiseObjectByNameAsync($"Attenuation:{attenuationName}");
            if (att == null)
            {
                att = await WwiseUtility.Instance.CreateObjectAtPathAsync(attenuationName, WwiseObject.ObjectType.Attenuation, @"\Attenuations\Default Work Unit");
            }
            if (att == null) return;
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverridePositioning(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_ListenerRelativeRouting(true));
            if (!String.IsNullOrEmpty(attenuationName))
            {
                await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_EnableAttenuation(true));
                await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_Attenuation(
                    att));
            }
        }

        public async Task SetSpatializationAsync(WwiseProperty.Option_3DSpatialization spatialization, uint speakerPanningSpacializationMix)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverridePositioning(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_ListenerRelativeRouting(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_3DSpatialization(spatialization));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_SpeakerPanning_3DSpatializationMix(speakerPanningSpacializationMix));
        }

        public async Task SetPostitioningCenterAsync(uint center)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverridePositioning(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_CenterPercentage(center));
        }

        public async Task SetSpeakerPanningModeAsync(WwiseProperty.Option_SpeakerPanning speakerPanning)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverridePositioning(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_SpeakerPanning(speakerPanning));
        }

        public async Task SetPlaybackLimitAsync(bool ignoreParent = false, WwiseProperty.Option_IsGlobalLimit option = WwiseProperty.Option_IsGlobalLimit.PerGameObject, uint soundInstanceLimit = 50)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_IgnoreParentMaxSoundInstance(ignoreParent));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_UseMaxSoundPerInstance(true));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_MaxSoundPerInstance(soundInstanceLimit));
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_IsGlobalLimit(option));
        }

        public async Task SetLimitReachedBehaviorAsync(WwiseProperty.Option_OverLimitBehavior behavior = WwiseProperty.Option_OverLimitBehavior.KillVoice)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, WwiseProperty.Prop_OverLimitBehavior(behavior));
        }

        public ActorMixer(WwiseObject wwiseObject) : base(wwiseObject, 
            "Sound,ActorMixer,BlendContainer,SwitchContainer,RandomSequenceContainer,MusicTrack,MusicSwitchContainer,MusicPlaylistContainer,MusicSegment")
        {
        }
    }
}
