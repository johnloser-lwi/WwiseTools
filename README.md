# Wwise Tools
正在开发的基于C# Waapi的Wwise生产力工具，可以快速生成、编辑Wwise的Object，达到批量添加的效果，提升工作效率。

*作者 : [杨惟勤 (AKA John Loser)](https://losersworldindustries.com/john-yang)*

*.NETFramework,Version=v4.5.2*

*Wwise,Version=v2021.1.2.7629*

**该项目仍处于开发初期，不少现有的功能将来都有被推翻或者有更优的解决方案的可能，请谨慎使用于实际项目中!**
___

## 使用说明
### 导入单个音频
```csharp
WwiseUtility.Init(); // 首先初始化Wwise工程连接(可以跳过)。
var obj = WwiseUtility.ImportSound(@"音频文件路径"); // 导入指定音频文件，返回"WwiseObject"。
Console.WriteLine(obj.ToString()); // 显示添加对象的信息。
```

运行程序后Wwise工程将会导入指定文件为Sound，默认路径为"\Actor-Mixer Hierarchy\Default Work Unit"，控制台将输出添加对象的名称，ID，类型信息。

### 从文件夹批量导入音频
```csharp
var objects = WwiseUtility.ImportSoundFromFolder(@"文件夹路径"); // 导入指定文件夹内的音频，返回"List<WwiseObject>"。
foreach (var obj in objects) { Console.WriteLine(obj.ToString()); } // 显示所有对象信息。
```

运行程序后Wwise工程将会导入指定文件夹内的所有文件为Sound，默认路径为"\Actor-Mixer Hierarchy\Default Work Unit"，控制台将输出所有添加对象的名称，ID，类型信息。

### 创建与移动对象
```csharp
var testFolder = new WwiseFolder("TestFolder"); // 创建一个名称为"TestFolder"的文件夹，默认路径为"\Actor-Mixer Hierarchy\Default Work Unit"。
var testSound = new WwiseSound("TestSound"); // 创建一个名称为"TestSound"的音频对象，默认路径为"\Actor-Mixer Hierarchy\Default Work Unit"。
testFolder.AddChild(testSound); // 将"testSound"移动至"testFolder"下。
```

运行程序后Wwise工程中将会有一个名为"TestFolder"的文件夹，其中包含一个名为"TestSound"的音频对象。

### 生成事件
延续上一个案例，我们可以为"testSound"创建一个播放事件。
```csharp
testSound.CreatePlayEvent("TestEvent"); // 生成一个名为"TestEvent"的事件播放"testSound"，默认路径为"\Events\Default Work Unit"
```

运行程序后Wwise工程中将会有一个名为"TestEvent"的事件，其中的"Play Action"包含一个名为"TestSound"的引用。
___

## 设置属性以及引用
### 设置衰减(Attenuation)引用
```csharp
var randomContainer = new WwiseRandomContainer("TestRandomContainer"); // 创建一个名为"TestRandomContainer"的RandomContainer，保存在"randomContainer"中。

/* 
设置"randomContainer"的"Attenuation"引用为"TestAttenuation"，
该函数会自动启用"Attenuation"选项，
如果无法找到"TestAttenuation"将会在"\Attenuations\Default Work Unit"下创建"TestAttenuation"。
*/
randomContainer.SetAttenuation("TestAttenuation"); 
```

运行程序后Wwise工程中将会有一个名为"TestRandomContainer"的RandomContainer，"Positioning"菜单中的"Attenuation"参数被勾选，引用设置为"TestAttenuation"。

### 手动设置属性以及引用
除了"RandomContainer"自带的"SetAttenuation"函数，我们还可以手动设置属性以及引用来实现相同的功能，同时拥有更大的灵活性。我们可以在Wwise的ShareSet/Attenuations/Default Work Unit中添加一个名为"TestAttenuation"的Attenuation，然后通过"WwiseUtility.SetObjectProperty"和"WwiseUtility.SetObjectReference"函数来设置属性以及引用。
```csharp
/*
 通过名称获取我们创建的Attenuation，存于"attenuation"中，此处名称必须为"type:name"的格式，
 该案例中的"type"为"Attenuation"，"name"为"TestAttenuation"。
 */
var attenuation = WwiseUtility.GetWwiseObjectByName("Attenuation:TestAttenuation"); 
WwiseUtility.SetObjectProperty(randomContainer, WwiseProperty.Prop_EnableAttenuation(true)); // 启用"Attenuation"。
WwiseUtility.SetObjectReference(randomContainer, WwiseReference.Ref_Attenuation(attenuation)); // 为"randomContainer"添加引用"attenuation"。
```

运行程序后，将会实现与上一个案例相同的效果。

### 自定义属性以及引用内容
虽然目前的"WwiseProperty"和"WwiseReference"类已经包含了大部分属性、引用的静态创建函数，有的时候我们仍然会需要手动设置属性、应用的内容。
```csharp
var randomContainer = new WwiseRandomContainer("TestRandomContainer"); // 创建一个名为"TestRandomContainer"的RandomContainer。

var testProperty = new WwiseProperty("EnableAttenuation", true); // 创建一个属性对象，属性名称为"EnableAttenuation"，值为"true"。

var attenuation = WwiseUtility.GetWwiseObjectByName("Attenuation:TestAttenuation"); // 从Wwise工程中获取名为"TestAttenuation"的"Attenuation"
var testReference = new WwiseReference(attenuation); // 创建一个引用对象，引用"attenuation"

WwiseUtility.SetObjectProperty(randomContainer, testProperty); // 为"randomContainer设置属性"testProperty""。
WwiseUtility.SetObjectReference(randomContainer, testReference); // 为"randomContainer"添加引用"testReference"。
```
运行程序后，将会实现与上一个案例相同的效果，当然我们也可以设置其他的属性与引用，可以在[Wwise Objects Reference](https://www.audiokinetic.com/zh/library/edge/?source=SDK&id=wobjects_index.html)中找到更多的属性、应用参数说明。

### 直接修改wwu文件
目前有一部分功能不能直接再waapi中实现，如设置"Sequence Container"中的"Playlist"，这时我们就需要一定方案来直接修改wwu文件。wwu文件为XML文件，在当前版本中我们可以使用"WwiseWorkUnitParser"来读取并设置参数，我们以设置"Sequence Container"的"Playlist"为例（该功能已经写入"WwiseSequenceContainer"的"SetPlaylist"函数）。
```csharp
WwiseSequenceContainer container = new WwiseSequenceContainer("TestContainer"); // 创建一个Sequence Container
WwiseSound sound = new WwiseSound("TestSound", container.Path); // 创建一个空音频

WwiseUtility.SaveWwiseProject(); // 保存工程
WwiseWorkUnitParser parser = new WwiseWorkUnitParser(WwiseUtility.GetWorkUnitFilePath(container)); // 创建WwiseWorkUnitParser，并获取container的WorkUnit文件

var playlists = parser.XML.GetElementsByTagName("Playlist"); // 获取所有Playlist节点

XmlElement playlist = null;

// 获取ID与container相同的Playlist节点（目标节点）
foreach (XmlElement list in playlists)
{
    if (list.ParentNode.Attributes["ID"].Value.ToString() == container.ID)
    {
        playlist = list;
        break;
    }
}

if (playlist != null)
{
    // 创建ItemRef节点
    var node = parser.XML.CreateElement("ItemRef"); 
    node.SetAttribute("Name", sound.Name);
    node.SetAttribute("ID", sound.ID);

    playlist.AppendChild(parser.XML.ImportNode(node, true)); // 添加节点

    parser.SaveFile(); // 保存文件
}

WwiseUtility.ReloadWwiseProject();// 为了使修改生效，避免错误，需要重新加载工程

```
运行程序后，工程中将会有一个名为"TestContainer"的"SequenceContainer"，其中包含一个名为"TestSound"的空音频，"TestContainer"的Playlist中回包含"TestSound"。
