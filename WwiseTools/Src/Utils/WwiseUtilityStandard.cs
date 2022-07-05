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
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.References;
using WwiseTools.Src.Models.SoundBank;

namespace WwiseTools.Utils
{
    public  partial class WwiseUtility
    {
        /// <summary>
        /// 获取属性、引用名称，异步执行
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <returns></returns>
        public async Task<string> GetPropertyAndReferenceNamesAsync(WwiseObject wwiseObject)
        {
            //ak.wwise.core.object.getPropertyAndReferenceNames

            if (!await TryConnectWaapiAsync() || wwiseObject == null) return "";

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
        /// 设置物体的引用，异步执行
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="wwiseReference"></param>
        /// <returns></returns>
        public async Task<bool> SetObjectReferenceAsync(WwiseObject wwiseObject, WwiseReference wwiseReference)
        {
            if (!await TryConnectWaapiAsync() || wwiseObject == null || wwiseReference == null) return false;

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
        
        /// <summary>
        /// 设置参数，异步执行
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> SetObjectPropertyAsync(WwiseObject wwiseObject, WwiseProperty wwiseProperty)
        {
            if (!await TryConnectWaapiAsync() || wwiseObject == null || wwiseProperty == null) return false;

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
        /// 修改名称，异步执行
        /// </summary>
        /// <param name="renameObject"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public async Task<bool> ChangeObjectNameAsync(WwiseObject renameObject, string newName)
        {
            if (!await TryConnectWaapiAsync() || renameObject == null || String.IsNullOrEmpty(newName)) return false;

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
        /// 将物体移动至指定父物体，异步执行
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public async Task<bool> CopyToParentAsync(WwiseObject child, WwiseObject parent)
        {
            if (!await TryConnectWaapiAsync() || child == null || parent == null) return false;

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
                        new JProperty("onNameConflict", "rename")
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
        /// 将物体复制至指定父物体，异步执行
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public async Task<bool> MoveToParentAsync(WwiseObject child, WwiseObject parent)
        {
            if (!await TryConnectWaapiAsync() || child == null || parent == null) return false;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.move");
                // 移动物体
                await _client.Call(func,
                    new JObject
                    {
                        new JProperty("object", child.ID),
                        new JProperty("parent", parent.ID),
                        new JProperty("onNameConflict", "rename")
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
        

        public async Task<bool> SetNoteAsync(WwiseObject target, string note)
        {
            if (!await TryConnectWaapiAsync() || target == null) return false;

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
        public async Task<WwiseObject> CreatePlayEventAsync(string eventName, string objectPath, string parentPath = @"\Events\Default Work Unit")
        {
            if (!await TryConnectWaapiAsync()) return null;
            return await AddEventActionAsync(eventName, objectPath, parentPath);
        }
        

        /// <summary>
        /// 生成播放事件，异步执行
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="objectPath"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public async Task<WwiseObject> AddEventActionAsync(string eventName, string objectPath, string parentPath = @"\Events\Default Work Unit", int actionType = 1)
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
                            new JProperty("onNameConflict", "merge"),
                            new JProperty("children", new JArray
                            {
                                new JObject
                                {
                                    new JProperty("name", ""),
                                    new JProperty("type", "Action"),
                                    new JProperty("@ActionType", actionType),
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


        public async Task<List<SoundBankInclusion>> GetSoundBankInclusion(WwiseObject soundBank)
        {
            var result = new List<SoundBankInclusion>();
            if (!await TryConnectWaapiAsync()) return result;

            try
            {
                var func = Function.Verify("ak.wwise.core.soundbank.getInclusions");
                var args = new
                {
                    soundbank = soundBank.ID
                };

                var jresult = await _client.Call(func, args, null, TimeOut);
                if (jresult == null || jresult["inclusions"] == null) return result;
                foreach (var inclusion in jresult["inclusions"])
                {
                    var id = inclusion["object"]?.ToString();
                    if (string.IsNullOrEmpty(id)) continue;

                    var filter = inclusion["filter"]?.ToString();
                    if (filter == null) continue;

                    var soundBankInclusion = new SoundBankInclusion();
                    soundBankInclusion.Object = await WwiseUtility.Instance.GetWwiseObjectByIDAsync(id);

                    if (filter.Contains("events")) soundBankInclusion.Filter |= SoundBankInclusionFilter.Events;
                    if (filter.Contains("structures")) soundBankInclusion.Filter |= SoundBankInclusionFilter.Structures;
                    if (filter.Contains("media")) soundBankInclusion.Filter |= SoundBankInclusionFilter.Media;


                    result.Add(soundBankInclusion);
                }
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to retrieve soundbank inclusions! ======> {e.Message}");
            }

            return result;
        }

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

        public async Task<WwiseObject> CreateObjectAsync(string objectName, WwiseObject.ObjectType objectType, WwiseObject parent)
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
                        new JProperty("onNameConflict", "fail")
                    },
                    null
                );

                WaapiLog.InternalLog($"Object {objectName} created successfully!");
                if (result["id"] == null) throw new Exception();
                return await GetWwiseObjectByIDAsync(result["id"].ToString());
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to create object : {objectName}! ======> {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 创建物体，异步执行
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="objectType"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public async Task<WwiseObject> CreateObjectAtPathAsync(string objectName, WwiseObject.ObjectType objectType, string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit")
        {
            if (!await TryConnectWaapiAsync()) return null;

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
                        new JProperty("parent", parentPath),
                        new JProperty("onNameConflict", "fail")
                    },
                    null
                    );

                WaapiLog.InternalLog($"Object {objectName} created successfully!");
                if (result["id"] == null) throw new Exception();
                return await GetWwiseObjectByIDAsync(result["id"].ToString());
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to create object : {objectName}! ======> {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 创建物体，异步执行
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="objectType"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        [Obsolete("Use CreateObjectAtPathAsync instead")]
        public async Task<WwiseObject> CreateObjectAsync(string objectName, WwiseObject.ObjectType objectType, string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit")
        {
            if (!await TryConnectWaapiAsync()) return null;

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
                        new JProperty("parent", parentPath),
                        new JProperty("onNameConflict", "fail")
                    },
                    null
                );

                WaapiLog.InternalLog($"Object {objectName} created successfully!");
                if (result["id"] == null) throw new Exception();
                return await GetWwiseObjectByIDAsync(result["id"].ToString());
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to create object : {objectName}! ======> {e.Message}");
                return null;
            }
        }



        public async Task<bool> DeleteObjectAsync(WwiseObject wwiseObject)
        {
            return await DeleteObjectAsync(await wwiseObject.GetPathAsync());
        }

        public async Task<bool> DeleteObjectAsync(string path)
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
                        new JProperty("object", path)
                    },
                    null
                    );

                WaapiLog.InternalLog($"Object {path} deleted successfully!");
                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to delete object : {path}! ======> {e.Message}");
                
            }

            return false;
        }

        
        /// <summary>
        /// 通过ID搜索物体，异步执行
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        public async Task<WwiseObject> GetWwiseObjectByIDAsync(string targetId)
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
                string name = jresult["return"].Last["name"]?.ToString();
                string id = jresult["return"].Last["id"]?.ToString();
                string type = jresult["return"].Last["type"]?.ToString();

                WaapiLog.InternalLog($"WwiseObject {name} successfully fetched!");

                return new WwiseObject(name, id, type);
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject from ID : {targetId}! ======> {e.Message}");
                return null;
            }

        }

        public async Task<JToken> GetWwiseObjectPropertyAsync(string targetId, string wwiseProperty)
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
                return jresult["return"].Last?["@" + wwiseProperty];
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject Property : {targetId}! ======> {e.Message}");
                return null;
            }

        }

        public async Task<WwiseProperty> GetWwiseObjectPropertyAsync(WwiseObject wwiseObject, string wwiseProperty)
        {
            if (!await TryConnectWaapiAsync() || wwiseObject == null) return null;

            try
            {
                var func = WaapiFunction.CoreObjectGet;

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

                    @return = new string[] { "@" + wwiseProperty }

                };

                JObject jresult = await _client.Call(func, query, options, TimeOut);


                if (jresult["return"] == null) throw new Exception();

                WaapiLog.InternalLog($"WwiseProperty {wwiseProperty} successfully fetched!");
                var r = jresult["return"].Last?["@" + wwiseProperty];
                if (r == null) return null;
                return new WwiseProperty(wwiseProperty, r.ToString());
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject Property : {wwiseObject.Name}! ======> {e.Message}");
                return null;
            }

        }


        public async Task<string> GetWwiseObjectPathAsync(string ID)
        {
            if (!await TryConnectWaapiAsync()) return null;

            try
            {
                var func = WaapiFunction.CoreObjectGet;

                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        id = new string[] { ID }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "path" }

                };


                JObject jresult = await WwiseUtility.Instance._client.Call(func, query, options, TimeOut);
                if (jresult["return"] == null || jresult["return"].Last == null || 
                    jresult["return"].Last["path"] == null) throw new Exception();
                string path = jresult["return"].Last["path"].ToString();

                return path;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get path of object : {ID}! =======> {e.Message}");
                return null;
            }

        }
        
