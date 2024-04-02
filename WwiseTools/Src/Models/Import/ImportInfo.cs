using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Utils;

namespace WwiseTools.Models.Import;

public class ImportInfo
{
    public ImportInfo(string audioFile, WwisePathBuilder pathBuilder, string language = "SFX", string subFolder = "")
    {
        AudioFile = audioFile;
        PathBuilder = pathBuilder;
        Language = language;
        SubFolder = subFolder;
    }

    private string _objectPath;
    public ImportInfo(string audioFile, string objectPath, string language = "SFX", string subFolder = "")
    {
        AudioFile = audioFile;
        Language = language;
        SubFolder = subFolder;

        _objectPath = objectPath;
    }

    private async Task TryParseObjectPathAsync(string path)
    {
        var split = path.Replace('/', '\\').Split('\\');

        bool isRoot = true;
        string root = "";

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

            await builder.AppendHierarchy(type, typeNameSplit[1]);
        }

        PathBuilder = builder;
    }

    public string Language { get; private set; }
    public string AudioFile { get; private set; }
    public WwisePathBuilder PathBuilder { get; private set; }
    public string SubFolder { get; private set; }

    public bool IsValid => !string.IsNullOrEmpty(Language) && !string.IsNullOrEmpty(AudioFile) &&
                           PathBuilder != null;

    internal async Task<JObject> ToJObjectImportProperty()
    {
        if (!String.IsNullOrEmpty(_objectPath)) await TryParseObjectPathAsync(_objectPath);
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