using System.Collections.Generic;
using Newtonsoft.Json;

namespace WwiseTools.Serialization;

[JsonObject]
public class GetProjectInfoData : CommonObjectData
{
    [JsonProperty("currentLanguageId")]
    public string CurrentLanguageID { get; set; }
    [JsonProperty("currentPlatformId")]
    public string CurrentPlatformId { get; set; }
    [JsonProperty("defaultConversionSettings")]
    public string DefaultConversionSettings { get; set; }
    [JsonProperty("directories")]
    public ProjectInfoDirectoriesData Directories { get; set; }
    [JsonProperty("displayTitle")]
    public string DisplayTitle { get; set; }
    [JsonProperty("isDirty")]
    public bool IsDirty { get; set; }
    [JsonProperty("languages")]
    public List<ShortIDObjectData> Languages { get; set; }
    [JsonProperty("platforms")]
    public List<ProjectInfoPlatformData> Platforms { get; set; }
    [JsonProperty("path")]    
    public string Path { get; set; }
    [JsonProperty("referenceLanguageId")]
    public string ReferenceLanguageID { get; set; }
}

[JsonObject]
public class ProjectInfoDirectoriesData
{
    [JsonProperty("cache")]
    public string Cache { get; set; }
    [JsonProperty("commands")]
    public string Commands { get; set; }
    [JsonProperty("originals")]
    public string Originals { get; set; }
    [JsonProperty("properties")]
    public string Properties { get; set; }
    [JsonProperty("root")]
    public string Root { get; set; }
    [JsonProperty("soundBankOutputRoot")]
    public string SoundBankOutputRoot { get; set; }
}

[JsonObject]
public class ProjectInfoPlatformData : CommonObjectData
{
    [JsonProperty("baseDisplayName")]
    public string BaseDisplayName { get; set; }
    [JsonProperty("baseName")]
    public string BaseName { get; set; }
    [JsonProperty("copiedMediaPath")]
    public string CopiedMediaPath { get; set; }
    [JsonProperty("soundBankPath")]
    public string SoundBankPath { get; set; }
}
