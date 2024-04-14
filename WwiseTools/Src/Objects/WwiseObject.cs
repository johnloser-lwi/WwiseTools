using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public partial class WwiseObject
    {
        public enum ObjectType { AcousticTexture, Action, ActionException, ActorMixer, Attenuation, AudioDevice, AudioFileSource, SourcePlugin, AuxBus, BlendContainer, BlendTrack, Bus, ControlSurfaceBinding, ControlSurfaceBindingGroup, ControlSurfaceSession, Conversion, Curve, CustomState, DialogueEvent, Deffect, Event, ExternalSource, ExternalSourceFile, Folder, GameParameter, Language, Metadata, MidiParameter, MixingSession, Modifier, ModulatorEnvelope, ModulatorLfo, ModulatorTime, MultiSwitchEntry, MusicClip, MusicClipMidi, MusicCue, MusicEventCue, MusicFade, MusicPlaylistContainer, MusicPlaylistItem, MusicSegment, MusicStinger, MusicSwitchContainer, MusicTrack, MusicTrackSequence, MusicTransition, ObjectSettingAssoc, Panner, ParamControl, Path, Platform, PluginDataSource, Position, Project, Query, RandomSequenceContainer, SearchCriteria, Sound, SoundBank, SoundcasterSession, State, StateGroup, Switch, SwitchContainer, SwitchGroup, Trigger, UserProjectSettings, WorkUnit, SegmentRef }

        public string Name { get; set; }
        public string ID { get; set; }
        public string Type { get; set; }
        
        public string Path { get; set; }

        public async Task<WwiseObject> GetParentAsync()
        {
            return await WwiseUtility.Instance.GetWwiseObjectParentAsync(this);
        }

        public string GetParentPath()
        {
            var split = Path.Split('\\');
            var parent = "";
            for (var i = 0; i < split.Length - 1; i++)
            {
                parent += split[i] + "\\";
            }
            return parent.TrimEnd('\\');
        }
        
        public async Task<string> GetPathAsync()
        {
            Path = await WwiseUtility.Instance.GetWwiseObjectPathAsync(ID);
            return Path;
        }

        public WwiseObject(string name, string id, string type, string path)
        {
            Name = name;
            ID = id;
            Type = type;
            Path = path;
        }
        
        public static WwiseObject Empty(ObjectType type)
        {
            return new WwiseObject("", "{00000000-0000-0000-0000-000000000000}",
                type.ToString(), "");
        }

        public async Task<string> GetPropertyAndReferenceNamesAsync()
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return null;


            try
            {
                var func = WwiseUtility.Instance.Function.Verify("ak.wwise.core.object.getPropertyAndReferenceNames");

              
                var result = await WwiseUtility.Instance.CallAsync
                    (
                        func,
                        new JObject
                        {
                            new JProperty("object", ID)
                        },
                        null,
                        WwiseUtility.Instance.TimeOut
                    );
                return result.ToString();
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get property and reference names from {Name}! ======> {e.Message}");
                return null;
            }
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var wwiseObject = obj as WwiseObject;
            if (wwiseObject is null) return false;

            return ID == wwiseObject.ID;
        }

        public static bool operator == (WwiseObject left, WwiseObject right)
        {
            if (left is null)
                return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(WwiseObject left, WwiseObject right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"Name : {Name}, ID : {ID}, Type: {Type}";
        }

    }
}
