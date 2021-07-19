using System;
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
        /// 从指定文件夹导入音频
        /// </summary>
        /// <param name="folder_path"></param>
        /// <param name="language"></param>
        /// <param name="subFolder"></param>
        /// <param name="parent_path"></param>
        /// <param name="work_unit"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        public static List<WwiseObject> ImportSoundFromFolder(string folder_path, string language = "SFX", string subFolder = "", string parent_path = "", string work_unit = "Default Work Unit", string hierarchy = "Actor-Mixer Hierarchy") // 从指定文件夹路径导入
        {
            if (!TryConnectWaapi()) return null; // 没有成功连接时返回空的WwiseObject List

            try
            {
                List<WwiseObject> results = new List<WwiseObject>();

                string[] files = Directory.GetFiles(folder_path);
                foreach (var f in files)
                {
                    if (!f.Contains(".wav")) continue;

                    var r = ImportSound(f, language, subFolder, parent_path, work_unit, hierarchy);
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
        public static WwiseObject ImportSound(string file_path, string language = "SFX", string subFolder = "", string parent_path = "", string work_unit = "Default Work Unit", string hierarchy = "Actor-Mixer Hierarchy") // 直接调用的版本
        {
            Task<WwiseObject> obj = WwiseUtility.ImportSoundAsync(file_path, language, subFolder, parent_path, work_unit, hierarchy);
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
        public static async Task<WwiseObject> ImportSoundAsync(string file_path, string language = "SFX", string subFolder = "", string parent_path = "", string work_unit = "Default Work Unit", string hierarchy = "Actor-Mixer Hierarchy") // Async版本
        {
            if (!file_path.EndsWith(".wav") || !TryConnectWaapi()) return new WwiseObject(null, null, null); // 目标不是文件或者没有成功连接时返回空的WwiseObject

            string file_name = "";
            try
            {
                file_name = Path.GetFileName(file_path).Replace(".wav", ""); // 尝试获取文件名
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return new WwiseObject(null, null, null);
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
                            new JProperty("objectPath", $"\\{hierarchy}\\{work_unit}\\{parent_path}\\<Sound>{file_name}")
                        }
                    })
                };
                
                if (!String.IsNullOrEmpty(subFolder))
                {
                    (import_q["default"] as JObject).Add(new JProperty("originalsSubFolder", subFolder));
                }

                var options = new JObject(new JProperty("return", new string[] { "name", "id", "type" })); // 设置返回参数

                var result = await client.Call(ak.wwise.core.audio.import, import_q, options); // 执行导入

                

                try // 尝试返回物体数据
                {
                    string name = result["objects"].Last["name"].ToString();
                    string id = result["objects"].Last["id"].ToString();
                    string type = result["objects"].Last["type"].ToString();

                    Console.WriteLine("File imported successfully!");

                    return new WwiseObject(name, id, type);
                }
                catch
                {
                    Console.WriteLine("Failed to return WwiseObject!");
                    return new WwiseObject(null, null, null);
                }
            }
            catch (Wamp.ErrorException e)
            {
                Console.WriteLine($"Failed to import file : {file_path} ======> {e.Message}");
                return new WwiseObject(null, null, null);
            }
        }
        
    }
}
