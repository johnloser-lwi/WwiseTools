using System.Threading.Tasks;
using WwiseTools.Components;
using WwiseTools.Objects;
using WwiseTools.Properties;

namespace WwiseTools.Utils
{
    public class WwiseFactory
    {
        public static async Task<WwiseObject> CreateMusicSegmentAsync(string name, string filePath,
            string subFolder, WwiseObject parent)
        {
            var segment = await WwiseUtility.Instance.CreateObjectAsync(name, WwiseObject.ObjectType.MusicSegment, parent);
            var parentPath = System.IO.Path.Combine(await parent.GetPathAsync(), name);
            await WwiseUtility.Instance.ImportSoundAsync(filePath, "SFX", subFolder, parentPath);

            return segment;
        }
    }
}
