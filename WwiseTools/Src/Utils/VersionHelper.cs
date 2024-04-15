using System.Runtime.CompilerServices;
using WwiseTools.Models;

namespace WwiseTools.Utils
{
    public class VersionHelper
    {
      
        public static readonly WwiseVersion V2019_2_11_7512 = new(2019, 2, 11, 7512);

      
        public static readonly WwiseVersion V2021_1_0_7575 = new(2021, 1, 0, 7575);

      
        public static readonly WwiseVersion V2022_1_0_7929 = new(2022, 1, 0, 7929);
        
        public static readonly WwiseVersion V2023_1_0_8367 = new(2023, 1, 0, 8367);

        public static bool VersionVerify(WwiseVersion minimumVersion, [CallerMemberName] string caller = "")
        {
            if (WwiseUtility.Instance.ConnectionInfo.Version < minimumVersion)
            {
              
                WaapiLog.InternalLog($"Warning: {caller} required minimum Wwise version {minimumVersion.ToString()}! " +
                             $"Current Wwise minimumVersion is {WwiseUtility.Instance.ConnectionInfo.Version.ToString()}.");
                return false;
            }
            return true;
        }
    }
}
