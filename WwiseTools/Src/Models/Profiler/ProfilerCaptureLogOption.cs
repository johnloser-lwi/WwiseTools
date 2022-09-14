using System.Linq;
using WwiseTools.Utils;

namespace WwiseTools.Models.Profiler
{
    public class ProfilerCaptureLogOption : WwiseOption
    {
        public bool Notification { get; set; }
        public bool MusicTransition { get; set; }
        public bool InteractiveMusic
        {
            get => MusicTransition;
            set => MusicTransition = value;
        }
        
        public bool Midi { get; set; }
        public bool ExternalSource { get; set; }
        public bool Marker { get; set; }
        public bool State { get; set; }
        public bool Switch { get; set; }
        public bool SetParameter { get; set; }
        public bool ParameterChanged { get; set; }
        public bool Bank { get; set; }
        public bool Prepare { get; set; }
        public bool Event { get; set; }
        public bool DialogueEventResolved { get; set; }
        public bool ActionTriggered { get; set; }
        public bool ActionDelayed { get; set; }
        public bool Message { get; set; }
        public bool APICall { get; set; }
        public bool GameObjectRegistration { get; set; }

        public override string[] GetOptions()
        {
            var options = base.GetOptions();
            if (!WwiseUtility.Instance.IsConnected()) return options;

            var listOptions = options.ToList();
            
            if (WwiseUtility.Instance.ConnectionInfo.Version.Year >= 2022 &&
                listOptions.Contains(nameof(MusicTransition)))
            {
                listOptions.Remove(nameof(MusicTransition));
            }

            return listOptions.ToArray();

        }
    }
}
