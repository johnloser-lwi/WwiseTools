namespace WwiseTools.Models.Profiler
{
    public class ProfilerCaptureLogItem
    {
        public string Type { get; set; }
        public int Time { get; set; }

        public string FormatTime
        {
            get
            {
                var seconds = (int)(Time / 1000) % 60;
                var minutes = (int)((Time / (1000 * 60)) % 60);
                var hours = (int)((Time / (1000 * 60 * 60)) % 24);

                return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
            }
        }
        public string ObjectID { get; set; }
        public string ObjectName { get; set; }
        public uint ObjectShortID { get; set; }
        public uint GameObjectID { get; set; }
        public string GameObjectName { get; set; }
        public uint PlayingID { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
        
        public string ErrorCodeName { get; set; }
    }
}
