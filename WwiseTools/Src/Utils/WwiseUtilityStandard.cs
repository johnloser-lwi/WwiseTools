#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json.Linq;
using WwiseTools.Models;
using WwiseTools.Models.Import;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.References;
using WwiseTools.Src.Models.SoundBank;

namespace WwiseTools.Utils
{
    public  partial class WwiseUtility
    {
        /// <summary>
        /// 获取属性、引用名称
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <returns></returns>
        public async Task<string> GetPropertyAndReferenceNamesAsync(WwiseObject wwiseObject)
        {
            //ak.wwise.core.object.getPropertyAndReferenceNames

            if (!await TryConnectWaapiAsync()) return "";

            try
            {
                var func = Function.Verify("ak.wwise.core.object.getPropertyAndReferenceNames");
                var result = await _client.Call(func,

                    new JObject(

                        new JProperty("object", wwiseObject.ID)),

                    null);
                WaapiLog.InternalLog("Property and References fetched successfully!");
                return result.ToString();


            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to fetch Property and References! ======> {e.Message}");
                return "";
            }
        }
        

        /// <summary>
        /// 设置物体的引用
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="wwiseReference"></param>
        /// <returns></returns>
        public async Task<bool> SetObjectReferenceAsync(WwiseObject wwiseObject, WwiseReference wwiseReference)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.setReference");
                await _client.Call(func,

                    new JObject(

                        new JProperty("object", wwiseObject.ID),

                        new JProperty("reference", wwiseReference.Name),

                        new JProperty("value", wwiseReference.Object.ID)),

                    null);

