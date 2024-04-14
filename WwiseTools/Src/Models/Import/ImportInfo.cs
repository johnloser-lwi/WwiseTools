using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Utils;

namespace WwiseTools.Models.Import;

public class ImportInfo
{
    [Obsolete("Use FromPath or FromPathBuilder instead")]
    public ImportInfo(string audioFile, WwisePathBuilder pathBuilder, string language = "SFX", string subFolder = "")
    {
        AudioFile = audioFile;
        PathBuilder = pathBuilder;
        Language = language;
        SubFolder = subFolder;
    }

    private ImportInfo()
    {
        
    }
    
    public static async Task<ImportInfo> FromPath(string audioFile, string objectPath, string language = "SFX", string subFolder = "")
    {
        var info = new ImportInfo();
        info.AudioFile = audioFile;
        info.Language = language;
        info.SubFolder = subFolder;

        await info.TryParseObjectPathAsync(objectPath);
        return info;
    }
    
    public static ImportInfo FromPathBuilder(string audioFile, WwisePathBuilder pathBuilder, string language = "SFX", string subFolder = "")
    {
        var info = new ImportInfo();
        info.AudioFile = audioFile;
        info.PathBuilder = pathBuilder;
        info.Language = language;
        info.SubFolder = subFolder;
        return info;
    }
    

    private async Task TryParseObjectPathAsync(string path)
    {
        var split = path.Replace('/', '\\').Split('\\');

        var isRoot = true;
        var root = "";

        WwisePathBuilder builder = null;

        void ThrowException()
        {
            var exceptionMsg = $"Object path {path} not valid!";
            throw new Exception(exceptionMsg);
        }
        
        
        foreach (var s in split)
        {
            if (string.IsNullOrEmpty(s)) continue;
            
            if (!s.StartsWith("<") && isRoot)
            {
                root += s + "\\";
                continue;
            }
            
            if (isRoot)
            {
                root = root.Trim('\\');
                builder = new WwisePathBuilder("\\" + root);
                isRoot = false;
            }

            if (!s.StartsWith("<")) ThrowException();

            var typeNameSplit = s.Split('>');
            if (typeNameSplit.Length != 2) ThrowException();

            var res = Enum.TryParse(typeNameSplit[0].Trim('<'), out WwiseObject.ObjectType type);

            if (!res) ThrowException();

            await builder.AppendHierarchyAsync(type, typeNameSplit[1]);
        }

        PathBuilder = builder;
    }

    public string Language { get; private set; }
    public string AudioFile { get; private set; }
    public WwisePathBuilder PathBuilder { get; private set; }
    public string SubFolder { get; private set; }

    public bool IsValid => !string.IsNullOrEmpty(Language) && !string.IsNullOrEmpty(AudioFile) &&
                           PathBuilder != null;

    internal async Task<JObject> ToJObjectImportPropertyAsync()
    {
        var properties = new JObject
        {
            new JProperty("importLanguage", Language),
            new JProperty("audioFile", AudioFile),
            new JProperty("objectPath", await PathBuilder.GetImportPathAsync())
        };
        if (!string.IsNullOrEmpty(SubFolder))
        {
            properties.Add(new JProperty("originalsSubFolder", SubFolder));
        }

        return properties;
    }
}