using System.Collections.Generic;
using System.IO;

namespace WwiseTools.Models.SoundBank;

public class GeneratedSoundBankInfo
{
    public string Platform { get; set; }
    public string Language { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public List<string> ReferencedStreamedFiles { get; set; } = new List<string>();

    public long SoundBankSize(bool includeStreamedMedia = true)
    {

        if (!File.Exists(Path)) return 0;

        long total = 0;
        long bankSize = (new FileInfo(Path)).Length;

        total += bankSize;

        if (!includeStreamedMedia) return total;

        total += StreamedMediaSize();

        return total;
    }

    public long StreamedMediaSize()
    {
        long total = 0;
        foreach (var referencedStreamedFile in ReferencedStreamedFiles)
        {
            if (!File.Exists(referencedStreamedFile)) continue;
            total += (new FileInfo(referencedStreamedFile).Length);
        }

        return total;
    }
}