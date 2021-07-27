using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    class WwiseFolder : WwiseObject
    {
        public WwiseFolder(string name, string parent_path = @"\Actor-Mixer Hierarchy\Default Work Unit") : base(name, "", "Folder")
        {
            var tempObj = WwiseUtility.CreateObject(name, ObjectType.Folder, parent_path);
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
        public void AddChild(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return;
            WwiseUtility.MoveToParent(wwiseObject, this);
        }
    }
}
