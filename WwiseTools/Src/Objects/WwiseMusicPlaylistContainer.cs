using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public class WwiseMusicPlaylistContainer : WwiseContainer
    {
        /// <summary>
        /// 创建一个音乐片段
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentPath"></param>
        [Obsolete("use WwiseUtility.CreateObjectAsync instead")]
        public WwiseMusicPlaylistContainer(string name, string parentPath = @"\Interactive Music Hierarchy\Default Work Unit\") : base(name, "", ObjectType.MusicPlaylistContainer.ToString())
        {
            var playlist = WwiseUtility.CreateObject(name, ObjectType.MusicPlaylistContainer, parentPath);
            ID = playlist.ID;
            Name = playlist.Name;
        }

        public WwiseMusicPlaylistContainer(WwiseObject @object) : base("", "", "")
        {
            if (@object == null) return;
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;
        }

        /// <summary>
        /// 添加播放列表组
        /// </summary>
        /// <returns></returns>
        [Obsolete("use async version instead")]
        public WwiseMusicPlaylistItem AddPlaylistItemGroup()
        {
            var rootItem = GetRootPlaylistItem();

            if (rootItem != null) // && segment != null)
            {
                var item = new WwiseMusicPlaylistItem(WwiseMusicPlaylistItem.Option_PlaylistItemType.Group, rootItem.ID);
                return item;
            }

            return null;
        }

        public async Task<WwiseMusicPlaylistItem> AddPlaylistItemGroupAsync()
        {
            var rootItem = await GetRootPlaylistItemAsync();

            if (rootItem != null) // && segment != null)
            {
                var item = await 
                    WwiseMusicPlaylistItem.CreateWwiseMusicPlaylistItem(WwiseMusicPlaylistItem.Option_PlaylistItemType.Group, rootItem.ID);
                return item;
            }

            return null;
        }

        /// <summary>
        /// 添加播放列表片段
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        [Obsolete("use async version instead")]
        public WwiseMusicPlaylistItem AddPlaylistItemSegment(WwiseMusicSegment segment)
        {
            var rootItem = GetRootPlaylistItem();

            if (rootItem != null) // && segment != null)
            {
                var item = new WwiseMusicPlaylistItem(segment, rootItem.ID);

                //WwiseUtility.ReloadWwiseProject();

                return item;
            }

            return null;
        }

        public async Task<WwiseMusicPlaylistItem> AddPlaylistItemSegmentAsync(WwiseMusicSegment segment)
        {
            var rootItem = await GetRootPlaylistItemAsync();

            if (rootItem != null) // && segment != null)
            {
                var item = await WwiseMusicPlaylistItem.CreateWwiseMusicPlaylistItem(segment, rootItem.ID);

                //WwiseUtility.ReloadWwiseProject();

                return item;
            }

            return null;
        }

        /// <summary>
        /// 获取播放列表根
        /// </summary>
        /// <returns></returns>
        [Obsolete("use async version instead")]
        public WwiseMusicPlaylistItem GetRootPlaylistItem()
        {

            var rootItem = GetRootPlaylistItemAsync();
            rootItem.Wait();


            return rootItem.Result;
        }

        /// <summary>
        /// 获取播放列表根，同步执行
        /// </summary>
        /// <returns></returns>
        public async Task<WwiseMusicPlaylistItem> GetRootPlaylistItemAsync()
        {
            if (!await WwiseUtility.TryConnectWaapiAsync()) return null;

            try
            {
                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        id = new string[] { ID }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "musicPlaylistRoot" }

                };

                var func = WaapiFunction.CoreObjectGet;

                JObject jresult = await WwiseUtility.Client.Call(func, query, options);

                try // 尝试返回物体数据
                {

                    if (jresult["return"].Last["musicPlaylistRoot"] == null) throw new Exception();
                    string id = jresult["return"].Last["musicPlaylistRoot"]["id"].ToString();

                    return new WwiseMusicPlaylistItem(await WwiseUtility.GetWwiseObjectByIDAsync(id));
                }
                catch (Exception e)
                {
                    WaapiLog.Log($"Failed to get PlaylistRoot of object : {Name}! =======> {e.Message}");
                    return null;
                }
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to get PlaylistRoot of object : {Name}! =======> {e.Message}");
                return null;
            }

        }


    }
}
