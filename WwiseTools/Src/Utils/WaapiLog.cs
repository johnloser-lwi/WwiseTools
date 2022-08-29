using System;
using System.Runtime.CompilerServices;

namespace WwiseTools.Utils
{
    public class WaapiLog
    {
        public delegate void LoggerDelegate(object msg);
        
        private static WaapiLog _instance;

        private static WaapiLog Instance
        {
            get
            {
                if (_instance == null) _instance = new WaapiLog();
                return _instance;
            }
        }
        private event LoggerDelegate Logger;

        private bool _enabled = true;

        private bool _enableInternalLog = true;

        private WaapiLog()
        {
            Logger = DefaultLog;
        }

        private static void DefaultLog(object msg)
        {
            Console.WriteLine(msg);
        }

        public static void Log(object message)
        {
            if (!Instance._enabled) return;

            Instance.Logger?.Invoke(message?.ToString());
        }

        internal static void InternalLog(object message, [CallerMemberName] string caller = "")
        {
            if (!Instance._enableInternalLog || !Instance._enabled) return;
            string msg = message?.ToString();
            msg = !string.IsNullOrEmpty(msg) ? $"[{caller}] " + msg : "";

            Instance.Logger?.Invoke(msg);
        }

        public static void AddCustomLogger(LoggerDelegate logger)
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
