# Wwise Tools
正在开发的基于C# Waapi的Wwise生产力工具，可以快速生成、编辑Wwise的Object，达到批量添加的效果，提升工作效率。

*作者 : John Loser*

*.NETFramework,Version=v4.7.2*
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