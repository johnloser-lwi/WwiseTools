﻿// See https://aka.ms/new-console-template for more information
using Examples;
using WwiseTools.Utils;

try
{
    WaapiLog.AddCustomLogger(ExampleFunctions.CustomLogger);
    if (await WwiseUtility.Instance.TryConnectWaapiAsync())
    {
        var obj = await WwiseUtility.Instance.GetWwiseObjectsBySelectionAsync();
        WaapiLog.Log(obj[0].GetParentPath());
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


