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
        var obj = await WwiseUtility.Instance.GetWwiseObjectByIDAsync("{61DAC669-F895-4736-B49F-183669888CA6}");

        var ass = await obj.AsSwitchContainer().GetSwitchAssignmentsAsync();

        foreach (var assignment in ass)
        {
            WaapiLog.Log(assignment.AssignedSwitch.Name);
            WaapiLog.Log(assignment.Child.Name);
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


