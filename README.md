# Wwise Tools
正在开发的基于C# Waapi的Wwise生产力工具，可以快速生成、编辑Wwise的Object，达到批量添加的效果，提升工作效率。

*作者 : [杨惟勤 (AKA John Loser)](https://losersworldindustries.com/john-yang)*

*.NETFramework,Version=v4.5.2*
___

## 基础使用
### 导入单个音频
1. `WwiseUtility.Init();`首先初始化Wwise工程连接(可以跳过)。
2. `var obj = WwiseUtility.ImportSound(@"音频文件路径");`导入指定音频文件，返回`WwiseObject`。
3. `Console.WriteLine(obj.ToString());`显示添加物体的信息。

运行程序后Wwise工程将会导入指定文件为Sound，默认路径为`@"\Actor-Mixer Hierarchy\Default Work Unit"`，控制台将输出添加物体的名称，ID，类型信息。

### 从文件夹批量导入音频
1. `var objects = WwiseUtility.ImportSoundFromFolder(@"文件夹路径");`导入指定文件夹内的音频，返回`List<WwiseObject>`。
2. `foreach (var obj in objects) { Console.WriteLine(obj.ToString()); }`显示所有物体信息。

运行程序后Wwise工程将会导入指定文件夹内的所有文件为Sound，默认路径为`@"\Actor-Mixer Hierarchy\Default Work Unit"`，控制台将输出所有添加物体的名称，ID，类型信息。

### 创建与移动物体
1. `var testFolder = WwiseUtility.CreateObject("TestFolder", WwiseObject.ObjectType.Folder);`创建一个名称为"TestFolder"的文件夹，默认路径为`@"\Actor-Mixer Hierarchy\Default Work Unit"`。
2. `var testSound = WwiseUtility.CreateObject("TestSound", WwiseObject.ObjectType.Sound);`创建一个名称为"TestSound"的音频对象，默认路径为`@"\Actor-Mixer Hierarchy\Default Work Unit"`。
3. `WwiseUtility.MoveToParent(testSound, testFolder);`将"testSound"移动至"testFolder"下。

运行程序后Wwise工程中将会有一个名为"TestFolder"的文件夹，其中包含一个名为"TestSound"的音频对象。

### 生成事件
延续上一个案例，我们可以将"testSound"放入一个播放事件。
1. `WwiseUtility.CreatePlayEvent("TestEvent", testSound.Path);`生成一个名为"TestEvent"的事件播放"testSound"，默认路径为`@"\Events\Default Work Unit"`

运行程序后Wwise工程中将会有一个名为"TestEvent"的事件，其中的"Play Action"包含一个名为"TestSound"的引用。
___

## 设置属性以及引用
### 设置属性
1. `var rscontainer = WwiseUtility.CreateObject("TestRS", WwiseObject.ObjectType.RandomSequenceContainer, @"\Actor-Mixer Hierarchy\Default Work Unit");`创建一个名为"TestRS"的RandomSequenceContainer，保存在"rscontainer"中。
2. `WwiseUtility.SetObjectProperty(rscontainer, WwiseProperty.Prop_EnableAttenuation(true));`设置"rscontainer"的"Enable_Attenuation"参数为"true"。

运行程序后Wwise工程中将会有一个名为"TestRS"的RandomSequenceContainer，它的"Positioning"菜单中的"Attenuation"参数被勾选。

### 设置引用
延续上一个案例，我们可以为"rscontainer"添加一个Attenuation的引用。我们可以在Wwise的ShareSet/Attenuations/Default Work Unit中添加一个名为"TestAttenuation"的Attenuation。
1. `var attenuation = WwiseUtility.GetWwiseObjectByName("Attenuation:TestAttenuation");`通过名称获取我们创建的Attenuation，存于"attenuation"中，此处名称必须为"type:name"的格式，该案例中的"type"为"Attenuation"，"name"为"TestAttenuation"。
2. `WwiseUtility.SetObjectReference(rscontainer, WwiseReference.Ref_Attenuation(attenuation));`为"rscontainer"添加引用"attenuation"。

运行程序后，Wwise的除了勾选了"Attenuation"参数，引用也设置为刚才设置的"TestAttenuation"。
___
