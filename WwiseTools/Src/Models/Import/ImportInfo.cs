using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Utils;

namespace WwiseTools.Models.Import;

public class ImportInfo
{
    public ImportInfo(string audioFile, WwisePathBuilder objectPath, string language = "SFX", string subFolder = "")
    {
        AudioFile = audioFile;
        ObjectPath = objectPath;
        Language = language;
        SubFolder = subFolder;
    }

    public string Language { get; private set; }
    public string AudioFile { get; private set; }
    public WwisePathBuilder ObjectPath { get; private set; }
    public string SubFolder { get; private set; }

    public bool IsValid => !string.IsNullOrEmpty(Language) && !string.IsNullOrEmpty(AudioFile) &&
                           ObjectPath != null;

    internal async Task<JObject> ToJObjectImportProperty()
    {
        var properties = new JObject
        {
            new JProperty("importLanguage", Language),
            new JProperty("audioFile", AudioFile),
            new JProperty("objectPath", await ObjectPath.GetImportPathAsync())
        };
        if (!string.IsNullOrEmpty(SubFolder))
        {
            properties.Add(new JProperty("originalsSubFolder", SubFolder));
        }

        return properties;
    }
}