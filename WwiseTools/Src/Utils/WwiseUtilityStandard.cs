#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WwiseTools.Models;
using WwiseTools.Models.Import;
using WwiseTools.Objects;
using WwiseTools.Properties;
using WwiseTools.Serialization;
using WwiseTools.Utils.Feature2022;

namespace WwiseTools.Utils
{
    public  partial class WwiseUtility
    {
        public async Task<string[]> GetPropertyAndReferenceNamesAsync(WwiseObject wwiseObject)
        {
          

            if (!await TryConnectWaapiAsync()) return new string[]{};

            try
            {
                var func = Function.Verify("ak.wwise.core.object.getPropertyAndReferenceNames");
                var result = await _client.Call(func,

                    new JObject(

                        new JProperty("object", wwiseObject.ID)),

                    null);
                WaapiLog.InternalLog("Property and References fetched successfully!");

                var returnData = WaapiSerializer.Deserialize<ReturnData<string>>(result.ToString());

                return returnData.Return;


            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to fetch Property and References! ======> {e.Message}");
                return new string[]{};
            }
        }
        

        private async Task<bool> SetObjectReferenceAsync(WwiseObject wwiseObject, WwiseProperty wwiseProperty)
        {
            if (!await TryConnectWaapiAsync()) return false;
            if (!wwiseProperty.IsReference) return false;
            
            try
            {
                var func = Function.Verify("ak.wwise.core.object.setReference");
                await _client.Call(func,

                    new JObject(

                        new JProperty("object", wwiseObject.ID),

                        new JProperty("reference", wwiseProperty.Name),

                        new JProperty("value", wwiseProperty.Value)),

                    null);

                WaapiLog.InternalLog("Reference set successfully!");

                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to set reference \"{wwiseProperty.Name}\" to object {wwiseObject.Name} ======> {e.Message}");
            }

            return false;
        }


