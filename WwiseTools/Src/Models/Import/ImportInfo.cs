using System.IO;
using Newtonsoft.Json.Linq;

namespace WwiseTools.Models.Import;

public class ImportInfo
{
    public ImportInfo(string audioFile, string objectPath, string language = "SFX", string subFolder = "")
    {
        AudioFile = audioFile;
        ObjectPath = objectPath;
        Language = language;
        SubFolder = subFolder;
    }

    public string Language { get; private set; }
    public string AudioFile { get; private set; }
    public string ObjectPath { get; private set; }
    public string SubFolder { get; private set; }
    
    public string SoundName { get; set; }

    public bool IsValid => !string.IsNullOrEmpty(Language) && !string.IsNullOrEmpty(AudioFile) &&
                           !string.IsNullOrEmpty(ObjectPath);

    internal JObject ToJObjectImportProperty()
    {
        if (string.IsNullOrEmpty(SoundName))
        {
            SoundName = Path.GetFileName(AudioFile).Replace(".wav", ""); // 尝试获取文件名
        }
        
        
        var properties = new JObject
        {
            new JProperty("importLanguage", Language),
            new JProperty("audioFile", AudioFile),
            new JProperty("objectPath", ObjectPath)
        };
        if (!string.IsNullOrEmpty(SubFolder))
        {
            properties.Add(new JProperty("originalsSubFolder", SubFolder));
        }

        return properties;
    }
}