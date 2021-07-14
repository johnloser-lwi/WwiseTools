using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
//using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools
{
    /// <summary>
    /// 容器继承WwiseUnit，默认引用了Default Conversion Settings, Master Audio Bus
    /// </summary>
    public class WwiseContainer : WwiseUnit
    {

        protected WwiseNode referenceList;
        public WwiseNode ReferenceList => referenceList;

        public WwiseContainer(string _name, string u_type, WwiseParser parser) : base(_name, u_type, parser)
        {
            //AddChildrenList();
            Init(parser);
        }

        public WwiseContainer(string _name, string u_type, string guid, WwiseParser parser) : base(_name, u_type, guid, parser)
        {
            //AddChildrenList();
            Init(parser);
        }

        protected virtual void Init(WwiseParser parser)
        {
            if (WwiseUtility.ProjectPath == null)
            {
                Console.WriteLine("WwiseUtility not initialized!");
                return;
            }

            referenceList = new WwiseNode("ReferenceList", parser);
            referenceList.AddChildNode(WwiseUtility.GetWwiseDefaultConversionSettings(parser));
            referenceList.AddChildNode(WwiseUtility.GetMasterAudioBus(parser));
            AddChildNode(referenceList);
        }


    }
}
