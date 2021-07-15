using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WwiseTools.Basics;
using WwiseTools.Utils;
//using WwiseTools.Properties;
//using WwiseTools.Utils;

namespace WwiseTools
{
    /// <summary>
    /// 所有可以被Wwise显示的节点被称为单元(Unit)
    /// </summary>
    public class WwiseUnit : WwiseNodeWithName//, IWwiseID, IWwiseName
    {
        public string ID => guid;

        protected string guid;

        protected WwiseNode childrenList;
        public WwiseNode ChildrenList => childrenList;

        protected WwiseNode propertyList;

        public WwiseNode PropertyList => propertyList;

        /// <summary>
        /// 创建一个包含名称、类型的单元
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="u_type"></param>
        public WwiseUnit(string _name, string u_type, WwiseParser parser) : base(u_type, _name, parser)
        {
            this.guid = Guid.NewGuid().ToString().ToUpper().Trim();
            node.SetAttribute("ID", "{" + guid + "}");
            childrenList = new WwiseNode("ChildrenList", parser);
            propertyList = new WwiseNode("PropertyList", parser);
        }

        /// <summary>
        /// 创建一个包含名称、类型的单元，并设置GUID
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="u_type"></param>
        /// <param name="guid"></param>
        public WwiseUnit(string _name, string u_type, string guid, WwiseParser parser) : base(u_type, _name, parser)
        {
            this.guid = guid;
            node.SetAttribute("ID", "{" + guid + "}");
            childrenList = new WwiseNode("ChildrenList", parser);
            propertyList = new WwiseNode("PropertyList", parser);
        }

        /// <summary>
        /// 添加子单元
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public virtual WwiseUnit AddChild(WwiseUnit child)
        {
            AddChildrenList();
            childrenList.AddChildNode(child);
            return child;
        }


        /// <summary>
        /// 为单元添加属性
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public virtual WwiseNodeWithName AddProperty(WwiseNodeWithName property)
        {
            bool added_propertyList = false;
            foreach (XmlElement c in ChildNodes)
            {
                if (c.Name == "PropertyList")
                {
                    added_propertyList = true;
                    break;
                }
            }
            if (!added_propertyList) AddChildNodeAtFront(PropertyList);

            foreach (XmlElement p in PropertyList.ChildNodes)
            {
                if (p.Attributes[0].Value == property.Name)
                {
                    p.SetAttribute("Value", property.Attributes[2].Value);
                    return property;
                }

            }

            PropertyList.AddChildNode(property);
            return property;
        }

        protected virtual void AddChildrenList()
        {
            foreach (XmlElement c in ChildNodes)
            {
                if (c.Name == "ChildrenList") return;
            }

            AddChildNode(childrenList);
        }

        public override wwiseObject ToObject()
        {
            wwiseObject result;
            result.Type = Type;
            result.Name = Name;
            result.ID = ID;
            return result;
        }
    }
}
