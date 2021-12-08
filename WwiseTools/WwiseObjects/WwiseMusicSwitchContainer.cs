﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public class WwiseMusicSwitchContainer : WwiseContainer
    {
        public WwiseMusicSwitchContainer(string name, string parent_path = @"\Interactive Music Hierarchy\Default Work Unit\") : base(name, "", ObjectType.MusicPlaylistContainer.ToString())
        {
            var playlist = WwiseUtility.CreateObject(name, ObjectType.MusicSwitchContainer, parent_path);
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
        public void SetContinuePlay(bool value)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_ContinuePlay(value));
        }
    }
}