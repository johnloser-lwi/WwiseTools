using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaapiClient;
using WwiseTools.Models.Profiler;

namespace WwiseTools.Utils.Profiler
{
    public static class WwiseUtilityProfilerExtension
    {

        public enum Cursor
        {
            user,
            capture
        }

        private static async Task<bool> isProfilerReady(this WwiseUtility util)
        {
            return await util.TryConnectWaapiAsync() && !util.ConnectionInfo.IsCommandLine;
        }

        public static async Task<List<ProfilerRemoteInfo>> ProfilerGetAvailableConsolesAsync(this WwiseUtility util)
        {
            if (!await util.isProfilerReady()) return new List<ProfilerRemoteInfo>();

            var result = new List<ProfilerRemoteInfo>();

            try
            {
                var func = util.Function.Verify("ak.wwise.core.remote.getAvailableConsoles");

                var jresult = await util.CallAsync(func, null, null, util.TimeOut);
                var consoles = jresult?["consoles"];
                if (consoles == null) throw new Exception("Result not valid!");

                

                foreach (var console in consoles)
                {
                    var name = console["name"]?.ToString();
                    var platform = console["platform"]?.ToString();
                    var host = console["host"]?.ToString();
                    var appName = console["appName"]?.ToString();
                    int.TryParse(console["commandPort"]?.ToString(), out int commandPort);
                    int.TryParse(console["notificationPort"]?.ToString(), out int notificationPort);

                    var remoteInfo = new ProfilerRemoteInfo()
                    {
                        Name = name,
                        Platform = platform,
                        Host = host,
                        AppName = appName,
                        CommandPort = commandPort,
                        NotificationPort = notificationPort
                    };

                    result.Add(remoteInfo);
                }

            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get consoles! ======> {e.Message}");
            }

            return result;
        }

        public static async Task ProfilerConnectToRemoteAsync(this WwiseUtility util, ProfilerRemoteInfo remoteInfo)
        {
            if (!await util.isProfilerReady() || remoteInfo == null) return;

            try
            {
                var func = util.Function.Verify("ak.wwise.core.remote.connect");

                var query = new
                {
                    host = remoteInfo.Host,
                    commandPort = remoteInfo.CommandPort,
                    notificationPort = remoteInfo.NotificationPort,
                    appName = remoteInfo.AppName
                };

                await util.CallAsync(func, query, null, util.TimeOut);

                WaapiLog.InternalLog($"Successfully connected to remote {remoteInfo}!");
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to connect to remote {remoteInfo}! ======> {e.Message}");
            }
        }

        public static async Task ProfilerSubscribeCaptureLogAsync(this WwiseUtility util, ProfilerCaptureLogOption captureLogOption, Action<ProfilerCaptureLogItem> handler)
        {
            if (!VersionHelper.VersionVerify(VersionHelper.V2019_2_11_7512)) return;

            try
            {
                var  topic = util.Topic.Verify("ak.wwise.core.profiler.captureLog.itemAdded");

                var options = new
                {
                    types = captureLogOption.GetOptions()
                };
                
                JsonClient.PublishHandler publishHandler = json =>
                {
                    string type = json["type"]?.ToString();
                    string objectId = json["objectId"]?.ToString();
                    string objectName = json["objectName"]?.ToString();
                    string gameObjectName = json["gameObjectName"]?.ToString();
                    string description = json["description"]?.ToString();
                    string severity = json["severity"]?.ToString();
                    int.TryParse(json["playingId"]?.ToString(), out int playingId);
                    int.TryParse(json["objectShortId"]?.ToString(), out int objectShortId);
                    int.TryParse(json["gameObjectId"]?.ToString(), out int gameObjectId);
                    int.TryParse(json["time"]?.ToString(), out int time);

                    var item = new ProfilerCaptureLogItem()
                    {
                        Type = type,
                        ObjectID = objectId,
                        ObjectName = objectName,
                        Description = description,
                        GameObjectID = gameObjectId,
                        GameObjectName = gameObjectName,
                        ObjectShortID = objectShortId,
                        PlayingID = playingId,
                        Severity = severity,
                        Time = time
                    };

                    handler?.Invoke(item);
                };

                if (!(await util.SubscribeAsync(topic, options, publishHandler, util.TimeOut)))
                    throw new Exception("Failed to subscribe capture log!");
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog(e);
            }
        }

        public static async Task ProfilerUnsubscribeCaptureLogAsync(this WwiseUtility util)
        {
            if (!VersionHelper.VersionVerify(VersionHelper.V2019_2_11_7512)) return;


            try
            {
                var topic = util.Topic.Verify("ak.wwise.core.profiler.captureLog.itemAdded");

                var result = await util.UnsubscribeAsync(topic, util.TimeOut);
                if (!result) throw new Exception("Failed to unsubscribe capture log!");
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog(e);
            }
        }

