# Wwise Tools
正在开发的Wwise工具，可以快速生成、编辑Wwise的Work Unit，达到批量添加的效果，提升工作效率。

*作者 : John Loser*

*.NETFramework,Version=v4.7.2*
___

## 基础使用
### 导入单个音频
1. `WwiseUtility.Init();`首先初始化Wwise工程连接(可以跳过)。
2. `var obj = WwiseUtility.ImportSound(@"音频文件路径");`导入指定音频文件，返回`WwiseObject`。
3. `Console.WriteLine(obj.ToString());`显示添加物体的信息。

运行程序后Wwise工程将会导入指定文件为Sound，默认路径为`Actor-Mixer Hierarchy\\Defult Work Unit\\音频文件`，控制台将输出添加物体的名称，ID，类型信息。

### 从文件夹批量导入音频
1. `var objects = WwiseUtility.ImportSoundFromFolder(@"文件夹路劲");`导入指定文件夹内的音频，返回`List<WwiseObject>`。
2. `foreach (var obj in objects) { Console.WriteLine(obj.ToString()); }`显示所有物体信息。

运行程序后Wwise工程将会导入指定文件夹内的所有文件为Sound，默认路径为`Actor-Mixer Hierarchy\\Defult Work Unit\\音频文件`，控制台将输出所有添加物体的名称，ID，类型信息。
___