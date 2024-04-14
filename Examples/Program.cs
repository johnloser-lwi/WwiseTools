// See https://aka.ms/new-console-template for more information
using Examples;
using WwiseTools.Models.Import;
using WwiseTools.Objects;
using WwiseTools.Serialization;
using WwiseTools.Utils;

try
{
    WaapiLog.AddCustomLogger(ExampleFunctions.CustomLogger);
    if (await WwiseUtility.Instance.TryConnectWaapiAsync())
    {
        var files = Directory.GetFiles("C:\\Test");
        List<ImportInfo> infos = new List<ImportInfo>();
        foreach (var file in files)
        {
            var pathBuilder = new WwisePathBuilder("\\Actor-Mixer Hierarchy\\Default Work Unit");
            await pathBuilder.AppendHierarchyAsync(WwiseObject.ObjectType.RandomSequenceContainer, "player_jump");
            await pathBuilder.AppendHierarchyAsync(WwiseObject.ObjectType.Sound, (new FileInfo(file).Name));
            var i = new ImportInfo(file, pathBuilder);
            infos.Add(i);
        }

        var wos= await WwiseUtility.Instance.BatchImportSoundAsync(infos.ToArray());
        foreach (var wo in wos)
        {
            Console.WriteLine($"{wo.Name}  {wo.Path}");
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


