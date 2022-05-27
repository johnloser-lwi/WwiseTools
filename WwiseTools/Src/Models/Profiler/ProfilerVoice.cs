namespace WwiseTools.Models.Profiler
{
    public class ProfilerVoice
    {
        public int PipelineID { get; set; }
        public int PlayingID{ get; set; }
        public int SoundID{ get; set; }
        public int PlayTargetID{ get; set; }
        public int Priority{ get; set; }
        public string ObjectGUID { get; set; }
        public string ObjectName { get; set; }
        public string PlayTargetGUID { get; set; }
        public string PlayTargetName { get; set; }
        public int GameObjectID { get; set; }
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
