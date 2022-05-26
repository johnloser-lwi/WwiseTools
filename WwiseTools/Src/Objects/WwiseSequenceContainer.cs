using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public class WwiseSequenceContainer : WwiseRandomSequenceContainer
    {
        /// <summary>
        /// 创建一个步进容器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentPath"></param>
        [Obsolete("use WwiseUtility.Instance.CreateObjectAsync instead")]
        public WwiseSequenceContainer(string name, string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit") : base(name, "", WwiseObject.ObjectType.RandomSequenceContainer.ToString())
        {
            var tempObj = WwiseUtility.Instance.CreateObject(name, ObjectType.RandomSequenceContainer, parentPath);
            ID = tempObj.ID;
            Name = tempObj.Name;
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_RandomOrSequence(WwiseProperty.Option_RandomOrSequence.Sequence));
        }

        public WwiseSequenceContainer(WwiseObject @object) : base("", "", "")
        {
            if (@object == null) return;
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;
        }

        /// <summary>
        /// 设置序列结束后的行为
        /// </summary>
        /// <param name="option"></param>
        [Obsolete("use async version instead")]
        public void SetSequenceEndBehavior(WwiseProperty.Option_RestartBeginningOrBackward option)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_RestartBeginningOrBackward(option));
        }

        public async Task SetSequenceEndBehaviorAsync(WwiseProperty.Option_RestartBeginningOrBackward option)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_RestartBeginningOrBackward(option));
        }

        /// <summary>
        /// 设置连续模式下是否重置播放列表
        /// </summary>
        /// <param name="reset"></param>
        [Obsolete("use async version instead")]
        public void SetAlwaysResetPlaylist(bool reset)
        {
            WwiseUtility.Instance.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismResetPlaylistEachPlay(reset));
        }

        public async Task SetAlwaysResetPlaylistAsync(bool reset)
        {
            await WwiseUtility.Instance.SetObjectPropertyAsync(this, WwiseProperty.Prop_PlayMechanismResetPlaylistEachPlay(reset));
        }

        /// <summary>
        /// 设置播放列表
        /// </summary>
        /// <param name="item"></param>
        /// <param name="atFront"></param>
        [Obsolete("Use async version instead")]
        public void SetPlaylist(WwiseObject item, bool atFront = false)
        {
            if (!item.Path.Contains(Path)) return;

            WwiseUtility.Instance.SaveWwiseProject();
            WwiseWorkUnitParser parser = new WwiseWorkUnitParser(WwiseUtility.Instance.GetWorkUnitFilePath(this));

            var playlists = parser.XML.GetElementsByTagName("Playlist");

            XmlElement playlist = null;

            foreach (XmlElement list in playlists)
            {
                if (list.ParentNode.Attributes["ID"].Value.ToString() == ID)
                {
                    playlist = list;
                    break;
                }
            }

            if (playlist != null)
            {
                var node = parser.XML.CreateElement("ItemRef");
                node.SetAttribute("Name", item.Name);
                node.SetAttribute("ID", item.ID);

                if (!atFront) playlist.AppendChild(parser.XML.ImportNode(node, true));
                else playlist.InsertBefore(parser.XML.ImportNode(node, true), playlist.FirstChild);

                //parser.AddToUnit(this, node);
                parser.SaveFile();
            }
            else
            {
                var newPlaylist = parser.XML.CreateElement("Playlist");

                var node = parser.XML.CreateElement("ItemRef");
                node.SetAttribute("Name", item.Name);
                node.SetAttribute("ID", item.ID);

                newPlaylist.AppendChild(node);

                var containers = parser.XML.GetElementsByTagName(Type);

                

                foreach (XmlElement container in containers)
                {
                    if (container.GetAttribute("ID") == ID)
                    {
                        container.AppendChild(parser.XML.ImportNode(newPlaylist, true));

                        parser.SaveFile();
                        break;
                    }
                }

            }

            WwiseUtility.Instance.ReloadWwiseProject();
            
        }
        
        public async Task SetPlaylistAsync(List<WwiseObject> items, bool autoReload = false)
        {
            foreach (var item in items)
            {
                if (!(await item.GetPathAsync()).Contains(await GetPathAsync())) return;
            }
            

            await WwiseUtility.Instance.SaveWwiseProjectAsync();
            WwiseWorkUnitParser parser = new WwiseWorkUnitParser(await WwiseUtility.Instance.GetWorkUnitFilePathAsync((this)));

            var xpath = "//*[@ID='" + ID + "']/Playlist";
            var playlistNode = parser.XML.SelectSingleNode(xpath);


            var containerNode = parser.GetNodeByID(ID);

            if (playlistNode != null)
            {

                containerNode.RemoveChild(playlistNode);
                parser.SaveFile();
            }
            
            
            var newPlaylist = parser.XML.CreateElement("Playlist");


            foreach (var item in items)
            {
                var node = parser.XML.CreateElement("ItemRef");
                node.SetAttribute("Name", item.Name);
                node.SetAttribute("ID", item.ID);
                newPlaylist.AppendChild(node);
            }

            containerNode.AppendChild(parser.XML.ImportNode(newPlaylist, true));

            parser.SaveFile();

            if (autoReload) await WwiseUtility.Instance.ReloadWwiseProjectAsync();
            
        }
    }
}
