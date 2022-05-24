using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Reference;

namespace WwiseTools.Utils.Feature2022
{
    public static class WwiseUtility2022Extension
    {
        public static async ValueTask BatchSetObjectPropertyAsync(this WwiseUtility utility, List<WwiseObject> wwiseObjects, WwiseProperty wwiseProperty)
        {
            if (!await WwiseUtility.TryConnectWaapiAsync() || wwiseObjects == null || wwiseProperty == null) return;
            try
            {
                var query = new
                {
                    objects = new List<object>()
                };

                foreach (WwiseObject wwiseObject in wwiseObjects)
                {
                    query.objects.Add(new JObject(
                        new JProperty("object", wwiseObject.ID),
                        new JProperty("@"+wwiseProperty.Name, wwiseProperty.Value)
                    ));
                }

                var func = WwiseUtility.Function.Verify("ak.wwise.core.object.set");
                await WwiseUtility.Client.Call(func,
                    query,
                    null);

                WaapiLog.Log($"Property {wwiseProperty.Name} successfully changed to {wwiseProperty.Value}!");
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to set property \"{wwiseProperty.Name}\" for {wwiseObjects.Count} object(s) ======> {e.Message}");
            }
        }


        public static async ValueTask BatchSetObjectReferenceAsync(this WwiseUtility utility, List<WwiseObject> wwiseObjects, WwiseReference wwiseReference)
        {
            if (!await WwiseUtility.TryConnectWaapiAsync() || wwiseObjects == null || wwiseReference == null) return;

            try
            {
                var query = new
                {
                    objects = new List<object>()
                };

                foreach (WwiseObject wwiseObject in wwiseObjects)
                {
                    query.objects.Add(new JObject(
                        new JProperty("object", wwiseObject.ID),
                        new JProperty("@" + wwiseReference.Name, wwiseReference.Object.ID)
                    ));
                }

                var func = WwiseUtility.Function.Verify("ak.wwise.core.object.set");
                await WwiseUtility.Client.Call(
                    func,

                    query,

                    null);

                WaapiLog.Log($"Reference {wwiseReference.Name} successfully set to {wwiseReference.Object.Name}!");
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to set reference \"{wwiseReference.Name}\" for {wwiseObjects.Count} object(s)  ======> {e.Message}");
            }
        }
    }
}
