using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.References;
using WwiseTools.Utils;

namespace WwiseTools.Components;

public class Action : ComponentBase
{
    public Action(WwiseObject wwiseObject) : base(wwiseObject, "Action")
    {
    }
    
    public enum ActionType
    {
        Play = 1,
        Stop = 2,
        StopAll = 3,
        Pause = 7,
        PauseAll = 8,
        Resume = 9,
        ResumeAll = 10,
        Break = 34,
        Seek = 36,
        SeekAll = 37,
        PostEvent = 41,
        SetBusVolume = 11,
        ResetBusVolume = 14,
        ResetBusVolumeAll = 15,
        SetVoiceVolume = 12,
        ResetVoiceVolume = 16,
        ResetVoiceVolumeAll = 17,
        SetVoicePitch = 13,
        ResetPitch = 18,
        ResetPitchAll = 19,
        SetLPF = 26,
        ResetLPF = 27,
        ResetLFPAll = 28,
        SetHPF = 29,
        ResetHPF = 30,
        ResetHPFAll = 31,
        Mute = 4,
        UnMute = 5,
        UnMuteAll = 6,
        SetGameParameter = 38,
        ResetGameParameter = 39,
        EnableState = 20,
        DisableState = 21,
        SetState = 22,
        SetSwitch = 23,
        Trigger = 35,
        EnableBypass = 24,
        DisableBypass = 25,
        ResetBypassEffect = 32,
        ResetBypassEffectAll = 33,
        ReleaseEnvelope = 40,
        ResetPlaylist = 42
        
    }

    public async Task SetTargetAsync(WwiseObject target)
    {
        await WwiseUtility.Instance.SetObjectReferenceAsync(WwiseObject, new WwiseReference("Target", target));
    }

    public async Task SetActionTypeAsync(ActionType type)
    {
        await WwiseUtility.Instance.SetObjectPropertyAsync(WwiseObject, new WwiseProperty("ActionType", (int) type));
    }
}