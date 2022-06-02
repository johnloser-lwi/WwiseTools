using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Utils;

namespace WwiseTools.Objects
{
    public class WwiseSwitchGroup : WwiseObject
    {
        [Obsolete("use WwiseUtility.Instance.CreateObjectAsync instead")]
        public WwiseSwitchGroup(string name, string parentPath = @"\Switches\Default Work Unit") : base(name, "", ObjectType.SwitchGroup.ToString())
        {
            var tempObj = WwiseUtility.Instance.CreateObject(name, ObjectType.SwitchGroup, parentPath);
            ID = tempObj.ID;
            Name = tempObj.Name;
        }

        public WwiseSwitchGroup(WwiseObject @object) : base ("", "", "")
        {
            ID = @object.ID;
            Name = @object.Name;
            Type = @object.Type;
        }


        /// <summary>
        /// 返回该Switch Goup中的所有Switch
        /// </summary>
        /// <returns></returns>
        [Obsolete("use async version instead")]
        public List<WwiseObject> GetSwitches()
        {
            List<WwiseObject> temp = WwiseUtility.Instance.GetWwiseObjectsOfType(ObjectType.Switch.ToString());
            List<WwiseObject> result = new List<WwiseObject>();
            foreach (var obj in temp)
            {
                if (obj.Path.Contains(Path))
                {
                    result.Add(obj);
                }
            }

            return result;

        }

        public async Task<List<WwiseObject>> GetSwitchesAsync()
        {
            if (Type != "SwitchGroup") return new List<WwiseObject> ();
            return await WwiseUtility.Instance.GetWwiseObjectChildrenAsync(this);

        }
    }
}
