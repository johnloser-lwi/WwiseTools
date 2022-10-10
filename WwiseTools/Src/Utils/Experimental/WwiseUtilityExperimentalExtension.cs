using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WwiseTools.Objects;

namespace WwiseTools.Utils.Experimental;

public static class WwiseUtilityExperimentalExtension
{
    public static async Task<bool> ImportLocalizedLanguageAsync(this WwiseUtility util, WwiseObject root, string wavFilesFolder,
        string language,
        ImportAction importAction = ImportAction.useExisting)
    {
        List<string> GetFilesRecursively(string folder)
        {
            List<string> results = new List<string>();

            foreach (var file in Directory.GetFiles(folder))
            {
                if (!file.EndsWith(".wav")) continue;
                
                results.Add(file);
            }

            foreach (var directory in Directory.GetDirectories(folder))
            {
                results.AddRange(GetFilesRecursively(directory));
            }

            return results;
        }

        return await util.ImportLocalizedLanguageAsync(root, GetFilesRecursively(wavFilesFolder).ToArray(), language, importAction);
    }

    public static async Task<bool> ImportLocalizedLanguageAsync(this WwiseUtility util, WwiseObject root, string[] wavFiles, string language,
        ImportAction importAction = ImportAction.useExisting)
    {
        if (!await util.TryConnectWaapiAsync()) return false;

        var children = await util.GetWwiseObjectDescendantsAsync(root);

        var soundTargets = children.Where(s => s.Type == "Sound").ToList();

        var projectPath = Path.GetDirectoryName(await util.GetWwiseProjectPathAsync());
        
        if (projectPath == null) return false;

        var voicePath = Path.Combine(projectPath, "Originals\\Voices");

        try
        {
            foreach (var soundTarget in soundTargets)
            {
                var sound = soundTarget.AsSound();
                if (!await sound.IsVoiceAsync()) continue;

                var parent = await soundTarget.GetParentAsync();
                
                var sources = await sound.GetAudioFileSourcesAsync();

                string validWavFile = "";
                string audioFilePath = "";

                foreach (var wavFile in wavFiles)
                {
                    var fileName = Path.GetFileName(wavFile);

                    for (var i = 0; i < sources.Count; i++)
                    {
                        var source = sources[i];
                        var sourcePath = await source.GetAudioFilePathAsync();
                        if (sourcePath == null) continue;
                        var sourceLanguage = await source.GetLanguageAsync();
                        var sourceName = Path.GetFileName(sourcePath);

                        if (sourceName == fileName)
                        {
                            validWavFile = wavFile;

                            audioFilePath = sourcePath.Replace(voicePath, "").Replace(sourceLanguage, "").Trim('\\').Trim('/');

                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(validWavFile) || string.IsNullOrEmpty(audioFilePath)) continue;

                var subFolder = Path.GetDirectoryName(audioFilePath);
                
                await util.ImportSoundAsync(validWavFile, language, subFolder,
                    await parent.GetPathAsync(), soundTarget.Name, importAction);
            }
        }
        catch (Exception e)
        {
            WaapiLog.InternalLog($"Failed to import localized language ======> {e.Message}");
            return false;
        }



        return true;
    }
}