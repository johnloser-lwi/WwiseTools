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

        

        Console.WriteLine("Make Changes!");
        await Task.Delay(5000);

        var res = await WwiseUtility.Instance.BatchUpdateObjectPathAsync(wo);
        
        foreach (var w in wo)
        {
            Console.WriteLine(w.Path);
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


