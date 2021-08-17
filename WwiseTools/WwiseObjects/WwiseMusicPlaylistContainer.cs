using AK.Wwise.Waapi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    class WwiseMusicPlaylistContainer : WwiseContainer
    {
        /// <summary>
        /// 创建一个音乐片段
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent_path"></param>
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

        /*
        public WwiseMusicPlaylistItem AddPlaylistItem(WwiseMusicPlaylistItem.Option_PlaylistItemType type)
        {
            var root_item = GetRootPlaylistItem();

            if (root_item != null)
            {
                var item = new WwiseMusicPlaylistItem(type, root_item.ID);
                return item;
            }

            return null;
        }

        public WwiseMusicPlaylistItem GetRootPlaylistItem()
        {
            var roots = WwiseUtility.GetWwiseObjectsOfType("MusicPlaylistItem");
            WwiseMusicPlaylistItem root_item = null;
            foreach (var root in roots)
            {
                if (root.Parent.Path == Path)
                {
                    root_item = new WwiseMusicPlaylistItem(root);
                    break;
                }
            }
            return root_item;
        }
        */
    }
}
