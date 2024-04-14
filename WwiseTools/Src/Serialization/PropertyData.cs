using System.Collections.Generic;

namespace WwiseTools.Serialization;

public class PropertyData : Dictionary<string, object>
{
    public T GetProperty<T>(string key)
    {
        if (ContainsKey(key) && this[key] is not null)
        {
            return WaapiSerializer.Deserialize<T>(this[key].ToString());
        }
        return default;
    }
}