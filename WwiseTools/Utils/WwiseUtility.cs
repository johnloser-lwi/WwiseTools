using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

using AK.Wwise.Waapi;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Reference;
using WwiseTools.Objects;

namespace WwiseTools.Utils
{
    /// <summary>
    /// 用于实现基础功能
    /// </summary>
    public class WwiseUtility
    {
        public static JsonClient Client { get; set; }

        public enum GlobalImportSettings
        {
            useExisting,
            replaceExisting,
            createNew
        }

        public static GlobalImportSettings ImportSettings = GlobalImportSettings.useExisting;

        /// <summary>
        /// 连接初始化
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> Init() // 初始化，返回连接状态
        {
            if (Client != null && Client.IsConnected()) return true;

            try
            {
                
                Client = new JsonClient();
                await Client.Connect(); // 尝试创建Wwise连接

                Console.WriteLine("Connected successfully!");

                Client.Disconnected += () =>
                {
                    Client = null;
                    System.Console.WriteLine("Connection closed!"); // 丢失连接提示
                };
                return true;
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to connect! ======> {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        public static async Task Close()
        {
            if (Client == null || !Client.IsConnected()) return;

            try
            {
                await Client.Close(); // 尝试断开连接
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Error while closing! ======> {e.Message}");
            }
        }

        /// <summary>
        /// 尝试连接并检查连接状态
        /// </summary>
        /// <returns></returns>
        public static bool TryConnectWaapi() 
        {
            var connected = Init();
            connected.Wait();

            return connected.Result && Client.IsConnected();
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
        public static string GetPropertyAndReferenceNames(WwiseObject wwiseObject)
        {
            if (!TryConnectWaapi() || wwiseObject == null) return "";

            var get = GetPropertyAndReferenceNamesAsync(wwiseObject);
            get.Wait();
            return get.Result;
        }

        /// <summary>
        /// 获取属性、引用名称，同步执行
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <returns></returns>
        public static async Task<string> GetPropertyAndReferenceNamesAsync(WwiseObject wwiseObject)
        {
            //ak.wwise.core.object.getPropertyAndReferenceNames

            if (!TryConnectWaapi() || wwiseObject == null) return "";

            try
            {
                var result = await Client.Call(ak.wwise.core.@object.getPropertyAndReferenceNames,

                    new JObject(

                        new JProperty("object", wwiseObject.ID)),

                    null);
                Console.WriteLine("Property and References fetched successfully!");
                return result.ToString();

                
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to fetch Property and References! ======> {e.Message}");
                return "";
            }
        }

        /// <summary>
        /// 设置物体的引用
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="wwiseReference"></param>
        public static void SetObjectReference(WwiseObject wwiseObject, WwiseReference wwiseReference)
        {
            if (!TryConnectWaapi() || wwiseObject == null || wwiseReference == null) return;
            var setRef = SetObjectReferenceAsync(wwiseObject, wwiseReference);
            setRef.Wait();
        }

        /// <summary>
        /// 设置物体的引用，同步执行
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="wwiseReference"></param>
        /// <returns></returns>
        public static async Task SetObjectReferenceAsync(WwiseObject wwiseObject, WwiseReference wwiseReference)
        {
            if (!TryConnectWaapi() || wwiseObject == null || wwiseReference == null) return;

            try
            {
                await Client.Call(ak.wwise.core.@object.setReference,

                    new JObject(

                        new JProperty("object", wwiseObject.ID),

                        new JProperty("reference", wwiseReference.Name),

                        new JProperty("value", wwiseReference.Object.ID)),

                    null);

                Console.WriteLine("Reference set successfully!");
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to set reference \"{wwiseReference.Name}\" to object {wwiseObject.Name} ======> {e.Message}");
            }
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="wwiseProperty"></param>
        public static void SetObjectProperty(WwiseObject wwiseObject, WwiseProperty wwiseProperty)
        {
            if (!TryConnectWaapi() || wwiseObject == null ||wwiseProperty == null) return;

            var set_prop = SetObjectPropertyAsync(wwiseObject, wwiseProperty);
            set_prop.Wait();

        }

        /// <summary>
        /// 设置参数，同步执行
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task SetObjectPropertyAsync(WwiseObject wwiseObject, WwiseProperty wwiseProperty)
        {
            if (!TryConnectWaapi() || wwiseObject == null || wwiseProperty == null) return;

            try
            {
                await Client.Call(ak.wwise.core.@object.setProperty,

                    new JObject(

                        new JProperty("property", wwiseProperty.Name),

                        new JProperty("object", wwiseObject.ID),

                        new JProperty("value", wwiseProperty.Value)),

                    null);

                Console.WriteLine($"Property {wwiseProperty.Name} successfully changed to {wwiseProperty.Value}!");
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to set property \"{wwiseProperty.Name}\" of object {wwiseObject.Name} ======> {e.Message}");
            }
        }

        /// <summary>
        /// 修改名称
        /// </summary>
        /// <param name="rename_object"></param>
        /// <param name="new_name"></param>
        public static void ChangeObjectName(WwiseObject rename_object, string new_name)
        {
            if (!TryConnectWaapi() || rename_object == null || String.IsNullOrEmpty(new_name)) return;
            var change_name = ChangeObjectNameAsync(rename_object, new_name);
            change_name.Wait();
        }

        /// <summary>
        /// 修改名称，同步执行
        /// </summary>
        /// <param name="rename_object"></param>
        /// <param name="new_name"></param>
        /// <returns></returns>
        public static async Task ChangeObjectNameAsync(WwiseObject rename_object, string new_name)
        {
            if(!TryConnectWaapi() || rename_object == null || String.IsNullOrEmpty(new_name)) return;

            string old_name = rename_object.Name;
            try
            {
                await Client.Call(
                    ak.wwise.core.@object.setName,
                    new
                    {
                        @object = rename_object.ID,
                        value = new_name,
                    });

                rename_object.Name = new_name;

                Console.WriteLine($"Object {old_name} successfully renamed to {new_name}!");
            }

            catch (AK.Wwise.Waapi.Wamp.ErrorException e)
            {
                System.Console.Write($"Failed to rename object : {old_name} ======> {e.Message}");
            }
        }

        /// <summary>
        /// 将物体移动至指定父物体
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        public static WwiseObject CopyToParent(WwiseObject child, WwiseObject parent)
        {
            if (!TryConnectWaapi() || child == null || parent == null) return null;

            var copy = CopyToParentAsync(child, parent);
            copy.Wait();
            return copy.Result;
        }

        /// <summary>
        /// 将物体移动至指定父物体，后台进行
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static async Task<WwiseObject> CopyToParentAsync(WwiseObject child, WwiseObject parent)
        {
            if (!TryConnectWaapi() || child == null || parent == null) return null;

            try
            {
                // 移动物体
                await Client.Call(
                    ak.wwise.core.@object.copy,
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

                // 获取子物体的新数据
                JObject jresult = await Client.Call(ak.wwise.core.@object.get, query, options);

                /*
                try // 尝试更新子物体数据
                {
                    child.Name = jresult["return"].Last["name"].ToString();
                    child.ID = jresult["return"].Last["id"].ToString();
                    child.Type = jresult["return"].Last["type"].ToString();

                    Console.WriteLine($"Moved {child.Name} to {parent.Path}!");
                }
                catch (Wamp.ErrorException e)
                {
                    Console.WriteLine($"Failed to update WwiseObject! ======> {e.Message}");
                }
                */

                Console.WriteLine($"Copied {child.Name} to {parent.Name}!");

                return GetWwiseObjectByID(jresult["return"].Last["id"].ToString());
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to copy {child.Name} to {parent.Name}! ======> {e.Message}");
            }

            return null;
        }
        
        
        /// <summary>
        /// 将物体复制至指定父物体
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        public static void MoveToParent(WwiseObject child, WwiseObject parent)
        {
            if (!TryConnectWaapi() || child == null || parent == null) return;

            var move = MoveToParentAsync(child, parent);
            move.Wait();
        }

        /// <summary>
        /// 将物体复制至指定父物体，后台进行
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static async Task MoveToParentAsync(WwiseObject child, WwiseObject parent)
        {
            if (!TryConnectWaapi() || child == null || parent == null) return;

            try
            {
                // 移动物体
                await Client.Call(
                    ak.wwise.core.@object.move,
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

                // 获取子物体的新数据
                JObject jresult = await Client.Call(ak.wwise.core.@object.get, query, options);

                /*
                try // 尝试更新子物体数据
                {
                    child.Name = jresult["return"].Last["name"].ToString();
                    child.ID = jresult["return"].Last["id"].ToString();
                    child.Type = jresult["return"].Last["type"].ToString();

                    Console.WriteLine($"Moved {child.Name} to {parent.Path}!");
                }
                catch (Wamp.ErrorException e)
                {
                    Console.WriteLine($"Failed to update WwiseObject! ======> {e.Message}");
                }
                */

                child.Name = jresult["return"].Last["name"].ToString();
                child.ID = jresult["return"].Last["id"].ToString();
                child.Type = jresult["return"].Last["type"].ToString();

                Console.WriteLine($"Moved {child.Name} to {parent.Name}!");
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to move {child.Name} to {parent.Name}! ======> {e.Message}");
            }
        }

        /// <summary>
        /// 生成播放事件
        /// </summary>
        /// <param name="event_name"></param>
        /// <param name="wwiseObject"></param>
        /// <param name="parent_path"></param>
        /// <returns></returns>
        public static WwiseObject CreatePlayEvent(string event_name, WwiseObject wwiseObject, string parent_path = @"\Events\Default Work Unit")
        {
            if (!TryConnectWaapi()) return null;
            var evt = CreatePlayEventAsync(event_name, wwiseObject.Path, parent_path);
            evt.Wait();
            return evt.Result;
        }


        /// <summary>
        /// 生成播放事件
        /// </summary>
        /// <param name="event_name"></param>
        /// <param name="object_path"></param>
        /// <param name="parent_path"></param>
        /// <returns></returns>
        public static WwiseObject CreatePlayEvent(string event_name, string object_path, string parent_path = @"\Events\Default Work Unit")
        {
            if (!TryConnectWaapi()) return null;
            var evt = CreatePlayEventAsync(event_name, object_path, parent_path);
            evt.Wait();
            return evt.Result;
        }


        /// <summary>
        /// 生成播放事件，同步执行
        /// </summary>
        /// <param name="event_name"></param>
        /// <param name="object_path"></param>
        /// <param name="parent_path"></param>
        /// <returns></returns>
        public static async Task<WwiseObject> CreatePlayEventAsync(string event_name, string object_path, string parent_path = @"\Events\Default Work Unit")
        {
            if (!TryConnectWaapi()) return null;

            try
            {
                var result = await Client.Call
                    (
                    ak.wwise.core.@object.create,
                    new JObject
                    {
                        new JProperty("parent", parent_path),
                        new JProperty("type", "Event"),
                        new JProperty("name", event_name),
                        new JProperty("onNameConflict", "merge"),
                        new JProperty("children", new JArray
                        {
                            new JObject
                            {
                                new JProperty("name", ""),
                                new JProperty("type", "Action"),
                                new JProperty("@ActionType", 1),
                                new JProperty("@Target", object_path)
                            }
                        })
                    }
                    );

                Console.WriteLine($"Event {event_name} created successfully!");
                return await GetWwiseObjectByIDAsync(result["id"].ToString());
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to created play event : {event_name}! ======> {e.Message} ");
                return null;
            }
        }

        public static void AddEventToBank(WwiseObject soundBank, string eventID)
        {
            var r = AddEventToBankAsync(soundBank, eventID);
            r.Wait();
        }

        public static async Task AddEventToBankAsync(WwiseObject soundBank, string eventID)
        {
            if (!TryConnectWaapi()) return;

            try
            {
                await Client.Call
                (
                    ak.wwise.core.soundbank.setInclusions,
                    new JObject
                    {
                        new JProperty("soundbank", soundBank.ID),
                        new JProperty("operation", "add"),
                        new JProperty("inclusions", new JArray
                        {
                            new JObject
                            {
                                new JProperty("object", eventID),
                                new JProperty("filter", new JArray("events", "structures", "media"))
                            }
                        })
                    }
                );
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to Add Event to Bank ======> {e.Message}");
            }
        }

        /// <summary>
        /// 创建物体
        /// </summary>
        /// <param name="object_name"></param>
        /// <param name="object_type"></param>
        /// <param name="parent_path"></param>
        /// <returns></returns>
        public static WwiseObject CreateObject(string object_name, WwiseObject.ObjectType object_type, string parent_path = @"\Actor-Mixer Hierarchy\Default Work Unit")
        {
            var obj = CreateObjectAsync(object_name, object_type, parent_path);
            obj.Wait();
            return obj.Result;
        }


        /// <summary>
        /// 创建物体，后台进行
        /// </summary>
        /// <param name="object_name"></param>
        /// <param name="object_type"></param>
        /// <param name="parent_path"></param>
        /// <returns></returns>
        public static async Task<WwiseObject> CreateObjectAsync(string object_name, WwiseObject.ObjectType object_type, string parent_path = @"\Actor-Mixer Hierarchy\Default Work Unit")
        {
            if (!TryConnectWaapi()) return null;

            try
            {
                // 创建物体
                var result = await Client.Call
                    (
                    ak.wwise.core.@object.create,
                    new JObject
                    {
                        new JProperty("name", object_name),
                        new JProperty("type", object_type.ToString()),
                        new JProperty("parent", parent_path),
                        new JProperty("onNameConflict", "fail")
                    },
                    null
                    );

                Console.WriteLine($"Object {object_name} created successfully!");
                return await GetWwiseObjectByIDAsync(result["id"].ToString());
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to create object : {object_name}! ======> {e.Message}");
                return null;
            }
        }

        public static void DeleteObject(string path)
        {
            var obj = DeleteObjectAsync(path);
            obj.Wait();
        }

        public static async Task DeleteObjectAsync(string path)
        {
            if (!TryConnectWaapi()) return;

            try
            {
                // 创建物体
                var result = await Client.Call
                    (
                    ak.wwise.core.@object.delete,
                    new JObject
                    {
                        new JProperty("object", path)
                    },
                    null
                    );

                Console.WriteLine($"Object {path} deleted successfully!");
                return;
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to delete object : {path}! ======> {e.Message}");
                return;
            }
        }



        /// <summary>
        /// 通过ID搜索物体
        /// </summary>
        /// <param name="target_id"></param>
        /// <returns></returns>
        public static WwiseObject GetWwiseObjectByID(string target_id)
        {
            var get = GetWwiseObjectByIDAsync(target_id);
            get.Wait();
            return get.Result;
        }

        /// <summary>
        /// 通过ID搜索物体，同步执行
        /// </summary>
        /// <param name="target_id"></param>
        /// <returns></returns>
        public static async Task<WwiseObject> GetWwiseObjectByIDAsync(string target_id)
        {
            if (!TryConnectWaapi() || String.IsNullOrWhiteSpace(target_id)) return null; 

            try
            {
                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        id = new string[] { target_id }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "name", "id", "type", "path", "musicPlaylistRoot" }

                };

                try // 尝试返回物体数据
                {
                    JObject jresult = await Client.Call(ak.wwise.core.@object.get, query, options);

                    string name = jresult["return"].Last["name"].ToString();
                    string id = jresult["return"].Last["id"].ToString();
                    string type = jresult["return"].Last["type"].ToString();

                    Console.WriteLine($"WwiseObject {name} successfully fetched!");

                    return new WwiseObject(name, id, type);
                }
                catch
                {
                    Console.WriteLine($"Failed to return WwiseObject from ID : {target_id}!");
                    return null;
                }
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to return WwiseObject from ID : {target_id}! ======> {e.Message}");
                return null;
            }
            
        }


        public static JToken GetWwiseObjectProperty(string target_id, string wwise_property)
        {
            var get = GetWwiseObjectPropertyAsync(target_id, wwise_property);
            get.Wait();
            return get.Result;
        }


        public static async Task<JToken> GetWwiseObjectPropertyAsync(string target_id, string wwise_property)
        {
            if (!TryConnectWaapi() || String.IsNullOrWhiteSpace(target_id)) return null;

            try
            {
                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        id = new string[] { target_id }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "@"+wwise_property }

                };

                try // 尝试返回物体数据
                {
                    JObject jresult = await Client.Call(ak.wwise.core.@object.get, query, options);

                    

                    Console.WriteLine($"WwiseProperty {wwise_property} successfully fetched!");

                    return jresult["return"].Last["@" + wwise_property];
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to return WwiseObject Property : {target_id}! ======> {e.Message}");
                    return null;
                }
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to return WwiseObject Property : {target_id}! ======> {e.Message}");
                return null;
            }

        }


        public static string GetWwiseObjectPath(string ID)
        {
            var r = GetWwiseObjectPathAsync(ID);
            r.Wait();
            return r.Result;
        }

        public static async Task<string> GetWwiseObjectPathAsync(string ID)
        {
            try
            {
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
                    JObject jresult = await WwiseUtility.Client.Call(ak.wwise.core.@object.get, query, options);
                    if (jresult["return"].Last["path"] == null) throw new Exception();
                    string path = jresult["return"].Last["path"].ToString();

                    return path;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to get path of object : {ID}! =======> {e.Message}");
                    return null;
                }
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to get path of object : {ID}! =======> {e.Message}");
                return null;
            }

        }


