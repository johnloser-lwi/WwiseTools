
[![Nuget](https://img.shields.io/nuget/v/LWI.WwiseTools)](https://www.nuget.org/packages/LWI.WwiseTools/)

# Wwise Tools
Wwise productivity tools based on C# Waapi under development, enabling rapid generation and editing of Wwise objects to achieve batch addition effects, improving work efficiency.

*Author: Yang Weiqin (AKA John Loser)*

___

## Compatibility Note
Currently, WwiseTools have been applied in projects of versions 2019.1, 2019.2, and 2021.1, but not all functions have been fully tested for compatibility with these versions.
<br/>
Adjustments have been made to existing interfaces according to [Wwise 2022.1 Important Migration Notes](https://www.audiokinetic.com/library/2022.1.0_7985/?source=SDK&id=whatsnew_2022_1_migration.html), and most functions are available in Wwise 2022.

## Instructions for Use
### Project Configuration
Find WwiseTools on NuGet and add to your project.
### Importing a Single Audio File
```csharp
// All non-asynchronous functions will be gradually removed, please use asynchronous execution functions as much as possible
static async Task Main(string[] args)
{
    await WwiseUtility.Instance.ConnectAsync(); // First initialize the Wwise project connection (optional).
    var obj = await WwiseUtility.Instance.ImportSoundAsync(@"audio file path"); // Import the specified audio file, returning a "WwiseObject".
    Console.WriteLine(obj.ToString()); // Display information about the added object.
    
    await WwiseUtility.Instance.DisconnectAsync(); // Close the Wwise project connection.
}
```

After running the program, the Wwise project will import the specified file as a Sound, with the default path being "\Actor-Mixer Hierarchy\Default Work Unit", and the console will output the name, ID, and type information of the added object.

### Creating and Moving Objects
```csharp
var testFolder = await WwiseUtility.Instance.CreateObjectAtPathAsync("TestFolder", WwiseObject.ObjectType.Folder); // Create a folder named "TestFolder", with the default path being "\Actor-Mixer Hierarchy\Default Work Unit".
var testSound = await WwiseUtility.Instance.CreateObjectAtPathAsync("TestSound", WwiseObject.ObjectType.Sound); // Create a sound object named "TestSound", with the default path being "\Actor-Mixer Hierarchy\Default Work Unit".
await testFolder.AsContainer().AddChildAsync(testSound); // Move "testSound" under "testFolder".
```

After running the program, there will be a folder named "TestFolder" in the Wwise project, containing a sound object named "TestSound".

### Generating Events
Continuing from the previous example, we can create a play event for "testSound".
```csharp
await WwiseUtility.Instance.CreatePlayEventAsync("TestEvent", await testSound.GetPathAsync()); // Create an event named "TestEvent" to play "testSound", with the default path being "\Events\Default Work Unit".
```

After running the program, there will be an event named "TestEvent" in the Wwise project, with the "Play Action" containing a reference to "TestSound".
___

## Setting Properties and References
### Setting Attenuation Reference
```csharp
var randomContainer = await WwiseUtility.Instance.CreateObjectAtPathAsync("TestRandomContainer", WwiseObject.ObjectType.RandomSequenceContainer); // Create a RandomContainer named "TestRandomContainer", stored in "randomContainer".

/* 
Set the "Attenuation" reference of "randomContainer" to "TestAttenuation",
this function will automatically enable the "Attenuation" option,
if "TestAttenuation" cannot be found, it will be created under "\Attenuations\Default Work Unit".
*/
await randomContainer.AsVoice().SetAttenuationAsync("TestAttenuation"); 
```

After running the program, there will be a RandomContainer named "TestRandomContainer" in the Wwise project, with the "Attenuation" parameter checked in the "Positioning" menu and referenced to "TestAttenuation".

### Manual Setting of Properties and References
In addition to the "SetAttenuation" function provided by "RandomContainer", we can also manually set properties and references to achieve the same functionality, while having greater flexibility. We can add an Attenuation named "TestAttenuation" to ShareSet/Attenuations/Default Work Unit in Wwise, then use the "WwiseUtility.Instance.SetObjectProperty" and "WwiseUtility.Instance.SetObjectReference" functions to set properties and references.
```csharp
/*
Get the Attenuation we created by name, stored in "attenuation", the name here must be in the format "type:name",
where "type" is "Attenuation" and "name" is "TestAttenuation" in this example.
*/
var attenuation = await WwiseUtility.Instance.GetWwiseObjectByNameAsync("Attenuation:TestAttenuation"); 
await WwiseUtility.Instance.SetObjectPropertyAsync(randomContainer, WwiseProperty.Prop_EnableAttenuation(true)); // Enable "Attenuation".
await WwiseUtility.Instance.SetObjectReferenceAsync(randomContainer, WwiseReference.Ref_Attenuation(attenuation)); // Add reference "attenuation" to "randomContainer".
```

After running the program, the same effect as the previous example will be achieved.

### Custom Property and Reference Content
Although the current "WwiseProperty" and "WwiseReference" classes already include most of the properties and references' static creation functions, sometimes we still need to manually set the content of properties and references.
```csharp
var randomContainer = await WwiseFactory.CreateRandomSequenceContainer("TestRandomContainer", true,
    await WwiseUtility.Instance.GetWwiseObjectByPathAsync("\\Actor-Mixer Hierarchy\\Default Work Unit")); // Create a RandomContainer named "TestRandomContainer".

var testProperty = new WwiseProperty("EnableAttenuation", true); // Create a property object, property name is "EnableAttenuation", value is "true".

var attenuation = await WwiseUtility.Instance.GetWwiseObjectByNameAsync("Attenuation:TestAttenuation"); // Get the "Attenuation" named "TestAttenuation" from the Wwise project
var testReference = WwiseReference.Ref_Attenuation(attenuation); // Create a reference object, reference "attenuation".

await WwiseUtility.Instance.SetObjectPropertyAsync(randomContainer, testProperty); // Set property "testProperty" for "randomContainer".
await WwiseUtility.Instance.SetObjectReferenceAsync(randomContainer, testReference); // Add reference "testReference" to "randomContainer".
```
After running the program, the same effect as the previous example will be achieved. Of course, we can also set other properties and references, and you can find more information about properties and reference parameters in [Wwise Objects Reference](https://www.audiokinetic.com/library/edge/?source=SDK&id=wobjects_index.html).

### Directly Modifying WWU Files
Currently, some functions cannot be directly implemented through Waapi, such as setting "Playlist" in "Sequence Container". In this case, we need a specific solution to directly modify WWU files. WWU files are XML files. In the current version, we can use "WwiseWorkUnitParser"

to read and set parameters. We take setting "Playlist" in "Sequence Container" as an example (this function has been written into "SetPlaylist" function of "WwiseSequenceContainer").
```csharp
static async Task Main(string[] args)
{
    WwiseObject container = await WwiseUtility.Instance.CreateObjectAtPathAsync("TestContainer", WwiseObject.ObjectType.RandomSequenceContainer); // Create a Sequence Container
    WwiseObject sound = await WwiseUtility.Instance.CreateObjectAtPathAsync("TestSound", WwiseObject.ObjectType.Sound, await container.GetPathAsync()); // Create an empty sound

    await WwiseUtility.Instance.SaveWwiseProjectAsync(); // Save the project
    WwiseWorkUnitParser parser = new WwiseWorkUnitParser(await WwiseUtility.Instance.GetWorkUnitFilePathAsync(container)); // Create WwiseWorkUnitParser and get the WorkUnit file of container

    // Get the Playlist node of container
    var xpath = "//*[@ID='" + container.ID + "']/Playlist";
    var playlistNode = parser.XML.SelectSingleNode(xpath);


    // Get the corresponding XML node of container
    var containerNode = parser.GetNodeByID(container.ID);

    // Clear the existing Playlist
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

    await WwiseUtility.Instance.ReloadWwiseProjectAsync();// To make the modification effective and avoid errors, the project needs to be reloaded

    await WwiseUtility.Instance.DisconnectAsync();
}
```
After running the program, there will be a SequenceContainer named "TestContainer" in the project, containing an empty sound named "TestSound", and the Playlist of "TestContainer" will contain "TestSound".