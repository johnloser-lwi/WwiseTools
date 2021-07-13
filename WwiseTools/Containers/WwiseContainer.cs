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
    /// <summary>
    /// 容器继承WwiseUnit，默认引用了Default Conversion Settings, Master Audio Bus
    /// </summary>
    public class WwiseContainer : WwiseUnit
    {

        public WwiseContainer(string _name, string u_type) : base(_name, u_type)
        {
            //AddChildrenList();
        }

        public WwiseContainer(string _name, string u_type, string guid) : base(_name, u_type, guid)
        {
            //AddChildrenList();
        }

        protected override void Init(string _name, string u_type, string guid)
        {
            base.Init(_name, u_type, guid);

            if (WwiseUtility.ProjectPath == null)
            {
                Console.WriteLine("WwiseUtility not initialized!");
                return;
            }

            AddChildNode(WwiseNode.NewReferenceList(new List<IWwisePrintable>()
            {
                WwiseUtility.DefualtConversionSettings,
                WwiseUtility.MasterAudioBus
            }));
        }


    }
}
