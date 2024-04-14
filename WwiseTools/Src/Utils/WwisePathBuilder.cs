#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WwiseTools.Objects;

namespace WwiseTools.Utils;

public class WwisePathBuilder
{
    private WwiseObject? _root = null;

    private string _rootPath;

    private List<WwiseObject> _hierarchy;
    
    
  
  
  
  
    public WwisePathBuilder(WwiseObject root)
    {
        _root = root;
        _rootPath = "";
        _hierarchy = new List<WwiseObject>();
    }

    public WwisePathBuilder(string rootPath = "\\Actor-Mixer Hierarchy")
    {
        _rootPath = rootPath;
        _hierarchy = new List<WwiseObject>();
    }


  
  
  
  
  
  
    public async Task<bool> AppendHierarchyAsync(WwiseObject.ObjectType type, string name)
    {
        var initializedWithPath = !string.IsNullOrEmpty(_rootPath);

        if (initializedWithPath) _root = await WwiseUtility.Instance.GetWwiseObjectByPathAsync(_rootPath);

        if (_root == null)
        {
            WaapiLog.InternalLog($"Failed to append {name}:{type} to hierarchy! Invalid root!");
            return false;
        }

        _hierarchy.Add(new WwiseObject(name, "", type.ToString(), ""));
        return true;
    }

    public void ClearHierarchy()
    {
        _hierarchy.Clear();
    }

  
  
  
  
    public async Task<string> GetPurePathAsync()
    {
        if (_root is null && !string.IsNullOrEmpty(_rootPath)) _root = await WwiseUtility.Instance.GetWwiseObjectByPathAsync(_rootPath);
        
        var rootPath = _root is null ? null : _root.Path;
        
#if DEBUG
        if (rootPath == null && !WwiseUtility.Instance.IsConnected()) rootPath = "\\Actor-Mixer Hierarchy";
#endif

        if (string.IsNullOrEmpty(rootPath)) throw new Exception("Invalid root!");
        
        var hierarchy = "";

        foreach (var wwiseObject in _hierarchy)
        {
            if (hierarchy != "") hierarchy += "\\";

            hierarchy += wwiseObject.Name;
        }
        
        return rootPath + "\\" + hierarchy;
    }

  
  
  
  
    public async Task<string> GetImportPathAsync()
    {
        if (_root is null&& !string.IsNullOrEmpty(_rootPath)) _root = await WwiseUtility.Instance.GetWwiseObjectByPathAsync(_rootPath);

        var rootPath = _root is null ? null : _root.Path;
        
#if DEBUG
        if (rootPath == null && !WwiseUtility.Instance.IsConnected()) rootPath = "\\Actor-Mixer Hierarchy";
#endif
        
        if (string.IsNullOrEmpty(rootPath)) throw new Exception("Invalid root!");
        
        var hierarchy = "";

        foreach (var wwiseObject in _hierarchy)
        {
            if (hierarchy != "") hierarchy += "\\";

            hierarchy += $"<{wwiseObject.Type}>{wwiseObject.Name}";
        }
        
        return rootPath + "\\" + hierarchy;
    }
    
    
}