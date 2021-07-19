using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

using AK.Wwise.Waapi;
using System.Threading.Tasks;

namespace WwiseTools.Utils
{
    /// <summary>
    /// 默认功能，负责初始化路径以及常用的转换等
    /// </summary>
    public class WwiseUtility
    {
        static JsonClient client;
        public static async Task Init() 
        {
            if (client != null) return;

            try
            {
                client = new JsonClient();
                await client.Connect();

                Console.WriteLine("Connection Completed!");

                client.Disconnected += () =>
                {
                    System.Console.WriteLine("We lost connection!");
                };
            }
            catch
            {
                Console.WriteLine("Connection Failed!");
            }
        }

        public static async Task Close()
        {
            if (client == null) return;

            await client.Close();
        }

        public static WwiseObject ImportSound(string file_path, string language = "SFX", string subFolder = "", string parent_path = "", string work_unit = "Default Work Unit", string hierarchy = "Actor-Mixer Hierarchy")
        {
            Task<WwiseObject> obj = WwiseUtility.ImportSoundAsync(@"D:\\BGM\\Login\\denglu_bpm120_4_4_1.wav", "SFX", "UI", "<Folder>TEST");
            obj.Wait();
            return obj.Result;
        }


        public static async Task<WwiseObject> ImportSoundAsync(string file_path, string language = "SFX", string subFolder = "", string parent_path = "", string work_unit = "Default Work Unit", string hierarchy = "Actor-Mixer Hierarchy")
        {
            Init().Wait();

            if (!file_path.EndsWith(".wav")) return new WwiseObject(null, null, null);

            string file_name = "";
            try
            {
                file_name = Path.GetFileName(file_path).Replace(".wav", "");
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return new WwiseObject(null, null, null);
            }

            try
            {
                

                var import_q = new JObject
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


                var options = new JObject(
                        new JProperty("return", new string[] { "name", "id", "type" }));

                var result = await client.Call(
                    ak.wwise.core.audio.import, import_q, options);

                

                try
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
                Console.WriteLine($"Failed to import file : {file_path} =======>" + e.Message);
                return new WwiseObject(null, null, null);
            }
        }
        
    }
}
