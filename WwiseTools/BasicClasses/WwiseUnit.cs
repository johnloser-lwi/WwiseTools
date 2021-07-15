using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basics;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools
{
    /// <summary>
    /// 所有可以被Wwise显示的节点被称为单元(Unit)
    /// </summary>
    public class WwiseUnit : WwiseNode, IWwiseID, IWwiseName
    {
        /// <summary>
        /// 获取该单元的所有子单元
        /// </summary>
        public WwiseNode Children => children;
        protected WwiseNode children;

        //public WwiseNode Properties => properties;
        protected WwiseNode properties;
        protected List<WwiseProperty> propertyList = new List<WwiseProperty>();

        public string name => unit_name;

        public string id => guid;

        protected string guid;
        protected string unit_name;

        /// <summary>
        /// 创建一个包含名称、类型的单元
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="u_type"></param>
        public WwiseUnit(string _name, string u_type) : base(u_type)
        {
            Init(_name, u_type, Guid.NewGuid().ToString().ToUpper());
        }

        /// <summary>
        /// 创建一个包含名称、类型的单元，并设置GUID
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="u_type"></param>
        /// <param name="guid"></param>
        public WwiseUnit(string _name, string u_type, string guid): base(u_type)
        {
            Init(_name, u_type, guid);
        }

        protected virtual void Init(string _name, string u_type, string guid)
        {
            unit_name = _name;
            this.u_type = u_type;
            this.guid = guid;
            xml_head = String.Format("<{0} Name=\"{1}\" ID=\"{{{2}}}\">", u_type, unit_name, guid);
            xml_tail = String.Format("</{0}>", u_type);
        }

        /// <summary>
        /// 添加子单元
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public virtual WwiseUnit AddChild(WwiseUnit child)
        {
            AddChildrenList();
            Children.AddChildNode(child);
            return child;
        }


        /// <summary>
        /// 为单元添加属性
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public virtual WwiseProperty AddProperty(WwiseProperty property)
        {
            if (properties == null)
            {
                properties = new WwiseNode("PropertyList");
                AddChildNodeAtFront(properties);
            }

            int remove_index = 0;
            bool remove = false;
            foreach (var p in propertyList)
            {
                if (p.Name == null) continue;
                if (p.Name == property.Name)
                {
                    properties.ChildNodes.RemoveAt(remove_index);
                    remove = true;
                    break;
                    //propertyList.Remove(p);
                }
                remove_index += 1;
            }
            properties.AddChildNode(property);
            if (remove) propertyList.RemoveAt(remove_index);
            propertyList.Add(property);
            //RefreshProperty();
            return property;
        }

        protected virtual void AddChildrenList()
        {
            if (children != null) return;
            AddChildNode(children = new WwiseNode("ChildrenList"));
        }
    }
}
