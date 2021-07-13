using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Properties
{
    /// <summary>
    /// Wwise属性，可添加至WwiseUnit
    /// </summary>
    public class WwiseProperty : IWwisePrintable
    {
        public int tabs { get => p_tabs; set => p_tabs = value; }
        public string Name => name;

        protected int p_tabs;

        protected string name;

        public string Body { get => body; set => body = value; }

        public string Print()
        {
            string t = "";
            t += "\n";
            for (int i = 0; i < tabs; i++)
            {
                t += "\t";
            }
            string result = "";
            result += t + body;
            return result;
        }

        protected string body;

        public WwiseProperty()
        {
            return;
        }

        /// <summary>
        /// 初始化属性的名称、类型以及值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public WwiseProperty(string name, string type, string value)
        {
            this.name = name;
            body = String.Format("<Property Name=\"{0}\" Type=\"{1}\" Value=\"{2}\"/>", name, type, value);
        }

        /// <summary>
        /// 初始化属性的类型以及值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public WwiseProperty(string type, string value)
        {
            body = String.Format("<{0}>{1}</{0}>", type, value);
        }
    }
}
