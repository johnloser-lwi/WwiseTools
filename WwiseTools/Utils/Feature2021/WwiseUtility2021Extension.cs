using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WwiseTools.Objects;

namespace WwiseTools.Utils.Feature2021
{
    public static class WwiseUtility2021Extension
    {
        private static bool VersionVerify([CallerMemberName] string caller = "")
        {
            if (WwiseUtility.ConnectionInfo.Version.Year < 2021)
            {
                //var caller = (new System.Diagnostics.StackTrace()).GetFrame(0).GetMethod().Name;
                WaapiLog.Log($"Warning: {caller} is a Wwise 2022 feature! " +
                             $"Current Wwise version is {WwiseUtility.ConnectionInfo.Version.ToString()}.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 通过Waql检索
        /// </summary>
        /// <param name="util"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async ValueTask<List<WwiseObject>> Waql(this WwiseUtility util, string query)
        {
            if (!await WwiseUtility.TryConnectWaapiAsync() || string.IsNullOrEmpty(query)) return new List<WwiseObject>();
            if (!VersionVerify()) return new List<WwiseObject>();

            Waql waql = new Waql(query);
            if (await waql.RunAsync())
            {
                return waql.Result;
            }

            return new List<WwiseObject>();
        }
    }
}
