// See https://aka.ms/new-console-template for more information
using Examples;
using WwiseTools.Models.Import;
using WwiseTools.Objects;
using WwiseTools.Serialization;
using WwiseTools.Utils;
using WwiseTools.Utils.Feature2022;

try
{
    WaapiLog.AddCustomLogger(ExampleFunctions.CustomLogger);
    if (await WwiseUtility.Instance.TryConnectWaapiAsync())
    {

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


