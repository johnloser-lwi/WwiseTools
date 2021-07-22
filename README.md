# Wwise Tools
正在开发的基于C# Waapi的Wwise生产力工具，可以快速生成、编辑Wwise的Object，达到批量添加的效果，提升工作效率。

*作者 : [杨惟勤 (AKA John Loser)](https://losersworldindustries.com/john-yang)*

*.NETFramework,Version=v4.5.2*
___

## 基础使用
### 导入单个音频
1. `WwiseUtility.Init();`首先初始化Wwise工程连接(可以跳过)。
2. `var obj = WwiseUtility.ImportSound(@"音频文件路径");`导入指定音频文件，返回`WwiseObject`。
3. `Console.WriteLine(obj.ToString());`显示添加对象的信息。

运行程序后Wwise工程将会导入指定文件为Sound，默认路径为`@"\Actor-Mixer Hierarchy\Default Work Unit"`，控制台将输出添加对象的名称，ID，类型信息。

### 从文件夹批量导入音频
1. `var objects = WwiseUtility.ImportSoundFromFolder(@"文件夹路径");`导入指定文件夹内的音频，返回`List<WwiseObject>`。
2. `foreach (var obj in objects) { Console.WriteLine(obj.ToString()); }`显示所有对象信息。

运行程序后Wwise工程将会导入指定文件夹内的所有文件为Sound，默认路径为`@"\Actor-Mixer Hierarchy\Default Work Unit"`，控制台将输出所有添加对象的名称，ID，类型信息。

### 创建与移动对象
1. `var testFolder = WwiseUtility.CreateObject("TestFolder", WwiseObject.ObjectType.Folder);`创建一个名称为"TestFolder"的文件夹，默认路径为`@"\Actor-Mixer Hierarchy\Default Work Unit"`。
2. `var testSound = new WwiseSound("TestSound")`创建一个名称为"TestSound"的音频对象，默认路径为`@"\Actor-Mixer Hierarchy\Default Work Unit"`。
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

# 作者简介
![photo](https://losersworldindustries.com/wp-content/uploads/2021/07/WechatIMG121-1536x583.jpeg)

**[杨惟勤 (AKA John Loser)](https://losersworldindustries.com/john-yang)，毕业于上海音乐学院音乐工程系。专业方向：音乐设计与制作、游戏声音设计，拥有Wwise 101、201、301认证。业余学习游戏制作，熟悉Unity3D、Unreal、Godot等游戏引擎，使用C#、python、gdscript等编程语言。目前在上海英澈网络科技有限公司担任游戏音效设计。**
___

## 个人状态
- 正在努力学习各方面知识，提高自身能力
- 2021 目标：完成并发布[Wwise Tools](https://github.com/johnlsoer/WwiseTools.git)帮助广大音频工作者更加高效地完成任务
- 未来目标：发布独立开发的游戏Demo（或者成品）
<br />

## 项目
* [Wwise Tools](https://github.com/johnlsoer/WwiseTools.git)
* [Godot Fmod Integration (forked from shinobi-lx/godot-fmod-integration)](https://github.com/johnlsoer/godot-fmod-integration.git)
<br />

## 社交媒体
[<img align="left" alt="losersworldindustries.com" width="22px" src="https://cdn.jsdelivr.net/npm/simple-icons@3.13.0/icons/internetexplorer.svg" />][website]
[<img align="left" alt="John Yang | YouTube" width="22px" src="https://cdn.jsdelivr.net/npm/simple-icons@v3/icons/youtube.svg" />][youtube]
[<img align="left" alt="Facebook" width="22px" src="https://cdn.jsdelivr.net/npm/simple-icons@v3/icons/facebook.svg" />][facebook]
<br />
___
## 博客文章
- [上古卷轴5：天际 UI音效制作](https://losersworldindustries.com/%e3%80%8a%e4%b8%8a%e5%8f%a4%e5%8d%b7%e8%bd%b45%ef%bc%9a%e5%a4%a9%e9%99%85%e3%80%8bui%e9%9f%b3%e6%95%88%e5%88%b6%e4%bd%9c/)
- [毁灭战士：永恒 处决音效制作分享](https://losersworldindustries.com/%e3%80%8a%e6%af%81%e7%81%ad%e6%88%98%e5%a3%ab%ef%bc%9a%e6%b0%b8%e6%81%92%e3%80%8b%e5%a4%84%e5%86%b3%e9%9f%b3%e6%95%88%e5%88%b6%e4%bd%9c%e5%88%86%e4%ba%ab/)
- [LOW POLY PROJECT氛围展示](https://losersworldindustries.com/%e3%80%8alow-poly-project%e3%80%8b%e6%b0%9b%e5%9b%b4%e5%b1%95%e7%a4%ba/)
- [使用MASSIVE X等合成器制作拳头击打音效](https://losersworldindustries.com/massive-x-punches/)

[更多文章](http://losersworldindustries.com)
___

[website]: http://losersworldindustries.com
[youtube]: https://www.youtube.com/channel/UCqibrQEeoU5W7Wgq1ngCM_A
[facebook]:https://www.facebook.com/john602724927
