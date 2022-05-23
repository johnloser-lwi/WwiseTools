// See https://aka.ms/new-console-template for more information

using Examples;
using WwiseTools.Utils;

WaapiLog.AddCustomLogger(ExampleFunctions.CustomLogger);

if (await WwiseUtility.TryConnectWaapiAsync())
{
    await ExampleFunctions.ParserTestAsync(); // 尝试不同的方法
}
else
{
    Console.WriteLine("Waapi Connection Failed!");
}

await WwiseUtility.DisconnectAsync();
Console.ReadLine();
