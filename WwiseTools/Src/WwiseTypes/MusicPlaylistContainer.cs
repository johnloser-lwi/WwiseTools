using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Utils;

namespace WwiseTools.WwiseTypes
{
    public class MusicPlaylistContainer : WwiseTypeBase
    {

        public async Task<WwiseObject> AddPlaylistItemGroupAsync()
        {
            var rootItem = await GetRootPlaylistItemAsync();

            if (rootItem != null) // && segment != null)
            {
                return await rootItem.AsMusicPlaylistItem().AddChildGroupAsync();
            }

            return null;
        }

        

        public async Task<WwiseObject> AddPlaylistItemSegmentAsync(WwiseObject segment)
        {
            var rootItem = await GetRootPlaylistItemAsync();

            if (rootItem != null) // && segment != null)
            {

                return await rootItem.AsMusicPlaylistItem().AddChildSegmentAsync(segment);
            }

            return null;
        }


        /// <summary>
        /// 获取播放列表根，同步执行
        /// </summary>
        /// <returns></returns>
        public async Task<WwiseObject> GetRootPlaylistItemAsync()
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return null;

            try
            {
                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        id = new string[] { WwiseObject.ID }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "musicPlaylistRoot" }

                };

                var func = WaapiFunctionList.CoreObjectGet;

                JObject jresult = await WwiseUtility.Instance.CallAsync(func, query, options, WwiseUtility.Instance.TimeOut);

                if (jresult["return"]?.Last == null) throw new Exception();
                if (jresult["return"].Last["musicPlaylistRoot"] == null) throw new Exception();
                string id = jresult["return"].Last["musicPlaylistRoot"]["id"]?.ToString();

                return await WwiseUtility.Instance.GetWwiseObjectByIDAsync(id);
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get PlaylistRoot of object : {WwiseObject.Name}! =======> {e.Message}");
                return null;
            }

        }

        public MusicPlaylistContainer(WwiseObject wwiseObject) : base(wwiseObject, nameof(MusicPlaylistContainer))
        {
        }
    }
}
