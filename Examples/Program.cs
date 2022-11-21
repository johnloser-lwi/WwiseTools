// See https://aka.ms/new-console-template for more information
using Examples;
using WwiseTools.Objects;
using WwiseTools.Utils;

try
{
    WaapiLog.AddCustomLogger(ExampleFunctions.CustomLogger);

    if (await WwiseUtility.Instance.TryConnectWaapiAsync())
    {
        //await SoundBankExamples.RemoveSoundBankInclusion(); // 尝试不同的方法
        var testSource =
            await WwiseUtility.Instance.GetWwiseObjectByPathAsync(
                "\\Actor-Mixer Hierarchy\\Default Work Unit\\Test_Source");
        
        var testTarget =
            await WwiseUtility.Instance.GetWwiseObjectByPathAsync(
                "\\Actor-Mixer Hierarchy\\Default Work Unit\\Test_Target");

        //await WwiseUtility.Instance.CopyObjectPropertiesAsync(testSource, testTarget, "Volume", "CenterPercentage");
        await WwiseUtility.Instance.CopyObjectReferencesAsync(testSource, new []{testTarget}, "Attenuation");
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