        public static async Task ProfilerDisconnectRemoteAsync(this WwiseUtility util)
        {
            if (!await util.isProfilerReady()) return;

            try
            {
                var func = util.Function.Verify("ak.wwise.core.remote.disconnect");

                await util.CallAsync(func, null, null, util.TimeOut);

                WaapiLog.InternalLog("Remote disconnected!");
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to disconnect to remote! ======> {e.Message}");
            }
        }

        public static async Task<bool> ProfilerGetRemoteConnectionStatusAsync(this WwiseUtility util)
        {
            if (!await util.isProfilerReady()) return false;

            try
            {
                var func = util.Function.Verify("ak.wwise.core.remote.getConnectionStatus");

                var jresult = await util.CallAsync(func, null, null, util.TimeOut);

                bool.TryParse(jresult["isConnected"]?.ToString(), out bool connected);
                return connected;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed get connection status! ======> {e.Message}");
            }

            return false;
        }

        public static async Task ProfilerStartCaptureAsync(this WwiseUtility util)
        {
            if (!await util.ProfilerGetRemoteConnectionStatusAsync()) return;

            try
            {
                var func = util.Function.Verify("ak.wwise.core.profiler.startCapture");
                await util.CallAsync(func, null, null, util.TimeOut);
                WaapiLog.InternalLog("Profiler capture started!");
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Profiler failed to start capture! ======> {e.Message}");
            }
        }

        public static async Task<int> ProfilerStopCaptureAsync(this WwiseUtility util)
        {
            if (!await util.ProfilerGetRemoteConnectionStatusAsync()) return -1;

            try
            {
                var func = util.Function.Verify("ak.wwise.core.profiler.stopCapture");
                var jresult = await util.CallAsync(func, null, null, util.TimeOut);
                int.TryParse(jresult["return"]?.ToString(), out int cursorTime);

                WaapiLog.InternalLog("Profiler capture stop!");

                return cursorTime;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Profiler failed to stop capture! ======> {e.Message}");
            }

            return -1;
        }

        public static async Task<int> ProfilerGetCursorTimeMsAsync(this WwiseUtility util, Cursor cursor = Cursor.capture)
        {
            if (!await util.isProfilerReady()) return -1;

            try
            {
                var func = util.Function.Verify("ak.wwise.core.profiler.getCursorTime");

                var option = new
                {
                    cursor = cursor.ToString()
                };

                var jresult = await util.CallAsync(func, option, null, util.TimeOut);
                int.TryParse(jresult["return"]?.ToString(), out int cursorTime);


                return cursorTime;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Profiler failed to get cursor time! ======> {e.Message}");
            }

            return -1;
        }

        public static async Task<List<ProfilerRTPC>> ProfilerGetRTPCsAsync(this WwiseUtility util, Cursor cursor = Cursor.capture)
        {
            if (!await util.isProfilerReady()) return null;

            var result = new List<ProfilerRTPC>();

            try
            {
                var func = util.Function.Verify("ak.wwise.core.profiler.getRTPCs");
                var query = new
                {
                    time = cursor.ToString()
                };


                var jresult = await util.CallAsync(func, query, null, util.TimeOut);
                var objects = jresult["return"];

                if (objects == null) return result;
                foreach (var rtpc in objects)
                {
                    string id = rtpc["id"]?.ToString();
                    string name = rtpc["name"]?.ToString();
                    int.TryParse(rtpc["gameObjectId"]?.ToString(), out int gameObjectId);
                    float.TryParse(rtpc["value"]?.ToString(), out float value);

                    result.Add(new ProfilerRTPC()
                    {
                        ID = id,
                        Name = name,
                        GameObjectID = gameObjectId,
                        Value = value
                    });
                }

                return result;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Profiler failed to get RTPCs! ======> {e.Message}");
            }

            return result;
        }

