using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Utils
{
    public class WaapiFunctionList : WaapiStringListBase
    {
        public override string ListContent => "Function";
        public static string CoreObjectGet => "ak.wwise.core.object.get";
    }
}
