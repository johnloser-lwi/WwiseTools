using System.Collections.Generic;
using Newtonsoft.Json;

namespace WwiseTools.Serialization;

[JsonObject]
public class CommonObjectData
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("id")]
    public string ID { get; set; }
}

public class WwiseObjectData : CommonObjectData
{
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("path")]
    public string Path { get; set; }
    
    [JsonProperty("filePath")]
    public string FilePath { get; set; }
    
    [JsonProperty("notes")]
    public string Notes { get; set; }
}

[JsonObject]
public class ShortIDObjectData : CommonObjectData
{
    [JsonProperty("shortId")]
    public string ShortID { get; set; }
}

public class ReturnData<T>
{
    [JsonProperty("return")]
    public List<T> Return { get; set; }
    [JsonProperty("objects")]
    public List<T> Objects { get; set; }
}