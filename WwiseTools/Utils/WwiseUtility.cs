using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

using System.Xml;
using WaapiClient;
using WwiseTools.Properties;
using WwiseTools.References;
using WwiseTools.Objects;
using WwiseTools.Utils.Feature2021;

namespace WwiseTools.Utils
{
    /// <summary>
    /// 用于实现基础功能
    /// </summary>
    public partial class WwiseUtility
    {
        public static JsonClient Client { get; set; }

        public static WwiseInfo ConnectionInfo { get; private set; }

        internal static WaapiFunction Function { get; set; }

        public static WwiseUtility Extensions
        {
            get
            {
                if (_extensions == null) _extensions = new WwiseUtility();
                return _extensions;
            }
        }

        private static WwiseUtility _extensions;

        public enum GlobalImportSettings
        {
            useExisting,
            replaceExisting,
            createNew
        }

        public static GlobalImportSettings ImportSettings = GlobalImportSettings.useExisting;

        private WwiseUtility()
        {
        }

        /// <summary>
        /// 连接初始化
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use WwiseUtility.ConnectAsync() instead")]
        public static async ValueTask<bool> Init() // 初始化，返回连接状态
        {
            if (Client != null && Client.IsConnected()) return true;

            try
            {
                
                Client = new JsonClient();
                await Client.Connect(); // 尝试创建Wwise连接

                WaapiLog.Log("Connected successfully!");

                Client.Disconnected += () =>
                {
                    Client = null;
                    WaapiLog.Log("Connection closed!"); // 丢失连接提示
                };
                return true;
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to connect! ======> {e.Message}");
                return false;
            }
        }

        public static bool IsConnected()
        {
            return Client != null && Client.IsConnected();
        }
        
