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

        private WaapiFunctionList _function;

        private WaapiTopicList _topic;

        private WaapiUICommandList _uiCommand;

        public WwiseInfo ConnectionInfo { get; private set; }

        public WaapiFunctionList Function
        {
            get
            {
                if (_function is null) _function = new WaapiFunctionList();
                return _function;
            }
        }

        public WaapiTopicList Topic
        {
            get
            {
                if (_topic is null) _topic = new WaapiTopicList();
                return _topic;
            }
        }

        public WaapiUICommandList UICommand
        {
            get
            {
                if (_uiCommand is null) _uiCommand = new WaapiUICommandList();
                return _uiCommand;
            }
        }

        internal int WampPort { get; set; } = -1;

        internal string IP { get; set; } = string.Empty;

        private readonly Dictionary<string, int> _subscriptions = new Dictionary<string, int>();

        public event Action Disconnected;

        public event Action<WwiseInfo> Connected;

        public int TimeOut { get; set; } = 10000;


        public static WwiseUtility Instance
        {
            get
            {
                if (_instance == null) _instance = new WwiseUtility();
                return _instance;
            }
        }

        

        private static WwiseUtility _instance;

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
                WaapiLog.InternalLog($"Failed to subscribe {topic} ======> {e.Message}");

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
                WaapiLog.InternalLog($"Failed to unsubscribe {topic} ======> {e.Message}");

                return true;
            }

            return true;

        }

        public async Task<bool> ConnectAsync(int wampPort = 8080)
        {
            return await ConnectAsync("localhost", wampPort);
        }

        public async Task<bool> ConnectAsync(string ip ,int wampPort = 8080) // 初始化，返回连接状态
        {
            if (_client != null && _client.IsConnected()) return true;


            if (_initializing) return false;
            try
            {
                _initializing = true;
                WaapiLog.InternalLog("Initializing...");
                _client = new JsonClient();
                var uri = $"ws://{ip}:{wampPort}/waapi";
                await _client.Connect(uri, TimeOut); // 尝试创建Wwise连接

                WaapiLog.InternalLog($"Connected successfully to {ip}:{wampPort}!");
                
                Function.Clear();
                UICommand.Clear();
                Topic.Clear();

                _client.Disconnected += () =>
                {
                    _client = null;
                    ConnectionInfo = null;
                    Disconnected?.Invoke();
                    WaapiLog.InternalLog("Connection closed!"); // 丢失连接提示
                };

                // 由于工程加载可能会导致信息获取失败，这里进行5次重复检查
                var retryCount = 5;
                for (int i = 1; i <= retryCount; i++)
                {
                    WaapiLog.InternalLog($"Trying to fetch connection info ({i}/{retryCount}) ...");

                    if (Function.Count == 0) await GetFunctionsAsync();
                    if (Topic.Count == 0) await GetTopicsAsync();
                    if (UICommand.Count == 0) await GetCommandsAsync();

                    if (ConnectionInfo == null) ConnectionInfo = await GetWwiseInfoAsync();

                    if (ConnectionInfo != null && Function.Count != 0 && Topic.Count != 0 && UICommand.Count != 0) break;

                    
                    WaapiLog.InternalLog("Failed to fetch connection info! Retry in 3 seconds ...");
                    
                    await Task.Delay(3000);
                }

                if (ConnectionInfo == null)
                {
                    WaapiLog.InternalLog("Failed to fetch connection info!");

                    await DisconnectAsync();
                    return false;
                }
                
                WaapiLog.InternalLog("Connection info fetched successfully!");

                WampPort = wampPort;
                WaapiLog.InternalLog(ConnectionInfo);
                Connected?.Invoke(ConnectionInfo);
                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to connect! ======> {e.Message}");
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
                WaapiLog.InternalLog($"Error while closing! ======> {e.Message}");
            }
            finally
            {
                WampPort = -1;
            }
        }

        public async Task<bool> TryConnectWaapiAsync(int wampPort = 8080)
        {
            return await TryConnectWaapiAsync("localhost", wampPort);
        }

        public async Task<bool> TryConnectWaapiAsync(string ip, int wampPort = 8080)
        {
            if (WampPort != -1) wampPort = WampPort;
            if (!string.IsNullOrEmpty(IP)) ip = IP;

            var connected = await ConnectAsync(ip, wampPort);

            return connected && _client.IsConnected();
        }

        public static string NewGUID()
        {
            return $"{{{Guid.NewGuid().ToString().ToUpper()}}}";
        }

        
    }
}
