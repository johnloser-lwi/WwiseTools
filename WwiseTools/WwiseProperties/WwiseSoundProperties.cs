using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Properties
{
    public class WwiseSoundProperties
    {
        public enum V_3DPosition { Emmiter = 0, EmitterWithAutomation = 1, ListenerWithAutomation = 2 }
        public static WwiseProperty P_3DPosition(V_3DPosition mode)
        {
            return new WwiseProperty("3DPosition", (int)mode);
        }

        public enum V_3DSpatialization { None = 0, Position = 1, PostionAndOrientation = 2 }
        public static WwiseProperty P_3DSpatialization(V_3DSpatialization mode)
        {
            return new WwiseProperty("3DSpatialization", (int)mode);
        }

        public static WwiseProperty P_EnableAttenuation(bool enable)
        {
            return new WwiseProperty("EnableAttenuation", enable);
        }

        public static WwiseProperty P_EnableDiffraction(bool enable)
        {
            return new WwiseProperty("EnableDiffraction", enable);
        }

        public static WwiseProperty P_EnableLoudnessNormalization(bool enable)
        {
            return new WwiseProperty("EnableLoudnessNormalization", enable);
        }

        public static WwiseProperty P_EnableMidiNoteTracking(bool enable)
        {
            return new WwiseProperty("EnableMidiNoteTracking", enable);
        }

        public static WwiseProperty P_GameAuxSendHPF(int value)
        {
            return new WwiseProperty("GameAuxSendHPF", value);
        }

        public static WwiseProperty P_GameAuxSendLPF(int value)
        {
            return new WwiseProperty("GameAuxSendLPF", value);
        }

        public static WwiseProperty P_GameAuxSendVolume(float value)
        {
            return new WwiseProperty("GameAuxSendVolume", valueLimiter(value, -200, 200));
        }

        public static WwiseProperty P_HdrActiveRange(float value)
        {
            return new WwiseProperty("HdrActiveRange", valueLimiter(value, 0, 96));
        }

        public static WwiseProperty P_HdrEnableEnvelope(bool value)
        {
            return new WwiseProperty("HdrEnableEnvelope", value);
        }

        public static WwiseProperty P_HdrEnvelopeSensitivity(float value)
        {
            return new WwiseProperty("HdrEnvelopeSensitivity", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty P_Highpass(int value)
        {
            return new WwiseProperty("Highpass", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty P_HoldEmitterPositionOrientation(bool hold)
        {
            return new WwiseProperty("HoldEmitterPositionOrientation", hold);
        }

        public static WwiseProperty P_HoldListenerOrientation(bool hold)
        {
            return new WwiseProperty("HoldListenerOrientation", hold);
        }

        public static WwiseProperty P_IgnoreParentMaxSoundInstance(bool ignore)
        {
            return new WwiseProperty("IgnoreParentMaxSoundInstance", ignore);
        }

        public static WwiseProperty P_Inclusion(bool include)
        {
            return new WwiseProperty("Inclusion", include);
        }

        public static WwiseProperty P_InitialDelay(float value)
        {
            return new WwiseProperty("InitialDelay", valueLimiter(value, 0, 3600));
        }

        public enum V_IsGlobalLimit { PerGameObject = 0, Globally = 1 }
        public static WwiseProperty P_IsGlobalLimit(V_IsGlobalLimit mode)
        {
            return new WwiseProperty("IsGlobalLimit", (int)mode);
        }

        public static WwiseProperty P_IsLoopingEnabled(bool enable)
        {
            return new WwiseProperty("IsLoopingEnabled", enable);
        }

        public static WwiseProperty P_IsLoopingInfinite(bool infinite)
        {
            return new WwiseProperty("IsLoopingInfinite", infinite);
        }

        public static WwiseProperty P_IsNonCachable(bool infinite)
        {
            return new WwiseProperty("IsNonCachable", infinite);
        }

        public static WwiseProperty P_IsStreamingEnabled(bool enable)
        {
            return new WwiseProperty("IsStreamingEnabled", enable);
        }

        public static WwiseProperty P_IsVoice(bool isVoice)
        {
            return new WwiseProperty("IsVoice", isVoice);
        }

        public static WwiseProperty P_IsZeroLantency(bool zeroLatency)
        {
            return new WwiseProperty("IsZeroLantency", zeroLatency);
        }

        public static WwiseProperty P_ListenerRelativeRouting(bool isListenerRelativeRouting)
        {
            return new WwiseProperty("ListenerRelativeRouting", isListenerRelativeRouting);
        }

        public static WwiseProperty P_LoopCount(uint value)
        {
            return new WwiseProperty("LoopCount", valueLimiter(value, 1, 32767));
        }

        public static WwiseProperty P_Lowpass(uint value)
        {
            return new WwiseProperty("Lowpass", valueLimiter(value, 1, 32767));
        }

        public static WwiseProperty P_MakeUpGain(float value)
        {
            return new WwiseProperty("MakeUpGain", valueLimiter(value, -96, 96));
        }

        public enum V_MaxReachedBehavior { DiscardOldestInstance = 0, DiscardNewestInstance = 1 }
        public static WwiseProperty P_MaxReachedBehavior(V_MaxReachedBehavior behavior)
        {
            return new WwiseProperty("MaxReachedBehavior", (int)behavior);
        }

        public static WwiseProperty P_MaxSoundPerInstance(uint value)
        {
            return new WwiseProperty("MaxSoundPerInstance", valueLimiter(value, 1, 1000));
        }

        public static WwiseProperty P_MidiBreakOnNoteOff(bool breakOnNoteOff)
        {
            return new WwiseProperty("MidiBreakOnNoteOff", breakOnNoteOff);
        }

        public static WwiseProperty P_MidiChannelFilter(uint value)
        {
            return new WwiseProperty("MidiChannelFilter", valueLimiter(value, 0, 65535));
        }

        public static WwiseProperty P_MidiKeyFilterMax(uint value)
        {
            return new WwiseProperty("MidiKeyFilterMax", valueLimiter(value, 0, 127));
        }

        public static WwiseProperty P_MidiKeyFilterMin(uint value)
        {
            return new WwiseProperty("MidiKeyFilterMin", valueLimiter(value, 0, 127));
        }

        public enum V_MidiPlayOnNoteType { NoteOn = 1, NoteOff = 2 }
        public static WwiseProperty P_MidiPlayOnNoteType(V_MidiPlayOnNoteType type)
        {
            return new WwiseProperty("MidiPlayOnNoteType", (int)type);
        }

        public static WwiseProperty P_MidiTrackingRootNote(uint value)
        {
            return new WwiseProperty("MidiTrackingRootNote", valueLimiter(value, 0, 127));
        }
        public static WwiseProperty P_MidiTransposition(uint value)
        {
            return new WwiseProperty("MidiTransposition", valueLimiter(value, -127, 127));
        }
        public static WwiseProperty P_MidiVelocityFilterMax(uint value)
        {
            return new WwiseProperty("MidiVelocityFilterMax", valueLimiter(value, 0, 127));
        }
        public static WwiseProperty P_MidiVelocityFilterMin(uint value)
        {
            return new WwiseProperty("MidiVelocityFilterMin", valueLimiter(value, 0, 127));
        }
        public static WwiseProperty P_MidiVelocityOffset(uint value)
        {
            return new WwiseProperty("MidiVelocityOffset", valueLimiter(value, -127, 127));
        }

        public static WwiseProperty P_OutputBusHighpass(uint value)
        {
            return new WwiseProperty("OutputBusHighpass", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty P_OutputBusLowpass(uint value)
        {
            return new WwiseProperty("OutputBusLowpass", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty P_OutputBusVolume(uint value)
        {
            return new WwiseProperty("OutputBusVolume", valueLimiter(value, -200, 200));
        }

        public enum V_OverLimitBehavior { KillVoice = 0, UseVirtualVoiceSettings = 1 }
        public static WwiseProperty P_OverLimitBehavior(V_OverLimitBehavior behavior)
        {
            return new WwiseProperty("OverLimitBehavior", (int)behavior);
        }

        public static WwiseProperty P_OverrideAnalysis(bool @override)
        {
            return new WwiseProperty("OverrideAnalysis", @override);
        }

        public static WwiseProperty P_OverrideAttachableMixerInput(bool @override)
        {
            return new WwiseProperty("OverrideAttachableMixerInput", @override);
        }

        public static WwiseProperty P_OverrideColor(bool @override)
        {
            return new WwiseProperty("OverrideColor", @override);
        }

        public static WwiseProperty P_OverrideConversion(bool @override)
        {
            return new WwiseProperty("OverrideConversion", @override);
        }

        public static WwiseProperty P_OverrideEarlyReflections(bool @override)
        {
            return new WwiseProperty("OverrideEarlyReflections", @override);
        }
        public static WwiseProperty P_OverrideEffect(bool @override)
        {
            return new WwiseProperty("OverrideEffect", @override);
        }

        public static WwiseProperty P_OverrideGameAuxSends(bool @override)
        {
            return new WwiseProperty("OverrideGameAuxSends", @override);
        }

        public static WwiseProperty P_OverrideHdrEnvelope(bool @override)
        {
            return new WwiseProperty("OverrideHdrEnvelope", @override);
        }

        public static WwiseProperty P_OverrideMetadata(bool @override)
        {
            return new WwiseProperty("OverrideMetadata", @override);
        }

        public static WwiseProperty P_OverrideMidiEventsBehavior(bool @override)
        {
            return new WwiseProperty("OverrideMidiEventsBehavior", @override);
        }

        public static WwiseProperty P_OverrideMidiNoteTracking(bool @override)
        {
            return new WwiseProperty("OverrideMidiNoteTracking", @override);
        }

        public static WwiseProperty P_OverrideOutput(bool @override)
        {
            return new WwiseProperty("OverrideOutput", @override);
        }

        public static WwiseProperty P_OverridePositioning(bool @override)
        {
            return new WwiseProperty("OverridePositioning", @override);
        }

        public static WwiseProperty P_OverridePriority(bool @override)
        {
            return new WwiseProperty("OverridePriority", @override);
        }

        public static WwiseProperty P_OverrideUserAuxSends(bool @override)
        {
            return new WwiseProperty("OverrideUserAuxSends", @override);
        }

        public static WwiseProperty P_OverrideVirtualVoice(bool @override)
        {
            return new WwiseProperty("OverrideVirtualVoice", @override);
        }

        public static WwiseProperty P_Pitch(int value)
        {
            return new WwiseProperty("Pitch", valueLimiter(value, -2400, 2400));
        }

        public static WwiseProperty P_PreFetchLength(uint value)
        {
            return new WwiseProperty("PreFetchLength", valueLimiter(value, 0, 10000));
        }

        public static WwiseProperty P_Priority(uint value)
        {
            return new WwiseProperty("Priority", valueLimiter(value, 0, 10000));
        }

        public static WwiseProperty P_PriorityDistanceFactor(bool use)
        {
            return new WwiseProperty("PriorityDistanceFactor", use);
        }

        public static WwiseProperty P_PriorityDistanceOffset(int value)
        {
            return new WwiseProperty("PriorityDistanceOffset", valueLimiter(value, -100, 100));
        }

        public static WwiseProperty P_ReflectionsVolume(float value)
        {
            return new WwiseProperty("ReflectionsVolume", valueLimiter(value, -200, 200));
        }

        public static WwiseProperty P_RenderEffect0(bool render)
        {
            return new WwiseProperty("RenderEffect0", render);
        }

        public static WwiseProperty P_RenderEffect1(bool render)
        {
            return new WwiseProperty("RenderEffect1", render);
        }

        public static WwiseProperty P_RenderEffect2(bool render)
        {
            return new WwiseProperty("RenderEffect2", render);
        }

        public static WwiseProperty P_RenderEffect3(bool render)
        {
            return new WwiseProperty("RenderEffect3", render);
        }


        public enum V_SpeakerPanning { DirectAssignment = 0, BalanceFade = 1, Steering = 2 }
        public static WwiseProperty P_SpeakerPanning(V_SpeakerPanning mode)
        {
            return new WwiseProperty("SpeakerPanning", (int)mode);
        }

        public static WwiseProperty P_SpeakerPanning_3DSpatializationMix(uint value)
        {
            return new WwiseProperty("SpeakerPanning3DSpatializationMix", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty P_UseGameAuxSends(bool use)
        {
            return new WwiseProperty("UseGameAuxSends", use);
        }

        public static WwiseProperty P_UseMaxSoundPerInstance(bool use)
        {
            return new WwiseProperty("UseMaxSoundPerInstance", use);
        }

        public static WwiseProperty P_UserAuxSendHPF0(uint value)
        {
            return new WwiseProperty("UserAuxSendHPF0", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty P_UserAuxSendHPF1(uint value)
        {
            return new WwiseProperty("UserAuxSendHPF1", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty P_UserAuxSendHPF2(uint value)
        {
            return new WwiseProperty("UserAuxSendHPF2", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty P_UserAuxSendHPF3(uint value)
        {
            return new WwiseProperty("UserAuxSendLPF3", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty P_UserAuxSendLPF0(uint value)
        {
            return new WwiseProperty("UserAuxSendLPF0", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty P_UserAuxSendLPF1(uint value)
        {
            return new WwiseProperty("UserAuxSendLPF1", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty P_UserAuxSendLPF2(uint value)
        {
            return new WwiseProperty("UserAuxSendLPF2", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty P_UserAuxSendLPF3(uint value)
        {
            return new WwiseProperty("UserAuxSendLPF3", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty P_UserAuxSendVolume0(float value)
        {
            return new WwiseProperty("UserAuxSendVolume0", valueLimiter(value, -200, 100));
        }

        public static WwiseProperty P_UserAuxSendVolume1(float value)
        {
            return new WwiseProperty("UserAuxSendVolume1", valueLimiter(value, -200, 100));
        }

        public static WwiseProperty P_UserAuxSendVolume2(float value)
        {
            return new WwiseProperty("UserAuxSendVolume2", valueLimiter(value, -200, 100));
        }

        public static WwiseProperty P_UserAuxSendVolume3(float value)
        {
            return new WwiseProperty("UserAuxSendVolume3", valueLimiter(value, -200, 100));
        }

        public enum V_VirtualVoiceQueueBehavior { PlayFromBeginning = 0, PlayFromElapsedTime = 1, Resume = 2  }
        public static WwiseProperty P_VirtualVoiceQueueBehavior(V_VirtualVoiceQueueBehavior behavior)
        {
            return new WwiseProperty("VirtualVoiceQueueBehavior", (int)behavior);
        }

        public static WwiseProperty P_Volume(float value)
        {
            return new WwiseProperty("Volume", valueLimiter(value, -200, 200));
        }

        public static WwiseProperty P_Weight(float value)
        {
            return new WwiseProperty("Weight", valueLimiter(value, 0.001f, 200f));
        }


        private static float valueLimiter(float value, float min, float max)
        {
            float result = value;
            if (result < min) value = min;
            if (result > max) value = max;

            return result;
        }

        private static int valueLimiter(int value, int min, int max)
        {
            int result = value;
            if (result < min) value = min;
            if (result > max) value = max;

            return result;
        }

        private static uint valueLimiter(uint value, uint min, uint max)
        {
            uint result = value;
            if (result < min) value = min;
            if (result > max) value = max;

            return result;
        }

    }
}