        public static async ValueTask<bool> ConnectAsync(int wampPort = 8080) // 初始化，返回连接状态
        {
            if (Client != null && Client.IsConnected()) return true;

            try
            {
                WaapiLog.Log("Initializing...");
                Client = new JsonClient();
                await Client.Connect($"ws://localhost:{wampPort}/waapi"); // 尝试创建Wwise连接
                await GetFunctionsAsync();
                WaapiLog.Log("Connected successfully!");

                Client.Disconnected += () =>
                {
                    Client = null;
                    ConnectionInfo = null;
                    WaapiLog.Log("Connection closed unexpectedly!"); // 丢失连接提示
                };

                ConnectionInfo = await GetWwiseInfoAsync();

                WaapiLog.Log(ConnectionInfo);
                return true;
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to connect! ======> {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use WwiseUtility.DisconnectAsync() instead")]
        public static async ValueTask Close()
        {
            if (Client == null || !Client.IsConnected()) return;

            try
            {
                await Client.Close(); // 尝试断开连接
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Error while closing! ======> {e.Message}");
            }
        }

        public static async ValueTask DisconnectAsync()
        {
            if (Client == null || !Client.IsConnected()) return;

            try
            {
                await Client.Close(); // 尝试断开连接
                Client = null;
                ConnectionInfo = null;
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Error while closing! ======> {e.Message}");
            }
        }

        /// <summary>
        /// 尝试连接并检查连接状态
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public static bool TryConnectWaapi() 
        {
            var connected = Init();
            connected.AsTask().Wait();

            return connected.Result && Client.IsConnected();
        }
        
        public static async ValueTask<bool> TryConnectWaapiAsync(int wampPort = 8080) 
        {
            var connected = await ConnectAsync(wampPort);

            return connected && Client.IsConnected();
        }

        public static string NewGUID()
        {
            return $"{{{Guid.NewGuid().ToString().ToUpper()}}}";
        }

        /// <summary>
        /// 获取属性、引用名称
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public static string GetPropertyAndReferenceNames(WwiseObject wwiseObject)
        {
            if (!TryConnectWaapi() || wwiseObject == null) return "";

            var get = GetPropertyAndReferenceNamesAsync(wwiseObject);
            get.AsTask().Wait();
            return get.Result;
        }

        /// <summary>
        /// 获取属性、引用名称，异步执行
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <returns></returns>
        public static async ValueTask<string> GetPropertyAndReferenceNamesAsync(WwiseObject wwiseObject)
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
        /// 设置物体的引用
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="wwiseReference"></param>
        [Obsolete("Use async version instead")]
        public static void SetObjectReference(WwiseObject wwiseObject, WwiseReference wwiseReference)
        {
            if (!TryConnectWaapi() || wwiseObject == null || wwiseReference == null) return;
            var setRef = SetObjectReferenceAsync(wwiseObject, wwiseReference);
            setRef.AsTask().Wait();
        }

        /// <summary>
        /// 设置物体的引用，异步执行
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="wwiseReference"></param>
        /// <returns></returns>
        public static async ValueTask SetObjectReferenceAsync(WwiseObject wwiseObject, WwiseReference wwiseReference)
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
        /// 设置参数
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="wwiseProperty"></param>
        [Obsolete("Use async version instead")]
        public static void SetObjectProperty(WwiseObject wwiseObject, WwiseProperty wwiseProperty)
        {
            if (!TryConnectWaapi() || wwiseObject == null ||wwiseProperty == null) return;

            var setProp = SetObjectPropertyAsync(wwiseObject, wwiseProperty);
            setProp.AsTask().Wait();

        }

        /// <summary>
        /// 设置参数，异步执行
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async ValueTask SetObjectPropertyAsync(WwiseObject wwiseObject, WwiseProperty wwiseProperty)
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
        /// 修改名称
        /// </summary>
        /// <param name="renameObject"></param>
        /// <param name="newName"></param>
        [Obsolete("Use async version instead")]
        public static void ChangeObjectName(WwiseObject renameObject, string newName)
        {
            if (!TryConnectWaapi() || renameObject == null || String.IsNullOrEmpty(newName)) return;
            var changeName = ChangeObjectNameAsync(renameObject, newName);
            changeName.AsTask().Wait();
        }

        /// <summary>
        /// 修改名称，异步执行
        /// </summary>
        /// <param name="renameObject"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public static async ValueTask ChangeObjectNameAsync(WwiseObject renameObject, string newName)
        {
            if(!await TryConnectWaapiAsync() || renameObject == null || String.IsNullOrEmpty(newName)) return;

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
                    });

                renameObject.Name = newName;

                WaapiLog.Log($"Object {oldName} successfully renamed to {newName}!");
            }

            catch (Exception e)
            {
                WaapiLog.Log($"Failed to rename object : {oldName} ======> {e.Message}");
            }
        }

        /// <summary>
        /// 将物体移动至指定父物体
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        [Obsolete("Use async version instead")]
        public static void CopyToParent(WwiseObject child, WwiseObject parent)
        {
            if (!TryConnectWaapi() || child == null || parent == null) return;

            var copy = CopyToParentAsync(child, parent);
            copy.AsTask().Wait();
        }

