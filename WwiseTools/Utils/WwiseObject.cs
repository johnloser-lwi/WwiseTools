using System;
namespace WwiseTools.Utils
{
    public class WwiseObject
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }

        public WwiseObject(string name, string id, string type, string path)
        {
            this.Name = name;
            this.ID = id;
            this.Type = type;
            this.Path = path;
        }

        public override string ToString()
        {
            return $"Name : {Name}, ID : {ID}, Type: {Type}, Path: {Path}";
        }
    }
}
