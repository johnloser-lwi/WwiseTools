# Wwise Tools
正在开发的Wwise工具，可以快速生成、编辑Wwise的Work Unit，达到批量添加的效果，提升工作效率。

*作者John Loser*

*.NETFramework,Version=v4.7.2*
___

## 基础使用
1. `WwiseUtility.Init(@"Wwise工程路径", @"音频源文件路径", true);`首先初始化Wwise工程路径，音频源文件路径以及是否自动复制文件。
2. `WwiseParser parser = new WwiseParser;`创建新的WwiseParser实例，用于解析与创建.wwu文件。
3. `parser.InitWorkUnit("Test", "Events", "Events/Test.wwu");`创建一个空的工作单元(Work Unit)，其名称为"Name"，类型为"Events"，目标文件夹为"Events/Test.wwu"。
4. `WwiseEvent test_event = new WwiseEvent("TestEvent", parser);`初始化一个新的Wwise事件(Event)。
5. `parser.AddChildToWorkUnit(test_event);`将事件添加至工作单元。
6. `parser.CommitChange();`应用更改，目标目录下的文件将会更新。

运行程序后Wwise工程中的Event列表将会拥有一个新的工作单元"Test"，其中包含一个叫"TestEvent"的空事件。
