using System;

namespace WwiseTools.Utils;

public abstract class WaapiStringListBase : WaapiListBase<string>
{
    public override string Verify(string item)
    {
        item = item.Trim();
        bool result = false;
        string final = null;
        if (_items.Contains(item))
        {
            final = item;
            result = true;
        }
        else
        {
            foreach (var i in this)
            {
                if (i.ToLower().Contains(item.ToLower()))
                {
                    final = i;
                    result = true;
                    break;
                }
            }
            if (result)
                WaapiLog.InternalLog($"Warning: No matching {ListContent.ToLower()} for {item}! Using {final} instead!");
        }

        if (!result) 
            throw new Exception($"{ListContent} {item} not available in wwise " +
                                $"{WwiseUtility.Instance.ConnectionInfo.Version.ToString()}!");
        return final;
    }
}