using System;

using Newtonsoft.Json.Linq;

using AK.Wwise.Waapi;
using System.Threading.Tasks;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public class WwiseObject
    {
        public enum ObjectType { AcousticTexture, Action, ActionException, ActorMixer, Attenuation, AudioDevice, AuxBus, BlendContainer, BlendTrack, Bus, ControlSurfaceBinding, ControlSurfaceBindingGroup, ControlSurfaceSession, Conversion, Curve, CustomState, DialogueEvent, Deffect, Event, ExternalSource, ExternalSourceFile, Folder, GameParameter, Language, Metadata, MidiParameter, MixingSession, Modifier, ModulatorEnvelope, ModulatorLfo, ModulatorTime, MultiSwitchEntry, MusicClip, MusicClipMidi, MusicCue, MusicEventCue, MusicFade, MusicPlaylistContainer, MusicPlaylistItem, MusicSegment, MusicStinger, MusicSwitchContainer, MusicTrack, MusicTrackSequence, MusicTransition, ObjectSettingAssoc, Panner, ParamControl, Path, Platform, PluginDataSource, Position, Project, Query, RandomSequenceContainer, SerchCriteria, Sound, SoundBank, SoundcasterSession, State, StateGroup, Switch, SwitchContainer, SwitchGroup, Trigger, UserProjectSettings, WorkUnit, SegmentRef }

        public string Name { get; set; }
        public string ID { get; set; }
        public string Type { get; set; }
        
        [Obsolete("Use GetPathAsync instead")]
        public string Path { get { return WwiseUtility.GetWwiseObjectPath(ID); } }

        [Obsolete("Use GetParentAsync() instead")]
        public WwiseObject Parent { get { return WwiseUtility.GetWwiseObjectByPath(Path.TrimEnd(Name.ToCharArray())); } }

        public async Task<WwiseObject> GetParentAsync()
        {
            string path = await GetPathAsync();
            return await WwiseUtility.GetWwiseObjectByPathAsync(System.IO.Path.GetDirectoryName(path));
        }
        
        public async Task<string> GetPathAsync()
        {
            return await WwiseUtility.GetWwiseObjectPathAsync(ID);
        }

        public WwiseObject(string name, string id, string type)
        {
            this.Name = name;
            this.ID = id;
            this.Type = type;
            //this.Path = WwiseUtility.GetWwiseObjectPath(id);
           // this.Parent = WwiseUtility.GetWwiseObjectByPath(this.Path.Replace(Name, ""));
        }
        
        public static WwiseObject Empty(ObjectType type)
        {
            return new WwiseObject("", "{00000000-0000-0000-0000-000000000000}",
                type.ToString());
        }

        public string GetPropertyAndReferenceNames()
        {
            var tmp = GetPropertyAndReferenceNamesAsync();
            tmp.Wait();
            return tmp.Result;
        }

        public async Task<string> GetPropertyAndReferenceNamesAsync()
        {
            try
            {
                // 创建物体
                var result = await WwiseUtility.Client.Call
                    (
                    ak.wwise.core.@object.getPropertyAndReferenceNames,
                    new JObject
                    {
                        new JProperty("object", ID)
                    }
                    );
                return result.ToString();
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to get property and reference names from {Name}! ======> {e.Message}");
                return null;
            }
        }

        public override string ToString()
        {
            return $"Name : {Name}, ID : {ID}, Type: {Type}, Path: {Path}";
        }
    }
}
