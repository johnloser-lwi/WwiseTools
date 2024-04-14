using Newtonsoft.Json;

namespace WwiseTools.Serialization;

public class GuidIdObjectData
{
    [JsonProperty("id")]
    public string ID { get; set; }
}

[JsonObject]
public class CommonObjectData : GuidIdObjectData
{
    [JsonProperty("name")]
    public string Name { get; set; }
}

public class ObjectReturnData : ShortIDObjectData
{
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("path")]
    public string Path { get; set; }
    
    [JsonProperty("filePath")]
    public string FilePath { get; set; }
    
    [JsonProperty("notes")]
    public string Notes { get; set; }
    
    [JsonProperty("pluginName")]
    public string PluginName { get; set; }
    
    [JsonProperty("parent")]
    public CommonObjectData Parent { get; set; }
    
    [JsonProperty("language")]
    public string Language { get; set; }
    
    [JsonProperty("maxDurationSource")]
    public MaxDurationSourceData MaxDurationSource { get; set; }
    
    [JsonProperty("owner")]
    public CommonObjectData Owner { get; set; }
    
    [JsonProperty("isPlayable")]
    public bool IsPlayable { get; set; }
    
    [JsonProperty("classId")]
    public string ClassID { get; set; }
    
    [JsonProperty("category")]
    public string Category { get; set; }
    
    [JsonProperty("workunit")]
    public CommonObjectData WorkUnit { get; set; }
    
    [JsonProperty("childrenCount")]
    public int ChildrenCount { get; set; }
    
    [JsonProperty("totalSize")]
    public int TotalSize { get; set; }
    
    [JsonProperty("mediaSize")]
    public int MediaSize { get; set; }
    
    [JsonProperty("objectSize")]
    public int ObjectSize { get; set; }
    
    [JsonProperty("structureSize")]
    public int StructureSize { get; set; }
    
    [JsonProperty("musicTransitionRoot")]
    public CommonObjectData MusicTransitionRoot { get; set; }
    
    [JsonProperty("musicPlaylistRoot")]
    public GuidIdObjectData MusicPlaylistRoot { get; set; }
    
    [JsonProperty("originalWavFilePath")]
    public string OriginalWavFilePath { get; set; }
    
    [JsonProperty("originalFilePath")]
    public string OriginalFilePath { get; set; }
    
    [JsonProperty("activeSource")]
    public CommonObjectData ActiveSource { get; set; }
    
    [JsonProperty("convertedWemFilePath")]
    public string ConvertedWemFilePath { get; set; }
    
    [JsonProperty("convertedFilePath")]
    public string ConvertedFilePath { get; set; }
    
    [JsonProperty("soundbankBnkFilePath")]
    public string SoundbankBnkFilePath { get; set; }
    
    [JsonProperty("playbackDuration")]
    public PlaybackDurationData PlaybackDuration { get; set; }
    
    [JsonProperty("duration")]
    public DurationData Duration { get; set; }
    
    [JsonProperty("audioSourceTrimValues")]
    public AudioSourceTrimValuesData AudioSourceTrimValues { get; set; }
    
    [JsonProperty("maxRadiusAttenuation")]
    public MaxRadiusAttenuationData MaxRadiusAttenuation { get; set; }
    
    [JsonProperty("audioSourceLanguage")]
    public CommonObjectData AudioSourceLanguage { get; set; }
    
    [JsonProperty("workunitIsDefault")]
    public bool WorkUnitIsDefault { get; set; }
    
    [JsonProperty("workunitType")]
    public WorkUnitTypeData WorkUnitType { get; set; }
    
    [JsonProperty("workunitIsDirty")]
    public bool WorkUnitIsDirty { get; set; }
    
    [JsonProperty("switchContainerChildContext")]
    public GuidIdObjectData SwitchContainerChildContext { get; set; }
    
    [JsonProperty("isExplicitMute")]
    public bool IsExplicitMute { get; set; }
    
    [JsonProperty("isExplicitSolo")]
    public bool IsExplicitSolo { get; set; }
    
    [JsonProperty("isImplicitMute")]
    public bool IsImplicitMute { get; set; }
    
    [JsonProperty("isImplicitSolo")]
    public bool IsImplicitSolo { get; set; }
    
    [JsonProperty("points")]
    public PointsData Points { get; set; }
}

[JsonObject]
public class PointsData 
{
    [JsonProperty("point")]
    public PointItemData[] Points { get; set; }
}

[JsonObject]
public class PointItemData 
{
    [JsonProperty("x")]
    public float X { get; set; }
    
    [JsonProperty("y")]
    public float Y { get; set; }
    
    [JsonProperty("shape")]
    public PointShape Shape { get; set; }
}

