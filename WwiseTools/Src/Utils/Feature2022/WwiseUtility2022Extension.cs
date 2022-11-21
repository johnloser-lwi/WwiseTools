using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Properties;

namespace WwiseTools.Utils.Feature2022
{
    public static class WwiseUtility2022Extension
    {
        public static async Task UndoAsync(this WwiseUtility utility)
        {
            if (!await utility.TryConnectWaapiAsync()) return;
            if (!VersionHelper.VersionVerify(VersionHelper.V2022_1_0_7929)) return;

            var func = utility.Function.Verify("ak.wwise.core.undo.undo");

            var res = await utility.CallAsync(func, null, null);
        }

        public enum PasteMode
        {
            replaceEntire,
            addReplace,
            addKeep
        }
        
        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="utility"></param>
        /// <param name="source"></param>
        /// <param name="targets"></param>
        /// <param name="properties"></param>
        /// <param name="pasteMode"></param>
        /// <param name="inclusionMode"></param>
        /// <returns></returns>
        public static async Task<bool> PastePropertiesAsync(this WwiseUtility utility, WwiseObject source, WwiseObject[] targets,
            PasteMode pasteMode = PasteMode.replaceEntire, bool inclusionMode = true, params string[] properties)
        {
            if (!await utility.TryConnectWaapiAsync() || targets.Length == 0) return false;
            if (!VersionHelper.VersionVerify(VersionHelper.V2022_1_0_7929)) return false;

            try
            {
                var jTargets = new JArray();

                foreach (var wwiseObject in targets)
                {
                    jTargets.Add(wwiseObject.ID);
                }

                var query = new JObject()
                {
                    new JProperty("source", source.ID),
                    new JProperty("pasteMode", pasteMode.ToString()),
                    new JProperty("targets", jTargets),
                };
                
                // Specify inclusion or exclusion according to inclusionMode.
                // Skip this step if no property provided, all properties will be copied to targets
                if (properties.Length != 0)
                {
                    var jProperties = new JArray();

                    foreach (var property in properties)
                    {
                        jProperties.Add(property);
                    }

                    var jInclusion = new JProperty(inclusionMode ? "inclusion" : "exclusion", jProperties);
                    
                    query.Add(jInclusion);
                }

                var func = utility.Function.Verify("ak.wwise.core.object.pasteProperties");

                await utility.CallAsync(func, query, null, utility.TimeOut);
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to copy properties from {source.Name} to {targets.Length} target(s)! ======> {e.Message}");
                
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
        public static async Task BatchSetObjectPropertyAsync(this WwiseUtility utility, WwiseObject[] wwiseObjects, 
            params WwiseProperty[] wwiseProperties)
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync() || wwiseObjects.Length == 0 || wwiseProperties.Length == 0) return;
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
                    WaapiLog.InternalLog($"Failed to set property \"{wwiseProperties[i].Name}\" for {wwiseObjects.Length} object(s) ======> {e.Message}");
            }
        }
    }
}
