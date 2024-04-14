namespace WwiseTools.Models.Profiler
{
    public class ProfilerBus
    {
        public uint PipelineID { get; set; }
        public ulong MixBusID { get; set; }
        public string ObjectGUID { get; set; }
        public string ObjectName { get; set; }
        public ulong GameObjectID { get; set; }
        public string GameObjectName { get; set; }
        public ulong MixerID { get; set; }
        public ulong DeviceID { get; set; }
        public float Volume { get; set; }
        public float DownStreamGain { get; set; }
        public uint VoiceCount { get; set; }
        public int  Depth { get; set; }

    }
}
