using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools
{
    public class WwiseUnit : WwiseNode, IWwiseID, IWwiseName
    {
        public WwiseNode Children => children;
        protected WwiseNode children;

        //public WwiseNode Properties => properties;
        protected WwiseNode properties;
        protected List<WwiseProperty> propertyList = new List<WwiseProperty>();

        public static WwiseUnit CreateSound(string name, string language, string file)
        {

            if (WwiseUtility.ProjectPath == null)
            {
                Console.WriteLine("WwiseUtility not initialized!");
                return null;
            }
            WwiseUnit sound = new WwiseUnit(name, "Sound");

            if (language != "SFX")
            {
                sound.AddChildNode(WwiseNode.NewPropertyList(new List<IWwisePrintable>()
                {
                    new WwiseProperty("IsVoice", "bool", "True")
                }));
            }
           

            WwiseUnit audioFileSource = new WwiseUnit(name, "AudioFileSource");
            audioFileSource.AddChildNode(new WwiseProperty("Language", language));
            audioFileSource.AddChildNode(new WwiseProperty("AudioFile", file));

            sound.AddChildNode(WwiseNode.NewReferenceList(new List<IWwisePrintable>()
            {
                WwiseUtility.DefualtConversionSettings,
                WwiseUtility.MasterAudioBus
            }));
            sound.AddChildNode(WwiseNode.NewChildrenList(new List<IWwisePrintable>()
            {
                audioFileSource
            }));

            
            //sound_cl.AddChildNode(audioFileSource);
            return sound;
        }

        public string name => unit_name;

        public string id => guid;

        protected string guid;
        protected string unit_name;
        public WwiseUnit(string _name, string u_type) : base(u_type)
        {
            Init(_name, u_type, Guid.NewGuid().ToString().ToUpper());
        }

        public WwiseUnit(string _name, string u_type, string guid): base(u_type)
        {
            Init(_name, u_type, guid);
        }

        protected virtual void Init(string _name, string u_type, string guid)
        {
            unit_name = _name;
            this.u_type = u_type;
            this.guid = guid;
            xml_head = String.Format("<{0} Name=\"{1}\" ID=\"{{{2}}}\">", u_type, unit_name, guid);
            xml_tail = String.Format("</{0}>", u_type);
        }

        public virtual WwiseUnit AddChild(WwiseUnit child)
        {
            AddChildrenList();
            Children.AddChildNode(child);
            return child;
        }

        public virtual WwiseProperty AddProperty(WwiseProperty property)
        {
            if (properties == null)
            {
                properties = new WwiseNode("PropertyList");
                AddChildNodeAtFront(properties);
            }

            int remove_index = 0;
            bool remove = false;
            foreach (var p in propertyList)
            {
                if (p.Name == null) continue;
                if (p.Name == property.Name)
                {
                    properties.body.RemoveAt(remove_index);
                    remove = true;
                    break;
                    //propertyList.Remove(p);
                }
                remove_index += 1;
            }
            properties.AddChildNode(property);
            if (remove) propertyList.RemoveAt(remove_index);
            propertyList.Add(property);
            //RefreshProperty();
            return property;
        }

        /// <summary>
        /// Generate children list and add it anyway.
        /// </summary>
        protected virtual void AddChildrenList()
        {
            if (children != null) return;
            AddChildNode(children = new WwiseNode("ChildrenList"));
        }
    }
}
