using Newtonsoft.Json;

namespace WwiseTools.Serialization;

[JsonObject]
public class GetInfoData
{
    [JsonProperty("displayName")]
    public string DisplayName { get; set; }
    [JsonProperty("platform")]
    public string Platform { get; set; }
    [JsonProperty("version")]
    public GetInfoVersionData Version { get; set; }
    [JsonProperty("apiVersion")]
    public int ApiVersion { get; set; }
    [JsonProperty("sessionId")]
    public string SessionID { get; set; }
    [JsonProperty("branch")]
    public string Branch { get; set; }
    [JsonProperty("copyRight")]
    public string CopyRight { get; set; }
    [JsonProperty("configuration")]
    public string Configuration { get; set; }
    [JsonProperty("processId")]
    public int ProcessID { get; set; }
    [JsonProperty("isCommandLine")]
    public bool IsCommandLine { get; set; }
    [JsonProperty("processPath")]
    public string ProcessPath { get; set; }
    [JsonProperty("directories")]
    public GetInfoDirectoriesData Directories { get; set; }
}

[JsonObject]
public class GetInfoVersionData
{
    [JsonProperty("displayName")]
    public string DisplayName { get; set; }
    [JsonProperty("major")]
    public int Major { get; set; }
    [JsonProperty("minor")]
    public int Minor { get; set; }
    [JsonProperty("year")]
    public int Year { get; set; }
    [JsonProperty("build")]
    public int Build { get; set; }
    [JsonProperty("schema")]
    public int Schema { get; set; }
    [JsonProperty("nickname")]
    public string Nickname { get; set; }
    
}

[JsonObject]
public class GetInfoDirectoriesData
{
    [JsonProperty("bin")]
    public string Bin { get; set; }
    [JsonProperty("user")]
    public string User { get; set; }
    [JsonProperty("help")]
    public string Help { get; set; }
    [JsonProperty("authoring")]    
    public string Authoring { get; set; }
    [JsonProperty("install")]
    public string Install { get; set; }
}