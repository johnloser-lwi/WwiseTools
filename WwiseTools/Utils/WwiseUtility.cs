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

        public static WwiseObject ImportSound(string file_path, string language = "SFX", string subFolder = "", string parent_path = "", string work_unit = "Default Work Unit", string hierarchy = "Actor-Mixer Hierarchy") // 直接调用的版本
        {
            Task<WwiseObject> obj = WwiseUtility.ImportSoundAsync(@"D:\\BGM\\Login\\denglu_bpm120_4_4_1.wav", "SFX", "UI", "<Folder>TEST");
            obj.Wait();
            return obj.Result;
        }


        public static async Task<WwiseObject> ImportSoundAsync(string file_path, string language = "SFX", string subFolder = "", string parent_path = "", string work_unit = "Default Work Unit", string hierarchy = "Actor-Mixer Hierarchy") // Async版本
        {
            var connected = Init();
            connected.Wait(); // 检查连接状态

            if (!file_path.EndsWith(".wav") || !connected.Result) return new WwiseObject(null, null, null); // 目标不是文件或者没有成功连接时返回空的WwiseObject

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
