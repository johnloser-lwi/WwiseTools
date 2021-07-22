using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Properties
{
    public class WwiseProperty
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public WwiseProperty(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public enum Option_3DPosition { Emmiter = 0, EmitterWithAutomation = 1, ListenerWithAutomation = 2 }
        public static WwiseProperty Prop_3DPosition(Option_3DPosition mode)
        {
            return new WwiseProperty("3DPosition", (int)mode);
        }

        public enum Option_3DSpatialization { None = 0, Position = 1, PostionAndOrientation = 2 }
        public static WwiseProperty Prop_3DSpatialization(Option_3DSpatialization mode)
        {
            return new WwiseProperty("3DSpatialization", (int)mode);
        }

        public enum Option_BelowThresholdBehavior { ContinueToPlay = 0, KillVoice = 1, SendToVirtualVoice = 2, KillIfFiniteElseVirtual = 3 }

        public static WwiseProperty Prop_BelowThresholdBehavior(Option_BelowThresholdBehavior behavior)
        {
            return new WwiseProperty("BelowThresholdBehavior", (int)behavior);
        }

        public static WwiseProperty Prop_BypassEffect(bool bypass)
        {
            return new WwiseProperty("BypassEffect", bypass);
        }

        public static WwiseProperty Prop_BypassEffect0(bool bypass)
        {
            return new WwiseProperty("BypassEffect0", bypass);
        }

        public static WwiseProperty Prop_BypassEffect1(bool bypass)
        {
            return new WwiseProperty("BypassEffect1", bypass);
        }

        public static WwiseProperty Prop_BypassEffect2(bool bypass)
        {
            return new WwiseProperty("BypassEffect2", bypass);
        }

        public static WwiseProperty Prop_BypassEffect3(bool bypass)
        {
            return new WwiseProperty("BypassEffect3", bypass);
        }

        public static WwiseProperty Prop_CenterPercentage(uint value)
        {
            return new WwiseProperty("CenterPercentage", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty Prop_Color(uint value)
        {
            return new WwiseProperty("Color", valueLimiter(value, 0, 26));
        }

        public static WwiseProperty Prop_EnableAttenuation(bool bypass)
        {
            return new WwiseProperty("EnableAttenuation", bypass);
        }

        public static WwiseProperty Prop_EnableDiffraction(bool enable)
        {
            return new WwiseProperty("EnableDiffraction", enable);
        }

        public static WwiseProperty Prop_EnableLoudnessNormalization(bool enable)
        {
            return new WwiseProperty("EnableLoudnessNormalization", enable);
        }

        public static WwiseProperty Prop_EnableMidiNoteTracking(bool enable)
        {
            return new WwiseProperty("EnableMidiNoteTracking", enable);
        }

        public static WwiseProperty Prop_GameAuxSendHPF(uint value)
        {
            return new WwiseProperty("GameAuxSendHPF", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty Prop_GameAuxSendLPF(int value)
        {
            return new WwiseProperty("GameAuxSendLPF", valueLimiter(value, 0, 100));
        }

        public enum Option_GlobalOrPerObject { GameObject = 0, Global = 1 }
        public static WwiseProperty Prop_GlobalOrPerObject(Option_GlobalOrPerObject option)
        {
            return new WwiseProperty("GlobalOrPerObject", (int)option);
        }

        public static WwiseProperty Prop_GameAuxSendVolume(float value)
        {
            return new WwiseProperty("GameAuxSendVolume", valueLimiter(value, -200, 200));
        }

        public static WwiseProperty Prop_HdrActiveRange(float value)
        {
            return new WwiseProperty("HdrActiveRange", valueLimiter(value, 0, 96));
        }

        public static WwiseProperty Prop_HdrEnableEnvelope(bool value)
        {
            return new WwiseProperty("HdrEnableEnvelope", value);
        }

        public static WwiseProperty Prop_HdrEnvelopeSensitivity(float value)
        {
            return new WwiseProperty("HdrEnvelopeSensitivity", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty Prop_Highpass(int value)
        {
            return new WwiseProperty("Highpass", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty Prop_HoldEmitterPositionOrientation(bool hold)
        {
            return new WwiseProperty("HoldEmitterPositionOrientation", hold);
        }

        public static WwiseProperty Prop_HoldListenerOrientation(bool hold)
        {
            return new WwiseProperty("HoldListenerOrientation", hold);
        }

        public static WwiseProperty Prop_IgnoreParentMaxSoundInstance(bool ignore)
        {
            return new WwiseProperty("IgnoreParentMaxSoundInstance", ignore);
        }

        public static WwiseProperty Prop_Inclusion(bool include)
        {
            return new WwiseProperty("Inclusion", include);
        }

        public static WwiseProperty Prop_InitialDelay(float value)
        {
            return new WwiseProperty("InitialDelay", valueLimiter(value, 0, 3600));
        }

        public enum Option_IsGlobalLimit { PerGameObject = 0, Globally = 1 }
        public static WwiseProperty Prop_IsGlobalLimit(Option_IsGlobalLimit mode)
        {
            return new WwiseProperty("IsGlobalLimit", (int)mode);
        }

        public static WwiseProperty Prop_IsLoopingEnabled(bool enable)
        {
            return new WwiseProperty("IsLoopingEnabled", enable);
        }

        public static WwiseProperty Prop_IsLoopingInfinite(bool infinite)
        {
            return new WwiseProperty("IsLoopingInfinite", infinite);
        }

        public static WwiseProperty Prop_IsNonCachable(bool infinite)
        {
            return new WwiseProperty("IsNonCachable", infinite);
        }

        public static WwiseProperty Prop_IsStreamingEnabled(bool enable)
        {
            return new WwiseProperty("IsStreamingEnabled", enable);
        }

        public static WwiseProperty Prop_IsVoice(bool isVoice)
        {
            return new WwiseProperty("IsVoice", isVoice);
        }

        public static WwiseProperty Prop_IsZeroLantency(bool zeroLatency)
        {
            return new WwiseProperty("IsZeroLantency", zeroLatency);
        }

        public static WwiseProperty Prop_ListenerRelativeRouting(bool isListenerRelativeRouting)
        {
            return new WwiseProperty("ListenerRelativeRouting", isListenerRelativeRouting);
        }

        public static WwiseProperty Prop_LoopCount(uint value)
        {
            return new WwiseProperty("LoopCount", valueLimiter(value, 1, 32767));
        }

        public static WwiseProperty Prop_Lowpass(uint value)
        {
            return new WwiseProperty("Lowpass", valueLimiter(value, 1, 32767));
        }

        public static WwiseProperty Prop_MakeUpGain(float value)
        {
            return new WwiseProperty("MakeUpGain", valueLimiter(value, -96, 96));
        }

        public enum Option_MaxReachedBehavior { DiscardOldestInstance = 0, DiscardNewestInstance = 1 }
        public static WwiseProperty Prop_MaxReachedBehavior(Option_MaxReachedBehavior behavior)
        {
            return new WwiseProperty("MaxReachedBehavior", (int)behavior);
        }

        public static WwiseProperty Prop_MaxSoundPerInstance(uint value)
        {
            return new WwiseProperty("MaxSoundPerInstance", valueLimiter(value, 1, 1000));
        }

        public static WwiseProperty Prop_MidiBreakOnNoteOff(bool breakOnNoteOff)
        {
            return new WwiseProperty("MidiBreakOnNoteOff", breakOnNoteOff);
        }

        public static WwiseProperty Prop_MidiChannelFilter(uint value)
        {
            return new WwiseProperty("MidiChannelFilter", valueLimiter(value, 0, 65535));
        }

        public static WwiseProperty Prop_MidiKeyFilterMax(uint value)
        {
            return new WwiseProperty("MidiKeyFilterMax", valueLimiter(value, 0, 127));
        }

        public static WwiseProperty Prop_MidiKeyFilterMin(uint value)
        {
            return new WwiseProperty("MidiKeyFilterMin", valueLimiter(value, 0, 127));
        }

        public enum Option_MidiPlayOnNoteType { NoteOn = 1, NoteOff = 2 }
        public static WwiseProperty Prop_MidiPlayOnNoteType(Option_MidiPlayOnNoteType type)
        {
            return new WwiseProperty("MidiPlayOnNoteType", (int)type);
        }

        public static WwiseProperty Prop_MidiTrackingRootNote(uint value)
        {
            return new WwiseProperty("MidiTrackingRootNote", valueLimiter(value, 0, 127));
        }
        public static WwiseProperty Prop_MidiTransposition(uint value)
        {
            return new WwiseProperty("MidiTransposition", valueLimiter(value, -127, 127));
        }
        public static WwiseProperty Prop_MidiVelocityFilterMax(uint value)
        {
            return new WwiseProperty("MidiVelocityFilterMax", valueLimiter(value, 0, 127));
        }
        public static WwiseProperty Prop_MidiVelocityFilterMin(uint value)
        {
            return new WwiseProperty("MidiVelocityFilterMin", valueLimiter(value, 0, 127));
        }
        public static WwiseProperty Prop_MidiVelocityOffset(uint value)
        {
            return new WwiseProperty("MidiVelocityOffset", valueLimiter(value, -127, 127));
        }

        public enum Option_NormalOrShuffle { Shuffle = 0, Standard = 1 }
        public static WwiseProperty Prop_NormalOrShuffle(Option_NormalOrShuffle option)
        {
            return new WwiseProperty("NormalOrShuffle", (int)option);
        }

        public static WwiseProperty Prop_OutputBusHighpass(uint value)
        {
            return new WwiseProperty("OutputBusHighpass", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty Prop_OutputBusLowpass(uint value)
        {
            return new WwiseProperty("OutputBusLowpass", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty Prop_OutputBusVolume(uint value)
        {
            return new WwiseProperty("OutputBusVolume", valueLimiter(value, -200, 200));
        }

        public enum Option_OverLimitBehavior { KillVoice = 0, UseVirtualVoiceSettings = 1 }
        public static WwiseProperty Prop_OverLimitBehavior(Option_OverLimitBehavior behavior)
        {
            return new WwiseProperty("OverLimitBehavior", (int)behavior);
        }

        public static WwiseProperty Prop_OverrideAnalysis(bool @override)
        {
            return new WwiseProperty("OverrideAnalysis", @override);
        }

        public static WwiseProperty Prop_OverrideAttachableMixerInput(bool @override)
        {
            return new WwiseProperty("OverrideAttachableMixerInput", @override);
        }

        public static WwiseProperty Prop_OverrideColor(bool @override)
        {
            return new WwiseProperty("OverrideColor", @override);
        }

        public static WwiseProperty Prop_OverrideConversion(bool @override)
        {
            return new WwiseProperty("OverrideConversion", @override);
        }

        public static WwiseProperty Prop_OverrideEarlyReflections(bool @override)
        {
            return new WwiseProperty("OverrideEarlyReflections", @override);
        }
        public static WwiseProperty Prop_OverrideEffect(bool @override)
        {
            return new WwiseProperty("OverrideEffect", @override);
        }

        public static WwiseProperty Prop_OverrideGameAuxSends(bool @override)
        {
            return new WwiseProperty("OverrideGameAuxSends", @override);
        }

        public static WwiseProperty Prop_OverrideHdrEnvelope(bool @override)
        {
            return new WwiseProperty("OverrideHdrEnvelope", @override);
        }

        public static WwiseProperty Prop_OverrideMetadata(bool @override)
        {
            return new WwiseProperty("OverrideMetadata", @override);
        }

        public static WwiseProperty Prop_OverrideMidiEventsBehavior(bool @override)
        {
            return new WwiseProperty("OverrideMidiEventsBehavior", @override);
        }

        public static WwiseProperty Prop_OverrideMidiNoteTracking(bool @override)
        {
            return new WwiseProperty("OverrideMidiNoteTracking", @override);
        }

        public static WwiseProperty Prop_OverrideOutput(bool @override)
        {
            return new WwiseProperty("OverrideOutput", @override);
        }

        public static WwiseProperty Prop_OverridePositioning(bool @override)
        {
            return new WwiseProperty("OverridePositioning", @override);
        }

        public static WwiseProperty Prop_OverridePriority(bool @override)
        {
            return new WwiseProperty("OverridePriority", @override);
        }

        public static WwiseProperty Prop_OverrideUserAuxSends(bool @override)
        {
            return new WwiseProperty("OverrideUserAuxSends", @override);
        }

        public static WwiseProperty Prop_OverrideVirtualVoice(bool @override)
        {
            return new WwiseProperty("OverrideVirtualVoice", @override);
        }

        public static WwiseProperty Prop_Pitch(int value)
        {
            return new WwiseProperty("Pitch", valueLimiter(value, -2400, 2400));
        }

        public enum Option_PlayMechanismInfiniteOrNumberOfLoops { NumberOfLoop = 0, Infinite = 1 }
        public static WwiseProperty Prop_PlayMechanismInfiniteOrNumberOfLoops(Option_PlayMechanismInfiniteOrNumberOfLoops option)
        {
            return new WwiseProperty("PlayMechanismInfiniteOrNumberOfLoops", (int)option);
        }

        public static WwiseProperty Prop_PlayMechanismLoop(bool loop)
        {
            return new WwiseProperty("PlayMechanismLoop", loop);
        }

        public static WwiseProperty Prop_PlayMechanismLoopCount(uint numberOfLoop)
        {
            return new WwiseProperty("PlayMechanismLoopCount", valueLimiter(numberOfLoop, 1, 32767));
        }

        public static WwiseProperty Prop_PlayMechanismResetPlaylistEachPlay(bool reset)
        {
            return new WwiseProperty("PlayMechanismResetPlaylistEachPlay", reset);
        }

        public static WwiseProperty Prop_PlayMechanismSpecialTransitions(bool transitions)
        {
            return new WwiseProperty("PlayMechanismSpecialTransitions", transitions);
        }

        public enum Option_PlayMechanismSpecialTransitionsType { Xfade_Amp = 0, Delay = 1, SampleAccurate = 2, TriggerRate = 3, Xfade_Power = 4 }
        public static WwiseProperty Prop_PlayMechanismSpecialTransitionsType(Option_PlayMechanismSpecialTransitionsType type)
        {
            return new WwiseProperty("PlayMechanismSpecialTransitionsType", (int)type);
        }

        public static WwiseProperty Prop_PlayMechanismSpecialTransitionsValue(float value)
        {
            return new WwiseProperty("PlayMechanismSpecialTransitionsValue", valueLimiter(value, 0, 3600));
        }

        public enum Option_PlayMechanismStepOrContinuous { Continous = 0, Step = 1 }
        public static WwiseProperty Prop_PlayMechanismStepOrContinuous(Option_PlayMechanismStepOrContinuous option)
        {
            return new WwiseProperty("PlayMechanismStepOrContinuous", (int)option);
        }

        public static WwiseProperty Prop_PreFetchLength(uint value)
        {
            return new WwiseProperty("PreFetchLength", valueLimiter(value, 0, 10000));
        }

        public static WwiseProperty Prop_Priority(uint value)
        {
            return new WwiseProperty("Priority", valueLimiter(value, 0, 10000));
        }

        public static WwiseProperty Prop_PriorityDistanceFactor(bool use)
        {
            return new WwiseProperty("PriorityDistanceFactor", use);
        }

        public static WwiseProperty Prop_PriorityDistanceOffset(int value)
        {
            return new WwiseProperty("PriorityDistanceOffset", valueLimiter(value, -100, 100));
        }

        public static WwiseProperty Prop_RandomAvoidRepeating(bool limitRepetition)
        {
            return new WwiseProperty("RandomAvoidRepeating", limitRepetition);
        }

        public static WwiseProperty Prop_RandomAvoidRepeatingCount(uint value)
        {
            return new WwiseProperty("RandomAvoidRepeatingCount", valueLimiter(value, 1, 999));
        }

        public enum Option_RandomOrSequence { Sequence = 0, Random = 1 }
        public static WwiseProperty Prop_RandomOrSequence(Option_RandomOrSequence option)
        {
            return new WwiseProperty("RandomOrSequence", (int)option);
        }

        public static WwiseProperty Prop_ReflectionsVolume(float value)
        {
            return new WwiseProperty("ReflectionsVolume", valueLimiter(value, -200, 200));
        }

        public static WwiseProperty Prop_RenderEffect0(bool render)
        {
            return new WwiseProperty("RenderEffect0", render);
        }

        public static WwiseProperty Prop_RenderEffect1(bool render)
        {
            return new WwiseProperty("RenderEffect1", render);
        }

        public static WwiseProperty Prop_RenderEffect2(bool render)
        {
            return new WwiseProperty("RenderEffect2", render);
        }

        public static WwiseProperty Prop_RenderEffect3(bool render)
        {
            return new WwiseProperty("RenderEffect3", render);
        }

        public enum Option_RestartBeginningOrBackward { PlayInReverseOrder = 0, Restart = 1 }
        public static WwiseProperty Prop_RestartBeginningOrBackward(Option_RestartBeginningOrBackward option)
        {
            return new WwiseProperty("RestartBeginningOrBackward", option);
        }


        public enum Option_SpeakerPanning { DirectAssignment = 0, BalanceFade = 1, Steering = 2 }
        public static WwiseProperty Prop_SpeakerPanning(Option_SpeakerPanning mode)
        {
            return new WwiseProperty("SpeakerPanning", (int)mode);
        }

        public static WwiseProperty Prop_SpeakerPanning_3DSpatializationMix(uint value)
        {
            return new WwiseProperty("SpeakerPanning3DSpatializationMix", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty Prop_UseGameAuxSends(bool use)
        {
            return new WwiseProperty("UseGameAuxSends", use);
        }

        public static WwiseProperty Prop_UseMaxSoundPerInstance(bool use)
        {
            return new WwiseProperty("UseMaxSoundPerInstance", use);
        }

        public static WwiseProperty Prop_UserAuxSendHPF0(uint value)
        {
            return new WwiseProperty("UserAuxSendHPF0", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty Prop_UserAuxSendHPF1(uint value)
        {
            return new WwiseProperty("UserAuxSendHPF1", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty Prop_UserAuxSendHPF2(uint value)
        {
            return new WwiseProperty("UserAuxSendHPF2", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty Prop_UserAuxSendHPF3(uint value)
        {
            return new WwiseProperty("UserAuxSendLPF3", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty Prop_UserAuxSendLPF0(uint value)
        {
            return new WwiseProperty("UserAuxSendLPF0", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty Prop_UserAuxSendLPF1(uint value)
        {
            return new WwiseProperty("UserAuxSendLPF1", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty Prop_UserAuxSendLPF2(uint value)
        {
            return new WwiseProperty("UserAuxSendLPF2", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty Prop_UserAuxSendLPF3(uint value)
        {
            return new WwiseProperty("UserAuxSendLPF3", valueLimiter(value, 0, 100));
        }

        public static WwiseProperty Prop_UserAuxSendVolume0(float value)
        {
            return new WwiseProperty("UserAuxSendVolume0", valueLimiter(value, -200, 100));
        }

        public static WwiseProperty Prop_UserAuxSendVolume1(float value)
        {
            return new WwiseProperty("UserAuxSendVolume1", valueLimiter(value, -200, 100));
        }

        public static WwiseProperty Prop_UserAuxSendVolume2(float value)
        {
            return new WwiseProperty("UserAuxSendVolume2", valueLimiter(value, -200, 100));
        }

        public static WwiseProperty Prop_UserAuxSendVolume3(float value)
        {
            return new WwiseProperty("UserAuxSendVolume3", valueLimiter(value, -200, 100));
        }

        public enum Option_VirtualVoiceQueueBehavior { PlayFromBeginning = 0, PlayFromElapsedTime = 1, Resume = 2 }
        public static WwiseProperty Prop_VirtualVoiceQueueBehavior(Option_VirtualVoiceQueueBehavior behavior)
        {
            return new WwiseProperty("VirtualVoiceQueueBehavior", (int)behavior);
        }

        public static WwiseProperty Prop_Volume(float value)
        {
            return new WwiseProperty("Volume", valueLimiter(value, -200, 200));
        }

        public static WwiseProperty Prop_Weight(float value)
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
