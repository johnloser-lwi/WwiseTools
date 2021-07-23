using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// 连接初始化
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> Init() // 初始化，返回连接状态
        {
            if (Client != null) return Client.IsConnected();

            try
            {
                Client = new JsonClient();
                await Client.Connect(); // 尝试创建Wwise连接

                Console.WriteLine("Connected successfully!");

                Client.Disconnected += () =>
                {
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
        private static bool TryConnectWaapi() 
        {
            var connected = Init();
            connected.Wait();

            return connected.Result;
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
        /// 设置物体的引用，后台运行
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
        /// 设置参数，后台运行
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

                Console.WriteLine("Property changed successfully!");
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
        /// 修改名称，后台运行
        /// </summary>
        /// <param name="rename_object"></param>
        /// <param name="new_name"></param>
        /// <returns></returns>
        public static async Task ChangeObjectNameAsync(WwiseObject rename_object, string new_name)
        {
            if(!TryConnectWaapi() || rename_object == null || String.IsNullOrEmpty(new_name)) return;

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

                Console.WriteLine("Renamed successfully!");
            }

            catch (AK.Wwise.Waapi.Wamp.ErrorException e)
            {
                System.Console.Write($"Failed to rename object : {rename_object.Name} ======> {e.Message}");
            }
        }

        /// <summary>
        /// 将物体移动至指定父物体
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
        /// 将物体移动至指定父物体，后台进行
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

                try // 尝试更新子物体数据
                {
                    child.Name = jresult["return"].Last["name"].ToString();
                    child.ID = jresult["return"].Last["id"].ToString();
                    child.Type = jresult["return"].Last["type"].ToString();
                }
                catch
                {
                    Console.WriteLine("Failed to update WwiseObject!");
                }

                Console.WriteLine("Move completed!");
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
        /// 生成播放事件，后台运行
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

                Console.WriteLine("Event created successfully!");
                return await GetWwiseObjectByIDAsync(result["id"].ToString());
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to created play event : {event_name}! ======> {e.Message} ");
                return null;
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
                        new JProperty("onNameConflict", "rename"),
                    },
                    null
                    );
                return await GetWwiseObjectByIDAsync(result["id"].ToString());
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to create object : {object_name}! ======> {e.Message}");
                return null;
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
        /// 通过ID搜索物体，后台运行
        /// </summary>
        /// <param name="target_id"></param>
        /// <returns></returns>
        public static async Task<WwiseObject> GetWwiseObjectByIDAsync(string target_id)
        {

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

                    @return = new string[] { "name", "id", "type", "path" }

                };

                JObject jresult = await Client.Call(ak.wwise.core.@object.get, query, options);

                try // 尝试返回物体数据
                {
                    string name = jresult["return"].Last["name"].ToString();
                    string id = jresult["return"].Last["id"].ToString();
                    string type = jresult["return"].Last["type"].ToString();
                    string path = jresult["return"].Last["path"].ToString();

                    Console.WriteLine("File imported successfully!");

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

        /// <summary>
        /// 通过名称搜索物体，格式必须为"type:name
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
        /// 通过名称搜索物体，后台运行，格式必须为"type:name"
        /// </summary>
        /// <param name="target_name"></param>
        /// <returns></returns>
        public static async Task<WwiseObject> GetWwiseObjectByNameAsync(string target_name)
        {
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

                JObject jresult = await Client.Call(ak.wwise.core.@object.get, query, options);

                try // 尝试返回物体数据
                {
                    string name = jresult["return"].Last["name"].ToString();
                    string id = jresult["return"].Last["id"].ToString();
                    string type = jresult["return"].Last["type"].ToString();
                    string path = jresult["return"].Last["path"].ToString();

                    Console.WriteLine("File imported successfully!");

                    return new WwiseObject(name, id, type);
                }
                catch
                {
                    Console.WriteLine($"Failed to return WwiseObject by name : {target_name}!");
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
        public static WwiseObject ImportSound(string file_path, string language = "SFX", string sub_folder = "", string parent_path = @"\Actor-Mixer Hierarchy\Default Work Unit") // 直接调用的版本
        {
            Task<WwiseObject> obj = WwiseUtility.ImportSoundAsync(file_path, language, sub_folder, parent_path);
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
        public static async Task<WwiseObject> ImportSoundAsync(string file_path, string language = "SFX", string subFolder = "", string parent_path = @"\Actor-Mixer Hierarchy\Default Work Unit") // Async版本
        {
            if (!file_path.EndsWith(".wav") || !TryConnectWaapi()) return null; // 目标不是文件或者没有成功连接时返回空的WwiseObject

            string file_name = "";
            try
            {
                file_name = Path.GetFileName(file_path).Replace(".wav", ""); // 尝试获取文件名
            }
            catch (IOException e)
            {
                Console.WriteLine($"Failed to get file name from { file_path } ======> { e.Message }");
                return null;
            }

            try
            {
                var import_q = new JObject // 导入配置
                {
                    new JProperty("importOperation", "useExisting"),
                    
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



                return await GetWwiseObjectByIDAsync(result["objects"].Last["id"].ToString());
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to import file : {file_path} ======> {e.Message}");
                return null;
            }
        }
        
    }
}
