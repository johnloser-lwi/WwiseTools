using Newtonsoft.Json;

namespace WwiseTools.Serialization;

[JsonObject]
public class GetConnectionStatusData
{
    [JsonProperty("isConnected")]
    public bool IsConnected { get; set; }
    
    [JsonProperty("status")]
    public string Status { get; set; }
    
    [JsonProperty("console")]
    public ConsoleData Console { get; set; }
}

[JsonObject]
public class ConsoleData
{
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("platform")]
    public string Platform { get; set; }
    
    [JsonProperty("customPlatform")]
    public string CustomPlatform { get; set; }
    
    [JsonProperty("host")]
    public string Host { get; set; }
    
    [JsonProperty("appName")]
    public string AppName { get; set; }    
}