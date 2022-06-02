using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.References;

namespace WwiseTools.Utils
{
    public partial class WwiseUtility
    {
        /// <summary>
        /// 获取属性、引用名称
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public string GetPropertyAndReferenceNames(WwiseObject wwiseObject)
        {
            if (!TryConnectWaapi() || wwiseObject == null) return "";

            var get = GetPropertyAndReferenceNamesAsync(wwiseObject);
            get.Wait();
            return get.Result;
        }


        /// <summary>
        /// 设置物体的引用
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="wwiseReference"></param>
        [Obsolete("Use async version instead")]
        public void SetObjectReference(WwiseObject wwiseObject, WwiseReference wwiseReference)
        {
            if (!TryConnectWaapi() || wwiseObject == null || wwiseReference == null) return;
            var setRef = SetObjectReferenceAsync(wwiseObject, wwiseReference);
            setRef.Wait();
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="wwiseProperty"></param>
        [Obsolete("Use async version instead")]
        public void SetObjectProperty(WwiseObject wwiseObject, WwiseProperty wwiseProperty)
        {
            if (!TryConnectWaapi() || wwiseObject == null || wwiseProperty == null) return;

            var setProp = SetObjectPropertyAsync(wwiseObject, wwiseProperty);
            setProp.Wait();

        }

        /// <summary>
        /// 修改名称
        /// </summary>
        /// <param name="renameObject"></param>
        /// <param name="newName"></param>
        [Obsolete("Use async version instead")]
        public void ChangeObjectName(WwiseObject renameObject, string newName)
        {
            if (!TryConnectWaapi() || renameObject == null || String.IsNullOrEmpty(newName)) return;
            var changeName = ChangeObjectNameAsync(renameObject, newName);
            changeName.Wait();
        }

        /// <summary>
        /// 将物体移动至指定父物体
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        [Obsolete("Use async version instead")]
        public void CopyToParent(WwiseObject child, WwiseObject parent)
        {
            if (!TryConnectWaapi() || child == null || parent == null) return;

            var copy = CopyToParentAsync(child, parent);
            copy.Wait();
        }

        
        /// <summary>
        /// 将物体复制至指定父物体
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        [Obsolete("Use async version instead")]
        public void MoveToParent(WwiseObject child, WwiseObject parent)
        {
            if (!TryConnectWaapi() || child == null || parent == null) return;

            var move = MoveToParentAsync(child, parent);
            move.Wait();
        }
        
        [Obsolete("Use async version instead")]
        public void SetNote(WwiseObject target, string note)
        {
            if (!TryConnectWaapi() || target == null) return;

            var set = SetNoteAsync(target, note);
            set.Wait();
        }
        
        /// <summary>
        /// 生成播放事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="wwiseObject"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public WwiseObject CreatePlayEvent(string eventName, string objectPath, string parentPath = @"\Events\Default Work Unit")
        {
            if (!TryConnectWaapi()) return null;
            var evt = AddEventActionAsync(eventName, objectPath, parentPath);
            evt.Wait();
            return evt.Result;
        }
        
        [Obsolete("Use async version instead")]
        public WwiseObject AddEventAction(string eventName, string objectPath,
            string parentPath = @"\Events\Default Work Unit", int actionType = 1)
        {
            if (!TryConnectWaapi()) return null;
            var evt = AddEventActionAsync(eventName, objectPath, parentPath, actionType);
            evt.Wait();
            return evt.Result;
        }

        
        /// <summary>
        /// 创建物体
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="objectType"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public WwiseObject CreateObject(string objectName, WwiseObject.ObjectType objectType, string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit")
        {
            var obj = CreateObjectAsync(objectName, objectType, parentPath);
            obj.Wait();
            return obj.Result;
        }

        

        [Obsolete("Use async version instead")]
        public void DeleteObject(string path)
        {
            var obj = DeleteObjectAsync(path);
            obj.Wait();
        }
        

        /// <summary>
        /// 通过ID搜索物体
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public WwiseObject GetWwiseObjectByID(string targetId)
        {
            var get = GetWwiseObjectByIDAsync(targetId);
            get.Wait();
            return get.Result;
        }
        

        [Obsolete("Use async version instead")]
        public JToken GetWwiseObjectProperty(string targetId, string wwiseProperty)
        {
            var get = GetWwiseObjectPropertyAsync(targetId, wwiseProperty);
            get.Wait();
            return get.Result;
        }

        
        [Obsolete("Use async version instead")]
        public string GetWwiseObjectPath(string ID)
        {
            var r = GetWwiseObjectPathAsync(ID);
            r.Wait();
            return r.Result;
        }
        
        /// <summary>
        /// 通过名称与类型检索对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Obsolete]
        public List<WwiseObject> GetWwiseObjectsByNameAndType(string name, string type)
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
        public List<WwiseObject> GetWwiseObjectsByTypeAndParent(string type, string parentPath)
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
        public WwiseObject GetWwiseObjectByName(string targetName)
        {

            var get = GetWwiseObjectByNameAsync(targetName);
            get.Wait();
            return get.Result;
        }
        
        /// <summary>
        /// 通过路径获取对象
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public WwiseObject GetWwiseObjectByPath(string path)
        {

            var get = GetWwiseObjectByPathAsync(path);
            get.Wait();
            return get.Result;
        }
        

        /// <summary>
        /// 获取指定类型的对象
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public List<WwiseObject> GetWwiseObjectsOfType(string targetType)
        {

            var get = GetWwiseObjectsOfTypeAsync(targetType);
            get.Wait();
            return get.Result;
        }
        
        [Obsolete("Use async version instead")]
        public List<WwiseObject> GetWwiseObjectsBySelection()
        {
            var get = GetWwiseObjectsBySelectionAsync();
            get.Wait();
            return get.Result;
        }

        [Obsolete("Use async version instead")]
        public List<string> GetLanguages()
        {
            var result = GetLanguagesAsync();
            result.Wait();
            return result.Result;
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
        public List<WwiseObject> ImportSoundFromFolder(string folderPath, string language = "SFX", string subFolder = "", string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit") // 从指定文件夹路径导入
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
        public WwiseObject ImportSound(string filePath, string language = "SFX", string subFolder = "", string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit", string soundName = "") // 直接调用的版本
        {
            Task<WwiseObject> obj = WwiseUtility.Instance.ImportSoundAsync(filePath, language, subFolder, parentPath, soundName);
            obj.Wait();
            return obj.Result;
        }
        

        /// <summary>
        /// 获取工作单元文件路径
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public string GetWorkUnitFilePath(WwiseObject @object)
        {
            var get = GetWorkUnitFilePathAsync(@object);
            get.Wait();
            return get.Result;
        }
        
        /// <summary>
        /// 重新加载当前工程
        /// </summary>
        [Obsolete("Use async version instead")]
        public void ReloadWwiseProject()
        {

            LoadWwiseProject(GetWwiseProjectPath(), true);
            Client = null;
            Init().Wait();
        }
        
        /// <summary>
        /// 加载工程
        /// </summary>
        /// <param name="path"></param>
        /// <param name="saveCurrent"></param>
        [Obsolete("Use async version instead")]
        public void LoadWwiseProject(string path, bool saveCurrent = true)
        {
            LoadWwiseProjectAsync(path, saveCurrent).Wait();
        }
        
        /// <summary>
        /// 获取工程路径
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public string GetWwiseProjectName()
        {
            var get = GetWwiseProjectNameAsync();
            get.Wait();
            return get.Result;
        }
        
        /// <summary>
        /// 获取工程路径
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use async version instead")]
        public string GetWwiseProjectPath()
        {
            var get = GetWwiseProjectPathAsync();
            get.Wait();
            return get.Result;
        }
        
        
        /// <summary>
        /// 保存工程
        /// </summary>
        [Obsolete("Use async version instead")]
        public void SaveWwiseProject()
        {
            SaveWwiseProjectAsync().Wait();
        }

        [Obsolete]
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

                JObject jresult = await Client.Call(func, query, options, TimeOut);


                if (jresult["return"] == null) throw new Exception();

                WaapiLog.Log($"WwiseProperty {wwiseProperty} successfully fetched!");
                return jresult["return"].Last?["@" + wwiseProperty];
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to return WwiseObject Property : {targetId}! ======> {e.Message}");
                return null;
            }

        }

    }
}