        /// <summary>
        /// 通过名称与类型检索对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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
        /// <param name="parent_path"></param>
        /// <returns></returns>
        public static List<WwiseObject> GetWwiseObjectsByTypeAndParent(string type, string parent_path)
        {
            List<WwiseObject> temp = GetWwiseObjectsOfType(type);
            List<WwiseObject> result = new List<WwiseObject>();
            foreach (var obj in temp)
            {
                if (obj.Path.Contains(parent_path))
                {
                    result.Add(obj);
                }
            }

            return result;
        }

        /// <summary>
        /// 通过名称搜索唯一命名对象，格式必须为"type:name
        /// </summary>
        /// <param name="target_name"></param>
        /// <returns></returns>
        public static WwiseObject GetWwiseObjectByName(string target_name)
        {

            var get = GetWwiseObjectByNameAsync(target_name);
            get.Wait();
            return get.Result;
        }

        /// <summary>
        /// 通过名称搜索唯一命名对象，同步执行，格式必须为"type:name"
        /// </summary>
        /// <param name="target_name"></param>
        /// <returns></returns>
        public static async Task<WwiseObject> GetWwiseObjectByNameAsync(string target_name)
        {
            if (!TryConnectWaapi() || String.IsNullOrWhiteSpace(target_name)) return null;

            try
            {
                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        name = new string[] { target_name }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "name", "id", "type", "path" }

                };

                

