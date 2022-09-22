using WwiseTools.Objects;
using WwiseTools.Utils;
namespace Examples;

public class UndoGroupExample
{
    public static async Task RunUndoGroupExampleAsync()
    {
        var parent = await WwiseUtility.Instance.GetWwiseObjectsBySelectionAsync();

        if (parent.Count == 0) return;


        await WwiseUtility.Instance.BeginUndoGroup();
        
        for (int i = 0; i < 10; i++)
        {
            await WwiseUtility.Instance.CreateObjectAsync($"test_{i}", WwiseObject.ObjectType.Sound, parent[0]);
        }

        await WwiseUtility.Instance.EndUndoGroup("Create 10 Test Objects");
    }
}