        /// <summary>
        /// 将物体移动至指定父物体，后台进行
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static async ValueTask CopyToParentAsync(WwiseObject child, WwiseObject parent)
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
                    );

                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        id = new string[] { child.ID }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "name", "id", "type"}

                };

                func = WaapiFunction.CoreObjectGet;
                // 获取子物体的新数据
                JObject jresult = await Client.Call(func, query, options);


                WaapiLog.Log($"Copied {child.Name} to {parent.Name}!");

            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to copy {child.Name} to {parent.Name}! ======> {e.Message}");
            }

            //return null;
        }
        
        
        /// <summary>
        /// 将物体复制至指定父物体
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        [Obsolete("Use async version instead")]
        public static void MoveToParent(WwiseObject child, WwiseObject parent)
        {
            if (!TryConnectWaapi() || child == null || parent == null) return;

            var move = MoveToParentAsync(child, parent);
            move.AsTask().Wait();
        }

        /// <summary>
        /// 将物体复制至指定父物体，后台进行
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static async ValueTask MoveToParentAsync(WwiseObject child, WwiseObject parent)
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
                    }
                    );

                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        id = new string[] { child.ID }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "name", "id", "type"}

                };

                func = WaapiFunction.CoreObjectGet;
                // 获取子物体的新数据
                JObject jresult = await Client.Call(func, query, options);
                if (jresult["return"]?.Last == null) throw new Exception();
                child.Name = jresult["return"].Last["name"]?.ToString();
                child.ID = jresult["return"].Last["id"]?.ToString();
                child.Type = jresult["return"].Last["type"]?.ToString();

                WaapiLog.Log($"Moved {child.Name} to {parent.Name}!");
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to move {child.Name} to {parent.Name}! ======> {e.Message}");
            }
        }
        
        [Obsolete("Use async version instead")]
        public static void SetNote(WwiseObject target, string note)
        {
            if (!TryConnectWaapi() || target== null) return;

            var set = SetNoteAsync(target, note);
            set.AsTask().Wait();
        }
        
         public static async ValueTask SetNoteAsync(WwiseObject target, string note)
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
                    }
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
        /// <param name="wwiseObject"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public static WwiseObject CreatePlayEvent(string eventName, string objectPath, string parentPath = @"\Events\Default Work Unit")
        {
            if (!TryConnectWaapi()) return null;
            var evt = AddEventActionAsync(eventName, objectPath, parentPath);
            evt.AsTask().Wait();
            return evt.Result;
        }


        /// <summary>
        /// 生成播放事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="objectPath"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public static async ValueTask<WwiseObject> CreatePlayEventAsync(string eventName, string objectPath, string parentPath = @"\Events\Default Work Unit")
        {
            if (!await TryConnectWaapiAsync()) return null;
            return await AddEventActionAsync(eventName, objectPath, parentPath);
        }

        [Obsolete("Use async version instead")]
        public static WwiseObject AddEventAction(string eventName, string objectPath,
            string parentPath = @"\Events\Default Work Unit", int actionType = 1)
        {
            if (!TryConnectWaapi()) return null;
            var evt = AddEventActionAsync(eventName, objectPath, parentPath, actionType);
            evt.AsTask().Wait();
            return evt.Result;
        }


        /// <summary>
        /// 生成播放事件，异步执行
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="objectPath"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public static async ValueTask<WwiseObject> AddEventActionAsync(string eventName, string objectPath, string parentPath = @"\Events\Default Work Unit", int actionType = 1)
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
                    }
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

        [Obsolete("Use async version instead")]
        public static void AddEventToBank(WwiseObject soundBank, string eventId)
        {
            var r = AddEventToBankAsync(soundBank, eventId);
            r.AsTask().Wait();
        }

        public static async ValueTask AddEventToBankAsync(WwiseObject soundBank, string eventId)
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
                    }
                );
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to Add Event to Bank ======> {e.Message}");
            }
        }

        /// <summary>
        /// 创建物体
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="objectType"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public static WwiseObject CreateObject(string objectName, WwiseObject.ObjectType objectType, string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit")
        {
            var obj = CreateObjectAsync(objectName, objectType, parentPath);
            obj.AsTask().Wait();
            return obj.Result;
        }


        /// <summary>
        /// 创建物体，后台进行
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="objectType"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public static async ValueTask<WwiseObject> CreateObjectAsync(string objectName, WwiseObject.ObjectType objectType, string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit")
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

        [Obsolete("Use async version instead")]
        public static void DeleteObject(string path)
        {
            var obj = DeleteObjectAsync(path);
            obj.AsTask().Wait();
        }

        public static async ValueTask DeleteObjectAsync(string path)
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
        /// 通过ID搜索物体
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public static WwiseObject GetWwiseObjectByID(string targetId)
        {
            var get = GetWwiseObjectByIDAsync(targetId);
            get.AsTask().Wait();
            return get.Result;
        }

        /// <summary>
        /// 通过ID搜索物体，异步执行
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        public static async ValueTask<WwiseObject> GetWwiseObjectByIDAsync(string targetId)
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

                try // 尝试返回物体数据
                {
                    JObject jresult = await Client.Call(func, query, options);
                    if (jresult["return"]?.Last == null) throw new Exception();
                    string name = jresult["return"].Last["name"]?.ToString();
                    string id = jresult["return"].Last["id"]?.ToString();
                    string type = jresult["return"].Last["type"]?.ToString();

                    WaapiLog.Log($"WwiseObject {name} successfully fetched!");

                    return new WwiseObject(name, id, type);
                }
                catch
                {
                    WaapiLog.Log($"Failed to return WwiseObject from ID : {targetId}!");
                    return null;
                }
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return WwiseObject from ID : {targetId}! ======> {e.Message}");
                return null;
            }
            
        }


        [Obsolete("Use async version instead")]
        public static JToken GetWwiseObjectProperty(string targetId, string wwiseProperty)
        {
            var get = GetWwiseObjectPropertyAsync(targetId, wwiseProperty);
            get.AsTask().Wait();
            return get.Result;
        }


        public static async ValueTask<JToken> GetWwiseObjectPropertyAsync(string targetId, string wwiseProperty)
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

                    @return = new string[] { "@"+wwiseProperty }

                };

                try // 尝试返回物体数据
                {
                    JObject jresult = await Client.Call(func, query, options);

                    

                    WaapiLog.Log($"WwiseProperty {wwiseProperty} successfully fetched!");
                    if (jresult["return"] == null) throw new Exception();
                    return jresult["return"].Last?["@" + wwiseProperty];
                }
                catch (Exception e)
                {
                    WaapiLog.Log($"Failed to return WwiseObject Property : {targetId}! ======> {e.Message}");
                    return null;
                }
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return WwiseObject Property : {targetId}! ======> {e.Message}");
                return null;
            }

        }


        [Obsolete("Use async version instead")]
        public static string GetWwiseObjectPath(string ID)
        {
            var r = GetWwiseObjectPathAsync(ID);
            r.AsTask().Wait();
            return r.Result;
        }

        public static async ValueTask<string> GetWwiseObjectPathAsync(string ID)
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



                try // 尝试返回物体数据
                {
                    JObject jresult = await WwiseUtility.Client.Call(func, query, options);
                    if (jresult["return"].Last["path"] == null) throw new Exception();
                    string path = jresult["return"].Last["path"].ToString();

                    return path;
                }
                catch (Exception e)
                {
                    WaapiLog.Log($"Failed to get path of object : {ID}! =======> {e.Message}");
                    return null;
                }
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to get path of object : {ID}! =======> {e.Message}");
                return null;
            }

        }


        /// <summary>
        /// 通过名称与类型检索对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Obsolete]
        public static List<WwiseObject> GetWwiseObjectsByNameAndType(string name, string type)
        {
            List<WwiseObject> temp = GetWwiseObjectsOfType(type);
            List<WwiseObject> result = new List<WwiseObject>();
            foreach (var obj in temp)
            {
                if (obj.Name == name)
                {
                    result.Add(obj);
                }
            }

            return result;

        }


        /// <summary>
        /// 通过类型与父对象路径检索对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        [Obsolete]
        public static List<WwiseObject> GetWwiseObjectsByTypeAndParent(string type, string parentPath)
        {
            List<WwiseObject> temp = GetWwiseObjectsOfType(type);
            List<WwiseObject> result = new List<WwiseObject>();
            foreach (var obj in temp)
            {
                if (obj.Path.Contains(parentPath))
                {
                    result.Add(obj);
                }
            }

            return result;
        }

        /// <summary>
        /// 通过名称搜索唯一命名对象，格式必须为"type:name
        /// </summary>
        /// <param name="targetName"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public static WwiseObject GetWwiseObjectByName(string targetName)
        {

            var get = GetWwiseObjectByNameAsync(targetName);
            get.AsTask().Wait();
            return get.Result;
        }

        /// <summary>
        /// 通过名称搜索唯一命名对象，异步执行，格式必须为"type:name"
        /// </summary>
        /// <param name="targetName"></param>
        /// <returns></returns>
        public static async ValueTask<WwiseObject> GetWwiseObjectByNameAsync(string targetName)
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

                

                try // 尝试返回物体数据
                {

                    JObject jresult = await Client.Call(func, query, options);
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
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return WwiseObject by name : {targetName}! ======> {e.Message}");
                return null;
            }
            
        }

        /// <summary>
        /// 通过路径获取对象
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public static WwiseObject GetWwiseObjectByPath(string path)
        {

            var get = GetWwiseObjectByPathAsync(path);
            get.AsTask().Wait();
            return get.Result;
        }

        /// <summary>
        /// 通过路径获取对象，异步执行
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async ValueTask<WwiseObject> GetWwiseObjectByPathAsync(string path)
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

                JObject jresult = await Client.Call(func, query, options);
                var obj = jresult["return"]?.Last;
                if (obj == null) throw new Exception("Object not found!");

                string name = obj["name"]?.ToString();
                string id = obj["id"]?.ToString();
                string type = obj["type"]?.ToString();

                WaapiLog.Log($"WwiseObject {name} successfully fetched!");

                return new WwiseObject(name, id, type);

                /*try // 尝试返回物体数据
                {

                    
                }
                catch (Exception e)
                {
                    WaapiLog.Log($"Failed to return WwiseObject by path : {path}! ======> {e.Message}");
                    return null;
                }*/
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return WwiseObject by path : {path}! ======> {e.Message}");
                return null;
            }

        }

        /// <summary>
        /// 获取指定类型的对象
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public static List<WwiseObject> GetWwiseObjectsOfType(string targetType)
        {

            var get = GetWwiseObjectsOfTypeAsync(targetType);
            get.AsTask().Wait();
            return get.Result;
        }

        /// <summary>
        /// 获取指定类型的对象，异步执行
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static async ValueTask<List<WwiseObject>> GetWwiseObjectsOfTypeAsync(string targetType, bool standardType = true)
        {
            if (!await TryConnectWaapiAsync() || String.IsNullOrWhiteSpace(targetType)) return null;

            if (standardType && ConnectionInfo.Version >= VersionHelper.V2021_1_0_7575)
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



                try // 尝试返回物体数据
                {
                    var func = WaapiFunction.CoreObjectGet;

                    JObject jresult = await Client.Call(func, query, options);

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
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return WwiseObject list of type : {targetType}! ======> {e.Message}");
                return null;
            }
        }


        [Obsolete("Use async version instead")]
        public static List<WwiseObject> GetWwiseObjectsBySelection()
        {
            var get = GetWwiseObjectsBySelectionAsync();
            get.AsTask().Wait();
            return get.Result;
        }
        public static async ValueTask<List<WwiseObject>> GetWwiseObjectsBySelectionAsync()
        {
            if (!await TryConnectWaapiAsync()) return null;
            try
            {
                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "name", "id", "type", "path" }

                };



                try // 尝试返回物体数据
                {
                    var func = Function.Verify("ak.wwise.ui.getSelectedObjects");

                    JObject jresult = await Client.Call(func, null, options);

                    List<WwiseObject> objList = new List<WwiseObject>();


                    if (jresult["objects"] == null) throw new Exception(); 
                    foreach (var obj in jresult["objects"])
                    {
                        string name = obj["name"]?.ToString();
                        string id = obj["id"]?.ToString();
                        string type = obj["type"]?.ToString();

                        objList.Add(new WwiseObject(name, id, type));
                    }



                    WaapiLog.Log($"Selected WwiseObject list successfully fetched!");

                    return objList;
                }
                catch (Exception e)
                {
                    WaapiLog.Log($"Failed to return Selected WwiseObject list! ======> {e.Message}");
                    return null;
                }
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return Selected WwiseObject list! ======> {e.Message}");
                return null;
            }
        }


        [Obsolete("Use async version instead")]
        public static List<string> GetLanguages()
        {
            var result = GetLanguagesAsync();
            result.AsTask().Wait();
            return result.Result;
        }
        
        public static async ValueTask<List<string>> GetLanguagesAsync()
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

                var result = await Client.Call(func, query, options);
                if (result["return"] == null) throw new Exception();
                foreach (var r in result["return"])
                {
                    string name = r["name"]?.ToString();
                    var ignoreList = new string[] {"Mixed", "SFX", "External", "SoundSeed Grain"};
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
        /// 从指定文件夹导入音频
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="language"></param>
        /// <param name="subFolder"></param>
        /// <param name="parentPath"></param>
        /// <param name="work_unit"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        [Obsolete]
        public static List<WwiseObject> ImportSoundFromFolder(string folderPath, string language = "SFX", string subFolder = "", string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit") // 从指定文件夹路径导入
        {
            if (!TryConnectWaapi()) return null; // 没有成功连接时返回空的WwiseObject List

            try
            {
                List<WwiseObject> results = new List<WwiseObject>();

                string[] files = Directory.GetFiles(folderPath);
                foreach (var f in files)
                {
                    if (!f.Contains(".wav")) continue;

                    var r = ImportSound(f, language, subFolder, parentPath);
                    results.Add(r);
                }
                WaapiLog.Log($"File(s) in folder {folderPath} imported successfully!");
                return results;
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to import from folder : {folderPath}! ======> {e.Message}");
                return null;
            }
        }


        /// <summary>
        /// 从指定路径导入音频
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="language"></param>
        /// <param name="subFolder"></param>
        /// <param name="parentPath"></param>
        /// <param name="work_unit"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public static WwiseObject ImportSound(string filePath, string language = "SFX", string subFolder = "", string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit", string soundName = "") // 直接调用的版本
        {
            ValueTask<WwiseObject> obj = WwiseUtility.ImportSoundAsync(filePath, language, subFolder, parentPath, soundName);
            obj.AsTask().Wait();
            return obj.Result;
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
        public static async ValueTask<WwiseObject> ImportSoundAsync(string filePath, string language = "SFX", string subFolder = "", string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit", string soundName = "") // Async版本
        {
            if (!filePath.EndsWith(".wav") || !await TryConnectWaapiAsync()) return null; // 目标不是文件或者没有成功连接时返回空的WwiseObject
            
            
            
            string fileName = "";
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
                    WaapiLog.Log($"Failed to get file name from { filePath } ======> { e.Message }");
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

                if (result == null || result["objects"] == null || result["objects"].Last == null || result["objects"].Last["id"] == null) return null;
                
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
        /// 获取工作单元文件路径
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public static string GetWorkUnitFilePath(WwiseObject @object)
        {
            var get = GetWorkUnitFilePathAsync(@object);
            get.AsTask().Wait();
            return get.Result;
        }

        /// <summary>
        /// 获取工作单元文件路径，异步执行
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static async ValueTask<string> GetWorkUnitFilePathAsync(WwiseObject @object)
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



                try // 尝试返回物体数据
                {
                    var func = WaapiFunction.CoreObjectGet;

                    JObject jresult = await Client.Call(func, query, options);

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
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return Work Unit file path of object : {@object.Name}! ======> {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 重新加载当前工程
        /// </summary>
        [Obsolete("Use async version instead")]
        public static void ReloadWwiseProject()
        {
            
            LoadWwiseProject(GetWwiseProjectPath(), true);
            Client = null;
            Init().AsTask().Wait();
        }
        
        public static async ValueTask ReloadWwiseProjectAsync()
        {
            await LoadWwiseProjectAsync(await GetWwiseProjectPathAsync(), true);
            await DisconnectAsync();
            Client = null;
            await ConnectAsync();
        }

        /// <summary>
        /// 加载工程
        /// </summary>
        /// <param name="path"></param>
        /// <param name="saveCurrent"></param>
        [Obsolete("Use async version instead")]
        public static void LoadWwiseProject(string path, bool saveCurrent = true)
        {
            LoadWwiseProjectAsync(path, saveCurrent).AsTask().Wait();
        }

        /// <summary>
        /// 加载工程，异步执行
        /// </summary>
        /// <param name="path"></param>
        /// <param name="saveCurrent"></param>
        /// <returns></returns>
        public static async ValueTask LoadWwiseProjectAsync(string path, bool saveCurrent = true)
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
                await Client.Call(func, query);

                WaapiLog.Log("Project loaded successfully!");
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to load project! =======> {e.Message}");
            }
        }

        /// <summary>
        /// 获取工程路径
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public static string GetWwiseProjectName()
        {
            var get = GetWwiseProjectNameAsync();
            get.AsTask().Wait();
            return get.Result;
        }

        /// <summary>
        /// 获取工程路径，异步执行
        /// </summary>
        /// <returns></returns>
        public static async ValueTask<string> GetWwiseProjectNameAsync()
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



                try // 尝试返回物体数据
                {
                    var func = WaapiFunction.CoreObjectGet;

                    JObject jresult = await Client.Call(func, query, options);

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
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return project name! ======> {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取工程路径
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public static string GetWwiseProjectPath()
        {
            var get = GetWwiseProjectPathAsync();
            get.AsTask().Wait();
            return get.Result;
        }

        /// <summary>
        /// 获取工程路径，异步执行
        /// </summary>
        /// <returns></returns>
        public static async ValueTask<string> GetWwiseProjectPathAsync()
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



                try // 尝试返回物体数据
                {
                    var func = WaapiFunction.CoreObjectGet;

                    JObject jresult = await Client.Call(func, query, options);

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
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return project path! ======> {e.Message}");
                return null;
            }
        }

        public static async ValueTask<WwiseInfo> GetWwiseInfoAsync()
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
                //string sessionId = result["sessionId"].ToString();
                int.TryParse(result["processId"]?.ToString(), out int processId);

                WwiseInfo wwiseInfo = new WwiseInfo()
                {
                    Version = new WwiseVersion(year, major, minor, build, schema),
                    //SessionID = sessionId,
                    ProcessID = processId
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
        public static async ValueTask ExecuteUICommand(string command, string[] objectIDs = null)
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

                    await Client.Call(func, query);
                }
                else
                {
                    var query = new { command = command };
                    await Client.Call(func, query);
                }
                
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to execute command {command}! ======> {e.Message}");
            }
        }

        public static async ValueTask<List<WwiseObject>> GetWwiseObjectChildrenAsync(WwiseObject wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();
            if (wwiseObject == null || !await TryConnectWaapiAsync()) return result;
            if (WwiseUtility.ConnectionInfo.Version >= VersionHelper.V2021_1_0_7575)
            {
                try
                {
                    var waql = new Waql($"where parent.id = \"{wwiseObject.ID}\"");
                    if (await waql.RunAsync())
                    {
                        result = waql.Result;
                    }
                    else
                    {
                        throw new Exception("waql failed");
                    }
                }
                catch (Exception e)
                {
                    WaapiLog.Log($"Failed to get children of {wwiseObject.Name} ======> {e.Message}");
                }

            }
            else
            {
                await WwiseUtility.SaveWwiseProjectAsync();

                WwiseWorkUnitParser parser = new WwiseWorkUnitParser(await WwiseUtility.GetWorkUnitFilePathAsync(wwiseObject));
                var node = parser.GetNodeByID(wwiseObject.ID);
                foreach (XmlElement child in parser.GetChildrenNodeList(node))
                {
                    result.Add(await WwiseUtility.GetWwiseObjectByIDAsync(child.GetAttribute("ID")));
                }

            }

            return result;
        }

        public static async ValueTask GenerateSelectedSoundBanksAllPlatformAsync(string[] soundBanks)
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

                await Client.Call(func, query);
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to generate sound bank! ======> {e.Message}");
            }
        }

        private static async ValueTask GetFunctionsAsync()
        {
            if (!await TryConnectWaapiAsync() || Function != null) return;
            Function = new WaapiFunction();
            try
            {
                var result = await Client.Call("ak.wwise.waapi.getFunctions");
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
        /// 保存工程
        /// </summary>
        [Obsolete("Use async version instead")]
        public static void SaveWwiseProject()
        {
            SaveWwiseProjectAsync().AsTask().Wait();
        }

        /// <summary>
        /// 保存工程，异步执行
        /// </summary>
        /// <returns></returns>
        public static async ValueTask SaveWwiseProjectAsync()
        {
            if (!await TryConnectWaapiAsync()) return;
            try
            {
                var func = Function.Verify("ak.wwise.core.project.save");

                await Client.Call(func);
                WaapiLog.Log("Project saved successfully!");
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to save project! =======> {e.Message}");
            }
        }
        
    }
}
