namespace WwiseTools.Models.Profiler
{
    public class ProfilerBus
    {
        public int PipelineID { get; set; }
        public int MixBusID { get; set; }
        public string ObjectGUID { get; set; }
        public string ObjectName { get; set; }
        public int GameObjectID { get; set; }
        public string GameObjectName { get; set; }
        public int MixerID { get; set; }
        public int DeviceID { get; set; }
        public float Volume { get; set; }
        public float DownStreamGain { get; set; }
        public int VoiceCount { get; set; }
        public int  Depth { get; set; }

    }
}
