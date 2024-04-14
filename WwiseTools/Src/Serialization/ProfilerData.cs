using Newtonsoft.Json;

namespace WwiseTools.Serialization;

[JsonObject]
public class ProfilerAddItemData
{
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("time")]
    public int Time { get; set; }
    
    [JsonProperty("objectId")]
    public string ObjectID { get; set; }

    [JsonProperty("objectName")]
    public string ObjectName { get; set; }
    
    [JsonProperty("objectShortId")]
    public uint ObjectShortID { get; set; }
    
    [JsonProperty("gameObjectId")]
    public uint GameObjectID { get; set; }
    
    [JsonProperty("gameObjectName")]
    public string GameObjectName { get; set; }
    
    [JsonProperty("playingId")]
    public uint PlayingID { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }
    
    [JsonProperty("severity")]
    public SeverityData Severity { get; set; }
    
    [JsonProperty("errorCodeName")]
    public string ErrorCodeName { get; set; }
}

public enum SeverityData
{
    [JsonProperty("Normal")]
    Normal,
    [JsonProperty("Message")]
    Message,
    [JsonProperty("Error")]
    Error
}

[JsonObject]
public class ProfilerRTPCData : CommonObjectData
{
    [JsonProperty("gameObjectId")]
    public ulong GameObjectID { get; set; }
    
    [JsonProperty("value")]
    public float Value { get; set; }
}

[JsonObject]
public class CursorTimeData
{
    [JsonProperty("return")]
    public int Time { get; set; }   
}