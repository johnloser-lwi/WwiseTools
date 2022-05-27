﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            List<WwiseObject> temp = await WwiseUtility.Instance.GetWwiseObjectsOfTypeAsync(ObjectType.Switch.ToString());
            List<WwiseObject> result = new List<WwiseObject>();
            foreach (var obj in temp)
            {
                if ((await obj.GetPathAsync()).Contains(await GetPathAsync()))
                {
                    result.Add(obj);
                }
            }

            return result;

        }
    }
}