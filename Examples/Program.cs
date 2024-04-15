// See https://aka.ms/new-console-template for more information
using Examples;
using WwiseTools.Models.Import;
using WwiseTools.Objects;
using WwiseTools.Serialization;
using WwiseTools.Utils;
using WwiseTools.Utils.Feature2023;

try
{
    WaapiLog.AddCustomLogger(ExampleFunctions.CustomLogger);
    if (await WwiseUtility.Instance.TryConnectWaapiAsync())
    {
        var ret = await WwiseUtility.Instance.ExecuteLuaScriptAsync("D:\\Temp\\main.lua", new string[] {}, new string[] {}, new string[]{});
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


