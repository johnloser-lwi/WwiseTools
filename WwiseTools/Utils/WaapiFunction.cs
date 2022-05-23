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

        public bool Contains(string function, out string final)
        {
            final = function; 
            var result = functions.Contains(function);
            if (!result) Console.WriteLine($"Function {function} not available in wwise {WwiseUtility.ConnectionInfo.Version.ToString()}!");
            return result;
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
