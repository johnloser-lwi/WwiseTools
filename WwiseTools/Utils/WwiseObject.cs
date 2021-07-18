using System;
namespace WwiseTools.Utils
{
    public struct WwiseObject
    {
        string name;
        string id;
        string type;

        public WwiseObject(string name, string id, string type)
        {
            this.name = name;
            this.id = id;
            this.type = type;
        }

        public override string ToString()
        {
            return $"Name : {name}, ID : {id}, Type: {type}";
        }
    }
}
