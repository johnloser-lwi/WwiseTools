using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Utils
{
    internal class WaapiFunction : IEnumerable<string>
    {
        public static string CoreObjectGet => "ak.wwise.core.object.get";

        private List<string> functions;

        public WaapiFunction()
        {
            functions = new List<string>();
        }

        public void AddFunction(string function)
        {
            if (!functions.Contains(function))
                functions.Add(function);
        }

        public string Verify(string func)
        {
            bool result = false;
            string final = null;
            if (functions.Contains(func))
            {
                final = func;
                result = true;
            }
            else
            {
                foreach (var function in this)
                {
                    if (function.ToLower().Contains(func.ToLower()))
                    {
                        final = function;
                        result = true;
                        break;
                    }
                }
                if (result)
                    WaapiLog.Log($"Warning: No matching function for {func}! Using {final} instead!");
            }

            if (!result) 
                throw new Exception($"Function {func} not available in wwise " +
                                    $"{WwiseUtility.ConnectionInfo.Version.ToString()}!");
            return final;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return functions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
