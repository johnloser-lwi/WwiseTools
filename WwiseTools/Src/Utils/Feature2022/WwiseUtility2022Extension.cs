using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.References;

namespace WwiseTools.Utils.Feature2022
{
    public static class WwiseUtility2022Extension
    {
        

        /// <summary>
        /// 批量配置属性
        /// </summary>
        /// <param name="utility"></param>
        /// <param name="wwiseObjects"></param>
        /// <param name="wwiseProperties"></param>
        /// <returns></returns>
        public static async Task BatchSetObjectPropertyAsync(this WwiseUtility utility, List<WwiseObject> wwiseObjects, 
            params WwiseProperty[] wwiseProperties)
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync() || wwiseObjects == null || wwiseProperties == null) return;
            if (!VersionHelper.VersionVerify(VersionHelper.V2022_1_0_7929)) return;
            try
            {
                var query = new
                {
                    objects = new List<object>()
                };

                foreach (WwiseObject wwiseObject in wwiseObjects)
                {
                    var jObject = new JObject(
                        new JProperty("object", wwiseObject.ID)
                    );
                    foreach (var wwiseProperty in wwiseProperties)
                    {
                        jObject.Add(new JProperty("@" + wwiseProperty.Name, wwiseProperty.Value));
                    }
                    query.objects.Add(jObject);
                }

                var func = WwiseUtility.Instance.Function.Verify("ak.wwise.core.object.set");
                await WwiseUtility.Instance.CallAsync(func,
                    query,
                    null, utility.TimeOut);

                for (int i = 0; i < wwiseProperties.Length; i++)
                {
                    WaapiLog.InternalLog($"Property {wwiseProperties[i].Name} successfully changed to {wwiseProperties[i].Value}!");
                }
                
            }
            catch (Exception e)
            {
                for (int i = 0; i < wwiseProperties.Length; i++)
                    WaapiLog.InternalLog($"Failed to set property \"{wwiseProperties[i].Name}\" for {wwiseObjects.Count} object(s) ======> {e.Message}");
            }
        }

        /// <summary>
        /// 批量配置引用
        /// </summary>
        /// <param name="utility"></param>
        /// <param name="wwiseObjects"></param>
        /// <param name="wwiseReferences"></param>
        /// <returns></returns>
        public static async Task BatchSetObjectReferenceAsync(this WwiseUtility utility, List<WwiseObject> wwiseObjects, 
            params WwiseReference[] wwiseReferences)
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync() || wwiseObjects == null || wwiseReferences == null) return;
            if (!VersionHelper.VersionVerify(VersionHelper.V2022_1_0_7929)) return;
            try
            {
                var query = new
                {
                    objects = new List<object>()
                };

                foreach (WwiseObject wwiseObject in wwiseObjects)
                {
                    var jObject = new JObject(
                        new JProperty("object", wwiseObject.ID)
                    );
                    foreach (var wwiseReference in wwiseReferences)
                    {
                        jObject.Add(new JProperty("@" + wwiseReference.Name, wwiseReference.Object.ID));
                    }
                    query.objects.Add(jObject);
                }

                var func = WwiseUtility.Instance.Function.Verify("ak.wwise.core.object.set");
                await WwiseUtility.Instance.CallAsync(
                    func,

                    query,

                    null, utility.TimeOut);

                for (int i = 0; i < wwiseReferences.Length; i++)
                    WaapiLog.InternalLog($"Reference {wwiseReferences[i].Name} successfully set to {wwiseReferences[i].Object.Name}!");
            }
            catch (Exception e)
            {
                for (int i = 0; i < wwiseReferences.Length; i++)
                    WaapiLog.InternalLog($"Failed to set reference \"{wwiseReferences[i].Name}\" " +
                                 $"for {wwiseObjects.Count} object(s)  ======> {e.Message}");
            }
        }
    }
}
