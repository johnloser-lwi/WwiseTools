#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WwiseTools.Objects;

namespace WwiseTools.Utils;

public class WwisePathBuilder
{
    private WwiseObject? _root = null;

    private string _rootPath;

    private List<WwiseObject> _hierarchy;
    
    
    /// <summary>
    /// 规范Wwise路径格式
    /// </summary>
    /// <param name="root">root为现有的Wwise对象路径，无需指定类别</param>
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


    /// <summary>
    /// 添加路径
    /// </summary>
    /// <param name="type">Wwise对象类型</param>
    /// <param name="name">Wwise对象名称</param>
    /// <returns>返回路径是否成功添加</returns>
    public async Task<bool> AppendHierarchy(WwiseObject.ObjectType type, string name)
    {
        WwiseObject? last;

        var initializedWithPath = !string.IsNullOrEmpty(_rootPath);

        if (initializedWithPath) _root = await WwiseUtility.Instance.GetWwiseObjectByPathAsync(_rootPath);

        if (_root == null)
        {
            WaapiLog.InternalLog($"Failed to append {name}:{type} to hierarchy! Invalid root!");
            return false;
        }

        _hierarchy.Add(new WwiseObject(name, "", type.ToString()));
        return true;
    }

    public void ClearHierarchy()
    {
        _hierarchy.Clear();
    }

    /// <summary>
    /// 获取不包含wwise类型的路径
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetPurePathAsync()
    {
        if (_root is null && !string.IsNullOrEmpty(_rootPath)) _root = await WwiseUtility.Instance.GetWwiseObjectByPathAsync(_rootPath);
        
        var rootPath = _root is null ? null : await _root.GetPathAsync();
        
#if DEBUG
        if (rootPath == null && !WwiseUtility.Instance.IsConnected()) rootPath = "\\Actor-Mixer Hierarchy";
#endif

        if (string.IsNullOrEmpty(rootPath)) throw new Exception("Invalid root!");
        
        string hierarchy = "";

        foreach (var wwiseObject in _hierarchy)
        {
            if (hierarchy != "") hierarchy += "\\";

            hierarchy += wwiseObject.Name;
        }
        
        return rootPath + "\\" + hierarchy;
    }

    /// <summary>
    /// 获取包含wwise类型的路径
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetImportPathAsync()
    {
        if (_root is null&& !string.IsNullOrEmpty(_rootPath)) _root = await WwiseUtility.Instance.GetWwiseObjectByPathAsync(_rootPath);
        
        var rootPath = _root is null ? null : await _root.GetPathAsync();
        
#if DEBUG
        if (rootPath == null && !WwiseUtility.Instance.IsConnected()) rootPath = "\\Actor-Mixer Hierarchy";
#endif
        
        if (string.IsNullOrEmpty(rootPath)) throw new Exception("Invalid root!");
        
        string hierarchy = "";

        foreach (var wwiseObject in _hierarchy)
        {
            if (hierarchy != "") hierarchy += "\\";

            hierarchy += $"<{wwiseObject.Type}>{wwiseObject.Name}";
        }
        
        return rootPath + "\\" + hierarchy;
    }
}