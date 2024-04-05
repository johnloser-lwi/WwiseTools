using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaapiClient;
using WwiseTools.Models.Profiler;
using WwiseTools.Serialization;

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
                    var data = WaapiSerializer.Deserialize<ProfilerAddItemData>(json.ToString());

                    var item = new ProfilerCaptureLogItem()
                    {
                        Type = data.Type,
                        ObjectID = data.ObjectID,
                        ObjectName = data.ObjectName,
                        Description = data.Description,
                        GameObjectID = data.GameObjectID,
                        GameObjectName = data.GameObjectName,
                        ObjectShortID = data.ObjectShortID,
                        PlayingID = data.PlayingID,
                        Severity = data.Severity.ToString(),
                        Time = data.Time,
                        ErrorCodeName = data.ErrorCodeName
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

                var returnData = WaapiSerializer.Deserialize<GetConnectionStatusData>(jresult.ToString());
                return returnData.IsConnected;
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
                var returnData = WaapiSerializer.Deserialize<CursorTimeData>(jresult.ToString());

                WaapiLog.InternalLog("Profiler capture stop!");

                return returnData.Time;
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
                var returnData = WaapiSerializer.Deserialize<CursorTimeData>(jresult.ToString());


                return returnData.Time;
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
                var returnData = WaapiSerializer.Deserialize<ReturnData<ProfilerRTPCData>>(jresult.ToString());
                if (returnData.Return == null || returnData.Return.Length == 0) return result;
                foreach (var rtpc in returnData.Return)
                {
                    result.Add(new ProfilerRTPC()
                    {
                        ID = rtpc.ID,
                        Name = rtpc.Name,
                        GameObjectID = rtpc.GameObjectID,
                        Value = rtpc.Value
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
                var returnData = WaapiSerializer.Deserialize<ReturnData<VoiceReturnData>>(jresult.ToString());
                if (returnData.Return == null || returnData.Return.Length == 0) return result;

                foreach (var voice in returnData.Return)
                {
                    result.Add(new ProfilerVoice()
                    {
                        PipelineID = voice.PipelineID,
                        PlayingID = voice.PlayingID,
                        SoundID = voice.SoundID,
                        PlayTargetID = voice.PlayTargetID,
                        Priority = voice.Priority,
                        BaseVolume = voice.BaseVolume,
                        GameAuxSendVolume = voice.GameAuxSendVolume,
                        Envelope = voice.Envelope,
                        NormalizationGain = voice.NormalizationGain,
                        LowPassFilter = voice.LowPassFilter,
                        HighPassFilter = voice.HighPassFilter,
                        ObjectGUID = voice.ObjectGUID,
                        ObjectName = voice.ObjectName,
                        GameObjectID = voice.GameObjectID,
                        GameObjectName = voice.GameObjectName,
                        PlayTargetGUID = voice.PlayTargetGUID,
                        PlayTargetName = voice.PlayTargetName,
                        IsStarted = voice.IsStarted,
                        IsVirtual = voice.IsVirtual,
                        IsForcedVirtual = voice.IsForcedVirtual
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
                var returnData = WaapiSerializer.Deserialize<ReturnData<BusReturnData>>(jresult.ToString());
                if (returnData.Return == null || returnData.Return.Length == 0) return result;

                foreach (var bus in returnData.Return)
                {

                    result.Add(new ProfilerBus()
                    {
                        PipelineID = bus.PipelineID,
                        MixBusID = bus.MixBusID,
                        GameObjectID = bus.GameObjectID,
                        MixerID = bus.MixerID,
                        DeviceID = bus.DeviceID,
                        VoiceCount = bus.VoiceCount,
                        Depth = bus.Depth,
                        ObjectGUID = bus.ObjectGUID,
                        ObjectName = bus.ObjectName,
                        GameObjectName = bus.GameObjectName,
                        Volume = bus.Volume,
                        DownStreamGain = bus.DownstreamGain
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
