﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

using AK.Wwise.Waapi;
using System.Threading.Tasks;

namespace WwiseTools.Utils
{
    /// <summary>
    /// 用于实现基础功能
    /// </summary>
    public class WwiseUtility
    {
        static JsonClient client;

        /// <summary>
        /// 连接初始化
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> Init() // 初始化，返回连接状态
        {
            if (client != null) return client.IsConnected();

            try
            {
                client = new JsonClient();
                await client.Connect(); // 尝试创建Wwise连接

                Console.WriteLine("Connected successfully!");

                client.Disconnected += () =>
                {
                    System.Console.WriteLine("Connection closed!"); // 丢失连接提示
                };

                return true;
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Connection Failed! ======> {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        public static async Task Close()
        {
            if (client == null || !client.IsConnected()) return;

            try
            {
                await client.Close(); // 尝试断开连接
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine(e.Message);
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
        /// 将物体移动至指定父物体
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        public static void MoveToParent(WwiseObject child, WwiseObject parent)
        {
            if (!TryConnectWaapi()) return;

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
            if (!TryConnectWaapi()) return;

            try
            {
                // 移动物体
                await client.Call(
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

                    @return = new string[] { "name", "id", "type", "path" }

                };

                // 获取子物体的新数据
                JObject jresult = await client.Call(ak.wwise.core.@object.get, query, options);

                try // 尝试更新子物体数据
                {
                    child.Name = jresult["return"].Last["name"].ToString();
                    child.ID = jresult["return"].Last["id"].ToString();
                    child.Type = jresult["return"].Last["type"].ToString();
                    child.Path = jresult["return"].Last["path"].ToString();
                }
                catch
                {
                    Console.WriteLine("Failed to update WwiseObject!");
                }

                Console.WriteLine("Move completed!");
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Move failed! ======> {e.Message}");
            }
        }

        /// <summary>
        /// 创建物体
        /// </summary>
        /// <param name="object_name"></param>
        /// <param name="object_type"></param>
        /// <param name="parent_path"></param>
        /// <returns></returns>
        public static WwiseObject CreateObject(string object_name, string object_type, string parent_path = @"\Actor-Mixer Hierarchy\Default Work Unit")
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
        public static async Task<WwiseObject> CreateObjectAsync(string object_name, string object_type, string parent_path = @"\Actor-Mixer Hierarchy\Default Work Unit")
        {
            if (!TryConnectWaapi()) return null;

            try
            {
                // 创建物体
                var result = await client.Call
                    (
                    ak.wwise.core.@object.create,
                    new JObject
                    {
                        new JProperty("name", object_name),
                        new JProperty("type", object_type),
                        new JProperty("parent", parent_path),
                        new JProperty("onNameConflict", "rename")
                    },
                    null
                    );

                // ak.wwise.core.@object.get 指令
                var query = new
                {
                    from = new
                    {
                        id = new string[] { result["id"].ToString() }
                    }
                };

                // ak.wwise.core.@object.get 返回参数设置
                var options = new
                {

                    @return = new string[] { "name", "id", "type", "path" }

                };

                JObject jresult = await client.Call(ak.wwise.core.@object.get, query, options);

                try // 尝试返回物体数据
                {
                    string name = jresult["return"].Last["name"].ToString();
                    string id = jresult["return"].Last["id"].ToString();
                    string type = jresult["return"].Last["type"].ToString();
                    string path = jresult["return"].Last["path"].ToString();

                    Console.WriteLine("File imported successfully!");

                    return new WwiseObject(name, id, type, path);
                }
                catch
                {
                    Console.WriteLine("Failed to return WwiseObject!");
                    return null;
                }
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Object failed to create! ======> {e.Message}");
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
                Console.WriteLine($"Failed to import from folder! ======> {e.Message}");
                return null;
            }
        }


        /// <summary>
        /// 从指定路径导入音频
        /// </summary>
        /// <param name="file_path"></param>
        /// <param name="language"></param>
        /// <param name="subFolder"></param>
        /// <param name="parent_path"></param>
        /// <param name="work_unit"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        public static WwiseObject ImportSound(string file_path, string language = "SFX", string subFolder = "", string parent_path = @"\Actor-Mixer Hierarchy\Default Work Unit") // 直接调用的版本
        {
            Task<WwiseObject> obj = WwiseUtility.ImportSoundAsync(file_path, language, subFolder, parent_path);
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
                Console.WriteLine(e.Message);
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

                var result = await client.Call(ak.wwise.core.audio.import, import_q, options); // 执行导入

                

                try // 尝试返回物体数据
                {
                    string name = result["objects"].Last["name"].ToString();
                    string id = result["objects"].Last["id"].ToString();
                    string type = result["objects"].Last["type"].ToString();
                    string path = result["objects"].Last["path"].ToString();

                    Console.WriteLine("File imported successfully!");

                    return new WwiseObject(name, id, type, path);
                }
                catch
                {
                    Console.WriteLine("Failed to return WwiseObject!");
                    return null;
                }
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to import file : {file_path} ======> {e.Message}");
                return null;
            }
        }
        
    }
}
