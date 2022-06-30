using System.Collections.Generic;
using System.IO;

namespace WwiseTools.Models.SoundBank;

public class GeneratedSoundBankInfo
{
    public string Platform { get; set; }
    public string Language { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public List<string> ReferencedStreamedFiles { get; } = new List<string>();
    public List<string> LooseMediaFiles { get; } = new List<string>();

    public long TotalSoundBankSize => SoundBankSize + StreamedMediaSize + LooseMediaSize;

    public long SoundBankSize
    {
        get
        {
            if (!File.Exists(Path)) return 0;
            return (new FileInfo(Path)).Length;
        }
    }


    public long StreamedMediaSize
    {
        get
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

    public long LooseMediaSize
    {
        get
        {
            long total = 0;
            foreach (var looseMediaFile in LooseMediaFiles)
            {
                if (!File.Exists(looseMediaFile)) continue;
                total += (new FileInfo(looseMediaFile).Length);
            }

            return total;
        }
    }

}