public enum PointShape
{
    [JsonProperty]
    Constant,
    [JsonProperty]
    Linear,
    [JsonProperty]
    Log3,
    [JsonProperty]
    Log2,
    [JsonProperty]
    Log1,
    [JsonProperty]
    InvertedSCurve,
    [JsonProperty]
    SCurve,
    [JsonProperty]
    Exp1,
    [JsonProperty]
    Exp2,
    [JsonProperty]
    Exp3,
}

public enum WorkUnitTypeData
{
    [JsonProperty("folder")]
    Folder,
    [JsonProperty("rootFile")]
    RootFile,
    [JsonProperty("nestedFile")]
    NestedFile
}

[JsonObject]
public class MaxRadiusAttenuationData : GuidIdObjectData
{
    [JsonProperty("radius")]
    public float MaxRadius { get; set; }
}

[JsonObject]
public class AudioSourceTrimValuesData
{
    [JsonProperty("trimBegin")]
    public float TrimBegin { get; set; }
    
    [JsonProperty("trimEnd")]
    public float TrimEnd { get; set; }
}

[JsonObject]
public class DurationData
{
    [JsonProperty("min")]
    public float Min { get; set; }
    
    [JsonProperty("max")]
    public float Max { get; set; }
    
    [JsonProperty("type")]
    public string Type { get; set; }

}

[JsonObject]
public class MaxDurationSourceData : GuidIdObjectData
{
    [JsonProperty("trimmedDuration")]
    public float TrimmedDuration { get; set; }
}

[JsonObject]
public class PlaybackDurationData
{
    [JsonProperty("playbackDurationMin")]
    public float PlaybackDurationMin { get; set; }
    
    [JsonProperty("playbackDurationMax")]
    public float PlaybackDurationMax { get; set; }
    
    [JsonProperty("playbackDurationType")]
    public string PlaybackDurationType { get; set; }
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
    public T[] Return { get; set; }
    [JsonProperty("objects")]
    public T[] Objects { get; set; }
}

[JsonObject]
public class BusReturnData
{
    [JsonProperty("pipelineID")]
    public uint PipelineID { get; set; }
    
    [JsonProperty("mixBusID")]
    public ulong MixBusID { get; set; }
    
    [JsonProperty("objectGUID")]
    public string ObjectGUID { get; set; }
    
    [JsonProperty("objectName")]
    public string ObjectName { get; set; }
    
    [JsonProperty("gameObjectID")]
    public ulong GameObjectID { get; set; }
    
    [JsonProperty("gameObjectName")]
    public string GameObjectName { get; set; }
    
    [JsonProperty("mixerID")]
    public uint MixerID { get; set; }
    
    [JsonProperty("deviceID")]
    public ulong DeviceID { get; set; }
    
    [JsonProperty("volume")]
    public float Volume { get; set; }
    
    [JsonProperty("downstreamGain")]
    public float DownstreamGain { get; set; }
    
    [JsonProperty("voiceCount")]
    public uint VoiceCount { get; set; }
    
    [JsonProperty("depth")]
    public int Depth { get; set; }
}

[JsonObject]
public class VoiceReturnData
{
    [JsonProperty("pipelineID")]
    public uint PipelineID { get; set; }
    
    [JsonProperty("playingID")]
    public uint PlayingID { get; set; }
    
    [JsonProperty("soundID")]
    public uint SoundID { get; set; }
    
    [JsonProperty("gameObjectID")]
    public ulong GameObjectID { get; set; }
    
    [JsonProperty("gameObjectName")]
    public string GameObjectName { get; set; }
    
    [JsonProperty("objectGUID")]
    public string ObjectGUID { get; set; }
    
    [JsonProperty("objectName")]
    public string ObjectName { get; set; }
    
    [JsonProperty("playTargetID")]
    public uint PlayTargetID { get; set; }  
    
    [JsonProperty("playTargetGUID")]
    public string PlayTargetGUID { get; set; }
    
    [JsonProperty("playTargetName")]
    public string PlayTargetName { get; set; }
    
    [JsonProperty("baseVolume")]
    public float BaseVolume { get; set; }
    
    [JsonProperty("gameAuxSendVolume")]
    public float GameAuxSendVolume { get; set; }
    
    [JsonProperty("envelope")]
    public float Envelope { get; set; }
    
    [JsonProperty("normalizationGain")]
    public float NormalizationGain { get; set; }
    
    [JsonProperty("lowPassFilter")]
    public float LowPassFilter { get; set; }
    
    [JsonProperty("highPassFilter")]
    public float HighPassFilter { get; set; }
    
    [JsonProperty("priority")]
    public sbyte Priority { get; set; }
    
    [JsonProperty("isStarted")]
    public bool IsStarted { get; set; }
    
    [JsonProperty("isVirtual")]
    public bool IsVirtual { get; set; }
    
    [JsonProperty("isForcedVirtual")]
    public bool IsForcedVirtual { get; set; }
}