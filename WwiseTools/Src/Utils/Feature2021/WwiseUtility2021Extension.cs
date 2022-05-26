using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WwiseTools.Objects;

namespace WwiseTools.Utils.Feature2021
{
    public static class WwiseUtility2021Extension
    {
        
        /// <summary>
        /// 通过Waql检索
        /// </summary>
        /// <param name="util"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async Task<List<WwiseObject>> Waql(this WwiseUtility util, string query)
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync() || string.IsNullOrEmpty(query)) return new List<WwiseObject>();
            if (!VersionHelper.VersionVerify(VersionHelper.V2021_1_0_7575)) return new List<WwiseObject>();

            Waql waql = new Waql(query);
            if (await waql.RunAsync())
            {
                return waql.Result;
            }

            return new List<WwiseObject>();
        }
    }
}
