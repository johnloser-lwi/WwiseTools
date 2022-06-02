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
using WwiseTools.Utils.Feature2021;

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
                var result = await Client.Call(func,

                    new JObject(

                        new JProperty("object", wwiseObject.ID)),

                    null);
                WaapiLog.Log("Property and References fetched successfully!");
                return result.ToString();


            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to fetch Property and References! ======> {e.Message}");
                return "";
            }
        }
        

        /// <summary>
        /// 设置物体的引用，异步执行
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="wwiseReference"></param>
        /// <returns></returns>
        public async Task SetObjectReferenceAsync(WwiseObject wwiseObject, WwiseReference wwiseReference)
        {
            if (!await TryConnectWaapiAsync() || wwiseObject == null || wwiseReference == null) return;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.setReference");
                await Client.Call(func,

                    new JObject(

                        new JProperty("object", wwiseObject.ID),

                        new JProperty("reference", wwiseReference.Name),

                        new JProperty("value", wwiseReference.Object.ID)),

                    null);

                WaapiLog.Log("Reference set successfully!");
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to set reference \"{wwiseReference.Name}\" to object {wwiseObject.Name} ======> {e.Message}");
            }
        }
        
        /// <summary>
        /// 设置参数，异步执行
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetObjectPropertyAsync(WwiseObject wwiseObject, WwiseProperty wwiseProperty)
        {
            if (!await TryConnectWaapiAsync() || wwiseObject == null || wwiseProperty == null) return;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.setProperty");
                await Client.Call(func,

                    new JObject(

                        new JProperty("property", wwiseProperty.Name),

                        new JProperty("object", wwiseObject.ID),

                        new JProperty("value", wwiseProperty.Value)),

                    null);

                WaapiLog.Log($"Property {wwiseProperty.Name} successfully changed to {wwiseProperty.Value}!");
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to set property \"{wwiseProperty.Name}\" of object {wwiseObject.Name} ======> {e.Message}");
            }
        }
        

        /// <summary>
        /// 修改名称，异步执行
        /// </summary>
        /// <param name="renameObject"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public async Task ChangeObjectNameAsync(WwiseObject renameObject, string newName)
        {
            if (!await TryConnectWaapiAsync() || renameObject == null || String.IsNullOrEmpty(newName)) return;

            string oldName = renameObject.Name;
            try
            {
                var func = Function.Verify("ak.wwise.core.object.setName");
                await Client.Call(func
                    ,
                    new
                    {
                        @object = renameObject.ID,
                        value = newName,
                    }, null, TimeOut);

                renameObject.Name = newName;

                WaapiLog.Log($"Object {oldName} successfully renamed to {newName}!");
            }

            catch (Exception e)
            {
                WaapiLog.Log($"Failed to rename object : {oldName} ======> {e.Message}");
            }
        }
        
        /// <summary>
        /// 将物体移动至指定父物体，后台进行
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public async Task CopyToParentAsync(WwiseObject child, WwiseObject parent)
        {
            if (!await TryConnectWaapiAsync() || child == null || parent == null) return;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.copy");
                // 移动物体
                await Client.Call(func
                    ,
                    new JObject
                    {
                        new JProperty("object", child.ID),
                        new JProperty("parent", parent.ID),
                        new JProperty("onNameConflict", "rename")
                    }
                    ,null, TimeOut);


                WaapiLog.Log($"Copied {child.Name} to {parent.Name}!");

            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to copy {child.Name} to {parent.Name}! ======> {e.Message}");
            }

            //return null;
        }

        

        /// <summary>
        /// 将物体复制至指定父物体，后台进行
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public async Task MoveToParentAsync(WwiseObject child, WwiseObject parent)
        {
            if (!await TryConnectWaapiAsync() || child == null || parent == null) return;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.move");
                // 移动物体
                await Client.Call(func,
                    new JObject
                    {
                        new JProperty("object", child.ID),
                        new JProperty("parent", parent.ID),
                        new JProperty("onNameConflict", "rename")
                    },
                    null,
                    TimeOut
                    );

                WaapiLog.Log($"Moved {child.Name} to {parent.Name}!");
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to move {child.Name} to {parent.Name}! ======> {e.Message}");
            }
        }
        

        public async Task SetNoteAsync(WwiseObject target, string note)
        {
            if (!await TryConnectWaapiAsync() || target == null) return;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.setNotes");
                // 移动物体
                await Client.Call(func,
                    new JObject
                    {
                        new JProperty("object", target.ID),
                        new JProperty("value", note)
                    },
                    null,
                    TimeOut
                    );

                WaapiLog.Log($"Successfully set {target.Name} note to \"{note}\"!");
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to set note for {target.Name}! ======> {e.Message}");
            }
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
                var result = await Client.Call
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

                WaapiLog.Log($"Event {eventName} created successfully!");
                if (result["id"] == null) throw new Exception();
                return await GetWwiseObjectByIDAsync(result["id"].ToString());
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to created play event : {eventName}! ======> {e.Message} ");
                return null;
            }
        }

        public async Task AddEventToBankAsync(WwiseObject soundBank, string eventId)
        {
            if (!await TryConnectWaapiAsync()) return;

            try
            {

                var func = Function.Verify("ak.wwise.core.soundbank.setInclusions");
                await Client.Call
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
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to Add Event to Bank ======> {e.Message}");
            }
        }
        

        /// <summary>
        /// 创建物体，后台进行
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="objectType"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public async Task<WwiseObject> CreateObjectAsync(string objectName, WwiseObject.ObjectType objectType, string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit")
        {
            if (!await TryConnectWaapiAsync()) return null;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.create");

                // 创建物体
                var result = await Client.Call
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

                WaapiLog.Log($"Object {objectName} created successfully!");
                if (result["id"] == null) throw new Exception();
                return await GetWwiseObjectByIDAsync(result["id"].ToString());
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to create object : {objectName}! ======> {e.Message}");
                return null;
            }
        }

        public async Task DeleteObjectAsync(WwiseObject wwiseObject)
        {
            await DeleteObjectAsync(await wwiseObject.GetPathAsync());
        }

        public async Task DeleteObjectAsync(string path)
        {
            if (!await TryConnectWaapiAsync()) return;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.delete");

                // 创建物体
                var result = await Client.Call
                    (
                        func,
                    new JObject
                    {
                        new JProperty("object", path)
                    },
                    null
                    );

                WaapiLog.Log($"Object {path} deleted successfully!");
                return;
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to delete object : {path}! ======> {e.Message}");
                return;
            }
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

                JObject jresult = await Client.Call(func, query, options, TimeOut);
                if (jresult["return"]?.Last == null) throw new Exception();
                string name = jresult["return"].Last["name"]?.ToString();
                string id = jresult["return"].Last["id"]?.ToString();
                string type = jresult["return"].Last["type"]?.ToString();

                WaapiLog.Log($"WwiseObject {name} successfully fetched!");

                return new WwiseObject(name, id, type);
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return WwiseObject from ID : {targetId}! ======> {e.Message}");
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

                JObject jresult = await Client.Call(func, query, options, TimeOut);


                if (jresult["return"] == null) throw new Exception();

                WaapiLog.Log($"WwiseProperty {wwiseProperty} successfully fetched!");
                var r = jresult["return"].Last?["@" + wwiseProperty];
                if (r == null) return null;
                return new WwiseProperty(wwiseProperty, r.ToString());
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return WwiseObject Property : {wwiseObject.Name}! ======> {e.Message}");
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


                JObject jresult = await WwiseUtility.Instance.Client.Call(func, query, options, TimeOut);
                if (jresult["return"] == null || jresult["return"].Last == null || 
                    jresult["return"].Last["path"] == null) throw new Exception();
                string path = jresult["return"].Last["path"].ToString();

                return path;
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to get path of object : {ID}! =======> {e.Message}");
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



                JObject jresult = await Client.Call(func, query, options, TimeOut);
                var obj = jresult["return"]?.Last;
                if (obj == null) throw new Exception("Object not found!");

                string name = obj["name"]?.ToString();
                string id = obj["id"]?.ToString();
                string type = obj["type"]?.ToString();

                WaapiLog.Log($"WwiseObject {name} successfully fetched!");

                return new WwiseObject(name, id, type);
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return WwiseObject by name : {targetName}! ======> {e.Message}");
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

                JObject jresult = await Client.Call(func, query, options, TimeOut);
                var obj = jresult["return"]?.Last;
                if (obj == null) throw new Exception("Object not found!");

                string name = obj["name"]?.ToString();
                string id = obj["id"]?.ToString();
                string type = obj["type"]?.ToString();

                WaapiLog.Log($"WwiseObject {name} successfully fetched!");

                return new WwiseObject(name, id, type);
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return WwiseObject by path : {path}! ======> {e.Message}");
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
            if (!await TryConnectWaapiAsync() || String.IsNullOrWhiteSpace(targetType)) return null;

            if (ConnectionInfo.Version >= VersionHelper.V2021_1_0_7575)
            {
                try
                {
                    var waql = new Waql($"where type = \"{targetType}\"");
                    if (await waql.RunAsync())
                    {
                        return waql.Result;
                    }
                    else
                    {
                        throw new Exception("waql failed");
                    }
                }
                catch (Exception e)
                {
                    WaapiLog.Log($"Failed to return WwiseObject list of type {targetType} ======> {e.Message}");
                    return null;
                }
            }

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

                JObject jresult = await Client.Call(func, query, options, TimeOut);

                List<WwiseObject> objList = new List<WwiseObject>();
                if (jresult["return"] == null) throw new Exception();
                foreach (var obj in jresult["return"])
                {
                    string name = obj["name"]?.ToString();
                    string id = obj["id"]?.ToString();
                    string type = obj["type"]?.ToString();

                    objList.Add(new WwiseObject(name, id, type));
                }



                WaapiLog.Log($"WwiseObject list or type {targetType} successfully fetched!");

                return objList;
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return WwiseObject list of type : {targetType}! ======> {e.Message}");
                return null;
            }
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

                JObject jresult = await Client.Call(func, null, options, TimeOut);

                


                if (jresult["objects"] == null) throw new Exception();
                foreach (var obj in jresult["objects"])
                {
                    string name = obj["name"]?.ToString();
                    string id = obj["id"]?.ToString();
                    string type = obj["type"]?.ToString();

                    result.Add(new WwiseObject(name, id, type));
                }

                WaapiLog.Log($"Selected WwiseObject list successfully fetched!");

               
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return Selected WwiseObject list! ======> {e.Message}");
            }

            return result;
        }
        
        public async Task<List<string>> GetLanguagesAsync()
        {
            if (!await TryConnectWaapiAsync()) return null;

            List<string> resultList = new List<string>();

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

                var result = await Client.Call(func, query, options, TimeOut);
                if (result["return"] == null) throw new Exception();
                foreach (var r in result["return"])
                {
                    string name = r["name"]?.ToString();
                    var ignoreList = new string[] { "Mixed", "SFX", "External", "SoundSeed Grain" };
                    if (!ignoreList.Contains(name))
                        resultList.Add(name);
                }

                WaapiLog.Log($"Language list fetched successfully!");

            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return language list! ======> {e.Message}");
            }

            return resultList;
        }
        
        
        /// <summary>
        /// 从指定路径导入音频，后台进行
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
                    WaapiLog.Log($"Failed to get file name from {filePath} ======> {e.Message}");
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

                var result = await Client.Call(func, importQ, options); // 执行导入

                if (result == null || result["objects"] == null || 
                    result["objects"].Last == null || result["objects"].Last["id"] == null) return null;

                WaapiLog.Log($"File {filePath} imported successfully!");

                return await GetWwiseObjectByIDAsync(result["objects"].Last["id"].ToString());
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to import file : {filePath} ======> {e.Message}");
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

                JObject jresult = await Client.Call(func, query, options, TimeOut);

                string filePath = "";
                if (jresult["return"] == null) throw new Exception();
                foreach (var obj in jresult["return"])
                {
                    filePath = obj["filePath"]?.ToString();
                }

                WaapiLog.Log($"Work Unit file path of object {@object.Name} successfully fetched!");

                return filePath;
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return Work Unit file path of object : {@object.Name}! ======> {e.Message}");
                return null;
            }
        }
        
        public async Task ReloadWwiseProjectAsync()
        {
            await LoadWwiseProjectAsync(await GetWwiseProjectPathAsync(), true);
            await DisconnectAsync();
            Client = null;
            await ConnectAsync();
        }
        
        /// <summary>
        /// 加载工程，异步执行
        /// </summary>
        /// <param name="path"></param>
        /// <param name="saveCurrent"></param>
        /// <returns></returns>
        public async Task LoadWwiseProjectAsync(string path, bool saveCurrent = true)
        {
            if (!await TryConnectWaapiAsync()) return;

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
                await Client.Call(func, query, null, TimeOut);

                WaapiLog.Log("Project loaded successfully!");
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to load project! =======> {e.Message}");
            }
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

                JObject jresult = await Client.Call(func, query, options, TimeOut);

                string name = "";
                if (jresult["return"] == null) throw new Exception();
                foreach (var obj in jresult["return"])
                {
                    name = obj["name"]?.ToString();
                }

                WaapiLog.Log($"Project name successfully fetched!");

                return name;
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return project name! ======> {e.Message}");
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

                JObject jresult = await Client.Call(func, query, options, TimeOut);

                string filePath = "";
                if (jresult["return"] == null) throw new Exception();
                foreach (var obj in jresult["return"])
                {
                    filePath = obj["filePath"]?.ToString();
                }

                WaapiLog.Log($"Project path successfully fetched!");

                return filePath;
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return project path! ======> {e.Message}");
                return null;
            }
        }

        public async Task<WwiseInfo> GetWwiseInfoAsync()
        {
            if (!await TryConnectWaapiAsync()) return null;

            try
            {
                var func = Function.Verify("ak.wwise.core.getInfo");

                JObject result = await Client.Call(func, null, null);
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
                WaapiLog.Log($"Failed to get Wwise info! ======> {e.Message}");
            }

            return null;
        }


        /// <summary>
        /// 支持command访问：https://www.audiokinetic.com/library/2019.2.14_7616/?source=SDK&id=globalcommandsids.html
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task ExecuteUICommandAsync(string command, string[] objectIDs = null)
        {
            if (!await TryConnectWaapiAsync()) return;

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

                    await Client.Call(func, query, null, TimeOut);
                }
                else
                {
                    var query = new { command = command };
                    await Client.Call(func, query, null, TimeOut);
                }

            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to execute command {command}! ======> {e.Message}");
            }
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

                var jresult = await Client.Call(func, query, options, TimeOut);
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
                WaapiLog.Log($"Failed to get references of {wwiseObject.Name} ======> {e.Message}");
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

                var jresult = await Client.Call(func, query, options, TimeOut);
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
                WaapiLog.Log($"Failed to get children ======> {e.Message}");
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

                var jresult = await Client.Call(func, query, options, TimeOut);
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
                WaapiLog.Log($"Failed to get children of {wwiseObject.Name} ======> {e.Message}");
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

                var jresult = await Client.Call(func, query, options, TimeOut);
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
                WaapiLog.Log($"Failed to get children ======> {e.Message}");
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

                var jresult = await Client.Call(func, query, options, TimeOut);
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
                WaapiLog.Log($"Failed to get children of {wwiseObject.Name} ======> {e.Message}");
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

                var jresult = await Client.Call(func, query, options, TimeOut);
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
                WaapiLog.Log($"Failed to get children of {wwiseObject.Name} ======> {e.Message}");
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

                var jresult = await Client.Call(func, query, options, TimeOut);
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
                WaapiLog.Log($"Failed to get children of {wwiseObject.Name} ======> {e.Message}");
            }

            return result;
        }

        public async Task GenerateSelectedSoundBanksAllPlatformAsync(string[] soundBanks)
        {
            if (!await TryConnectWaapiAsync() || soundBanks == null) return;

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

                await Client.Call(func, query, null, TimeOut);
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to generate sound bank! ======> {e.Message}");
            }
        }

        private async Task GetFunctionsAsync()
        {
            if (!await TryConnectWaapiAsync() || Function != null) return;
            Function = new WaapiFunction();
            try
            {
                var result = await Client.Call("ak.wwise.waapi.getFunctions", null, null, TimeOut);
                if (result["functions"] == null) throw new Exception();
                foreach (var func in result["functions"])
                {
                    Function.AddFunction(func.ToString());
                }
            }
            catch (Exception e)
            {
                WaapiLog.Log(e);
            }
        }


        /// <summary>
        /// 保存工程，异步执行
        /// </summary>
        /// <returns></returns>
        public async Task SaveWwiseProjectAsync()
        {
            if (!await TryConnectWaapiAsync()) return;
            try
            {
                var func = Function.Verify("ak.wwise.core.project.save");

                await Client.Call(func, null, null, TimeOut);
                WaapiLog.Log("Project saved successfully!");
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to save project! =======> {e.Message}");
            }
        }
    }
}
