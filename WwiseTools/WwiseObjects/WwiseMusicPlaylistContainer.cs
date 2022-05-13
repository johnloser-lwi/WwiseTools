using AK.Wwise.Waapi;
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
        /// <param name="parent_path"></param>
        [Obsolete("use WwiseUtility.CreateObjectAsync instead")]
        public WwiseMusicPlaylistContainer(string name, string parent_path = @"\Interactive Music Hierarchy\Default Work Unit\") : base(name, "", ObjectType.MusicPlaylistContainer.ToString())
        {
            var playlist = WwiseUtility.CreateObject(name, ObjectType.MusicPlaylistContainer, parent_path);
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
            var root_item = GetRootPlaylistItem();

            if (root_item != null) // && segment != null)
            {
                var item = new WwiseMusicPlaylistItem(WwiseMusicPlaylistItem.Option_PlaylistItemType.Group, root_item.ID);
                return item;
            }

            return null;
        }

        public async Task<WwiseMusicPlaylistItem> AddPlaylistItemGroupAsync()
        {
            var root_item = await GetRootPlaylistItemAsync();

            if (root_item != null) // && segment != null)
            {
                var item = await 
                    WwiseMusicPlaylistItem.CreateWwiseMusicPlaylistItem(WwiseMusicPlaylistItem.Option_PlaylistItemType.Group, root_item.ID);
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
            var root_item = GetRootPlaylistItem();

            if (root_item != null) // && segment != null)
            {
                var item = new WwiseMusicPlaylistItem(segment, root_item.ID);

                //WwiseUtility.ReloadWwiseProject();

                return item;
            }

            return null;
        }

        public async Task<WwiseMusicPlaylistItem> AddPlaylistItemSegmentAsync(WwiseMusicSegment segment)
        {
            var root_item = await GetRootPlaylistItemAsync();

            if (root_item != null) // && segment != null)
            {
                var item = await WwiseMusicPlaylistItem.CreateWwiseMusicPlaylistItem(segment, root_item.ID);

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

            var root_item = GetRootPlaylistItemAsync();
            root_item.Wait();


            return root_item.Result;
        }

        /// <summary>
        /// 获取播放列表根，同步执行
        /// </summary>
        /// <returns></returns>
        public async Task<WwiseMusicPlaylistItem> GetRootPlaylistItemAsync()
        {
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

                JObject jresult = await WwiseUtility.Client.Call(ak.wwise.core.@object.get, query, options);

                try // 尝试返回物体数据
                {

                    if (jresult["return"].Last["musicPlaylistRoot"] == null) throw new Exception();
                    string id = jresult["return"].Last["musicPlaylistRoot"]["id"].ToString();

                    return new WwiseMusicPlaylistItem(await WwiseUtility.GetWwiseObjectByIDAsync(id));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to get PlaylistRoot of object : {Name}! =======> {e.Message}");
                    return null;
                }
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to get PlaylistRoot of object : {Name}! =======> {e.Message}");
                return null;
            }

        }


    }
}
