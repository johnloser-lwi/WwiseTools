using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    [Obsolete]
    public class WwiseFolder : WwiseObject
    {
        /// <summary>
        /// 创建一个虚拟文件夹
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentPath"></param>
        [Obsolete("use WwiseUtility.Instance.CreateObjectAsync instead")]
        public WwiseFolder(string name, string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit") : base(name, "", "Folder")
        {
            var tempObj = WwiseUtility.Instance.CreateObject(name, ObjectType.Folder, parentPath);
            ID = tempObj.ID;
            Name = tempObj.Name;
        }

        public WwiseFolder(WwiseObject @object) : base("", "", "")
        {
            if (@object == null) return;
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;
        }

        /// <summary>
        /// 添加子对象
        /// </summary>
        /// <param name="wwiseObject"></param>
        [Obsolete("use async version instead")]
        public void AddChild(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return;
            WwiseUtility.Instance.MoveToParent(wwiseObject, this);
        }

        public async Task AddChildAsync(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return;
            await WwiseUtility.Instance.MoveToParentAsync(wwiseObject, this);
        }

        [Obsolete("use async version instead")]
        public List<WwiseObject> GetChildren()
        {
            List<WwiseObject> result = new List<WwiseObject>();

            WwiseWorkUnitParser parser = new WwiseWorkUnitParser(WwiseUtility.Instance.GetWorkUnitFilePath(this));
            var folders = parser.XML.GetElementsByTagName("Folder");
            foreach (XmlElement folder in folders)
            {
                if (folder.GetAttribute("ID") == ID)
                {
                    var children = (folder.GetElementsByTagName("ChildrenList")[0] as XmlElement).ChildNodes;
                    foreach (XmlElement child in children)
                    {
                        result.Add(WwiseUtility.Instance.GetWwiseObjectByID(child.GetAttribute("ID")));
                    }
                    break;
                }
            }

            return result;
        }

        public async Task<List<WwiseObject>> GetChildrenAsync()
        {
            return await WwiseUtility.Instance.GetWwiseObjectChildrenAsync(this);
        }
    }
}
