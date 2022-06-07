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

        private bool _initializing = false;

        public WwiseInfo ConnectionInfo { get; private set; }

        internal WaapiFunction Function { get; set; }

        internal WaapiTopic Topic { get; set; }

        private readonly Dictionary<string, int> _subscriptions = new Dictionary<string, int>();

        public event Action Disconnected;

        public int TimeOut => 10000;


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

        public async Task<bool> SubscribeAsync(string topic, JObject options, JsonClient.PublishHandler publishHandler, int timeOut = Int32.MaxValue)
        {
            return await SubscribeAsync(topic, (object)options, publishHandler, timeOut);
        }

        public async Task<bool> SubscribeAsync(string topic, object options, JsonClient.PublishHandler publishHandler, int timeOut = Int32.MaxValue)
        {
            if (!await UnsubscribeAsync(topic, timeOut)) return false;

            try
            {
                int id = await _client.Subscribe(topic, options, publishHandler, timeOut);

                _subscriptions.Add(topic, id);
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to subscribe {topic} ======> {e.Message}");

                return false;
            }

            return true;
        }

        public async Task<bool> UnsubscribeAsync(string topic, int timeOut = Int32.MaxValue)
        {
            if (!_subscriptions.ContainsKey(topic)) return true;

            try
            {
                await _client.Unsubscribe(_subscriptions[topic], timeOut);

                _subscriptions.Remove(topic);
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to unsubscribe {topic} ======> {e.Message}");

                return true;
            }

            return true;

        }

        public async Task<bool> ConnectAsync(int wampPort = 8080) // 初始化，返回连接状态
        {
            if (_client != null && _client.IsConnected()) return true;


            if (_initializing) return false;
            try
            {
                _initializing = true;
                WaapiLog.Log("Initializing...");
                _client = new JsonClient();
                await _client.Connect($"ws://localhost:{wampPort}/waapi", TimeOut); // 尝试创建Wwise连接
                await GetFunctionsAsync();
                await GetTopicsAsync();
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
            finally
            {
                _initializing = false;
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
