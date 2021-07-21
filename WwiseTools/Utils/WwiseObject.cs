using System;

using Newtonsoft.Json.Linq;

using AK.Wwise.Waapi;
using System.Threading.Tasks;

namespace WwiseTools.Utils
{
    public class WwiseObject
    {
        public enum ObjectType { AcousticTexture, Action, ActionException, ActorMixer, Attenuation, AudioDevice, AuxBux, BlendContainer, BlendTrack, Bus, ControlSurfaceBinding, ControlSurfaceBindingGroup, ControlSurfaceSession, Conversion, Curve, CustomState, DialogueEvent, Deffect, Event, ExternalSource, ExternalSourceFile, Folder, GameParameter, Language, Metadata, MidiParameter, MixingSession, Modifier, ModulatorEnvelope, ModulatorLfo, ModulatorTime, MultiSwitchEntry, MusicClip, MusicClipMidi, MusicCue, MusicEventCue, MusicFade, MusicPlaylistContainer, MusicPlaylistItem, MusicSegment, MusicStinger, MusicSwitchContainer, MusicTrack, MusicTrackSequence, MusicTransition, ObjectSettingAssoc, Panner, ParamControl, Path, Platform, PluginDataSource, Position, Project, Query, RandomSequenceContainer, SerchCriteria, Sound, SoundBank, SoundcasterSession, State, StateGroup, Switch, SwitchContainer, SwitchGroup, Trigger, UserProjectSettings, WorkUnit }

        public string Name { get; set; }
        public string ID { get; set; }
        public string Type { get; set; }
        public string Path { get
            {
                var get_path = getPath();
                get_path.Wait();
                return get_path.Result;
            }
        }

        private async Task<string> getPath()
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

                @return = new string[] {"path" }

            };

            JObject jresult = await WwiseUtility.Client.Call(ak.wwise.core.@object.get, query, options);

            try // 尝试返回物体数据
            {
                string path = jresult["return"].Last["path"].ToString();

                return path;
            }
            catch
            {
                Console.WriteLine($"Get path of object : {Name}!");
                return null;
            }
        }

        public WwiseObject(string name, string id, string type)
        {
            this.Name = name;
            this.ID = id;
            this.Type = type;
        }

        public override string ToString()
        {
            return $"Name : {Name}, ID : {ID}, Type: {Type}, Path: {Path}";
        }
    }
}
