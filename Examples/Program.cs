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
        var prop = await WwiseUtility.Instance.CoreObjectGetAsync<PropertyData>(new []{"{CAFE56CB-8F13-4E96-A757-53D31EE541CA}"}, 
            new string[]{}, new []{"@@OutputBus", "Volume", "Conversion"});
        
        WaapiLog.Log($"Bus: {prop.Return[0].GetProperty<CommonObjectData>("@@OutputBus").Name}");
        WaapiLog.Log($"Volume: {prop.Return[0].GetProperty<float>("Volume")}");
        WaapiLog.Log($"Conversion: {prop.Return[0].GetProperty<CommonObjectData>("Conversion").Name}");
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


