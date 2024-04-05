using Newtonsoft.Json;

namespace WwiseTools.Serialization;

[JsonObject]
public class GetSoundBankInclusionData
{
    [JsonProperty("inclusions")]
    public InclusionItemData[] Inclusions { get; set; }
}

[JsonObject]
public class InclusionItemData
{
    [JsonProperty("object")]
    public string Object { get; set; }
    
    [JsonProperty("filter")]
    public InclusionFilterData[] Filter { get; set; }
}

public enum InclusionFilterData
{
    [JsonProperty("events")]
    Events,
    [JsonProperty("media")]
    Media,
    [JsonProperty("structures")]
    Structures
}