        public static async Task<List<ProfilerVoice>> ProfilerGetVoicesAsync(this WwiseUtility util, Cursor cursor = Cursor.capture)
        {
            if (!await util.isProfilerReady()) return null;

            var result = new List<ProfilerVoice>();

            try
            {
                var func = util.Function.Verify("ak.wwise.core.profiler.getVoices");
                var query = new
                {
                    time = cursor.ToString()
                };

                var options = new
                {
                    @return = new string[]
                    {
                        "pipelineID",
                        "playingID",
                        "soundID",
                        "gameObjectID",
                        "gameObjectName",
                        "objectGUID",
                        "objectName",
                        "playTargetID",
                        "playTargetGUID",
                        "playTargetName",
                        "baseVolume",
                        "gameAuxSendVolume",
                        "envelope",
                        "normalizationGain",
                        "lowPassFilter",
                        "highPassFilter",
                        "priority",
                        "isStarted",
                        "isVirtual",
                        "isForcedVirtual"

                    }
                };

                var jresult = await util.CallAsync(func, query, options, util.TimeOut);
                var objects = jresult["return"];
                if (objects == null) return result;

                foreach (var voice in objects)
                {
                    int.TryParse(voice["pipelineID"]?.ToString(), out int piplineID);
                    int.TryParse(voice["playingID"]?.ToString(), out int playingID);
                    int.TryParse(voice["soundID"]?.ToString(), out int soundID);
                    int.TryParse(voice["playTargetID"]?.ToString(), out int playTargetID);
                    int.TryParse(voice["priority"]?.ToString(), out int priority);
                    int.TryParse(voice["gameObjectID"]?.ToString(), out int gameObjectID);
                    float.TryParse(voice["baseVolume"]?.ToString(), out float baseVolume);
                    float.TryParse(voice["gameAuxSendVolume"]?.ToString(), out float gameAuxSendVolume);
                    float.TryParse(voice["envelope"]?.ToString(), out float envelope);
                    float.TryParse(voice["normalizationGain"]?.ToString(), out float normalizationGain);
                    float.TryParse(voice["lowPassFilter"]?.ToString(), out float lowPassFilter);
                    float.TryParse(voice["highPassFilter"]?.ToString(), out float highPassFilter);
                    string objectGUID = voice["objectGUID"]?.ToString();
                    string objectName = voice["objectName"]?.ToString();
                    string gameObjectName = voice["gameObjectName"]?.ToString();
                    string playTargetGUID = voice["playTargetGUID"]?.ToString();
                    string playTargetName = voice["playTargetName"]?.ToString();
                    bool.TryParse(voice["isStarted"]?.ToString(), out bool isStarted);
                    bool.TryParse(voice["isVirtual"]?.ToString(), out bool isVirtual);
                    bool.TryParse(voice["isForcedVirtual"]?.ToString(), out bool isForcedVirtual);


                    result.Add(new ProfilerVoice()
                    {
                        PipelineID = piplineID,
                        PlayingID = playingID,
                        SoundID = soundID,
                        PlayTargetID = playTargetID,
                        Priority = priority,
                        BaseVolume = baseVolume,
                        GameAuxSendVolume = gameAuxSendVolume,
                        Envelope = envelope,
                        NormalizationGain = normalizationGain,
                        LowPassFilter = lowPassFilter,
                        HighPassFilter = highPassFilter,
                        ObjectGUID = objectGUID,
                        ObjectName = objectName,
                        GameObjectID = gameObjectID,
                        GameObjectName = gameObjectName,
                        PlayTargetGUID = playTargetGUID,
                        PlayTargetName = playTargetName,
                        IsStarted = isStarted,
                        IsVirtual = isVirtual,
                        IsForcedVirtual = isForcedVirtual
                    });

                }

                return result;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Profiler failed to get voices! ======> {e.Message}");
            }

            return result;
        }

        public static async Task<List<ProfilerBus>> ProfilerGetBussesAsync(this WwiseUtility util, Cursor cursor = Cursor.capture)
        {
            if (!await util.isProfilerReady()) return null;

            var result = new List<ProfilerBus>();

            try
            {
                var func = util.Function.Verify("ak.wwise.core.profiler.getBusses");
                var query = new
                {
                    time = cursor.ToString()
                };

                var options = new
                {
                    @return = new string[]
                    {
                        "pipelineID",
                        "mixBusID",
                        "objectGUID",
                        "objectName",
                        "gameObjectName",
                        "gameObjectID",
                        "mixerID",
                        "deviceID",
                        "volume",
                        "downstreamGain",
                        "voiceCount",
                        "depth"

                    }
                };

                var jresult = await util.CallAsync(func, query, options, util.TimeOut);
                var objects = jresult["return"];
                if (objects == null) return result;

                foreach (var bus in objects)
                {
                    int.TryParse(bus["pipelineID"]?.ToString(), out int piplineID);
                    int.TryParse(bus["mixBusID"]?.ToString(), out int mixBusID);
                    int.TryParse(bus["gameObjectID"]?.ToString(), out int gameObjectID);
                    int.TryParse(bus["mixerID"]?.ToString(), out int mixerID);
                    int.TryParse(bus["deviceID"]?.ToString(), out int deviceID);
                    int.TryParse(bus["voiceCount"]?.ToString(), out int voiceCount);
                    int.TryParse(bus["depth"]?.ToString(), out int depth);
                    string objectGUID = bus["objectGUID"]?.ToString();
                    string objectName = bus["objectName"]?.ToString();
                    string gameObjectName = bus["gameObjectName"]?.ToString();
                    float.TryParse(bus["volume"]?.ToString(), out float volume);
                    float.TryParse(bus["downstreamGain"]?.ToString(), out float downstreamGain);

                    result.Add(new ProfilerBus()
                    {
                        PipelineID = piplineID,
                        MixBusID = mixBusID,
                        GameObjectID = gameObjectID,
                        MixerID = mixerID,
                        DeviceID = deviceID,
                        VoiceCount = voiceCount,
                        Depth = depth,
                        ObjectGUID = objectGUID,
                        ObjectName = objectName,
                        GameObjectName = gameObjectName,
                        Volume = volume,
                        DownStreamGain = downstreamGain
                    });
                }

                return result;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Profiler failed to get busses! ======> {e.Message}");
            }

            return result;
        }
    }

}
