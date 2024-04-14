// See https://aka.ms/new-console-template for more information
using Examples;
using WwiseTools.Objects;
using WwiseTools.Serialization;
using WwiseTools.Utils;

try
{
    WaapiLog.AddCustomLogger(ExampleFunctions.CustomLogger);
    if (await WwiseUtility.Instance.TryConnectWaapiAsync())
    {
        var wo = await WwiseUtility.Instance.GetWwiseObjectsBySelectionAsync();

        foreach (var wwiseObject in wo)
        {
            await WwiseUtility.Instance.SetWwiseObjectColorAsync(wwiseObject, WwiseColor.Bittersweet);
        }
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


