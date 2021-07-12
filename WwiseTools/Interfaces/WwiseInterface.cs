using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools
{
    public interface IWwiseName
    {
        string name { get; }
    }

    public interface IWwiseID
    {
        string id { get; }
    }

    public interface IWwiseNode
    {
        string type { get; }
        string head { get; }
        string tail { get; }

        IWwisePrintable AddChildNode(IWwisePrintable node);

        List<IWwisePrintable> body { get; }
        
    }

    public interface IWwisePrintable
    {
        int tabs { get; set; }

        string Print();
    }
}
