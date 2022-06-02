using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WaapiClient;
using WwiseTools.Models;

namespace WwiseTools.Utils
{
    /// <summary>
    /// 用于实现基础功能
    /// </summary>
    public partial class WwiseUtility
    {
        private JsonClient _client;

        public WwiseInfo ConnectionInfo { get; private set; }

        internal WaapiFunction Function { get; set; }

        public int TimeOut => 5000;

        public static WwiseUtility Instance
        {
            get
            {
                if (_instance == null) _instance = new WwiseUtility();
                return _instance;
            }
        }

        private static WwiseUtility _instance;

        public enum GlobalImportSettings
        {
            useExisting,
            replaceExisting,
            createNew
        }

        public GlobalImportSettings ImportSettings = GlobalImportSettings.useExisting;

        private WwiseUtility()
        {
        }

        /// <summary>
        /// 连接初始化
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use WwiseUtility.ConnectAsync() instead")]
        public async Task<bool> Init() // 初始化，返回连接状态
        {
            if (_client != null && _client.IsConnected()) return true;

            try
            {
                
                _client = new JsonClient();
                await _client.Connect(); // 尝试创建Wwise连接

                WaapiLog.Log("Connected successfully!");

                _client.Disconnected += () =>
                {
                    _client = null;
                    WaapiLog.Log("Connection closed!"); // 丢失连接提示
                };
                return true;
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to connect! ======> {e.Message}");
                return false;
            }
        }

        public bool IsConnected()
        {
            return _client != null && _client.IsConnected();
        }

        public async Task<JObject> CallAsync(string uri, JObject args, JObject options, int timeOut = Int32.MaxValue)
        {
            return await _client.Call(uri, args, options, timeOut);
        }
        public async Task<JObject> CallAsync(string uri, object args, object options, int timeOut = Int32.MaxValue)
        {
            return await _client.Call(uri, args, options, timeOut);
        }

        public async Task<bool> ConnectAsync(int wampPort = 8080) // 初始化，返回连接状态
        {
            if (_client != null && _client.IsConnected()) return true;

            try
            {
                WaapiLog.Log("Initializing...");
                _client = new JsonClient();
                await _client.Connect($"ws://localhost:{wampPort}/waapi", TimeOut); // 尝试创建Wwise连接
                await GetFunctionsAsync();
                WaapiLog.Log("Connected successfully!");

                _client.Disconnected += () =>
                {
                    _client = null;
                    ConnectionInfo = null;
                    WaapiLog.Log("Connection closed!"); // 丢失连接提示
                };


               

                for (int i = 0; i < 5; i++)
                {
                    WaapiLog.Log("Trying to fetch connection info ...");

                    ConnectionInfo = await GetWwiseInfoAsync();

                    if (ConnectionInfo != null) break;

                    await Task.Delay(3000);
                }

                if (ConnectionInfo == null)
                {
                    WaapiLog.Log("Failed to fetch connection info!");

                    await DisconnectAsync();
                    return false;
                }


                WaapiLog.Log(ConnectionInfo);
                return true;
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to connect! ======> {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use WwiseUtility.DisconnectAsync() instead")]
        public async Task Close()
        {
            if (_client == null || !_client.IsConnected()) return;

            try
            {
                await _client.Close(); // 尝试断开连接
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Error while closing! ======> {e.Message}");
            }
        }

        public async Task DisconnectAsync()
        {
            if (_client == null || !_client.IsConnected()) return;

            try
            {
                await _client.Close(); // 尝试断开连接
                _client = null;
                ConnectionInfo = null;
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Error while closing! ======> {e.Message}");
            }
        }

        /// <summary>
        /// 尝试连接并检查连接状态
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public bool TryConnectWaapi() 
        {
            var connected = Init();
            connected.Wait();

            return connected.Result && _client.IsConnected();
        }
        
        public async Task<bool> TryConnectWaapiAsync(int wampPort = 8080) 
        {
            var connected = await ConnectAsync(wampPort);

            return connected && _client.IsConnected();
        }

        public static string NewGUID()
        {
            return $"{{{Guid.NewGuid().ToString().ToUpper()}}}";
        }

        
    }
}
