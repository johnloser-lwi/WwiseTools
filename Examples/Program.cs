// See https://aka.ms/new-console-template for more information
using Examples;
using WwiseTools.Objects;
using WwiseTools.Serialization;
using WwiseTools.Utils;
using WwiseTools.Utils.Feature2022;

try
{
    WaapiLog.AddCustomLogger(ExampleFunctions.CustomLogger);

    if (await WwiseUtility.Instance.TryConnectWaapiAsync())
    {
        //await SoundBankExamples.RemoveSoundBankInclusion(); // 尝试不同的方法
        //var objects = await WwiseUtility.Instance.GetWwiseObjectsOfTypeAsync(WwiseObject.ObjectType.WorkUnit);
        var obj = await WwiseUtility.Instance.GetWwiseObjectByNameAsync("Event:TestEvent"); 
        WaapiLog.Log($"{obj.Type}: {obj.Name} ({obj.ID})");
        
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


