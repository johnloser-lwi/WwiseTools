using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.References;

namespace WwiseTools.Utils.Feature2022
{
    public static class WwiseUtility2022Extension
    {
        private static bool VersionVerify([CallerMemberName] string caller = "")
        {
            if (WwiseUtility.ConnectionInfo.Version.Year < 2022)
            {
                //var caller = (new System.Diagnostics.StackTrace()).GetFrame(0).GetMethod().Name;
                WaapiLog.Log($"Warning: {caller} is a Wwise 2022 feature! " +
                             $"Current Wwise version is {WwiseUtility.ConnectionInfo.Version.ToString()}.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 批量配置属性
        /// </summary>
        /// <param name="utility"></param>
        /// <param name="wwiseObjects"></param>
        /// <param name="wwiseProperties"></param>
        /// <returns></returns>
        public static async ValueTask BatchSetObjectPropertyAsync(this WwiseUtility utility, List<WwiseObject> wwiseObjects, 
            params WwiseProperty[] wwiseProperties)
        {
            if (!await WwiseUtility.TryConnectWaapiAsync() || wwiseObjects == null || wwiseProperties == null) return;
            if (!VersionVerify()) return;
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

                var func = WwiseUtility.Function.Verify("ak.wwise.core.object.set");
                await WwiseUtility.Client.Call(func,
                    query,
                    null);

                for (int i = 0; i < wwiseProperties.Length; i++)
                {
                    WaapiLog.Log($"Property {wwiseProperties[i].Name} successfully changed to {wwiseProperties[i].Value}!");
                }
                
            }
            catch (Exception e)
            {
                for (int i = 0; i < wwiseProperties.Length; i++)
                    WaapiLog.Log($"Failed to set property \"{wwiseProperties[i].Name}\" for {wwiseObjects.Count} object(s) ======> {e.Message}");
            }
        }

        /// <summary>
        /// 批量配置引用
        /// </summary>
        /// <param name="utility"></param>
        /// <param name="wwiseObjects"></param>
        /// <param name="wwiseReferences"></param>
        /// <returns></returns>
        public static async ValueTask BatchSetObjectReferenceAsync(this WwiseUtility utility, List<WwiseObject> wwiseObjects, 
            params WwiseReference[] wwiseReferences)
        {
            if (!await WwiseUtility.TryConnectWaapiAsync() || wwiseObjects == null || wwiseReferences == null) return;
            if (!VersionVerify()) return;
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

                var func = WwiseUtility.Function.Verify("ak.wwise.core.object.set");
                await WwiseUtility.Client.Call(
                    func,

                    query,

                    null);

                for (int i = 0; i < wwiseReferences.Length; i++)
                    WaapiLog.Log($"Reference {wwiseReferences[i].Name} successfully set to {wwiseReferences[i].Object.Name}!");
            }
            catch (Exception e)
            {
                for (int i = 0; i < wwiseReferences.Length; i++)
                    WaapiLog.Log($"Failed to set reference \"{wwiseReferences[i].Name}\" " +
                                 $"for {wwiseObjects.Count} object(s)  ======> {e.Message}");
            }
        }
    }
}
