﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WwiseTools
{
    public class WwiseWorkUnit : WwiseUnit, IWwiseID, IWwisePrintable
    {

        public override int tabs { get => 0; set { return; } }

        public string id { get { return guid;  }}

        public int SchemaVersion { get => schemaVersion; set { schemaVersion = value; xml_head = String.Format("<WwiseDocument Type=\"WorkUnit\" ID=\"{{{0}}}\" SchemaVersion=\"{1}\">", id, schemaVersion); } }

        private int schemaVersion = 97;


        public WwiseWorkUnit(string _name, string u_type): base(_name, u_type)
        {
            Init(_name, u_type, Guid.NewGuid().ToString().ToUpper());
            AddChildrenList();
        }

        public WwiseWorkUnit(string _name, string u_type, string guid):base(_name, u_type, guid)
        {

            Init(_name, u_type, guid);
            AddChildrenList();
        }


        public override string Print()
        {
            string result = "";
            result += xml_head;

            foreach (IWwisePrintable u in xml_body)
            {
                if (u == null) continue;
                u.tabs = tabs + 3;
                result += u.Print();
            }

            result += xml_tail;

            return result;
        }

        protected override void Init(string _name, string u_type, string guid)
        {
            xml_head = "<WwiseDocument Type=\"WorkUnit\" ID=\"{{{0}}}\" SchemaVersion=\"{1}\">";
            xml_tail = "</WwiseDocument>";

            unit_name = _name;
            this.u_type = u_type;
            this.guid = guid;
            xml_head = String.Format(xml_head, id, schemaVersion);

            xml_head = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" + xml_head;
            xml_head += String.Format("\n\t<{0}>", type);
            xml_head += String.Format("\n\t\t<WorkUnit Name=\"{0}\" ID=\"{{{1}}}\">", unit_name, guid);
            xml_tail = String.Format("\n\t</{0}>\n", type) + xml_tail;
            xml_tail = "\n\t\t</WorkUnit>" + xml_tail;
        }

    }
}
