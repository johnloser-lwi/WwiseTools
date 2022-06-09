# Wwise Tools
正在开发的基于C# Waapi的Wwise生产力工具，可以快速生成、编辑Wwise的Object，达到批量添加的效果，提升工作效率。

*作者 : [杨惟勤 (AKA John Loser)](https://losersworldindustries.com/john-yang)*

___

## 使用说明
### 导入单个音频
```csharp
// 所有非异步执行的函数将会被逐步删除，请尽可能使用异步执行函数
static async Task Main(string[] args)
{
    await WwiseUtility.Instance.ConnectAsync(); // 首先初始化Wwise工程连接(可以跳过)。
    var obj = await WwiseUtility.Instance.ImportSoundAsync(@"音频文件路径"); // 导入指定音频文件，返回"WwiseObject"。
    Console.WriteLine(obj.ToString()); // 显示添加对象的信息。
    
    await WwiseUtility.Instance.DisconnectAsync(); // 关闭Wwise工程连接。
}

```

运行程序后Wwise工程将会导入指定文件为Sound，默认路径为"\Actor-Mixer Hierarchy\Default Work Unit"，控制台将输出添加对象的名称，ID，类型信息。

### 创建与移动对象
```csharp
var testFolder = await CreateObjectAtPathAsync("TestFolder", WwiseObject.ObjectType.Folder); // 创建一个名称为"TestFolder"的文件夹，默认路径为"\Actor-Mixer Hierarchy\Default Work Unit"。
var testSound = await CreateObjectAtPathAsync("TestSound", WwiseObject.ObjectType.Sound); // 创建一个名称为"TestSound"的音频对象，默认路径为"\Actor-Mixer Hierarchy\Default Work Unit"。
await testFolder.GetHierarchy().AddChildAsync(testSound); // 将"testSound"移动至"testFolder"下。
```

运行程序后Wwise工程中将会有一个名为"TestFolder"的文件夹，其中包含一个名为"TestSound"的音频对象。

### 生成事件
延续上一个案例，我们可以为"testSound"创建一个播放事件。
```csharp
await CreatePlayEventAsync("TestEvent", await testSound.GetPathAsync()); // 生成一个名为"TestEvent"的事件播放"testSound"，默认路径为"\Events\Default Work Unit"
```

运行程序后Wwise工程中将会有一个名为"TestEvent"的事件，其中的"Play Action"包含一个名为"TestSound"的引用。
___

## 设置属性以及引用
### 设置衰减(Attenuation)引用
```csharp
var randomContainer = await CreateObjectAtPathAsync("TestRandomContainer", WwiseObject.ObjectType.RandomSequenceContainer); // 创建一个名为"TestRandomContainer"的RandomContainer，保存在"randomContainer"中。

/* 
设置"randomContainer"的"Attenuation"引用为"TestAttenuation"，
该函数会自动启用"Attenuation"选项，
如果无法找到"TestAttenuation"将会在"\Attenuations\Default Work Unit"下创建"TestAttenuation"。
*/
await randomContainer.GetVoice().SetAttenuationAsync("TestAttenuation"); 
```

运行程序后Wwise工程中将会有一个名为"TestRandomContainer"的RandomContainer，"Positioning"菜单中的"Attenuation"参数被勾选，引用设置为"TestAttenuation"。

### 手动设置属性以及引用
除了"RandomContainer"自带的"SetAttenuation"函数，我们还可以手动设置属性以及引用来实现相同的功能，同时拥有更大的灵活性。我们可以在Wwise的ShareSet/Attenuations/Default Work Unit中添加一个名为"TestAttenuation"的Attenuation，然后通过"WwiseUtility.Instance.SetObjectProperty"和"WwiseUtility.Instance.SetObjectReference"函数来设置属性以及引用。
```csharp
/*
 通过名称获取我们创建的Attenuation，存于"attenuation"中，此处名称必须为"type:name"的格式，
 该案例中的"type"为"Attenuation"，"name"为"TestAttenuation"。
 */
var attenuation = await WwiseUtility.Instance.GetWwiseObjectByNameAsync("Attenuation:TestAttenuation"); 
await WwiseUtility.Instance.SetObjectPropertyAsync(randomContainer, WwiseProperty.Prop_EnableAttenuation(true)); // 启用"Attenuation"。
await WwiseUtility.Instance.SetObjectReferenceAsync(randomContainer, WwiseReference.Ref_Attenuation(attenuation)); // 为"randomContainer"添加引用"attenuation"。
```

运行程序后，将会实现与上一个案例相同的效果。

### 自定义属性以及引用内容
虽然目前的"WwiseProperty"和"WwiseReference"类已经包含了大部分属性、引用的静态创建函数，有的时候我们仍然会需要手动设置属性、应用的内容。
```csharp
var randomContainer = await CreateObjectAtPathAsync("TestRandomContainer"); // 创建一个名为"TestRandomContainer"的RandomContainer。

var testProperty = new WwiseProperty("EnableAttenuation", true); // 创建一个属性对象，属性名称为"EnableAttenuation"，值为"true"。

var attenuation = await WwiseUtility.Instance.GetWwiseObjectByNameAsync("Attenuation:TestAttenuation"); // 从Wwise工程中获取名为"TestAttenuation"的"Attenuation"
var testReference = new WwiseReference(attenuation); // 创建一个引用对象，引用"attenuation"

await WwiseUtility.Instance.SetObjectPropertyAsync(randomContainer, testProperty); // 为"randomContainer设置属性"testProperty""。
await WwiseUtility.Instance.SetObjectReferenceAsync(randomContainer, testReference); // 为"randomContainer"添加引用"testReference"。
```
运行程序后，将会实现与上一个案例相同的效果，当然我们也可以设置其他的属性与引用，可以在[Wwise Objects Reference](https://www.audiokinetic.com/zh/library/edge/?source=SDK&id=wobjects_index.html)中找到更多的属性、应用参数说明。

### 直接修改wwu文件
目前有一部分功能不能直接再waapi中实现，如设置"Sequence Container"中的"Playlist"，这时我们就需要一定方案来直接修改wwu文件。wwu文件为XML文件，在当前版本中我们可以使用"WwiseWorkUnitParser"来读取并设置参数，我们以设置"Sequence Container"的"Playlist"为例（该功能已经写入"WwiseSequenceContainer"的"SetPlaylist"函数）。
```csharp
static async Task Main(string[] args)
{
    WwiseObject container = await CreateObjectAtPathAsync("TestContainer", WwiseObject.ObjectType.RandomSequenceContainer); // 创建一个Sequence Container
    WwiseObject sound = await CreateObjectAtPathAsync("TestSound", WwiseObject.ObjectType.Sound, await container.GetPathAsync()); // 创建一个空音频
    
    await WwiseUtility.Instance.SaveWwiseProjectAsync(); // 保存工程
    WwiseWorkUnitParser parser = new WwiseWorkUnitParser(await WwiseUtility.Instance.GetWorkUnitFilePathAsync(container)); // 创建WwiseWorkUnitParser，并获取container的WorkUnit文件

    // 获取container的Playlist节点
    var xpath = "//*[@ID='" + container.ID + "']/Playlist";
    var playlistNode = parser.XML.SelectSingleNode(xpath);


    // 获取对应container的xml节点
    var containerNode = parser.XML.SelectSingleNode("//*[@ID='" + container.ID + "']");

    // 清空现有Playlist
    if (playlistNode != null)
    {
        containerNode.RemoveChild(playlistNode);
        parser.SaveFile();
    }
    
    
    var new_playlist = parser.XML.CreateElement("Playlist");


    var node = parser.XML.CreateElement("ItemRef");
    node.SetAttribute("Name", sound.Name);
    node.SetAttribute("ID", sound.ID);
    new_playlist.AppendChild(node);

    containerNode.AppendChild(parser.XML.ImportNode(new_playlist, true));

    parser.SaveFile();
    
    await WwiseUtility.Instance.ReloadWwiseProjectAsync();// 为了使修改生效，避免错误，需要重新加载工程
    
    await WwiseUtility.Instance.DisconnectAsync();
}

```
运行程序后，工程中将会有一个名为"TestContainer"的"SequenceContainer"，其中包含一个名为"TestSound"的空音频，"TestContainer"的Playlist中回包含"TestSound"。
