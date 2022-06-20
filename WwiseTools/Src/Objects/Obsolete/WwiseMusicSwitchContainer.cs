using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    [Obsolete]
    public class WwiseMusicSwitchContainer : WwiseContainer
    {
        [Obsolete("use WwiseUtility.Instance.CreateObjectAsync instead")]
        public WwiseMusicSwitchContainer(string name, string parentPath = @"\Interactive Music Hierarchy\Default Work Unit\") : base(name, "", ObjectType.MusicPlaylistContainer.ToString())
        {
            var playlist = WwiseUtility.Instance.CreateObject(name, ObjectType.MusicSwitchContainer, parentPath);
            ID = playlist.ID;
            Name = playlist.Name;
        }

        public WwiseMusicSwitchContainer(WwiseObject @object) : base("", "", "")
        {
            if (@object == null) return;
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;
        }

        /// <summary>
        /// 设置继续播放
        /// </summary>
        /// <param name="value"></param>
        [Obsolete("use async version instead")]
        public void SetContinuePlay(bool value)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_ContinuePlay(value));
        }

        public async Task SetContinuePlayAsync(bool value)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_ContinuePlay(value));
        }
    }
}
