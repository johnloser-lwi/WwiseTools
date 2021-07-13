using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools
{
    public interface IWwiseName
    {
        /// <summary>
        /// Wwise单元的名字
        /// </summary>
        string name { get; }
    }

    public interface IWwiseID
    {
        /// <summary>
        /// Wwise单元独有的GUID
        /// </summary>
        string id { get; }
    }

    public interface IWwiseNode
    {
        /// <summary>
        /// 节点类型
        /// </summary>
        string type { get; }

        /// <summary>
        /// 节点开头
        /// </summary>
        string head { get; }

        /// <summary>
        /// 节点结尾
        /// </summary>
        string tail { get; }


        /// <summary>
        /// 增加子节点，并返回该节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns>IWwisePrintable</returns>
        IWwisePrintable AddChildNode(IWwisePrintable node);

        /// <summary>
        /// 节点的内容
        /// </summary>
        List<IWwisePrintable> body { get; }
        
    }

    public interface IWwisePrintable
    {

        /// <summary>
        /// 输出内容的缩进
        /// </summary>
        int tabs { get; set; }

        /// <summary>
        /// 将内容输出为字符串
        /// </summary>
        /// <returns>string</returns>
        string Print();
    }
}
