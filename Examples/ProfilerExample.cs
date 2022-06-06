using WwiseTools.Models.Profiler;
using WwiseTools.Utils;
using WwiseTools.Utils.Profiler;

namespace Examples
{
    internal class ProfilerExample
    {
        static WwiseUtility Waapi = WwiseUtility.Instance;

        public static async Task ProfilerTestAsync()
        {
            if (Waapi.ConnectionInfo.IsCommandLine) return;

            var result = await Waapi.ProfilerGetAvailableConsolesAsync();
            if (result == null) return;
            foreach (var info in result)
            {
                Console.WriteLine($"{info.AppName} {info.Platform}");
            }

            var remote = result[0];

            await Waapi.ProfilerSubscribeCaptureLogAsync(new ProfilerCaptureLogOption() { Event = true, Bank = true},
                item => { Console.WriteLine(item.Description); });
            await Waapi.ProfilerConnectToRemoteAsync(remote);

            Console.WriteLine(await Waapi.ProfilerGetRemoteConnectionStatusAsync());
            await Waapi.ProfilerStartCaptureAsync();
            /*for (int i = 0; i < 50; i++)
            {
                // 获取时间
                WaapiLog.Log(await Waapi.ProfilerGetCursorTimeMsAsync());

                await GetVoices();

                await GetBusses();

                await GetRTPCs();

                await Task.Delay(100);
            }*/
            await Task.Delay(10000);

            int time = await Waapi.ProfilerStopCaptureAsync();
            WaapiLog.Log(time);
            await Waapi.ProfilerDisconnectRemoteAsync();
            await Waapi.ProfilerUnsubscribeCaptureLogAsync();
            Console.WriteLine(await Waapi.ProfilerGetRemoteConnectionStatusAsync());
        }

        private static async Task GetRTPCs()
        {
            // 获取RTPC 信息
            var rtpcs = await Waapi.ProfilerGetRTPCsAsync();
            foreach (var rtpc in rtpcs)
            {
                WaapiLog.Log(rtpc.Name + " : " + rtpc.Value);
            }
        }

        private static async Task GetBusses()
        {
            // 获取总线信息
            var busses = await Waapi.ProfilerGetBussesAsync();
            if (busses == null) return;

            foreach (var bus in busses)
            {
                WaapiLog.Log(bus.ObjectName + " : " + bus.Volume);
            }
        }

        private static async Task GetVoices()
        {
            // 获取声部信息
            var voices = await Waapi.ProfilerGetVoicesAsync();
            if (voices == null) return;

            foreach (var vo in voices)
            {
                WaapiLog.Log(vo.ObjectName + " : " + vo.GameObjectName);
            }
        }
    }
}
