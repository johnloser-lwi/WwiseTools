using System;
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
        /// <param name="parent_path"></param>
        [Obsolete("use WwiseUtility.CreateObjectAsync instead")]
        public WwiseSequenceContainer(string name, string parent_path = @"\Actor-Mixer Hierarchy\Default Work Unit") : base(name, "", WwiseObject.ObjectType.RandomSequenceContainer.ToString())
        {
            var tempObj = WwiseUtility.CreateObject(name, ObjectType.RandomSequenceContainer, parent_path);
            ID = tempObj.ID;
            Name = tempObj.Name;
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_RandomOrSequence(WwiseProperty.Option_RandomOrSequence.Sequence));
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
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_RestartBeginningOrBackward(option));
        }

        public async Task SetSequenceEndBehaviorAsync(WwiseProperty.Option_RestartBeginningOrBackward option)
        {
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_RestartBeginningOrBackward(option));
        }

        /// <summary>
        /// 设置连续模式下是否重置播放列表
        /// </summary>
        /// <param name="reset"></param>
        [Obsolete("use async version instead")]
        public void SetAlwaysResetPlaylist(bool reset)
        {
            WwiseUtility.SetObjectProperty(this, WwiseProperty.Prop_PlayMechanismResetPlaylistEachPlay(reset));
        }

        public async Task SetAlwaysResetPlaylistAsync(bool reset)
        {
            await WwiseUtility.SetObjectPropertyAsync(this, WwiseProperty.Prop_PlayMechanismResetPlaylistEachPlay(reset));
        }

        /// <summary>
        /// 设置播放列表
        /// </summary>
        /// <param name="item"></param>
        /// <param name="at_front"></param>
        [Obsolete("Use async version instead")]
        public void SetPlaylist(WwiseObject item, bool at_front = false)
        {
            if (!item.Path.Contains(Path)) return;

            WwiseUtility.SaveWwiseProject();
            WwiseWorkUnitParser parser = new WwiseWorkUnitParser(WwiseUtility.GetWorkUnitFilePath(this));

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

                if (!at_front) playlist.AppendChild(parser.XML.ImportNode(node, true));
                else playlist.InsertBefore(parser.XML.ImportNode(node, true), playlist.FirstChild);

                //parser.AddToUnit(this, node);
                parser.SaveFile();
            }
            else
            {
                var new_playlist = parser.XML.CreateElement("Playlist");

                var node = parser.XML.CreateElement("ItemRef");
                node.SetAttribute("Name", item.Name);
                node.SetAttribute("ID", item.ID);

                new_playlist.AppendChild(node);

                var containers = parser.XML.GetElementsByTagName(Type);

                

                foreach (XmlElement container in containers)
                {
                    if (container.GetAttribute("ID") == ID)
                    {
                        container.AppendChild(parser.XML.ImportNode(new_playlist, true));

                        parser.SaveFile();
                        break;
                    }
                }

            }

            WwiseUtility.ReloadWwiseProject();
            
        }
        
        public async Task SetPlaylistAsync(WwiseObject item, bool at_front = false)
        {
            if (!(await item.GetPathAsync()).Contains(await GetPathAsync())) return;

            await WwiseUtility.SaveWwiseProjectAsync();
            WwiseWorkUnitParser parser = new WwiseWorkUnitParser(await WwiseUtility.GetWorkUnitFilePathAsync((this)));

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

                if (!at_front) playlist.AppendChild(parser.XML.ImportNode(node, true));
                else playlist.InsertBefore(parser.XML.ImportNode(node, true), playlist.FirstChild);

                //parser.AddToUnit(this, node);
                parser.SaveFile();
            }
            else
            {
                var new_playlist = parser.XML.CreateElement("Playlist");

                var node = parser.XML.CreateElement("ItemRef");
                node.SetAttribute("Name", item.Name);
                node.SetAttribute("ID", item.ID);

                new_playlist.AppendChild(node);

                var containers = parser.XML.GetElementsByTagName(Type);

                

                foreach (XmlElement container in containers)
                {
                    if (container.GetAttribute("ID") == ID)
                    {
                        container.AppendChild(parser.XML.ImportNode(new_playlist, true));

                        parser.SaveFile();
                        break;
                    }
                }

            }

            await WwiseUtility.ReloadWwiseProjectAsync();
            
        }
    }
}
