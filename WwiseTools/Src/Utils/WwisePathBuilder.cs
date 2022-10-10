using WwiseTools.Objects;

namespace WwiseTools.Utils;

public class WwisePathBuilder
{
    private string _root;

    private string _hierarchy;
    
    public WwisePathBuilder(string root = "\\Actor-Mixer Hierarchy")
    {
        _root = root;
        _hierarchy = "";
    }

    public void AppendHierarchy(WwiseObject.ObjectType type, string name)
    {
        if (!string.IsNullOrEmpty(_hierarchy)) _hierarchy += "\\";
        _hierarchy += $"<{type}>{name}";
    }

    public void ClearHierarchy()
    {
        _hierarchy = "";
    }

    public override string ToString()
    {
        return _root + "\\" + _hierarchy;
    }
}