        public async Task<bool> SetObjectPropertiesAsync(WwiseObject wwiseObject, params WwiseProperty[] properties)
        {
            if (!await TryConnectWaapiAsync()) return false;

            var ret = true;
            
            foreach (var property in properties)
            {
                var res = await SetObjectPropertyAsync(wwiseObject, property);

                if (!res) ret = res;
            }

            return ret;
        }
        
      
      
      
      
      
      
      
        public async Task<bool> SetObjectPropertyAsync(WwiseObject wwiseObject, WwiseProperty wwiseProperty)
        {
            if (!await TryConnectWaapiAsync()) return false;

            if (wwiseProperty.IsReference) return await SetObjectReferenceAsync(wwiseObject, wwiseProperty);
            
            try
            {
                var list = await GetPropertyAndReferenceNamesAsync(wwiseObject);
                if (!list.Contains(wwiseProperty.Name)) return false;
                
                
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

        public async Task<bool> CopyObjectPropertiesAsync(WwiseObject source, WwiseObject[] targets, params string[] properties)
        {
            if (!await TryConnectWaapiAsync()) return false;

            bool ret = true;
            
            try
            {
                if (ConnectionInfo.Version >= VersionHelper.V2022_1_0_7929)
                    return await Instance.PastePropertiesAsync(source, targets, WwiseUtility2022Extension.PasteMode.replaceEntire, true, properties);


                var propertyList = new List<WwiseProperty>();
                for (var i = 0; i < properties.Length; i++)
                {
                    var prop = properties[i];

                    var val = await GetWwiseObjectPropertyAsync(source, prop);
                    
                    if (val == null) continue;
                    
                    propertyList.Add(val);
                }
                
                for (var i = 0; i < targets.Length; i++)
                {
                    var target = targets[i];

                    var availableProps = (await GetPropertyAndReferenceNamesAsync(target)).ToList();

                    var res = await SetObjectPropertiesAsync(target, propertyList.Where(p => availableProps.Contains(p.Name)).ToArray());

                    if (!res) ret = false;
                }

            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to copy properties from {source.Name} to {targets.Length} target(s) ======> {e.Message}");
            }

            return ret;
        }
        

      
      
      
      
      
      
        public async Task<bool> ChangeObjectNameAsync(WwiseObject renameObject, string newName)
        {
            if (!await TryConnectWaapiAsync() || String.IsNullOrEmpty(newName)) return false;

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
        
      
      
      
      
      
      
        public async Task<bool> CopyToParentAsync(WwiseObject child, WwiseObject parent, NameConflictBehaviour conflictBehaviour = NameConflictBehaviour.rename)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.copy");
                
              
                await _client.Call(func
                    ,
                    new JObject
                    {
                        new JProperty("object", child.ID),
                        new JProperty("parent", parent.ID),
                        new JProperty("onNameConflict", conflictBehaviour.ToString())
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

        

      
      
      
      
      
      
        public async Task<bool> MoveToParentAsync(WwiseObject child, WwiseObject parent, NameConflictBehaviour conflictBehaviour = NameConflictBehaviour.rename)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.move");
                
              
                await _client.Call(func,
                    new JObject
                    {
                        new JProperty("object", child.ID),
                        new JProperty("parent", parent.ID),
                        new JProperty("onNameConflict", conflictBehaviour.ToString())
                    },
                    null,
                    TimeOut
                    );
                
                WaapiLog.InternalLog($"Moved {child.Name} to {parent.Name}!");

                await child.GetPathAsync();

                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to move {child.Name} to {parent.Name}! ======> {e.Message}");
            }

            return false;
        }

        public async Task<string> GetNotesAsync(WwiseObject target)
        {
            if (!await TryConnectWaapiAsync()) return "";

            try
            {
                var returnData = await CoreObjectGetAsync(target.ID, "notes");
                if (returnData.Return.Length == 0) throw new Exception("Object not found!");
                
                return returnData.Return[0].Notes;

            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to fetch notes! ======> {e.Message}");
                return "";
            }
        }

        public async Task<bool> SetNoteAsync(WwiseObject target, string note)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.setNotes");
              
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

      
      
      
      
      
      
      
        [Obsolete("Use Event component instead!")]
        public async Task<WwiseObject?> CreatePlayEventAsync(string eventName, string objectPath, string parentPath = @"\Events\Default Work Unit")
        {
            if (!await TryConnectWaapiAsync()) return null;
            return await AddEventActionAsync(eventName, objectPath, parentPath);
        }
        

      
      
      
      
      
      
      
        [Obsolete("Use Event component instead!")]
        public async Task<WwiseObject?> AddEventActionAsync(string eventName, string objectPath, string parentPath = @"\Events\Default Work Unit", 
            WwiseProperty.Option_ActionType actionType = WwiseProperty.Option_ActionType.Play, NameConflictBehaviour conflictBehaviour = NameConflictBehaviour.merge)
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
                            new JProperty("onNameConflict", conflictBehaviour.ToString()),
                            new JProperty("children", new JArray
                            {
                                new JObject
                                {
                                    new JProperty("name", ""),
                                    new JProperty("type", "Action"),
                                    new JProperty("@ActionType", actionType.ToString()),
                                    new JProperty("@Target", objectPath)
                                }
                            })
                        },
                        null,
                        TimeOut
                    );

                WaapiLog.InternalLog($"Event {eventName} created successfully!");

                var returnData = WaapiSerializer.Deserialize<GuidIdObjectData>(result.ToString());

                if (string.IsNullOrEmpty(returnData.ID)) throw new Exception();
                return await GetWwiseObjectByIDAsync(returnData.ID);
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to created play event : {eventName}! ======> {e.Message} ");
                return null;
            }
        }

        [Obsolete("Use AddSoundBankInclusionAsync instead")]
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

        public async Task<WwiseObject?> CreateObjectAsync(string objectName, WwiseObject.ObjectType objectType, WwiseObject parent, 
            NameConflictBehaviour conflictBehaviour = NameConflictBehaviour.fail,
            params WwiseProperty[] properties)
        {
            if(!await TryConnectWaapiAsync()) return null;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.create");

              
                var result = await _client.Call
                (
                    func,
                    new JObject
                    {
                        new JProperty("name", objectName),
                        new JProperty("type", objectType.ToString()),
                        new JProperty("parent", parent.ID),
                        new JProperty("onNameConflict", conflictBehaviour.ToString())
                    },
                    null
                );


                var returnData = WaapiSerializer.Deserialize<GuidIdObjectData>(result.ToString());

                var ret = await GetWwiseObjectByIDAsync(returnData.ID);

                if (ret == null) return null;
                
                WaapiLog.InternalLog($"Object {ret.Name} created successfully!");
                
                foreach (var wwiseProperty in properties)
                {
                    await SetObjectPropertyAsync(ret, wwiseProperty);
                }
                
                return ret;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to create object : {objectName}! ======> {e.Message}");
                return null;
            }
        }

      
      
      
      
      
      
      
        public async Task<WwiseObject?> CreateObjectAtPathAsync(string objectName, WwiseObject.ObjectType objectType, 
            string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit", NameConflictBehaviour conflictBehaviour = NameConflictBehaviour.fail,
            params WwiseProperty[] properties)
        {
            if (!await TryConnectWaapiAsync()) return null;

            var parent = await WwiseUtility.Instance.GetWwiseObjectByPathAsync(parentPath);

            if (parent == null) return null;
            
            return await CreateObjectAsync(objectName, objectType,
                parent, conflictBehaviour, properties);
        }

        public async Task<bool> DeleteObjectAsync(WwiseObject wwiseObject)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {
                var func = Function.Verify("ak.wwise.core.object.delete");

              
                var result = await _client.Call
                    (
                        func,
                    new JObject
                    {
                        new JProperty("object", wwiseObject.ID)
                    },
                    null
                    );

                WaapiLog.InternalLog($"Object {wwiseObject.Name} deleted successfully!");
                return true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to delete object : {wwiseObject.Name}! ======> {e.Message}");
                
            }

            return false;
        }

        
      
      
      
      
      
        public async Task<WwiseObject?> GetWwiseObjectByIDAsync(string targetId)
        {
            if (!await TryConnectWaapiAsync() || String.IsNullOrWhiteSpace(targetId)) return null;

            try
            {
                var func = WaapiFunctionList.CoreObjectGet;

              
                var query = new
                {
                    from = new
                    {
                        id = new string[] { targetId }
                    }
                };
                return await GetSingleWwiseObjectAsync(query);

            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject from ID : {targetId}! ======> {e.Message}");
                return null;
            }
        }

        public async Task<ReturnData<PropertyData>> BatchGetWwiseObjectProperties(string[] ids, string[] properties)
        {
            return await WwiseUtility.Instance.CoreObjectGetAsync<PropertyData>(ids, 
                new string[]{}, properties);
        }

        public async Task<JToken?> GetWwiseObjectPropertyByIDAsync(string targetId, string wwiseProperty)
        {
            if (!await TryConnectWaapiAsync() || String.IsNullOrWhiteSpace(targetId)) return null;

            try
            {
                var func = WaapiFunctionList.CoreObjectGet;

              
                var query = new
                {
                    from = new
                    {
                        id = new string[] { targetId }
                    }
                };

              
                var options = new
                {

                    @return = new string[] { "@" + wwiseProperty }  

                };

                JObject jresult = await _client.Call(func, query, options, TimeOut);


                if (jresult["return"] == null) throw new Exception();

                WaapiLog.InternalLog($"WwiseProperty {wwiseProperty} successfully fetched!");
                return jresult["return"]!.Last?["@" + wwiseProperty];
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject Property : {targetId}! ======> {e.Message}");
                return null;
            }

        }

        public async Task<WwiseProperty?> GetWwiseObjectPropertyAsync(WwiseObject wwiseObject, string wwiseProperty)
        {
            if (!await TryConnectWaapiAsync()) return null;

            try
            {
                var jresult = await GetWwiseObjectPropertyByIDAsync(wwiseObject.ID, wwiseProperty);
                if (jresult is null) return null;
                bool isRef = jresult.HasValues && jresult["id"] != null;
                
                var ret = new WwiseProperty(wwiseProperty, isRef ? jresult["id"].ToString() : jresult.ToString());
                
                ret.IsReference = isRef;
                
                return ret;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject Property : {wwiseProperty}! ======> {e.Message}");
                return null;
            }

        }


        public async Task<string?> GetWwiseObjectPathAsync(string id)
        {
            if (!await TryConnectWaapiAsync() || string.IsNullOrEmpty(id)) return null;

            try
            {
                var returnData = await CoreObjectGetAsync(id, "path");
                if (returnData.Return.Length == 0) throw new Exception("Object not found!");
                string? path = returnData.Return[0].Path;

                return path;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get path of object : {id}! =======> {e.Message}");
                return null;
            }

        }
        
      
      
      
      
      
        public async Task<WwiseObject?> GetWwiseObjectByNameAsync(string targetName)
        {
            if (!await TryConnectWaapiAsync() || String.IsNullOrWhiteSpace(targetName)) return null;

            try
            {
              
                var query = new
                {
                    from = new
                    {
                        name = new string[] { targetName }
                    }
                };

                return await GetSingleWwiseObjectAsync(query);

            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject by name : {targetName}! ======> {e.Message}");
                return null;
            }

            return null;
        }
        
      
      
      
      
      
        public async Task<WwiseObject?> GetWwiseObjectByPathAsync(string path)
        {
            if (!await TryConnectWaapiAsync() || String.IsNullOrWhiteSpace(path)) return null;

            try
            {
              
                var query = new
                {
                    from = new
                    {
                        path = new string[] { path }
                    }
                };

                return await GetSingleWwiseObjectAsync(query);
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return WwiseObject by path : {path}! ======> {e.Message}");
                return null;
            }

        }

        private async Task<WwiseObject?> GetSingleWwiseObjectAsync(object query)
        {
            try
            {
                var func = WaapiFunctionList.CoreObjectGet;

              
                var options = new
                {

                    @return = new string[] { "name", "id", "type", "path" }

                };

                JObject jresult = await _client.Call(func, query, options, TimeOut);
                
                var returnData = WaapiSerializer.Deserialize<ReturnData<ObjectReturnData>>(jresult.ToString());
                if (returnData.Return.Length == 0) throw new Exception("Object not found!");

                var obj = returnData.Return[0];
                
                if (!string.IsNullOrEmpty(obj.ID) && !string.IsNullOrEmpty(obj.Type))
                {
                    WaapiLog.InternalLog($"WwiseObject {obj.Name} successfully fetched!");
                    return new WwiseObject(obj.Name, obj.ID, obj.Type, obj.Path);
                }
            }
            catch
            {
                throw;
            }

            return null;

        }

        public async Task<List<WwiseObject>> GetWwiseObjectsOfTypeAsync(WwiseObject.ObjectType targetType)
        {
            return await GetWwiseObjectsOfTypeAsync(targetType.ToString());
        }
        
      
      
      
      
      
        public async Task<List<WwiseObject>> GetWwiseObjectsOfTypeAsync(string targetType)
        {
            List<WwiseObject> result = new List<WwiseObject>();

            if (!await TryConnectWaapiAsync() || String.IsNullOrWhiteSpace(targetType)) return result;

            try
            {

              
                var query = new
                {
                    from = new
                    {
                        ofType = new string[] { targetType }
                    }
                };

              
                var options = new
                {

                    @return = new string[] { "name", "id", "type", "path" }

                };



                var func = WaapiFunctionList.CoreObjectGet;

                JObject jresult = await _client.Call(func, query, options, TimeOut);
                var returnData = WaapiSerializer.Deserialize<ReturnData<ObjectReturnData>>(jresult.ToString());
                
                if (returnData.Return.Length == 0) throw new Exception();
                result.AddRange(returnData.Objects.Select(o => new WwiseObject(o.Name, o.ID, o.Type, o.Path)));

                WaapiLog.InternalLog($"WwiseObject list of type {targetType} successfully fetched!");

                
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
              
                var options = new
                {

                    @return = new string[] { "name", "id", "type", "path" }

                };

                var func = Function.Verify("ak.wwise.ui.getSelectedObjects");

                
                JObject jresult = await _client.Call(func, null, options, TimeOut);
                var returnData = WaapiSerializer.Deserialize<ReturnData<ObjectReturnData>>(jresult.ToString());


                if (returnData.Objects.Length == 0) throw new Exception("No object found!");
                result.AddRange(returnData.Objects.Select(o => new WwiseObject(o.Name, o.ID, o.Type, o.Path)));

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

                var func = WaapiFunctionList.CoreObjectGet;

                var result = await _client.Call(func, query, options, TimeOut);

                var returnData = WaapiSerializer.Deserialize<ReturnData<CommonObjectData>>(result.ToString());

                if (returnData.Return.Length == 0) throw new Exception();
                foreach (var r in returnData.Return)
                {
                    if (string.IsNullOrEmpty(r.Name)) continue;
                    string name = r.Name;
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

            if (resultList.Count != 0 && ConnectionInfo != null) ConnectionInfo.Languages = resultList; 

            return resultList;
        }

      
      
      
      
      
      
      
      
      
        public async Task<WwiseObject?> ImportSoundAsync(string filePath, string language = "SFX", string subFolder = "", string parentPath = @"\Actor-Mixer Hierarchy\Default Work Unit", string soundName = "", ImportAction importAction = ImportAction.useExisting) // Async版本
        {
            var parent = await GetWwiseObjectByPathAsync(parentPath);

            if (parent == null) return null;
            
            return await ImportSoundAsync(parent, filePath, language, subFolder, soundName, importAction);
        }
        
        public async Task<WwiseObject?> ImportSoundAsync(WwiseObject parent, string filePath, string language = "SFX", string subFolder = "", string soundName = "", ImportAction importAction = ImportAction.useExisting)
        {
            if (string.IsNullOrEmpty(soundName))
            {
                soundName = Path.GetFileName(filePath).Replace(".wav", ""); // 尝试获取文件名
            }

            var objectPath = new WwisePathBuilder(parent);
            var res = await objectPath.AppendHierarchyAsync(WwiseObject.ObjectType.Sound, soundName);

            if (!res)
            {
                WaapiLog.InternalLog($"Failed to import {filePath}! Invalid parent!");
                return null;
            }
            
            ImportInfo info = ImportInfo.FromPathBuilder(filePath, objectPath, language, subFolder);

            return await ImportSoundAsync(info, importAction);
        }
        
        public async Task<WwiseObject?> ImportSoundAsync(ImportInfo info, ImportAction importAction = ImportAction.useExisting) // Async版本
        {
            if (!info.IsValid) return null; // 目标不是文件或者没有成功连接时返回空的WwiseObject
            

            try
            {
                var importQ = new JObject // 导入配置
                {
                    new JProperty("importOperation", importAction.ToString()),
                    
                    new JProperty("imports", new JArray
                    {
                        await info.ToJObjectImportPropertyAsync()
                    })
                };

                var options = new JObject(new JProperty("return", new object[] { "id", "name", "path", "type" })); // 设置返回参数

                var func = Function.Verify("ak.wwise.core.audio.import");

                var result = await _client.Call(func, importQ, options); // 执行导入

                var returnData = WaapiSerializer.Deserialize<ReturnData<ObjectReturnData>>(result.ToString());
                if (returnData.Objects.Length == 0) return null;
                var wo = returnData.Objects[0];
                return new WwiseObject(wo.Name, wo.ID, wo.Type, wo.Path);

                return null;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to import file : {info.AudioFile} ======> {e.Message}");
                return null;
            }
        }
        
        public async Task<List<WwiseObject>> ImportTabDelimitedAsync(string filePath, string importLanguage = "SFX", ImportAction importAction = ImportAction.useExisting)
        {
            var ret = new List<WwiseObject>();
            try
            {
                var importQ = new JObject // 导入配置
                {
                    new JProperty("importLanguage", importLanguage),
                    
                    new JProperty("importOperation", importAction.ToString()),
                    
                    new JProperty("importFile", filePath)
                };

                var options = new JObject(new JProperty("return", new object[] { "id", "path", "type", "name" })); // 设置返回参数

                var func = Function.Verify("ak.wwise.core.audio.importTabDelimited");

                var result = await _client.Call(func, importQ, options); // 执行导入
                var returnData = WaapiSerializer.Deserialize<ReturnData<ObjectReturnData>>(result.ToString());

                if (returnData.Objects.Length == 0) return ret;
                ret.AddRange(returnData.Objects.Select(o => new WwiseObject(o.Name, o.ID, o.Type, o.Path)));

                return ret;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to import tab delimited : {filePath} ======> {e.Message}");
                return ret;
            }
        }
        
        public async Task<List<WwiseObject>> BatchImportSoundAsync(ImportInfo[] infos, ImportAction importAction = ImportAction.useExisting) // Async版本
        {
            var ret = new List<WwiseObject>();
            try
            {
                JArray importArray = new JArray();
                
                for (var i = 0; i < infos.Length; i++)
                {
                    var info = infos[i];
                    if (!info.IsValid) continue;
                    
                    var jobjecty = await info.ToJObjectImportPropertyAsync();
                    importArray.Add(jobjecty);
                }
                
                var importQ = new JObject // 导入配置
                {
                    new JProperty("importOperation", importAction.ToString()),
                    
                    new JProperty("imports", importArray)
                };

                var options = new JObject(new JProperty("return", new object[] { "name", "id", "type", "path" })); // 设置返回参数

                var func = Function.Verify("ak.wwise.core.audio.import");

                var result = await _client.Call(func, importQ, options); // 执行导入

                var returnData = WaapiSerializer.Deserialize<ReturnData<ObjectReturnData>>(result.ToString());

                ret.AddRange(returnData.Return.Select(o => new WwiseObject(o.Name, o.ID, o.Type, o.Path)));
                
                return ret;

            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Batch import failed! ======> {e.Message}");
                ret.Clear();
                return ret;
            }
        }
        
      
      
      
      
      
        public async Task<string?> GetWorkUnitFilePathAsync(WwiseObject wwiseObject)
        {
            if (!await TryConnectWaapiAsync()) return null;

            try
            {
                var returnData = await CoreObjectGetAsync(wwiseObject.ID, "filePath");
                if (returnData.Return.Length == 0) throw new Exception("No object found!");
                string? filePath = returnData.Return[0].FilePath;

                WaapiLog.InternalLog($"Work Unit file path of object {wwiseObject.Name} successfully fetched!");

                return filePath;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return Work Unit file path of object : {wwiseObject.Name}! ======> {e.Message}");
                return null;
            }
        }
        
        public async Task<bool> ReloadWwiseProjectAsync()
        {
            var projectPath = ConnectionInfo.ProjectPath;
            if (string.IsNullOrEmpty(projectPath)) return false;
            await LoadWwiseProjectAsync(projectPath!, true);
            await DisconnectAsync();
            _client = null;
            return await ConnectAsync();
        }
        
      
      
      
      
      
      
        public async Task<bool> LoadWwiseProjectAsync(string path, bool saveCurrent = true)
        {
            if (!await TryConnectWaapiAsync()) return false;

            if (saveCurrent) await SaveWwiseProjectAsync();

            var projectPath = path;

            try
            {
              
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
        
      
      
      
      
        public async Task<string?> GetWwiseProjectNameAsync()
        {
            if (!await TryConnectWaapiAsync()) return null;

            try
            {
              
                var query = new
                {
                    from = new
                    {
                        ofType = new string[] { "Project" }
                    }
                };

              
                var options = new
                {

                    @return = new string[] { "name"}

                };

                var func = WaapiFunctionList.CoreObjectGet;

                JObject jresult = await _client.Call(func, query, options, TimeOut);
                
                var returnData = WaapiSerializer.Deserialize<ReturnData<CommonObjectData>>(jresult.ToString());

                if (returnData.Return.Length == 0) throw new Exception("No project found!");
                
                string name = returnData.Return[0].Name;
                
                WaapiLog.InternalLog($"Project name successfully fetched!");

                return name;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return project name! ======> {e.Message}");
                return null;
            }
        }
        
      
      
      
      
        public async Task<string?> GetWwiseProjectPathAsync()
        {
            if (!await TryConnectWaapiAsync()) return null;

            try
            {
               
                var func = Function.Verify("ak.wwise.core.getProjectInfo");

                JObject jresult = await _client.Call(func, null, null, TimeOut);
                
                var getProjectInfoData = WaapiSerializer.Deserialize<GetProjectInfoData>(jresult.ToString());
                
                if (getProjectInfoData.Path == null) throw new Exception("No project found!");
                
                string filePath = getProjectInfoData.Path;

                WaapiLog.InternalLog($"Project path successfully fetched!");

                return filePath;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to return project path! ======> {e.Message}");
                return null;
            }
        }

        private async Task<WwiseInfo?> GetWwiseInfoAsync()
        {
            if (!await TryConnectWaapiAsync()) return null;

            try
            {
                var func = Function.Verify("ak.wwise.core.getInfo");

                JObject result = await _client.Call(func, null, null);
                var getInfoData = WaapiSerializer.Deserialize<GetInfoData>(result.ToString());

                var projectName = await GetWwiseProjectNameAsync();

                var projectPath = await GetWwiseProjectPathAsync();

                var languages = new List<string>();
                
                for (int i = 0; i < 4; i++)
                {
                    languages = await GetLanguagesAsync();
                    if (languages.Count != 0) break;
                }

                if (languages.Count == 0) return null;

                WwiseInfo wwiseInfo = new WwiseInfo()
                {
                    ProjectName = projectName,
                    Version = new WwiseVersion(getInfoData.Version),
                    ProcessID = getInfoData.ProcessID,
                    IsCommandLine = getInfoData.IsCommandLine,
                    ProjectPath = projectPath,
                    Languages = languages
                };

                return wwiseInfo;
            }
            catch
            {
              
            }

            return null;
        }


      
      
      
      
      
        public async Task<bool> ExecuteUICommandAsync(string command, string[]? objectIDs = null)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {
                var func = Function.Verify("ak.wwise.ui.commands.execute");
                var cmd = UICommand.Verify(command);

                if (objectIDs is not null)
                {
                    var query = new
                    {
                        command = cmd,
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

        public async Task<List<WwiseObject>> GetSoundBanksReferencingWwiseObjectAsync(WwiseObject wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();

            var references = await GetEventsReferencingWwiseObjectAndParentsAsync(wwiseObject);

            if (references.Count > 0)
            {
                var ancestors = await CoreObjectGetAsync(references.Select(o => o.ID).ToArray(),
                    new []{"ancestors"}, new []{"name", "id", "type", "path"});
            
                var ids = ancestors.Return.Select(a => a.ID).Distinct().ToList();
                ids.AddRange(references.Select(r => r.ID).Distinct().ToList());
            
                var indirectRefs = await CoreObjectGetAsync(ids.ToArray(), new []{"referencesTo"}, new []{"name", "id", "type", "path"});
            
                result.AddRange(indirectRefs.Return.Where(r => r.Type == "SoundBank").Select(o => new WwiseObject(o.Name, o.ID,o.Type, o.Path)).Distinct().ToList());

            }
            
           
            var directRefs = await GetReferencesToWwiseObjectAndParentsAsync(wwiseObject);

            result.AddRange(directRefs.Where(r => r.Type == "SoundBank").Distinct().ToList());

            return result.Distinct().ToList();
        }



        public async Task<List<WwiseObject>> GetEventsReferencingWwiseObjectAsync(WwiseObject wwiseObject)
        {
            var references = await GetReferencesToWwiseObjectAsync(wwiseObject);
            
            var actions = references.Where(r => r.Type == "Action").ToList();
            var result = await BatchGetWwiseObjectParentAsync(actions);

            return result;
        }

        public async Task<List<WwiseObject>> GetEventsReferencingWwiseObjectAndParentsAsync(WwiseObject wwiseObject)
        {
            var references = await GetReferencesToWwiseObjectAndParentsAsync(wwiseObject);
            
            var actions = references.Where(r => r.Type == "Action").ToList();

            var result = await BatchGetWwiseObjectParentAsync(actions);

            return result;
        }


        public async Task<List<WwiseObject>> GetReferencesToWwiseObjectAndParentsAsync(WwiseObject wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();
            
            var objectList = await GetWwiseObjectAncestorsAsync(wwiseObject);
            if (!objectList.Contains(wwiseObject)) objectList.Add(wwiseObject);
            
            var returnData = await CoreObjectGetAsync(objectList.Select(o => o.ID).ToArray(), new []{"referencesTo"}, new []{"name", "id", "type", "path"});
            
            if (returnData.Return.Length == 0) throw new Exception("No object found!");
            result.AddRange(returnData.Return.Select(o => new WwiseObject(o.Name, o.ID, o.Type, o.Path)).Distinct());

            return result;
        }

        public async Task<List<WwiseObject>> GetReferencesToWwiseObjectAsync(WwiseObject? wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();
            if (wwiseObject is null || !await TryConnectWaapiAsync()) return result;
            try
            {
                var returnData = await CoreObjectGetAsync(wwiseObject.ID, new []{"referencesTo"});
                if (returnData.Return.Length == 0) throw new Exception("No parent found!");
                result.AddRange(returnData.Return.Select(o => new WwiseObject(o.Name, o.ID, o.Type, o.Path)));
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
            if (wwiseObjects.Count == 0 || !await TryConnectWaapiAsync()) return result;
            try
            {
                var returnData =
                    await CoreObjectGetAsync(wwiseObjects.Select(w => w.ID).ToArray(), 
                        new[] { "parent" },
                        new string[] { "name", "id", "type", "path" });
                if (returnData.Return.Length == 0) throw new Exception("No parent found!");
                result.AddRange(returnData.Return.Select(o => new WwiseObject(o.Name, o.ID, o.Type, o.Path)));
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children ======> {e.Message}");
            }

            return result;
        }

        public async Task<WwiseObject?>  GetWwiseObjectParentAsync(WwiseObject? wwiseObject)
        {
            if (wwiseObject is null || !await TryConnectWaapiAsync()) return null;
            try
            {
                var returnData = await CoreObjectGetAsync(wwiseObject.ID, new []{"parent"});
                if (returnData.Return.Length == 0) throw new Exception("No parent found!");
                var obj = returnData.Return[0];
                return new WwiseObject(obj.Name, obj.ID, obj.Type, obj.Path);
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
            if (wwiseObjects.Count == 0 || !await TryConnectWaapiAsync()) return result;
            try
            {
                var returnData =
                    await CoreObjectGetAsync(wwiseObjects.Select(w => w.ID).ToArray(), 
                        new[] { "children" },
                        new string[] { "name", "id", "type", "path" });
                if (returnData.Return.Length == 0) throw new Exception("No children found!");
                result.AddRange(returnData.Return.Select(o => new WwiseObject(o.Name, o.ID, o.Type, o.Path)));
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children ======> {e.Message}");
            }

            return result;
        }

        public async Task<List<WwiseObject>> GetWwiseObjectChildrenAsync(WwiseObject? wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();
            if (wwiseObject is null || !await TryConnectWaapiAsync()) return result;
            try
            {
                var returnData = await CoreObjectGetAsync(wwiseObject.ID, new []{"children"});
                if (returnData.Return.Length == 0) throw new Exception("No children found!");
                result.AddRange(returnData.Return.Select(o => new WwiseObject(o.Name, o.ID, o.Type, o.Path)));
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children of {wwiseObject.Name} ======> {e.Message}");
            }

            return result;
        }

        public async Task<List<WwiseObject>> GetWwiseObjectAncestorsAsync(WwiseObject? wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();
            if (wwiseObject is null || !await TryConnectWaapiAsync()) return result;
            try
            {

                var returnData = await CoreObjectGetAsync(wwiseObject.ID, new []{"ancestors"});
                if (returnData.Return.Length == 0) throw new Exception("No parent found!");
                result.AddRange(returnData.Return.Select(o => new WwiseObject(o.Name, o.ID, o.Type, o.Path)));


            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children of {wwiseObject.Name} ======> {e.Message}");
            }

            return result;
        }

        public async Task<List<WwiseObject>> GetWwiseObjectDescendantsAsync(WwiseObject? wwiseObject)
        {
            List<WwiseObject> result = new List<WwiseObject>();
            if (wwiseObject is null || !await TryConnectWaapiAsync()) return result;
            try
            {
                var returnData = await CoreObjectGetAsync(wwiseObject.ID, new []{"descendants"});
                if (returnData.Return.Length == 0) throw new Exception("No parent found!");
                
                result.AddRange(returnData.Return.Select(o => new WwiseObject(o.Name, o.ID, o.Type, o.Path)));
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to get children of {wwiseObject.Name} ======> {e.Message}");
            }

            return result;
        }

        public async Task<bool> GenerateSelectedSoundBanksAsync(string[] soundBanks)
        {
            return await GenerateSelectedSoundBanksAsync(soundBanks, new string[] { }, new string[] { });
        }

        public async Task<bool> GenerateSelectedSoundBanksAsync(string[] soundBanks, string[] platforms, string[] languages)
        {
            if (!await TryConnectWaapiAsync()) return false;

            try
            {
                if (platforms.Length == 0)
                {
                    platforms = (await GetPlatformsAsync()).ToArray();
                }

                if (languages.Length == 0)
                {
                    languages = ConnectionInfo.Languages.ToArray();
                }
                
                var query = new
                {
                    soundbanks = new List<object>(),
                    platforms = platforms,
                    languages = languages,
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
            if (!await TryConnectWaapiAsync()) return false;
            
            Function.Clear();
            
            try
            {
                
                var result = await _client.Call("ak.wwise.waapi.getFunctions", null, null, TimeOut);
                if (result["functions"] == null) throw new Exception();
                foreach (var func in result["functions"]!)
                {
                    Function.Add(func.ToString());
                }

                return true;
            }
            catch
            {
            }

            return false;
        }
        
        private async Task<bool> GetCommandsAsync()
        {
            if (!await TryConnectWaapiAsync()) return false;
            
            UICommand.Clear();
            
            try
            {
                
                var result = await _client.Call("ak.wwise.ui.commands.getCommands", null, null, TimeOut);
                if (result["commands"] == null) throw new Exception();
                foreach (var command in result["commands"]!)
                {
                    UICommand.Add(command.ToString());
                }

                return true;
            }
            catch
            {
            }

            return false;
        }

        private async Task<bool> GetTopicsAsync()
        {
            if (!await TryConnectWaapiAsync()) return false;
            
            Topic.Clear();
            
            try
            {
                var result = await _client.Call("ak.wwise.waapi.getTopics", null, null, TimeOut);
                if (result["topics"] == null) throw new Exception();
                foreach (var topic in result["topics"]!)
                {
                    Topic.Add(topic.ToString());
                }

                return true;
            }
            catch
            {
            }

            return false;
        }


        public async Task<bool> SetWwiseObjectColorAsync(WwiseObject wwiseObject, WwiseColor color, bool overrideColor = true)
        {
            if (ConnectionInfo.Version >= VersionHelper.V2022_1_0_7929)
            {
                return await Instance.BatchSetObjectPropertyAsync(new[] { wwiseObject }, new[]
                {
                    WwiseProperty.Prop_OverrideColor(overrideColor),
                    WwiseProperty.Prop_Color((uint)color), 
                });
            }
            
            var res = true;
            res &= await SetObjectPropertyAsync(wwiseObject, WwiseProperty.Prop_OverrideColor(overrideColor));
            res &= await SetObjectPropertyAsync(wwiseObject, WwiseProperty.Prop_Color((uint)color));
            return res;
        }

        public async Task<bool> BatchUpdateObjectPathAsync(List<WwiseObject> wwiseObjects)
        {
            if (wwiseObjects.Count == 0) return true;
            
            var returnData = await CoreObjectGetAsync(wwiseObjects.Select(o => o.ID).ToArray(), new string[] { }, new[] { "name", "id", "path" });

            if (returnData.Return.Length == 0) return false;

            var res = true;
            foreach (var data in returnData.Return)
            {
                var id = data.ID;
                var path = data.Path;
                var name = data.Name;

                var wo = wwiseObjects.First(o => o.ID == id);
                if (wo is null)
                {
                    WaapiLog.InternalLog($"Failed to update path for {name}");
                    res = false;
                    continue;
                }

                wo.Path = path;
            }

            return res;
        }
        
        public async Task<ReturnData<ObjectReturnData>> CoreObjectGetAsync(string id, string returnSingle)
        {
            return await CoreObjectGetAsync(new []{id}, new string[]{}, new []{ returnSingle});
        }
        
        public async Task<ReturnData<ObjectReturnData>> CoreObjectGetAsync(string id, string[] select)
        {
            return await CoreObjectGetAsync(new []{id}, select, new []{ "name", "path", "type", "id"});
        }


        public async Task<ReturnData<ObjectReturnData>> CoreObjectGetAsync(string[] ids, string[] select, string[] returns)
        {
            return await CoreObjectGetAsync<ObjectReturnData>(ids, select, returns);
        }
        
        
        public async Task<ReturnData<T>> CoreObjectGetAsync<T>(string[] ids, string[] select, string[] returns)
        {
            JObject query;
            if (ConnectionInfo.Version < VersionHelper.V2022_1_0_7929)
            {
                if (select.Length > 1) throw new Exception($"Select can only have one element in Wwise {ConnectionInfo.Version.VersionString}!");
                
                query = new JObject
                {
                    new JProperty("from",
                        new JObject
                        {
                            new JProperty("id", new JArray(ids))
                        }
                    ),
                };
                if (select.Length > 0)
                {
                    query.Add("transform", new JArray(
                        new JObject(
                            new JProperty("select", new JArray(select))
                        )
                    ));
                }
            }
            else
            {
                var waql = $"where {string.Join(" or ", ids.Select(id => $"id=\"{id}\""))}";
                if (select.Length > 0) waql += " select ";
                for (int i = 0; i < select.Length; i++)
                {
                    waql += select[i];
                    if (i < select.Length - 1) waql += ", ";
                }
                query = new JObject
                {
                    new JProperty("waql",
                        waql
                    ),
                };
            }
            
            var options = new
            {

                @return = returns

            };

            var func = WaapiFunctionList.CoreObjectGet;
            var jresult = await _client.Call(func, query, options, TimeOut);
            var returnData = WaapiSerializer.Deserialize<ReturnData<T>>(jresult.ToString());
            return returnData;
        }

      
      
      
      
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
        
        
        public async Task BeginUndoGroup()
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return;

            var func = Function.Verify("ak.wwise.core.undo.beginGroup");

            var res = await CallAsync(func, null, null);
        }
        
        public async Task CancelUndoGroup()
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return;

            var func = Function.Verify("ak.wwise.core.undo.cancelGroup");

            var res = await CallAsync(func, null, null);
        }
        
        public async Task EndUndoGroup(string undoGroupName)
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return;

            var func = Function.Verify("ak.wwise.core.undo.endGroup");

            var arg = new
            {
                displayName = undoGroupName
            };
            
            await CallAsync(func, arg, null);
        }
    }
}
