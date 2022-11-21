using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.Utils;

namespace WwiseTools.References
{
    public class WwiseReference
    {
        public string Name { get; set; }
        public string ID { get; set; }
        

        public WwiseReference(string name, WwiseObject wwiseObject)
        {
            Name = name;
            ID = wwiseObject.ID;
        }

        public WwiseReference(string name, string id)
        {
            Name = name;
            ID = id;
        }

        public static WwiseReference Ref_Attenuation(WwiseObject wwiseObject)
        {
            //if (wwiseObject == null) return null;

            if (wwiseObject != null && wwiseObject.Type != WwiseObject.ObjectType.Attenuation.ToString())
            {
                return null;
            }
            return new WwiseReference("Attenuation", wwiseObject);
        }

        public static WwiseReference Ref_Conversion(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return null;
            if (wwiseObject.Type != WwiseObject.ObjectType.Conversion.ToString())
            {
                return null;
            }
            return new WwiseReference("Conversion", wwiseObject);
        }

        public static WwiseReference Ref_Effect0(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return null;
            if (wwiseObject.Type != "Effect")
            {
                return null;
            }
            return new WwiseReference("Effect0", wwiseObject);
        }

        public static WwiseReference Ref_Effect1(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return null;
            if (wwiseObject.Type != "Effect")
            {
                return null;
            }
            return new WwiseReference("Effect1", wwiseObject);
        }

        public static WwiseReference Ref_Effect2(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return null;
            if (wwiseObject.Type != "Effect")
            {
                return null;
            }
            return new WwiseReference("Effect2", wwiseObject);
        }

        public static WwiseReference Ref_Effect3(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return null;
            if (wwiseObject.Type != "Effect")
            {
                return null;
            }
            return new WwiseReference("Effect3", wwiseObject);
        }

        public static WwiseReference Ref_OutputBus(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return null;
            if (wwiseObject.Type != "Bus")
            {
                return null;
            }
            return new WwiseReference("OutputBus", wwiseObject);
        }
        public static WwiseReference Ref_ReflectionsAuxSend(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return null;
            if (wwiseObject.Type != "AuxBus")
            {
                return null;
            }
            return new WwiseReference("ReflectionsAuxSend", wwiseObject);
        }

        public static WwiseReference Ref_UserAuxSend0(WwiseObject wwiseObject)
        {
            //if (wwiseObject == null) return null;
            if (wwiseObject != null && wwiseObject.Type != "AuxBus")
            {
                return null;
            }
            return new WwiseReference("UserAuxSend0", wwiseObject);
        }

        public static WwiseReference Ref_UserAuxSend1(WwiseObject wwiseObject)
        {
            //if (wwiseObject == null) return null;
            if (wwiseObject != null && wwiseObject.Type != "AuxBus")
            {
                return null;
            }
            return new WwiseReference("UserAuxSend1", wwiseObject);
        }

        public static WwiseReference Ref_UserAuxSend2(WwiseObject wwiseObject)
        {
            //if (wwiseObject == null) return null;
            if (wwiseObject != null && wwiseObject.Type != "AuxBus")
            {
                return null;
            }
            return new WwiseReference("UserAuxSend2", wwiseObject);
        }

        public static WwiseReference Ref_UserAuxSend3(WwiseObject wwiseObject)
        {
            //if (wwiseObject == null) return null;
            if (wwiseObject != null && wwiseObject.Type != "AuxBus")
            {
                return null;
            }
            return new WwiseReference("UserAuxSend3", wwiseObject);
        }

        public static WwiseReference Ref_SwitchGroupOrStateGroup(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return null;
            if (wwiseObject.Type != "SwitchGroup" && wwiseObject.Type != "StateGroup")
            {
                return null;
            }
            return new WwiseReference("SwitchGroupOrStateGroup", wwiseObject);
        }

        public static WwiseReference Ref_DefaultSwitchOrState(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return null;
            if (wwiseObject.Type != "Switch" && wwiseObject.Type != "State")
            {
                return null;
            }
            return new WwiseReference("DefaultSwitchOrState", wwiseObject);
        }
        
        public static WwiseReference Ref_Target(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return null;

            return new WwiseReference("Target", wwiseObject);
        }
    }
}
