using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using WwiseTools.Models.SoundBank;

namespace WwiseTools.Utils.SoundBank
{
    public static class GeneratedSoundBankExtension
    {
        public static async Task<List<GeneratedSoundBankPath>> GetGeneratedSoundBankPathsAsync(this WwiseUtility util)
        {
            var result = new List<GeneratedSoundBankPath>();

            if (!(await util.TryConnectWaapiAsync())) return result;

            var projectPath = util.ConnectionInfo.ProjectPath;
            XmlDocument doc = new XmlDocument();
            doc.Load(projectPath);
            XmlElement pathValues = doc.SelectSingleNode("//*/PropertyList/Property[@Name='SoundBankPaths']/ValueList") as XmlElement;
            if (pathValues == null) return result;

            foreach (XmlElement value in pathValues.GetElementsByTagName("Value"))
            {
                var bankPath = new GeneratedSoundBankPath() {
                    Platform = value.GetAttribute("Platform"), 
                    Path = Path.Combine(Path.GetDirectoryName(projectPath) ?? string.Empty, value.InnerText) 

                };

                result.Add(bankPath);
            }

            

            return result;
        }

        public static double BytesToMB(this WwiseUtility util, long bytes)
        {
            double result = (double)bytes / Math.Pow(1024.0, 2);

            return result;
        }

        
        
        
        public static async Task<long> GetTotalSoundBankSizeAsync(this WwiseUtility util, string platform = "")
        {
            if (!(await util.TryConnectWaapiAsync())) return 0;

            var infos = await  util.GetGeneratedSoundBankInfosAsync();

            List<string> files = new List<string>();

            var items = platform == "" ? infos : infos.Where(s => s.Platform == platform);

            foreach (var info in items)
            {
                files.Add(info.Path);
                files.AddRange(info.ReferencedStreamedFiles);
                files.AddRange(info.LooseMediaFiles);
            }

            long sum = 0;

            foreach (var file in files.Distinct())
            {
                if (File.Exists(file))
                {
                    sum += (new FileInfo(file)).Length;
                }
            }

            return sum;
        }

        public static async Task<List<GeneratedSoundBankInfo>> GetGeneratedSoundBankInfosAsync(this WwiseUtility util)
        {
            var result = new List<GeneratedSoundBankInfo>();

            if (!(await util.TryConnectWaapiAsync())) return result;

            var platformPaths = await util.GetGeneratedSoundBankPathsAsync();

            

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


                    if (util.ConnectionInfo.Version.Year >= 2022)
                    {
                        var mediaFiles = soundBankNode.SelectNodes(".//Media/File");

                        if (mediaFiles != null && mediaFiles.Count > 0)
                        {
                            foreach (XmlElement mediaFile in mediaFiles)
                            {
                                var fileRelativePath = info.Language == "SFX" ? "Media\\" : info.Language + "\\Media\\";
                                fileRelativePath += mediaFile.GetAttribute("Id") + ".wem";

                                string isStreaming = mediaFile.GetAttribute("Streaming");
                                string location = mediaFile.GetAttribute("Location");

                            
                            
                                var filePath = Path.Combine(path, fileRelativePath);


                                if (location != "Loose") continue;
                            
                                if (isStreaming == "true")
                                    info.ReferencedStreamedFiles.Add(filePath);
                                else
                                    info.LooseMediaFiles.Add(filePath);

                            }
                        }
                    }
                    else
                    {
                        var streamedFiles = soundBankNode.SelectNodes(".//ReferencedStreamedFiles/File");

                        if (streamedFiles != null && streamedFiles.Count > 0)
                        {
                            foreach (XmlElement streamedFile in streamedFiles)
                            {
                                var fileRelativePath = info.Language == "SFX" ? "" : info.Language + "\\";
                                fileRelativePath += streamedFile.GetAttribute("Id") + ".wem";

                                var filePath = Path.Combine(path, fileRelativePath);

                                info.ReferencedStreamedFiles.Add(filePath);

                            }
                        }

                        var looseMedias = soundBankNode.SelectNodes(".//ExcludedMemoryFiles/File");

                        if (looseMedias != null && looseMedias.Count > 0)
                        {
                            foreach (XmlElement looseMedia in looseMedias)
                            {
                                var fileRelativePath = info.Language == "SFX" ? "" : info.Language + "\\";
                                fileRelativePath += looseMedia.GetAttribute("Id") + ".wem";

                                var filePath = Path.Combine(path, fileRelativePath);

                                info.LooseMediaFiles.Add(filePath);

                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