                try // 尝试返回物体数据
                {

                    JObject jresult = await Client.Call(ak.wwise.core.@object.get, query, options);

                    string name = jresult["return"].Last["name"].ToString();
                    string id = jresult["return"].Last["id"].ToString();
                    string type = jresult["return"].Last["type"].ToString();

                    Console.WriteLine($"WwiseObject {name} successfully fetched!");

                    return new WwiseObject(name, id, type);
                }
                catch (Wamp.ErrorException e)
                {
                    Console.WriteLine($"Failed to return WwiseObject by name : {target_name}! ======> {e.Message}");
                    return null;
                }
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to return WwiseObject by name : {target_name}! ======> {e.Message}");
                return null;
            }
            
        }

        /// <summary>
        /// 通过路径获取对象
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static WwiseObject GetWwiseObjectByPath(string path)
        {

            var get = GetWwiseObjectByPathAsync(path);
            get.Wait();
            return get.Result;
        }

        /// <summary>
        /// 通过路径获取对象，同步执行
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<WwiseObject> GetWwiseObjectByPathAsync(string path)
        {
            if (!TryConnectWaapi() || String.IsNullOrWhiteSpace(path)) return null;

            try
            {
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



                try // 尝试返回物体数据
                {

                    JObject jresult = await Client.Call(ak.wwise.core.@object.get, query, options);

                    string name = jresult["return"].Last["name"].ToString();
                    string id = jresult["return"].Last["id"].ToString();
                    string type = jresult["return"].Last["type"].ToString();

                    Console.WriteLine($"WwiseObject {name} successfully fetched!");

                    return new WwiseObject(name, id, type);
                }
                catch (Wamp.ErrorException e)
                {
                    Console.WriteLine($"Failed to return WwiseObject by path : {path}! ======> {e.Message}");
                    return null;
                }
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to return WwiseObject by path : {path}! ======> {e.Message}");
                return null;
            }

        }

        /// <summary>
        /// 获取指定类型的对象
        /// </summary>
        /// <param name="target_type"></param>
        /// <returns></returns>
        public static List<WwiseObject> GetWwiseObjectsOfType(string target_type)
        {

            var get = GetWwiseObjectsOfTypeAsync(target_type);
            get.Wait();
            return get.Result;
        }

        /// <summary>
        /// 获取指定类型的对象，同步执行
        /// </summary>
        /// <param name="target_type"></param>
        /// <returns></returns>
        public static async Task<List<WwiseObject>> GetWwiseObjectsOfTypeAsync(string target_type)
        {
            if (!TryConnectWaapi() || String.IsNullOrWhiteSpace(target_type)) return null;

            try
            {
                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        ofType = new string[] { target_type }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "name", "id", "type", "path" }

                };



                try // 尝试返回物体数据
                {
                    JObject jresult = await Client.Call(ak.wwise.core.@object.get, query, options);

                    List<WwiseObject> obj_list = new List<WwiseObject>();

                    foreach (var obj in jresult["return"])
                    {
                        string name = obj["name"].ToString();
                        string id = obj["id"].ToString();
                        string type = obj["type"].ToString();

                        obj_list.Add(new WwiseObject(name, id, type));
                    }

                    

                    Console.WriteLine($"WwiseObject list or type {target_type} successfully fetched!");

                    return obj_list;
                }
                catch (Wamp.ErrorException e)
                {
                    Console.WriteLine($"Failed to return WwiseObject list of type : {target_type}! ======> {e.Message}");
                    return null;
                }
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to return WwiseObject list of type : {target_type}! ======> {e.Message}");
                return null;
            }
        }


        public static List<WwiseObject> GetWwiseObjectsBySelection()
        {
            var get = GetWwiseObjectsBySelectionAsync();
            get.Wait();
            return get.Result;
        }
        public static async Task<List<WwiseObject>> GetWwiseObjectsBySelectionAsync()
        {
            if (!TryConnectWaapi()) return null;
            try
            {
                // ak.wwise.core.@object.get 指令
                /*var query = new
                {
                    from = new
                    {
                        ofType = new string[] { target_type }
                    }
                };*/

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "name", "id", "type", "path" }

                };



                try // 尝试返回物体数据
                {
                    JObject jresult = await Client.Call(ak.wwise.ui.getSelectedObjects, null, options);

                    List<WwiseObject> obj_list = new List<WwiseObject>();

                    foreach (var obj in jresult["objects"])
                    {
                        string name = obj["name"].ToString();
                        string id = obj["id"].ToString();
                        string type = obj["type"].ToString();

                        obj_list.Add(new WwiseObject(name, id, type));
                    }



                    Console.WriteLine($"Selected WwiseObject list successfully fetched!");

                    return obj_list;
                }
                catch (Wamp.ErrorException e)
                {
                    Console.WriteLine($"Failed to return Selected WwiseObject list! ======> {e.Message}");
                    return null;
                }
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to return Selected WwiseObject list! ======> {e.Message}");
                return null;
            }
        }


        public static List<string> GetLanguages()
        {
            var result = GetLanguagesAsync();
            result.Wait();
            return result.Result;
        }
        
        public static async Task<List<string>> GetLanguagesAsync()
        {
            if (!TryConnectWaapi()) return null;

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

                var result = await Client.Call(ak.wwise.core.@object.get, query, options);

                foreach (var r in result["return"])
                {
                    string name = r["name"].ToString();
                    var ignoreList = new string[] {"Mixed", "SFX", "External", "SoundSeed Grain"};
                    if (!ignoreList.Contains(name))
                        resultList.Add(name);
                }

                Console.WriteLine($"Language list fetched successfully!");
                
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to return language list! ======> {e.Message}");
            }

            return resultList;
        }

        /// <summary>
        /// 从指定文件夹导入音频
        /// </summary>
        /// <param name="folder_path"></param>
        /// <param name="language"></param>
        /// <param name="subFolder"></param>
        /// <param name="parent_path"></param>
        /// <param name="work_unit"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        public static List<WwiseObject> ImportSoundFromFolder(string folder_path, string language = "SFX", string subFolder = "", string parent_path = @"\Actor-Mixer Hierarchy\Default Work Unit") // 从指定文件夹路径导入
        {
            if (!TryConnectWaapi()) return null; // 没有成功连接时返回空的WwiseObject List

            try
            {
                List<WwiseObject> results = new List<WwiseObject>();

                string[] files = Directory.GetFiles(folder_path);
                foreach (var f in files)
                {
                    if (!f.Contains(".wav")) continue;

                    var r = ImportSound(f, language, subFolder, parent_path);
                    results.Add(r);
                }
                Console.WriteLine($"File(s) in folder {folder_path} imported successfully!");
                return results;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to import from folder : {folder_path}! ======> {e.Message}");
                return null;
            }
        }


        /// <summary>
        /// 从指定路径导入音频
        /// </summary>
        /// <param name="file_path"></param>
        /// <param name="language"></param>
        /// <param name="sub_folder"></param>
        /// <param name="parent_path"></param>
        /// <param name="work_unit"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        public static WwiseObject ImportSound(string file_path, string language = "SFX", string sub_folder = "", string parent_path = @"\Actor-Mixer Hierarchy\Default Work Unit", string sound_name = "") // 直接调用的版本
        {
            Task<WwiseObject> obj = WwiseUtility.ImportSoundAsync(file_path, language, sub_folder, parent_path, sound_name);
            obj.Wait();
            return obj.Result;
        }

        /// <summary>
        /// 从指定路径导入音频，后台进行
        /// </summary>
        /// <param name="file_path"></param>
        /// <param name="language"></param>
        /// <param name="subFolder"></param>
        /// <param name="parent_path"></param>
        /// <param name="work_unit"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        public static async Task<WwiseObject> ImportSoundAsync(string file_path, string language = "SFX", string subFolder = "", string parent_path = @"\Actor-Mixer Hierarchy\Default Work Unit", string sound_name = "") // Async版本
        {
            if (!file_path.EndsWith(".wav") || !TryConnectWaapi()) return null; // 目标不是文件或者没有成功连接时返回空的WwiseObject
            
            
            
            string file_name = "";
            if (!string.IsNullOrEmpty(sound_name))
            {
                file_name = sound_name;
            }
            else
            {
                try
                {
                    file_name = Path.GetFileName(file_path).Replace(".wav", ""); // 尝试获取文件名
                }
                catch (IOException e)
                {
                    Console.WriteLine($"Failed to get file name from { file_path } ======> { e.Message }");
                    return null;
                }
            }

            try
            {
                var import_q = new JObject // 导入配置
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
                            new JProperty("audioFile", file_path),
                            new JProperty("objectPath", $"{parent_path}\\<Sound>{file_name}")
                        }
                    })
                };
                
                if (!String.IsNullOrEmpty(subFolder))
                {
                    (import_q["default"] as JObject).Add(new JProperty("originalsSubFolder", subFolder));
                }

                var options = new JObject(new JProperty("return", new string[] { "name", "id", "type", "path" })); // 设置返回参数

                var result = await Client.Call(ak.wwise.core.audio.import, import_q, options); // 执行导入

                if (result == null || result["objects"] == null || result["objects"].Last == null || result["objects"].Last["id"] == null) return null;
                
                Console.WriteLine($"File {file_path} imported successfully!");

                return await GetWwiseObjectByIDAsync(result["objects"].Last["id"].ToString());
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to import file : {file_path} ======> {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取工作单元文件路径
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static string GetWorkUnitFilePath(WwiseObject @object)
        {
            var get = GetWorkUnitFilePathAsync(@object);
            get.Wait();
            return get.Result;
        }

        /// <summary>
        /// 获取工作单元文件路径，同步执行
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static async Task<string> GetWorkUnitFilePathAsync(WwiseObject @object)
        {
            if (!TryConnectWaapi() || @object == null) return null;

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
                    JObject jresult = await Client.Call(ak.wwise.core.@object.get, query, options);

                    string file_path = "";
                    foreach (var obj in jresult["return"])
                    {
                        file_path = obj["filePath"].ToString();
                    }

                    Console.WriteLine($"Work Unit file path of object {@object.Name} successfully fetched!");

                    return file_path;
                }
                catch (Wamp.ErrorException e)
                {
                    Console.WriteLine($"Failed to return Work Unit file path of object : {@object.Name}! ======> {e.Message}");
                    return null;
                }
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to return Work Unit file path of object : {@object.Name}! ======> {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 重新加载当前工程
        /// </summary>
        public static void ReloadWwiseProject()
        {
            
            LoadWwiseProject(GetWwiseProjectPath(), true);
            Client = null;
            Init().Wait();
        }

        /// <summary>
        /// 加载工程
        /// </summary>
        /// <param name="path"></param>
        /// <param name="save_current"></param>
        public static void LoadWwiseProject(string path, bool save_current = true)
        {
            LoadWwiseProjectAsync(path, save_current).Wait();
        }

        /// <summary>
        /// 加载工程，同步执行
        /// </summary>
        /// <param name="path"></param>
        /// <param name="save_current"></param>
        /// <returns></returns>
        public static async Task LoadWwiseProjectAsync(string path, bool save_current = true)
        {
            if (!TryConnectWaapi()) return;

            if (save_current) SaveWwiseProject();

            var project_path = GetWwiseProjectPath();

            try
            {
                //await Client.Call(ak.wwise.ui.project.close);
                var query = new
                {
                    path = project_path
                };
                await Client.Call(ak.wwise.ui.project.open, query);

                Console.WriteLine("Project loaded successfully!");
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to load project! =======> {e.Message}");
            }
        }

        /// <summary>
        /// 获取工程路径
        /// </summary>
        /// <returns></returns>
        public static string GetWwiseProjectPath()
        {
            var get = GetWwiseProjectPathAsync();
            get.Wait();
            return get.Result;
        }
        
        /// <summary>
        /// 获取工程名称
        /// </summary>
        /// <returns></returns>
        public static string GetWwiseProjectName()
        {
            var get = GetWwiseProjectNameAsync();
            get.Wait();
            return get.Result;
        }

        /// <summary>
        /// 获取工程名称，同步执行
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetWwiseProjectNameAsync()
        {
            if (!TryConnectWaapi()) return null;

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
                    JObject jresult = await Client.Call(ak.wwise.core.@object.get, query, options);

                    string name = "";
                    foreach (var obj in jresult["return"])
                    {
                        name = obj["name"].ToString();
                    }

                    Console.WriteLine($"Project name successfully fetched!");

                    return name;
                }
                catch (Wamp.ErrorException e)
                {
                    Console.WriteLine($"Failed to return project name! ======> {e.Message}");
                    return null;
                }
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to return project name! ======> {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取工程路径，同步执行
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetWwiseProjectPathAsync()
        {
            if (!TryConnectWaapi()) return null;

            try
            {
                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    waql = "\"\\\""
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "filePath" }

                };



                try // 尝试返回物体数据
                {
                    JObject jresult = await Client.Call(ak.wwise.core.@object.get, query, options);

                    string file_path = "";
                    foreach (var obj in jresult["return"])
                    {
                        file_path = obj["filePath"].ToString();
                    }

                    Console.WriteLine($"Project path successfully fetched!");

                    return file_path;
                }
                catch (Wamp.ErrorException e)
                {
                    Console.WriteLine($"Failed to return project path! ======> {e.Message}");
                    return null;
                }
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to return project path! ======> {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 保存工程
        /// </summary>
        public static void SaveWwiseProject()
        {
            SaveWwiseProjectAsync().Wait();
        }

        /// <summary>
        /// 保存工程，同步执行
        /// </summary>
        /// <returns></returns>
        public static async Task SaveWwiseProjectAsync()
        {
            if (!TryConnectWaapi()) return;
            try
            {
                await Client.Call(ak.wwise.core.project.save);
                Console.WriteLine("Project saved successfully!");
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to save project! =======> {e.Message}");
            }
        }
        
    }
}