        /// <summary>
        /// 通过名称搜索唯一命名对象，异步执行，格式必须为"type:name"
        /// </summary>
        /// <param name="targetName"></param>
        /// <returns></returns>
        public async Task<WwiseObject> GetWwiseObjectByNameAsync(string targetName)
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

                string name = obj["name"]?.ToString();
                string id = obj["id"]?.ToString();
                string type = obj["type"]?.ToString();

                WaapiLog.InternalLog($"WwiseObject {name} successfully fetched!");

                return new WwiseObject(name, id, type);
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject by name : {targetName}! ======> {e.Message}");
                return null;
            }

        }
        
        /// <summary>
        /// 通过路径获取对象，异步执行
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<WwiseObject> GetWwiseObjectByPathAsync(string path)
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

                string name = obj["name"]?.ToString();
                string id = obj["id"]?.ToString();
                string type = obj["type"]?.ToString();

                WaapiLog.InternalLog($"WwiseObject {name} successfully fetched!");

                return new WwiseObject(name, id, type);
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject by path : {path}! ======> {e.Message}");
                return null;
            }

        }
        
        /// <summary>
        /// 获取指定类型的对象，异步执行
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
                foreach (var obj in jresult["return"])
                {
                    string name = obj["name"]?.ToString();
                    string id = obj["id"]?.ToString();
                    string type = obj["type"]?.ToString();

                    result.Add(new WwiseObject(name, id, type));
                }



                WaapiLog.InternalLog($"WwiseObject list or type {targetType} successfully fetched!");

                
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
                    string name = obj["name"]?.ToString();
                    string id = obj["id"]?.ToString();
                    string type = obj["type"]?.ToString();

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
                foreach (var r in result["return"])
                {
                    string name = r["name"]?.ToString();
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
        /// 从指定路径导入音频，异步执行
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="language"></param>
        /// <param name="subFolder"></param>
        /// <param name="parentPath"></param>
        /// <param name="work_unit"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        public async Task<WwiseObject> ImportSoundAsync(string filePath, string language = "SFX", string subFolder = "", string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit", string soundName = "") // Async版本
        {
            if (!filePath.EndsWith(".wav") || !await TryConnectWaapiAsync()) return null; // 目标不是文件或者没有成功连接时返回空的WwiseObject



            string fileName;
            if (!string.IsNullOrEmpty(soundName))
            {
                fileName = soundName;
            }
            else
            {
                try
                {
                    fileName = Path.GetFileName(filePath).Replace(".wav", ""); // 尝试获取文件名
                }
                catch (IOException e)
                {
                    WaapiLog.InternalLog($"Failed to get file name from {filePath} ======> {e.Message}");
                    return null;
                }
            }

            try
            {
                var importQ = new JObject // 导入配置
                {
                    new JProperty("importOperation", ImportSettings.ToString()),

                    new JProperty("default", new JObject
                    {
                        new JProperty("importLanguage", language),

                    }),
                    new JProperty("imports", new JArray
                    {
                        new JObject
                        {
                            new JProperty("audioFile", filePath),
                            new JProperty("objectPath", $"{parentPath}\\<Sound>{fileName}")
                        }
                    })
                };

                if (!String.IsNullOrEmpty(subFolder))
                {
                    (importQ["default"] as JObject)?.Add(new JProperty("originalsSubFolder", subFolder));
                }

                var options = new JObject(new JProperty("return", new object[] { "name", "id", "type", "path" })); // 设置返回参数

                var func = Function.Verify("ak.wwise.core.audio.import");

                var result = await _client.Call(func, importQ, options); // 执行导入

                if (result == null || result["objects"] == null || 
                    result["objects"].Last == null || result["objects"].Last["id"] == null) return null;

                WaapiLog.InternalLog($"File {filePath} imported successfully!");

                return await GetWwiseObjectByIDAsync(result["objects"].Last["id"].ToString());
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to import file : {filePath} ======> {e.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// 获取工作单元文件路径，异步执行
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public async Task<string> GetWorkUnitFilePathAsync(WwiseObject @object)
        {
            if (!await TryConnectWaapiAsync() || @object == null) return null;

            try
            {
                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        id = new string[] { @object.ID }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "filePath" }

                };

                var func = WaapiFunction.CoreObjectGet;

                JObject jresult = await _client.Call(func, query, options, TimeOut);

                string filePath = "";
                if (jresult["return"] == null) throw new Exception();
                foreach (var obj in jresult["return"])
                {
                    filePath = obj["filePath"]?.ToString();
                }

                WaapiLog.InternalLog($"Work Unit file path of object {@object.Name} successfully fetched!");

                return filePath;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return Work Unit file path of object : {@object.Name}! ======> {e.Message}");
                return null;
            }
        }
        
        public async Task<bool> ReloadWwiseProjectAsync()
        {
            await LoadWwiseProjectAsync(await GetWwiseProjectPathAsync(), true);
            await DisconnectAsync();
            _client = null;
            return await ConnectAsync();
        }
        
        /// <summary>
        /// 加载工程，异步执行
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
        /// 获取工程路径，异步执行
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWwiseProjectNameAsync()
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

                string name = "";
                if (jresult["return"] == null) throw new Exception();
                foreach (var obj in jresult["return"])
                {
                    name = obj["name"]?.ToString();
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
        /// 获取工程路径，异步执行
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWwiseProjectPathAsync()
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

                string filePath = "";
                if (jresult["return"] == null) throw new Exception();
                foreach (var obj in jresult["return"])
                {
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

        public async Task<WwiseInfo> GetWwiseInfoAsync()
        {
            if (!await TryConnectWaapiAsync()) return null;

            try
            {
                var func = Function.Verify("ak.wwise.core.getInfo");

                JObject result = await _client.Call(func, null, null);
                if (result["version"] == null) throw new Exception("Failed to fetch version info!");
                int.TryParse(result["version"]["major"]?.ToString(), out int major);
                int.TryParse(result["version"]["minor"]?.ToString(), out int minor);
                int.TryParse(result["version"]["build"]?.ToString(), out int build);
                int.TryParse(result["version"]["year"]?.ToString(), out int year);
                int.TryParse(result["version"]["schema"]?.ToString(), out int schema);
                int.TryParse(result["processId"]?.ToString(), out int processId);
                bool.TryParse(result["isCommandLine"]?.ToString(), out bool isCommandLine);

                WwiseInfo wwiseInfo = new WwiseInfo()
                {
                    Version = new WwiseVersion(year, major, minor, build, schema),
                    ProcessID = processId,
                    IsCommandLine = isCommandLine
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
        public async Task<bool> ExecuteUICommandAsync(string command, string[] objectIDs = null)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {
                var func = Function.Verify("ak.wwise.ui.commands.execute");

                if (objectIDs != null)
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

        public async Task<List<WwiseObject>> GetReferencesToWwiseObjectAsync(WwiseObject wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();
            if (wwiseObject == null || !await TryConnectWaapiAsync()) return result;
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
                foreach (var obj in jresult["return"])
                {
                    string name = obj["name"]?.ToString();
                    string id = obj["id"]?.ToString();
                    string type = obj["type"]?.ToString();

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
            if (wwiseObjects == null || !await TryConnectWaapiAsync()) return result;
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
                foreach (var obj in jresult["return"])
                {
                    string name = obj["name"]?.ToString();
                    string id = obj["id"]?.ToString();
                    string type = obj["type"]?.ToString();

                    result.Add(new WwiseObject(name, id, type));
                }


            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children ======> {e.Message}");
            }

            return result;
        }

        public async Task<WwiseObject> GetWwiseObjectParentAsync(WwiseObject wwiseObject)
        {
            if (wwiseObject == null || !await TryConnectWaapiAsync()) return null;
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
                foreach (var obj in jresult["return"])
                {
                    string name = obj["name"]?.ToString();
                    string id = obj["id"]?.ToString();
                    string type = obj["type"]?.ToString();

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
            if (wwiseObjects == null || !await TryConnectWaapiAsync()) return result;
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
                foreach (var obj in jresult["return"])
                {
                    string name = obj["name"]?.ToString();
                    string id = obj["id"]?.ToString();
                    string type = obj["type"]?.ToString();

                    result.Add(new WwiseObject(name, id, type));
                }


            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children ======> {e.Message}");
            }

            return result;
        }

        public async Task<List<WwiseObject>> GetWwiseObjectChildrenAsync(WwiseObject wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();
            if (wwiseObject == null || !await TryConnectWaapiAsync()) return result;
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
                foreach (var obj in jresult["return"])
                {
                    string name = obj["name"]?.ToString();
                    string id = obj["id"]?.ToString();
                    string type = obj["type"]?.ToString();

                    result.Add(new WwiseObject(name, id, type));
                }


            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children of {wwiseObject.Name} ======> {e.Message}");
            }

            return result;
        }

        public async Task<List<WwiseObject>> GetWwiseObjectAncestorsAsync(WwiseObject wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();
            if (wwiseObject == null || !await TryConnectWaapiAsync()) return result;
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
                foreach (var obj in jresult["return"])
                {
                    string name = obj["name"]?.ToString();
                    string id = obj["id"]?.ToString();
                    string type = obj["type"]?.ToString();

                    result.Add(new WwiseObject(name, id, type));
                }


            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children of {wwiseObject.Name} ======> {e.Message}");
            }

            return result;
        }

        public async Task<List<WwiseObject>> GetWwiseObjectDescendantsAsync(WwiseObject wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();
            if (wwiseObject == null || !await TryConnectWaapiAsync()) return result;
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
                foreach (var obj in jresult["return"])
                {
                    string name = obj["name"]?.ToString();
                    string id = obj["id"]?.ToString();
                    string type = obj["type"]?.ToString();

                    result.Add(new WwiseObject(name, id, type));
                }


            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children of {wwiseObject.Name} ======> {e.Message}");
            }

            return result;
        }

        public async Task<bool> GenerateSelectedSoundBanksAllPlatformAsync(string[] soundBanks)
        {
            if (!await TryConnectWaapiAsync() || soundBanks == null) return false;

            try
            {
                var query = new
                {
                    soundbanks = new List<object>(),
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
            if (!await TryConnectWaapiAsync() || Function != null) return false;
            Function = new WaapiFunction();
            try
            {
                var result = await _client.Call("ak.wwise.waapi.getFunctions", null, null, TimeOut);
                if (result["functions"] == null) throw new Exception();
                foreach (var func in result["functions"])
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
            Topic = new WaapiTopic();
            try
            {
                var result = await _client.Call("ak.wwise.waapi.getTopics", null, null, TimeOut);
                if (result["topics"] == null) throw new Exception();
                foreach (var topic in result["topics"])
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
        /// 保存工程，异步执行
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
    }
}
