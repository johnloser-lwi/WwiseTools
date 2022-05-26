using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Utils
{
    public class WaapiLog
    {
        private static WaapiLog _instance;

        private static WaapiLog Instance
        {
            get
            {
                if (_instance == null) _instance = new WaapiLog();
                return _instance;
            }
        }
        internal event Action<object, bool> Logger;


        public bool firstLog = true;
        private WaapiLog()
        {
            Logger = (msg, _) => Console.WriteLine(msg);
        }

        public static void Log(object message)
        {
            Instance.Logger(message.ToString(), Instance.firstLog);
            if (Instance.firstLog) Instance.firstLog = false;
        }

        public static void AddCustomLogger(Action<object, bool> logger)
        {
            Instance.Logger += logger;
        }
    }
}
