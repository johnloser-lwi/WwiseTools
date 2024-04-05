using Newtonsoft.Json;

namespace WwiseTools.Serialization;

[JsonObject]
public class GetSwitchAssingmentsData
{
    [JsonProperty("child")]
    public string Child { get; set; }
    
    [JsonProperty("stateOrSwitch")]
    public string StateOrSwitch { get; set; }
}