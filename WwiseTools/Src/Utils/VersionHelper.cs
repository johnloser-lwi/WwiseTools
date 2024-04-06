using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Models;
using WwiseTools.Objects;

namespace WwiseTools.Utils
{
    public class VersionHelper
    {
      
        public static readonly WwiseVersion V2019_2_11_7512 = new(2019, 2, 11, 7512);

      
        public static readonly WwiseVersion V2021_1_0_7575 = new(2021, 1, 0, 7575);

      
        public static readonly WwiseVersion V2022_1_0_7929 = new(2022, 1, 0, 7929);

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