                WaapiLog.InternalLog("Reference set successfully!");

                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to set reference \"{wwiseReference.Name}\" to object {wwiseObject.Name} ======> {e.Message}");
            }

            return false;
        }

        public async Task<bool> SetObjectPropertiesAsync(WwiseObject wwiseObject, params WwiseProperty[] properties)
        {
            if (!await TryConnectWaapiAsync()) return false;

            var ret = true;
            
            foreach (var property in properties)
            {
                var res = await SetObjectPropertyAsync(wwiseObject, property);

                if (!res) ret = res;
            }

            return ret;
        }
        
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> SetObjectPropertyAsync(WwiseObject wwiseObject, WwiseProperty wwiseProperty)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.setProperty");
                await _client.Call(func,

                    new JObject(

                        new JProperty("property", wwiseProperty.Name),

                        new JProperty("object", wwiseObject.ID),

                        new JProperty("value", wwiseProperty.Value)),

                    null);

                WaapiLog.InternalLog($"Property {wwiseProperty.Name} successfully changed to {wwiseProperty.Value}!");

                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to set property \"{wwiseProperty.Name}\" of object {wwiseObject.Name} ======> {e.Message}");
            }

            return false;
        }
        

        /// <summary>
        /// 修改名称
        /// </summary>
        /// <param name="renameObject"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public async Task<bool> ChangeObjectNameAsync(WwiseObject renameObject, string newName)
        {
            if (!await TryConnectWaapiAsync() || String.IsNullOrEmpty(newName)) return false;

            string oldName = renameObject.Name;
            try
            {
                var func = Function.Verify("ak.wwise.core.object.setName");
                await _client.Call(func
                    ,
                    new
                    {
                        @object = renameObject.ID,
                        value = newName,
                    }, null, TimeOut);

                renameObject.Name = newName;

                WaapiLog.InternalLog($"Object {oldName} successfully renamed to {newName}!");
                return true;
            }

            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to rename object : {oldName} ======> {e.Message}");
            }

            return false;
        }
        
        /// <summary>
        /// 将物体移动至指定父物体
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public async Task<bool> CopyToParentAsync(WwiseObject child, WwiseObject parent, NameConflictBehaviour conflictBehaviour = NameConflictBehaviour.rename)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.copy");
                // 移动物体
                await _client.Call(func
                    ,
                    new JObject
                    {
                        new JProperty("object", child.ID),
                        new JProperty("parent", parent.ID),
                        new JProperty("onNameConflict", conflictBehaviour.ToString())
                    }
                    ,null, TimeOut);


                WaapiLog.InternalLog($"Copied {child.Name} to {parent.Name}!");


                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to copy {child.Name} to {parent.Name}! ======> {e.Message}");
            }

            return false;
        }

        

        /// <summary>
        /// 将物体复制至指定父物体
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public async Task<bool> MoveToParentAsync(WwiseObject child, WwiseObject parent, NameConflictBehaviour conflictBehaviour = NameConflictBehaviour.rename)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.move");
                // 移动物体
                await _client.Call(func,
                    new JObject
                    {
                        new JProperty("object", child.ID),
                        new JProperty("parent", parent.ID),
                        new JProperty("onNameConflict", conflictBehaviour.ToString())
                    },
                    null,
                    TimeOut
                    );

                WaapiLog.InternalLog($"Moved {child.Name} to {parent.Name}!");

                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to move {child.Name} to {parent.Name}! ======> {e.Message}");
            }

            return false;
        }

        public async Task<string> GetNotesAsync(WwiseObject target)
        {
            //ak.wwise.core.object.getPropertyAndReferenceNames

            if (!await TryConnectWaapiAsync()) return "";

            try
            {
                var func = Function.Verify(WaapiFunction.CoreObjectGet);

                var query = new
                {
                    from = new
                    {
                        id = new string[]{ target.ID }
                    }
                };
                
                var options = new
                {

                    @return = new string[] { "notes" }

                };
                
                var result = await _client.Call(func,

                    query,

                    options, TimeOut);
                WaapiLog.InternalLog("Notes fetched successfully!");
                
                if (result["return"] is null) return "";
                if (result["return"].Last?["notes"] is null) return "";
                
                return result["return"].Last["notes"].ToString();

            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to fetch notes! ======> {e.Message}");
                return "";
            }
        }

        public async Task<bool> SetNoteAsync(WwiseObject target, string note)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.setNotes");
                // 移动物体
                await _client.Call(func,
                    new JObject
                    {
                        new JProperty("object", target.ID),
                        new JProperty("value", note)
                    },
                    null,
                    TimeOut
                    );

                WaapiLog.InternalLog($"Successfully set {target.Name} note to \"{note}\"!");

                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to set note for {target.Name}! ======> {e.Message}");
            }

            return false;
        }

        /// <summary>
        /// 生成播放事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="objectPath"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        [Obsolete("Use Event component instead!")]
        public async Task<WwiseObject?> CreatePlayEventAsync(string eventName, string objectPath, string parentPath = @"\Events\Default Work Unit")
        {
            if (!await TryConnectWaapiAsync()) return null;
            return await AddEventActionAsync(eventName, objectPath, parentPath);
        }
        

        /// <summary>
        /// 生成播放事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="objectPath"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        [Obsolete("Use Event component instead!")]
        public async Task<WwiseObject?> AddEventActionAsync(string eventName, string objectPath, string parentPath = @"\Events\Default Work Unit", 
            WwiseProperty.Option_ActionType actionType = WwiseProperty.Option_ActionType.Play, NameConflictBehaviour conflictBehaviour = NameConflictBehaviour.merge)
        {
            if (!await TryConnectWaapiAsync()) return null;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.create");
                var result = await _client.Call
                    (
                        func,
                        new JObject
                        {
                            new JProperty("parent", parentPath),
                            new JProperty("type", "Event"),
                            new JProperty("name", eventName),
                            new JProperty("onNameConflict", conflictBehaviour.ToString()),
                            new JProperty("children", new JArray
                            {
                                new JObject
                                {
                                    new JProperty("name", ""),
                                    new JProperty("type", "Action"),
                                    new JProperty("@ActionType", actionType.ToString()),
                                    new JProperty("@Target", objectPath)
                                }
                            })
                        },
                        null,
                        TimeOut
                    );

                WaapiLog.InternalLog($"Event {eventName} created successfully!");
                if (result["id"] == null) throw new Exception();
                return await GetWwiseObjectByIDAsync(result["id"].ToString());
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to created play event : {eventName}! ======> {e.Message} ");
                return null;
            }
        }

        [Obsolete("Use AddSoundBankInclusionAsync instead")]
        public async Task<bool> AddEventToBankAsync(WwiseObject soundBank, string eventId)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {

                var func = Function.Verify("ak.wwise.core.soundbank.setInclusions");
                await _client.Call
                (
                    func,
                    new JObject
                    {
                        new JProperty("soundbank", soundBank.ID),
                        new JProperty("operation", "add"),
                        new JProperty("inclusions", new JArray
                        {
                            new JObject
                            {
                                new JProperty("object", eventId),
                                new JProperty("filter", new JArray("events", "structures", "media"))
                            }
                        })
                    },
                    null,
                    TimeOut
                );

                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to Add Event to Bank ======> {e.Message}");
            }

            return false;
        }

        public async Task<WwiseObject?> CreateObjectAsync(string objectName, WwiseObject.ObjectType objectType, WwiseObject parent, 
            NameConflictBehaviour conflictBehaviour = NameConflictBehaviour.fail,
            params WwiseProperty[] properties)
        {
            if(!await TryConnectWaapiAsync()) return null;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.create");

                // 创建物体
                var result = await _client.Call
                (
                    func,
                    new JObject
                    {
                        new JProperty("name", objectName),
                        new JProperty("type", objectType.ToString()),
                        new JProperty("parent", parent.ID),
                        new JProperty("onNameConflict", conflictBehaviour.ToString())
                    },
                    null
                );

                
                if (result["id"] == null) throw new Exception();

                var ret = await GetWwiseObjectByIDAsync(result["id"]?.ToString());

                if (ret == null) return null;
                
                WaapiLog.InternalLog($"Object {ret.Name} created successfully!");
                
                foreach (var wwiseProperty in properties)
                {
                    await SetObjectPropertyAsync(ret, wwiseProperty);
                }
                
                return ret;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to create object : {objectName}! ======> {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 创建物体
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="objectType"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public async Task<WwiseObject?> CreateObjectAtPathAsync(string objectName, WwiseObject.ObjectType objectType, 
            string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit", NameConflictBehaviour conflictBehaviour = NameConflictBehaviour.fail,
            params WwiseProperty[] properties)
        {
            if (!await TryConnectWaapiAsync()) return null;

            var parent = await WwiseUtility.Instance.GetWwiseObjectByPathAsync(parentPath);

            if (parent == null) return null;
            
            return await CreateObjectAsync(objectName, objectType,
                parent, conflictBehaviour, properties);
        }

        public async Task<bool> DeleteObjectAsync(WwiseObject wwiseObject)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.delete");

                // 创建物体
                var result = await _client.Call
                    (
                        func,
                    new JObject
                    {
                        new JProperty("object", wwiseObject.ID)
                    },
                    null
                    );

                WaapiLog.InternalLog($"Object {wwiseObject.Name} deleted successfully!");
                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to delete object : {wwiseObject.Name}! ======> {e.Message}");
                
            }

            return false;
        }

        
        /// <summary>
        /// 通过ID搜索物体
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        public async Task<WwiseObject?> GetWwiseObjectByIDAsync(string targetId)
        {
            if (!await TryConnectWaapiAsync() || String.IsNullOrWhiteSpace(targetId)) return null;

            try
            {
                var func = WaapiFunction.CoreObjectGet;

                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        id = new string[] { targetId }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "name", "id", "type", "path", "musicPlaylistRoot" }

                };

                JObject jresult = await _client.Call(func, query, options, TimeOut);
                if (jresult["return"]?.Last == null) throw new Exception();
                string? name = jresult["return"]?.Last?["name"]?.ToString();
                string? id = jresult["return"]?.Last?["id"]?.ToString();
                string? type = jresult["return"]?.Last?["type"]?.ToString();

                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type))
                {
                    WaapiLog.InternalLog($"WwiseObject {name} successfully fetched!");

                    return new WwiseObject(name, id, type);
                }
                
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject from ID : {targetId}! ======> {e.Message}");
                return null;
            }

            return null;
        }

        public async Task<JToken?> GetWwiseObjectPropertyByIDAsync(string targetId, string wwiseProperty)
        {
            if (!await TryConnectWaapiAsync() || String.IsNullOrWhiteSpace(targetId)) return null;

            try
            {
                var func = WaapiFunction.CoreObjectGet;

                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        id = new string[] { targetId }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "@" + wwiseProperty }

                };

                JObject jresult = await _client.Call(func, query, options, TimeOut);


                if (jresult["return"] == null) throw new Exception();

                WaapiLog.InternalLog($"WwiseProperty {wwiseProperty} successfully fetched!");
                return jresult["return"]!.Last?["@" + wwiseProperty];
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject Property : {targetId}! ======> {e.Message}");
                return null;
            }

        }

        public async Task<WwiseProperty?> GetWwiseObjectPropertyAsync(WwiseObject wwiseObject, string wwiseProperty)
        {
            if (!await TryConnectWaapiAsync()) return null;

            try
            {
                var jresult = await GetWwiseObjectPropertyByIDAsync(wwiseObject.ID, wwiseProperty);
                if (jresult is null) return null;
                return new WwiseProperty(wwiseProperty, jresult.ToString());
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject Property : {wwiseObject.Name}! ======> {e.Message}");
                return null;
            }

        }


        public async Task<string?> GetWwiseObjectPathAsync(string Id)
        {
            if (!await TryConnectWaapiAsync() || string.IsNullOrEmpty(Id)) return null;

            try
            {
                var func = WaapiFunction.CoreObjectGet;

                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        id = new string[] { Id }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "path" }

                };


                JObject jresult = await WwiseUtility.Instance._client.Call(func, query, options, TimeOut);
                if (jresult["return"] == null || jresult["return"]!.Last == null || 
                    jresult["return"]!.Last!["path"] == null) throw new Exception();
                string? path = jresult["return"]?.Last!["path"]?.ToString();

                return path;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get path of object : {Id}! =======> {e.Message}");
                return null;
            }

        }
        
        /// <summary>
        /// 通过名称搜索唯一命名对象，格式必须为"type:name"
        /// </summary>
        /// <param name="targetName"></param>
        /// <returns></returns>
        public async Task<WwiseObject?> GetWwiseObjectByNameAsync(string targetName)
        {
            if (!await TryConnectWaapiAsync() || String.IsNullOrWhiteSpace(targetName)) return null;

            try
            {
                var func = WaapiFunction.CoreObjectGet;

                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        name = new string[] { targetName }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "name", "id", "type", "path" }

                };



                JObject jresult = await _client.Call(func, query, options, TimeOut);
                var obj = jresult["return"]?.Last;
                if (obj == null) throw new Exception("Object not found!");

                string? name = obj["name"]?.ToString();
                string? id = obj["id"]?.ToString();
                string? type = obj["type"]?.ToString();
                
                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type))
                {
                    WaapiLog.InternalLog($"WwiseObject {name} successfully fetched!");
                    return new WwiseObject(name, id, type);
                }
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject by name : {targetName}! ======> {e.Message}");
                return null;
            }

            return null;
        }
        
        /// <summary>
        /// 通过路径获取对象
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<WwiseObject?> GetWwiseObjectByPathAsync(string path)
        {
            if (!await TryConnectWaapiAsync() || String.IsNullOrWhiteSpace(path)) return null;

            try
            {
                var func = WaapiFunction.CoreObjectGet;

                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        path = new string[] { path }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "name", "id", "type", "path" }

                };

                JObject jresult = await _client.Call(func, query, options, TimeOut);
                var obj = jresult["return"]?.Last;
                if (obj == null) throw new Exception("Object not found!");

                string? name = obj["name"]?.ToString();
                string? id = obj["id"]?.ToString();
                string? type = obj["type"]?.ToString();
                
                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type))
                {
                    WaapiLog.InternalLog($"WwiseObject {name} successfully fetched!");
                    return new WwiseObject(name, id, type);
                }
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject by path : {path}! ======> {e.Message}");
                return null;
            }

            return null;

        }

        public async Task<List<WwiseObject>> GetWwiseObjectsOfTypeAsync(WwiseObject.ObjectType targetType)
        {
            return await GetWwiseObjectsOfTypeAsync(targetType.ToString());
        }
        
        /// <summary>
        /// 获取指定类型的对象
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public async Task<List<WwiseObject>> GetWwiseObjectsOfTypeAsync(string targetType)
        {
            List<WwiseObject> result = new List<WwiseObject>();

            if (!await TryConnectWaapiAsync() || String.IsNullOrWhiteSpace(targetType)) return result;

            try
            {

                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        ofType = new string[] { targetType }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "name", "id", "type", "path" }

                };



                var func = WaapiFunction.CoreObjectGet;

                JObject jresult = await _client.Call(func, query, options, TimeOut);

                
                if (jresult["return"] == null) throw new Exception();
                foreach (var obj in jresult["return"]!)
                {
                    string? name = obj["name"]?.ToString();
                    string? id = obj["id"]?.ToString();
                    string? type = obj["type"]?.ToString();

                    if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type)) 
                        result.Add(new WwiseObject(name, id, type));
                }



                WaapiLog.InternalLog($"WwiseObject list of type {targetType} successfully fetched!");

                
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject list of type : {targetType}! ======> {e.Message}");
            }

            return result;
        }

        
        public async Task<List<WwiseObject>> GetWwiseObjectsBySelectionAsync()
        {
            List<WwiseObject> result = new List<WwiseObject>();

            if (!await TryConnectWaapiAsync()) return result;
            try
            {
                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "name", "id", "type", "path" }

                };

                var func = Function.Verify("ak.wwise.ui.getSelectedObjects");

                JObject jresult = await _client.Call(func, null, options, TimeOut);

                


                if (jresult["objects"] == null) throw new Exception();
                foreach (var obj in jresult["objects"])
                {
                    string? name = obj["name"]?.ToString();
                    string? id = obj["id"]?.ToString();
                    string? type = obj["type"]?.ToString();

                    if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type)) 
                        result.Add(new WwiseObject(name, id, type));
                }

                WaapiLog.InternalLog($"Selected WwiseObject list successfully fetched!");

               
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return Selected WwiseObject list! ======> {e.Message}");
            }

            return result;
        }

        public async Task<List<string>> GetPlatformsAsync()
        {
            var result = new List<string>();

            if (!await TryConnectWaapiAsync()) return result;

            try
            {
                var targets = await GetWwiseObjectsOfTypeAsync("Platform");

                string[] ignoreList = { "WwiseAuthoringPlayback" };

                foreach (var wwiseObject in targets)
                {
                    if (ignoreList.Contains(wwiseObject.Name)) continue;

                    result.Add(wwiseObject.Name);
                }
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get platforms! ======> {e.Message}");
            }

            return result;
        }
        
        public async Task<List<string>> GetLanguagesAsync()
        {
            List<string> resultList = new List<string>();

            if (!await TryConnectWaapiAsync()) return resultList;

            

            try // 尝试返回物体数据
            {
                var query = new
                {
                    from = new
                    {
                        ofType = new string[]
                        {
                            "Language"
                        }
                    }
                };

                var options = new
                {

                    @return = new string[] { "name", "id" }

                };

                var func = WaapiFunction.CoreObjectGet;

                var result = await _client.Call(func, query, options, TimeOut);
                if (result["return"] == null) throw new Exception();
                foreach (var r in result["return"]!)
                {
                    if (r["name"] == null) continue;
                    string name = r["name"]!.ToString();
                    var ignoreList = new string[] { "Mixed", "SFX", "External", "SoundSeed Grain" };
                    if (!ignoreList.Contains(name))
                        resultList.Add(name);
                }

                WaapiLog.InternalLog($"Language list fetched successfully!");

            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return language list! ======> {e.Message}");
            }

            return resultList;
        }

        /// <summary>
        /// 从指定路径导入音频
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="language"></param>
        /// <param name="subFolder"></param>
        /// <param name="parentPath"></param>
        /// <param name="soundName"></param>
        /// <returns></returns>
        public async Task<WwiseObject?> ImportSoundAsync(string filePath, string language = "SFX", string subFolder = "", string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit", string soundName = "", ImportAction importAction = ImportAction.useExisting) // Async版本
        {
            var parent = await GetWwiseObjectByPathAsync(parentPath);

            if (parent == null) return null;
            
            return await ImportSoundAsync(parent, filePath, language, subFolder, soundName, importAction);
        }
        
        public async Task<WwiseObject?> ImportSoundAsync(WwiseObject parent, string filePath, string language = "SFX", string subFolder = "", string soundName = "", ImportAction importAction = ImportAction.useExisting)
        {
            if (string.IsNullOrEmpty(soundName))
            {
                soundName = Path.GetFileName(filePath).Replace(".wav", ""); // 尝试获取文件名
            }

            var objectPath = new WwisePathBuilder(parent);
            var res = await objectPath.AppendHierarchy(WwiseObject.ObjectType.Sound, soundName);

            if (!res)
            {
                WaapiLog.InternalLog($"Failed to import {filePath}! Invalid parent!");
                return null;
            }
            
            ImportInfo info = new ImportInfo(filePath, objectPath, language, subFolder);

            return await ImportSoundAsync(info, importAction);
        }
        
        public async Task<WwiseObject?> ImportSoundAsync(ImportInfo info, ImportAction importAction = ImportAction.useExisting) // Async版本
        {
            if (!info.IsValid) return null; // 目标不是文件或者没有成功连接时返回空的WwiseObject
            

            try
            {
                var importQ = new JObject // 导入配置
                {
                    new JProperty("importOperation", importAction.ToString()),
                    
                    new JProperty("imports", new JArray
                    {
                        await info.ToJObjectImportProperty()
                    })
                };

                var options = new JObject(new JProperty("return", new object[] { "name", "id", "type", "path" })); // 设置返回参数

                var func = Function.Verify("ak.wwise.core.audio.import");

                var result = await _client.Call(func, importQ, options); // 执行导入


                if (result == null || result["objects"] == null) return null;
                foreach (var token in result["objects"]!)
                {
                    if (token["id"] == null) continue;

                    var id = token["id"]?.ToString();
                    
                    if (string.IsNullOrEmpty(id)) continue;

                    var wwiseObject = await WwiseUtility.Instance.GetWwiseObjectByIDAsync(id!);

                    if (wwiseObject != null && wwiseObject.Type == "Sound") return wwiseObject;

                }

                return null;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to import file : {info.AudioFile} ======> {e.Message}");
                return null;
            }
        }
        
        public async Task<List<WwiseObject>> BatchImportSoundAsync(ImportInfo[] infos, ImportAction importAction = ImportAction.useExisting) // Async版本
        {
            var ret = new List<WwiseObject>();
            try
            {
                JArray importArray = new JArray();
                
                for (var i = 0; i < infos.Length; i++)
                {
                    var info = infos[i];
                    if (!info.IsValid) continue;
                    
                    importArray.Add(await info.ToJObjectImportProperty());
                }
                
                var importQ = new JObject // 导入配置
                {
                    new JProperty("importOperation", importAction.ToString()),
                    
                    new JProperty("imports", importArray)
                };

                var options = new JObject(new JProperty("return", new object[] { "name", "id", "type", "path" })); // 设置返回参数

                var func = Function.Verify("ak.wwise.core.audio.import");

                var result = await _client.Call(func, importQ, options); // 执行导入

                if (result == null || result["objects"] == null) return ret;

                foreach (var token in result["objects"]!)
                {
                    if (token["id"] == null) continue;

                    var id = token["id"]?.ToString();
                    
                    if (string.IsNullOrEmpty(id)) continue;

                    var wwiseObject = await WwiseUtility.Instance.GetWwiseObjectByIDAsync(id!);

                    if (wwiseObject != null && wwiseObject.Type == "Sound") ret.Add(wwiseObject);
                }
                
                return ret;

            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Batch import failed! ======> {e.Message}");
                ret.Clear();
                return ret;
            }
        }
        
        /// <summary>
        /// 获取工作单元文件路径
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <returns></returns>
        public async Task<string?> GetWorkUnitFilePathAsync(WwiseObject wwiseObject)
        {
            if (!await TryConnectWaapiAsync()) return null;

            try
            {
                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        id = new string[] { wwiseObject.ID }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "filePath" }

                };

                var func = WaapiFunction.CoreObjectGet;

                JObject jresult = await _client.Call(func, query, options, TimeOut);

                string? filePath = "";
                if (jresult["return"] == null) throw new Exception();
                foreach (var obj in jresult["return"]!)
                {
                    if (obj["filePath"] == null) continue;
                    filePath = obj["filePath"]!.ToString();
                }

                WaapiLog.InternalLog($"Work Unit file path of object {wwiseObject.Name} successfully fetched!");

                return filePath;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return Work Unit file path of object : {wwiseObject.Name}! ======> {e.Message}");
                return null;
            }
        }
        
        public async Task<bool> ReloadWwiseProjectAsync()
        {
            var projectPath = await GetWwiseProjectPathAsync();
            if (string.IsNullOrEmpty(projectPath)) return false;
            await LoadWwiseProjectAsync(projectPath!, true);
            await DisconnectAsync();
            _client = null;
            return await ConnectAsync();
        }
        
        /// <summary>
        /// 加载工程
        /// </summary>
        /// <param name="path"></param>
        /// <param name="saveCurrent"></param>
        /// <returns></returns>
        public async Task<bool> LoadWwiseProjectAsync(string path, bool saveCurrent = true)
        {
            if (!await TryConnectWaapiAsync()) return false;

            if (saveCurrent) await SaveWwiseProjectAsync();

            var projectPath = await GetWwiseProjectPathAsync();

            try
            {
                //await Client.Call(ak.wwise.ui.project.close);
                var query = new
                {
                    path = projectPath
                };

                var func = Function.Verify("ak.wwise.ui.project.open");
                await _client.Call(func, query, null, TimeOut);

                WaapiLog.InternalLog("Project loaded successfully!");

                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to load project! =======> {e.Message}");
            }

            return false;
        }
        
        /// <summary>
        /// 获取工程路径
        /// </summary>
        /// <returns></returns>
        public async Task<string?> GetWwiseProjectNameAsync()
        {
            if (!await TryConnectWaapiAsync()) return null;

            try
            {
                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        ofType = new string[] { "Project" }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "name" }

                };

                var func = WaapiFunction.CoreObjectGet;

                JObject jresult = await _client.Call(func, query, options, TimeOut);

                string? name = null;
                if (jresult["return"] == null) throw new Exception();
                foreach (var obj in jresult["return"]!)
                {
                    if (obj["name"] == null) continue;
                    name = obj["name"]!.ToString();
                }

                WaapiLog.InternalLog($"Project name successfully fetched!");

                return name;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return project name! ======> {e.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// 获取工程路径
        /// </summary>
        /// <returns></returns>
        public async Task<string?> GetWwiseProjectPathAsync()
        {
            if (!await TryConnectWaapiAsync()) return null;

            try
            {
                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        ofType = new string[] { "Project" }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "filePath" }

                };

                var func = WaapiFunction.CoreObjectGet;

                JObject jresult = await _client.Call(func, query, options, TimeOut);

                string? filePath = "";
                if (jresult["return"] == null) throw new Exception();
                foreach (var obj in jresult["return"]!)
                {
                    if (obj["filePath"] == null) continue;
                    filePath = obj["filePath"]?.ToString();
                }

                WaapiLog.InternalLog($"Project path successfully fetched!");

                return filePath;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return project path! ======> {e.Message}");
                return null;
            }
        }

        public async Task<WwiseInfo?> GetWwiseInfoAsync()
        {
            if (!await TryConnectWaapiAsync()) return null;

            try
            {
                var func = Function.Verify("ak.wwise.core.getInfo");

                JObject result = await _client.Call(func, null, null);
                if (result["version"] == null) throw new Exception("Failed to fetch version info!");
                int.TryParse(result["version"]!["major"]?.ToString(), out int major);
                int.TryParse(result["version"]!["minor"]?.ToString(), out int minor);
                int.TryParse(result["version"]!["build"]?.ToString(), out int build);
                int.TryParse(result["version"]!["year"]?.ToString(), out int year);
                int.TryParse(result["version"]!["schema"]?.ToString(), out int schema);
                int.TryParse(result["processId"]?.ToString(), out int processId);
                bool.TryParse(result["isCommandLine"]?.ToString(), out bool isCommandLine);

                var projectName = await GetWwiseProjectNameAsync();

                var projectPath = await GetWwiseProjectPathAsync();

                WwiseInfo wwiseInfo = new WwiseInfo()
                {
                    ProjectName = projectName,
                    Version = new WwiseVersion(year, major, minor, build, schema),
                    ProcessID = processId,
                    IsCommandLine = isCommandLine,
                    ProjectPath = projectPath
                };

                return wwiseInfo;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get Wwise info! ======> {e.Message}");
            }

            return null;
        }


        /// <summary>
        /// 支持command访问：https://www.audiokinetic.com/library/2019.2.14_7616/?source=SDK&id=globalcommandsids.html
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteUICommandAsync(string command, string[]? objectIDs = null)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {
                var func = Function.Verify("ak.wwise.ui.commands.execute");

                if (objectIDs is not null)
                {
                    var query = new
                    {
                        command = command,
                        objects = objectIDs
                    };

                    await _client.Call(func, query, null, TimeOut);
                }
                else
                {
                    var query = new { command = command };
                    await _client.Call(func, query, null, TimeOut);
                }

                return true;

            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to execute command {command}! ======> {e.Message}");
            }

            return false;
        }

        public async Task<List<WwiseObject>> GetSoundBanksReferencingWwiseObjectAsync(WwiseObject wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();

            var references = await GetEventsReferencingWwiseObjectAndParentsAsync(wwiseObject);

            foreach (var reference in references)
            {

                var soundBankRefs = (await GetReferencesToWwiseObjectAndParentsAsync(reference)).Where(b => b.Type == "SoundBank")
                    .Distinct().ToList();

                result.AddRange(soundBankRefs);
            }

            var directRefs = await GetReferencesToWwiseObjectAndParentsAsync(wwiseObject);

            result.AddRange(directRefs.Where(r => r.Type == "SoundBank").Distinct().ToList());

            return result.Distinct().ToList();
        }



        public async Task<List<WwiseObject>> GetEventsReferencingWwiseObjectAsync(WwiseObject wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();

            var references = await GetReferencesToWwiseObjectAsync(wwiseObject);

            if (references.Count == 0) return result;

            foreach (var reference in references.Where(r => r.Type == "Action"))
            {
                var e = await GetWwiseObjectParentAsync(reference);
                if (e == null || e.Type != "Event") continue;

                if (!result.Contains(e)) result.Add(e);
            }

            return result.Distinct().ToList();
        }

        public async Task<List<WwiseObject>> GetEventsReferencingWwiseObjectAndParentsAsync(WwiseObject wwiseObject)
        {

            List<WwiseObject> result = new List<WwiseObject>();

            WwiseObject? current = wwiseObject;
            
            while (true)
            {
                if (current is null) break;
                result.AddRange(await GetEventsReferencingWwiseObjectAsync(current));

                current = await GetWwiseObjectParentAsync(current);
            }

            return result.Distinct().ToList();
        }


        public async Task<List<WwiseObject>> GetReferencesToWwiseObjectAndParentsAsync(WwiseObject wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();

            WwiseObject? current = wwiseObject;
            
            while (true)
            {
                if (current is null) break;
                
                result.AddRange(await GetReferencesToWwiseObjectAsync(current));

                current = await GetWwiseObjectParentAsync(current);
            }

            return result.Distinct().ToList();
        }

        public async Task<List<WwiseObject>> GetReferencesToWwiseObjectAsync(WwiseObject? wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();
            if (wwiseObject is null || !await TryConnectWaapiAsync()) return result;
            try
            {
                var query = new
                {
                    from = new
                    {
                        id = new string[] { wwiseObject.ID }
                    },
                    transform = new object[] {
                        new
                        {
                            select = new string[]
                            {
                                "referencesTo"
                            }
                        }
                    }
                };

                var options = new
                {

                    @return = new string[] { "name", "id", "type" }

                };

                var func = WaapiFunction.CoreObjectGet;

                var jresult = await _client.Call(func, query, options, TimeOut);
                if (jresult == null || jresult["return"] == null) return result;
                foreach (var obj in jresult["return"]!)
                {
                    string? name = obj["name"]?.ToString();
                    string? id = obj["id"]?.ToString();
                    string? type = obj["type"]?.ToString();
                    
                    if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type)) 
                        result.Add(new WwiseObject(name, id, type));
                }


            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get references of {wwiseObject.Name} ======> {e.Message}");
            }

            return result;
        }

        public async Task<List<WwiseObject>> BatchGetWwiseObjectParentAsync(List<WwiseObject> wwiseObjects)
        {
            var result = new List<WwiseObject>();
            if (wwiseObjects.Count == 0 || !await TryConnectWaapiAsync()) return result;
            try
            {
                var query = new
                {
                    from = new
                    {
                        id = wwiseObjects.Select(w => w.ID).ToArray()
                    },
                    transform = new object[] {
                        new
                        {
                            select = new string[]
                            {
                                "parent"
                            }
                        }
                    }
                };

                var options = new
                {

                    @return = new string[] { "name", "id", "type" }

                };

                var func = WaapiFunction.CoreObjectGet;

                var jresult = await _client.Call(func, query, options, TimeOut);
                if (jresult == null || jresult["return"] == null) return result;
                foreach (var obj in jresult["return"]!)
                {
                    string? name = obj["name"]?.ToString();
                    string? id = obj["id"]?.ToString();
                    string? type = obj["type"]?.ToString();

                    if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type)) 
                        result.Add(new WwiseObject(name, id, type));
                }


            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children ======> {e.Message}");
            }

            return result;
        }

        public async Task<WwiseObject?> GetWwiseObjectParentAsync(WwiseObject? wwiseObject)
        {
            if (wwiseObject is null || !await TryConnectWaapiAsync()) return null;
            try
            {
                var query = new
                {
                    from = new
                    {
                        id = new string[] { wwiseObject.ID }
                    },
                    transform = new object[] {
                        new
                        {
                            select = new string[]
                            {
                                "parent"
                            }
                        }
                    }
                };

                var options = new
                {

                    @return = new string[] { "name", "id", "type" }

                };

                var func = WaapiFunction.CoreObjectGet;

                var jresult = await _client.Call(func, query, options, TimeOut);
                if (jresult == null || jresult["return"] == null) return null;
                foreach (var obj in jresult["return"]!)
                {
                    string? name = obj["name"]?.ToString();
                    string? id = obj["id"]?.ToString();
                    string? type = obj["type"]?.ToString();
                    
                    if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type)) 
                        return new WwiseObject(name, id, type);
                }


            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children of {wwiseObject.Name} ======> {e.Message}");
            }

            return null;
        }

        public async Task<List<WwiseObject>> BatchGetWwiseObjectChildrenAsync(List<WwiseObject> wwiseObjects)
        {
            List<WwiseObject> result = new List<WwiseObject>();
            if (wwiseObjects.Count == 0 || !await TryConnectWaapiAsync()) return result;
            try
            {
                var query = new
                {
                    from = new
                    {
                        id = wwiseObjects.Select(w => w.ID).ToArray()
                    },
                    transform = new object[] {
                        new
                        {
                            select = new string[]
                            {
                                "children"
                            }
                        }
                    }
                };

                var options = new
                {

                    @return = new string[] { "name", "id", "type" }

                };

                var func = WaapiFunction.CoreObjectGet;

                var jresult = await _client.Call(func, query, options, TimeOut);
                if (jresult == null || jresult["return"] == null) return result;
                foreach (var obj in jresult["return"]!)
                {
                    string? name = obj["name"]?.ToString();
                    string? id = obj["id"]?.ToString();
                    string? type = obj["type"]?.ToString();

                    if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type)) 
                        result.Add(new WwiseObject(name, id, type));
                }


            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children ======> {e.Message}");
            }

            return result;
        }

        public async Task<List<WwiseObject>> GetWwiseObjectChildrenAsync(WwiseObject? wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();
            if (wwiseObject is null || !await TryConnectWaapiAsync()) return result;
            try
            {
                var query = new
                {
                    from = new
                    {
                        id = new string[]{ wwiseObject.ID }
                    },
                    transform = new object[] {
                        new
                        {
                            select = new string[]
                            {
                                "children"
                            }
                        }
                    }
                };

                var options = new
                {

                    @return = new string[] { "name", "id", "type" }

                };

                var func = WaapiFunction.CoreObjectGet;

                var jresult = await _client.Call(func, query, options, TimeOut);
                if (jresult == null || jresult["return"] == null) return result;
                foreach (var obj in jresult["return"]!)
                {
                    string? name = obj["name"]?.ToString();
                    string? id = obj["id"]?.ToString();
                    string? type = obj["type"]?.ToString();

                    if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type)) 
                        result.Add(new WwiseObject(name, id, type));
                }


            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children of {wwiseObject.Name} ======> {e.Message}");
            }

            return result;
        }

        public async Task<List<WwiseObject>> GetWwiseObjectAncestorsAsync(WwiseObject? wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();
            if (wwiseObject is null || !await TryConnectWaapiAsync()) return result;
            try
            {
                var query = new
                {
                    from = new
                    {
                        id = new string[] { wwiseObject.ID }
                    },
                    transform = new object[] {
                        new
                        {
                            select = new string[]
                            {
                                "ancestors"
                            }
                        }
                    }
                };

                var options = new
                {

                    @return = new string[] { "name", "id", "type" }

                };

                var func = WaapiFunction.CoreObjectGet;

                var jresult = await _client.Call(func, query, options, TimeOut);
                if (jresult == null || jresult["return"] == null) return result;
                foreach (var obj in jresult["return"]!)
                {
                    string? name = obj["name"]?.ToString();
                    string? id = obj["id"]?.ToString();
                    string? type = obj["type"]?.ToString();

                    if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type)) 
                        result.Add(new WwiseObject(name, id, type));
                }


            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children of {wwiseObject.Name} ======> {e.Message}");
            }

            return result;
        }

        public async Task<List<WwiseObject>> GetWwiseObjectDescendantsAsync(WwiseObject? wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();
            if (wwiseObject is null || !await TryConnectWaapiAsync()) return result;
            try
            {
                var query = new
                {
                    from = new
                    {
                        id = new string[] { wwiseObject.ID }
                    },
                    transform = new object[] {
                        new
                        {
                            select = new string[]
                            {
                                "descendants"
                            }
                        }
                    }
                };

                var options = new
                {

                    @return = new string[] { "name", "id", "type" }

                };

                var func = WaapiFunction.CoreObjectGet;

                var jresult = await _client.Call(func, query, options, TimeOut);
                if (jresult == null || jresult["return"] == null) return result;
                foreach (var obj in jresult["return"]!)
                {
                    string? name = obj["name"]?.ToString();
                    string? id = obj["id"]?.ToString();
                    string? type = obj["type"]?.ToString();

                    if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type)) 
                        result.Add(new WwiseObject(name, id, type));
                }


            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children of {wwiseObject.Name} ======> {e.Message}");
            }

            return result;
        }

        public async Task<bool> GenerateSelectedSoundBanksAsync(string[] soundBanks)
        {
            return await GenerateSelectedSoundBanksAsync(soundBanks, new string[] { }, new string[] { });
        }

        public async Task<bool> GenerateSelectedSoundBanksAsync(string[] soundBanks, string[] platforms, string[] languages)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {
                if (platforms.Length == 0)
                {
                    platforms = (await GetPlatformsAsync()).ToArray();
                }

                if (languages.Length == 0)
                {
                    languages = (await GetLanguagesAsync()).ToArray();
                }
                
                var query = new
                {
                    soundbanks = new List<object>(),
                    platforms = platforms,
                    languages = languages,
                    writeToDisk = true
                };

                foreach (var soundbank in soundBanks)
                {
                    query.soundbanks.Add(new { name = soundbank });
                }

                var func = Function.Verify("ak.wwise.core.soundbank.generate");

                await _client.Call(func, query, null, TimeOut);


                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to generate sound bank! ======> {e.Message}");
            }

            return false;
        }

        private async Task<bool> GetFunctionsAsync()
        {
            if (!await TryConnectWaapiAsync()) return false;
            
            Function.Clear();
            
            try
            {
                
                var result = await _client.Call("ak.wwise.waapi.getFunctions", null, null, TimeOut);
                if (result["functions"] == null) throw new Exception();
                foreach (var func in result["functions"]!)
                {
                    Function.AddFunction(func.ToString());
                }

                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog(e);
            }

            return false;
        }

        private async Task<bool> GetTopicsAsync()
        {
            if (!await TryConnectWaapiAsync()) return false;
            
            Topic.Clear();
            
            try
            {
                var result = await _client.Call("ak.wwise.waapi.getTopics", null, null, TimeOut);
                if (result["topics"] == null) throw new Exception();
                foreach (var topic in result["topics"]!)
                {
                    Topic.AddTopic(topic.ToString());
                }

                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog(e);
            }

            return false;
        }

        /// <summary>
        /// 保存工程
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveWwiseProjectAsync()
        {
            if (!await TryConnectWaapiAsync()) return false;
            try
            {
                var func = Function.Verify("ak.wwise.core.project.save");

                await _client.Call(func, null, null, TimeOut);
                WaapiLog.InternalLog("Project saved successfully!");

                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to save project! =======> {e.Message}");
            }

            return false;
        }
        
        
        public async Task BeginUndoGroup()
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return;

            var func = Function.Verify("ak.wwise.core.undo.beginGroup");

            var res = await CallAsync(func, null, null);
        }
        
        public async Task CancelUndoGroup()
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return;

            var func = Function.Verify("ak.wwise.core.undo.cancelGroup");

            var res = await CallAsync(func, null, null);
        }
        
        public async Task EndUndoGroup(string undoGroupName)
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return;

            var func = Function.Verify("ak.wwise.core.undo.endGroup");

            var arg = new
            {
                displayName = undoGroupName
            };
            
            await CallAsync(func, arg, null);
        }
    }
}
