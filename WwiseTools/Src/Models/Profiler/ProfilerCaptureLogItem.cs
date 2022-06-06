namespace WwiseTools.Models.Profiler
{
    public class ProfilerCaptureLogItem
    {
        public string Type { get; set; }
        public int Time { get; set; }
        public string ObjectID { get; set; }
        public string ObjectName { get; set; }
        public int ObjectShortID { get; set; }
        public int GameObjectID { get; set; }
        public string GameObjectName { get; set; }
        public int PlayingID { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
    }
}
