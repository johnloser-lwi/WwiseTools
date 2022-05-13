using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Utils;
using System.IO;
using System.Xml;
using WwiseTools.Properties;
using WwiseTools.Reference;
using WwiseTools.Objects;

namespace WwiseTools
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var selection = await WwiseUtility.GetWwiseObjectsBySelectionAsync();

            foreach (var wwiseObject in selection)
            {
                Console.WriteLine(wwiseObject.Name);
            }
            
            
            await WwiseUtility.DisconnectAsync();
        }


        /*
        static void CopyRTPC()
        {
            WwiseUtility.Init().Wait();
            
            string[] rtpcProperties = new[] {"EnableAttenuation", "Highpass", "Lowpass", "Volume", "Priority" };
            string[] interestActorMixer = new[] {"Attack", "Explosion", "Motion", "Pre-Explosion"};

            Console.WriteLine("Available RTPC properties: ");
            string rtpcs = "";
            foreach (var rtpcProperty in rtpcProperties)
            {
                rtpcs += rtpcProperty + " ";
            }
            Console.WriteLine(rtpcs);
            string newProp = Console.ReadLine();
            if (!string.IsNullOrEmpty(newProp))
            {
                rtpcProperties = newProp.Split(' ');
            }
            
            Console.WriteLine("Available ActorMixers: ");
            string mixers = "";
            foreach (var mixer in interestActorMixer)
            {
                mixers += mixer + " ";

            }
            Console.WriteLine(mixers);
            string newMixer = Console.ReadLine();
            if (!string.IsNullOrEmpty(newMixer))
            {
                interestActorMixer = newMixer.Split(' ');
            }

            var targetProperties = new Dictionary<string, XmlElement>();

            var obj = WwiseUtility.GetWwiseObjectsBySelection()[0];
            
            Console.WriteLine("Parsing RTPC settings ...");
           
            {
                var wu = WwiseUtility.GetWorkUnitFilePath(obj);
                WwiseWorkUnitParser parser = new WwiseWorkUnitParser(wu);
                var actorMixers = parser.XML.GetElementsByTagName("ActorMixer");
                foreach (XmlElement actorMixer in actorMixers)
                {
                    if (!interestActorMixer.Contains(actorMixer.GetAttribute("Name"))) continue;
                    
                    //Console.WriteLine(actorMixer.GetAttribute("Name"));
                    
                    var properties = actorMixer.GetElementsByTagName("Property");
                    foreach (XmlElement property in properties)
                    {
                        if (rtpcProperties.Contains(property.GetAttribute("Name")))
                        {
                            var list = property.GetElementsByTagName("RTPCList");
                            
                            if (list.Count > 0)
                            {
                                targetProperties.Add(actorMixer.GetAttribute("Name") + property.GetAttribute("Name"), list[0] as XmlElement);
                                //Console.WriteLine(property.GetAttribute("Name"));
                                //Console.WriteLine(list[0].InnerXml);
                            }
                                
                        }
                    }
                }
            }

            Console.WriteLine("Please select targets and press Enter ...");
            Console.ReadLine();
            
            Console.WriteLine("Copying RTPC Settings ...");
            var targets = WwiseUtility.GetWwiseObjectsBySelection();
            foreach (var target in targets)
            {
                var wu = WwiseUtility.GetWorkUnitFilePath(target);
                WwiseWorkUnitParser parser = new WwiseWorkUnitParser(wu);
                var actorMixers = parser.XML.GetElementsByTagName("ActorMixer");
                foreach (XmlElement actorMixer in actorMixers)
                {
                    if (!interestActorMixer.Contains(actorMixer.GetAttribute("Name"))) continue;
                    
                    //Console.WriteLine(actorMixer.GetAttribute("Name"));
                    
                    var properties = actorMixer.GetElementsByTagName("Property");
                    for (int i = 0; i < properties.Count; i++)
                    {
                        XmlElement property = properties[i] as XmlElement;;
                        if (rtpcProperties.Contains(property.GetAttribute("Name")))
                        {
                            var list = property.GetElementsByTagName("RTPCList");
                            
                            
                            if (list.Count > 0)
                            {
                                var newNode = property.InsertAfter(parser.XML.ImportNode(
                                    targetProperties[actorMixer.GetAttribute("Name") + property.GetAttribute("Name")],
                                    true), list[0]);
                                
                                SetNewGUIDRecursively(newNode as XmlElement);

                                property.RemoveChild(list[0]);
                            }
                                
                        }
                    }
                    
                    
                }
                
                parser.SaveFile();
            }

        }

        static void SetNewGUIDRecursively(XmlElement node)
        {
            foreach (XmlElement child in node.GetElementsByTagName("RTPC"))
            {
                child.SetAttribute("ID", WwiseUtility.NewGUID());
                child.SetAttribute("ShortID", Math.Abs(child.GetHashCode()).ToString());
                foreach (XmlElement curve in child.GetElementsByTagName("Curve"))
                {
                    curve.SetAttribute("ID", WwiseUtility.NewGUID());
                }
            }
        }

        static void MoveToWorkUnit()
        {
            var obj = WwiseUtility.GetWwiseObjectsBySelection();

            foreach (var wo in obj)
            {
                string name = wo.Name;
                //Console.WriteLine(wo.Path);
                var wu = WwiseUtility.CreateObject(wo.Name + "_", WwiseObject.ObjectType.WorkUnit, wo.Parent.Path);

                WwiseUtility.MoveToParent(wo, wu);

                WwiseUtility.SaveWwiseProject();
            }

            WwiseUtility.Close().Wait();
        }


        static void MoveOutOfFolder()
        {
            WwiseUtility.SaveWwiseProject();
            var obj = WwiseUtility.GetWwiseObjectsBySelection();

            foreach (var wo in obj)
            {
                if (wo.Type == WwiseObject.ObjectType.Folder.ToString())
                {
                    //Console.WriteLine(wo.Path);
                    var children = new WwiseFolder(wo).GetChildren();
                    foreach (var child in children)
                    {
                        WwiseUtility.MoveToParent(child, wo.Parent);
                    }
                    WwiseUtility.DeleteObject(wo.Path);
                    WwiseUtility.SaveWwiseProject();
                }
            }

            WwiseUtility.Close().Wait();
        }

        static void ChangeFolderToActorMixer()
        {
            WwiseUtility.SaveWwiseProject();

            var obj = WwiseUtility.GetWwiseObjectsBySelection();

            foreach (var wo in obj)
            {
                if (wo.Type == WwiseObject.ObjectType.Folder.ToString())
                {
                    //if (wo.Parent.Type == WwiseObject.ObjectType.Folder.ToString()) continue;
                    //Console.WriteLine(wo.Path);
                    var am = WwiseUtility.CreateObject(wo.Name + "_", WwiseObject.ObjectType.ActorMixer, wo.Parent.Path);
                    var children = new WwiseFolder(wo).GetChildren();
                    foreach (var child in children)
                    {
                        WwiseUtility.MoveToParent(child, am);
                    }
                    WwiseUtility.DeleteObject(wo.Path);
                    WwiseUtility.ChangeObjectName(am, am.Name.Substring(0, am.Name.Length - 1));

                    WwiseUtility.SaveWwiseProject();
                }
            }

            WwiseUtility.Close().Wait();
        }


        static void AddPrefix()
        {
            WwiseUtility.SaveWwiseProject();

            var obj = WwiseUtility.GetWwiseObjectsBySelection();

            Console.WriteLine("Enter Prefix:\n");

            string prefix = Console.ReadLine();

            foreach (var wo in obj)
            {
                WwiseUtility.ChangeObjectName(wo, prefix + wo.Name);
            }

            WwiseUtility.Close().Wait();
        }

        static void ChangeFolderToWorkUnit()
        {
            WwiseUtility.SaveWwiseProject();

            var obj = WwiseUtility.GetWwiseObjectsBySelection();

            foreach (var wo in obj)
            {
                if (wo.Type == WwiseObject.ObjectType.Folder.ToString())
                {
                    string project_path = WwiseUtility.GetWwiseProjectPath();
                    string wo_path = wo.Path;
                    string wwu_path = project_path.Replace($"{WwiseUtility.GetWwiseProjectName()}.wproj", "") + wo_path.Split(new char[] { '\\', '\\'})[1] + "\\" + wo.Name + ".wwu";
                    if (File.Exists(wwu_path))
                    {
                        Console.WriteLine($"\n" +
                            $"File {wwu_path} already exists! Please remove or rename it first!\nPress \'Enter\' to continue ...");
                        Console.ReadLine();
                        continue;
                    }
                    string origin_name = wo.Name;
                    WwiseUtility.ChangeObjectName(wo, wo.Name+"_");
                    //if (wo.Parent.Type == WwiseObject.ObjectType.Folder.ToString()) continue;
                    //Console.WriteLine(wo.Path);
                    var wu = WwiseUtility.CreateObject(origin_name, WwiseObject.ObjectType.WorkUnit, wo.Parent.Path);
                    var children = new WwiseFolder(wo).GetChildren();
                    foreach (var child in children)
                    {
                        WwiseUtility.MoveToParent(child, wu);
                    }
                    WwiseUtility.DeleteObject(wo.Path);

                    WwiseUtility.SaveWwiseProject();
                }
            }

            WwiseUtility.Close().Wait();
        }*/
    }
}
