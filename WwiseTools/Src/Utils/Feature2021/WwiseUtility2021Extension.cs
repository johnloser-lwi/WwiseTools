using System.Collections.Generic;
using System.Threading.Tasks;
using WwiseTools.Objects;

namespace WwiseTools.Utils.Feature2021
{
    public static class WwiseUtility2021Extension
    {
        
      
      
      
      
      
      
        public static async Task<List<WwiseObject>> Waql(this WwiseUtility util, string query)
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync() || string.IsNullOrEmpty(query)) return new List<WwiseObject>();
            if (!VersionHelper.VersionVerify(VersionHelper.V2021_1_0_7575)) return new List<WwiseObject>();

            var waql = new Waql(query);
            if (await waql.RunAsync())
            {
                return waql.Result;
            }

            return new List<WwiseObject>();
        }
    }
}
