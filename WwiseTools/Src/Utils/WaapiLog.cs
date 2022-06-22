using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private event Action<object, bool> Logger;


        private bool _firstLog = true;

        private bool _enabled = true;

        private bool _enableStandardLog = true;

        private WaapiLog()
        {
            Logger = DefaultLog;
        }

        private static void DefaultLog(object msg, bool firstLog)
        {
            Console.WriteLine(msg);
        }

        public static void Log(object message)
        {
            if (!Instance._enableStandardLog)
            {
                try
                {
                    var mth = new StackTrace()?.GetFrame(1)?.GetMethod()?.ReflectedType;
                    if (mth != null && mth.Namespace.StartsWith("WwiseTools")) return;
                }
                catch
                {
                }
                
            }

            if (Instance._enabled) Instance.Logger?.Invoke(message?.ToString(), Instance._firstLog);
            if (Instance._firstLog) Instance._firstLog = false;
        }

        public static void AddCustomLogger(Action<object, bool> logger)
        {
            Instance.Logger += logger;
        }

        public static void SetEnabled(bool enabled)
        {
            Instance._enabled = enabled;
        }

        public static void SetEnableStandardLog(bool enable)
        {
            Instance._enableStandardLog = enable;
        }
    }
}
