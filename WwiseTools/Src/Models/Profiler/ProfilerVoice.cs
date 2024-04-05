namespace WwiseTools.Models.Profiler
{
    public class ProfilerVoice
    {
        public uint PipelineID { get; set; }
        public uint PlayingID{ get; set; }
        public uint SoundID{ get; set; }
        public uint PlayTargetID{ get; set; }
        public sbyte Priority{ get; set; }
        public string ObjectGUID { get; set; }
        public string ObjectName { get; set; }
        public string PlayTargetGUID { get; set; }
        public string PlayTargetName { get; set; }
        public ulong GameObjectID { get; set; }
        public string GameObjectName { get; set; }
        public float BaseVolume { get; set; }
        public float GameAuxSendVolume { get; set; }
        public float Envelope{ get; set; }
        public float NormalizationGain{ get; set; }
        public float LowPassFilter{ get; set; }
        public float HighPassFilter{ get; set; }
        public bool IsStarted { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsForcedVirtual { get; set; }

    }
}
