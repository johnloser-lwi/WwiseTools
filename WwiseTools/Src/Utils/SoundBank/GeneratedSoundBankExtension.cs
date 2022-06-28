using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using WwiseTools.Models.SoundBank;

namespace WwiseTools.Utils.SoundBank
{
    public static class GeneratedSoundBankExtension
    {
        public static async Task<List<GeneratedSoundBankPath>> GetGeneratedSoundBankPaths(this WwiseUtility util)
        {
            var result = new List<GeneratedSoundBankPath>();

            var projectPath = await util.GetWwiseProjectPathAsync();
            XmlDocument doc = new XmlDocument();
            doc.Load(projectPath);
            XmlElement pathValues = doc.SelectSingleNode("//*/PropertyList/Property[@Name='SoundBankPaths']/ValueList") as XmlElement;
            if (pathValues == null) return result;

            foreach (XmlElement value in pathValues.GetElementsByTagName("Value"))
            {
                var bankPath = new GeneratedSoundBankPath() {
                    Platform = value.GetAttribute("Platform"), 
                    Path = Path.Combine(Path.GetDirectoryName(projectPath), value.InnerText) 

                };

                result.Add(bankPath);
            }

            

            return result;
        }

        public static async Task<List<GeneratedSoundBankInfo>> GetGeneratedSoundBankInfos(this WwiseUtility util)
        {
            var platformPaths = await util.GetGeneratedSoundBankPaths();

            var result = new List<GeneratedSoundBankInfo>();

            foreach (var generatedSoundBankPath in platformPaths)
            {
                var path = generatedSoundBankPath.Path;
                var platform = generatedSoundBankPath.Platform;

                var soundBankInfoPath = Path.Combine(path, "SoundbanksInfo.xml");
                if (!File.Exists(soundBankInfoPath)) continue;

                XmlDocument doc = new XmlDocument();
                doc.Load(soundBankInfoPath);

                var soundBankNodes = doc.GetElementsByTagName("SoundBank");

                foreach (XmlElement soundBankNode in soundBankNodes)
                {
                    var info = new GeneratedSoundBankInfo();
                    info.Platform = platform;

                    info.Name = soundBankNode.SelectSingleNode("ShortName")?.InnerText;


                    var rPath = soundBankNode.SelectSingleNode("Path")?.InnerText;

                    if (string.IsNullOrEmpty(rPath)) continue;
                    info.Path = Path.Combine(path, rPath);
                    info.Language = soundBankNode.GetAttribute("Language");

                    result.Add(info);

                    var streamedFiles = soundBankNode.SelectNodes(".//ReferencedStreamedFiles/File");

                    if (streamedFiles == null || streamedFiles.Count <= 0) continue;

                    foreach (XmlElement streamedFile in streamedFiles)
                    {
                        var fileRelativePath = info.Language == "SFX"? "" : info.Language + "\\";
                        fileRelativePath += streamedFile.GetAttribute("Id") + ".wem";

                        var filePath = Path.Combine(path, fileRelativePath);

                        info.ReferencedStreamedFiles.Add(filePath);

                    }


                }
            }

            return result;
        }
    }
}
