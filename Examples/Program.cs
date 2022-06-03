// See https://aka.ms/new-console-template for more information

using System.Reflection.Metadata.Ecma335;
using Examples;
using WwiseTools.Components;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Utils;

try
{
    WaapiLog.AddCustomLogger(ExampleFunctions.CustomLogger);

    if (await WwiseUtility.Instance.TryConnectWaapiAsync())
    {
        await ProfilerExample.ProfilerTestAsync(); // 尝试不同的方法
    }
    else
    {
        WaapiLog.Log("Waapi Connection Failed!");
    }

    await WwiseUtility.Instance.DisconnectAsync();

    await Task.Delay(3000);
}
catch (Exception e)
{
    WaapiLog.Log($"Exception: {e.Message}");
}


