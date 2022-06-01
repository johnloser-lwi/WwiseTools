using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WaapiClient;
using WwiseTools.Models;

namespace WwiseTools.Utils
{
    /// <summary>
    /// 用于实现基础功能
    /// </summary>
    public partial class WwiseUtility
    {
        public JsonClient Client { get; set; }

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
            if (Client != null && Client.IsConnected()) return true;

            try
            {
                
                Client = new JsonClient();
                await Client.Connect(); // 尝试创建Wwise连接

                WaapiLog.Log("Connected successfully!");

                Client.Disconnected += () =>
                {
                    Client = null;
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
            return Client != null && Client.IsConnected();
        }
        
        public async Task<bool> ConnectAsync(int wampPort = 8080) // 初始化，返回连接状态
        {
            if (Client != null && Client.IsConnected()) return true;

            try
            {
                WaapiLog.Log("Initializing...");
                Client = new JsonClient();
                await Client.Connect($"ws://localhost:{wampPort}/waapi", TimeOut); // 尝试创建Wwise连接
                await GetFunctionsAsync();
                WaapiLog.Log("Connected successfully!");

                Client.Disconnected += () =>
                {
                    Client = null;
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
            if (Client == null || !Client.IsConnected()) return;

            try
            {
                await Client.Close(); // 尝试断开连接
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Error while closing! ======> {e.Message}");
            }
        }

        public async Task DisconnectAsync()
        {
            if (Client == null || !Client.IsConnected()) return;

            try
            {
                await Client.Close(); // 尝试断开连接
                Client = null;
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

            return connected.Result && Client.IsConnected();
        }
        
        public async Task<bool> TryConnectWaapiAsync(int wampPort = 8080) 
        {
            var connected = await ConnectAsync(wampPort);

            return connected && Client.IsConnected();
        }

        public static string NewGUID()
        {
            return $"{{{Guid.NewGuid().ToString().ToUpper()}}}";
        }

        
    }
}
