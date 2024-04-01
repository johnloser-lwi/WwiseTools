using Newtonsoft.Json;

namespace WwiseTools.Serialization;

public class WaapiSerializer
{
    public static T Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }

    public static string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
}