using System;
using System.Runtime.CompilerServices;

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

        private bool _enableInternalLog = true;

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
            if (!Instance._enabled) return;

            Instance.Logger?.Invoke(message?.ToString(), Instance._firstLog);
            if (Instance._firstLog) Instance._firstLog = false;
        }

        internal static void InternalLog(object message, [CallerMemberName] string caller = "")
        {
            if (!Instance._enableInternalLog || !Instance._enabled) return;
            string msg = message?.ToString();
            msg = !string.IsNullOrEmpty(msg) ? $"[{caller}] " + msg : "";

            Instance.Logger?.Invoke(msg, Instance._firstLog);
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

        public static void SetEnableInternalLog(bool enable)
        {
            Instance._enableInternalLog = enable;
        }
    }
}
