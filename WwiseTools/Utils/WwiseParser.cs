using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WwiseTools.Basic;

namespace WwiseTools.Utils
{
    public class WwiseParser
    {
        public string[] WorkUnit => workUnit;
        string[] workUnit;

        public string FilePath => path;
        string path;

        public WwiseParser()
        {
            if (WwiseUtility.ProjectPath == null)
            {
                Console.WriteLine("WwiseUtility not initialized!");
                return;
            }
        }

        public string[] Parse(string file_path)
        {
            string _path = Path.Combine(WwiseUtility.ProjectPath, file_path);
            
            //StreamReader f = new StreamReader();
            List<string> lines = new List<string>();
            StreamReader f = File.OpenText(_path);
            try
            {
                
                while (true)
                {
                    string line = f.ReadLine();
                    if (line == null || line.Trim() == "")
                    {
                        break;
                    }
                    lines.Add(line);
                }

                path = _path;

            }
            catch
            {
                Console.WriteLine("Invalid path!");
            }
            finally
            {
                f.Close();
            }

            return workUnit = lines.ToArray();
        }

        public string[] ParseText(string text)
        {
            var result = Regex.Split(text, "\r\n|\r|\n");
            return result;
        }

        public void CommitChange(bool backup = true)
        {
            File.Copy(path, path + ".backup", true);
            string text = "";
            foreach (var l in workUnit)
            {
                text += l + "\n";
            }
            File.WriteAllText(path, text, Encoding.UTF8);
        }

        public void AddChildToWorkUnit(WwiseUnit child)
        {
            if (WorkUnit == null)
            {
                Console.WriteLine("Parse the file first!");
                return;
            }

            if (WorkUnit[3].Contains("/>"))
            {
                WwiseWorkUnit wu = GetWorkUnit();
                wu.AddChild(child);
                workUnit = ParseText(wu.Print());

            }
            else
            {
                List<string> lines = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    lines.Add(workUnit[i]);
                }
                child.tabs = 4;
                string[] newLines = ParseText(child.Print(false));
                foreach (var l in newLines)
                {
                    lines.Add(l);
                }
                for (int i = 5; i < workUnit.Length; i++)
                {
                    lines.Add(workUnit[i]);
                }
                workUnit = lines.ToArray();
            }
        }

        public void AddChildToUnit(string unitName, string type, WwiseUnit child)
        {
            if (WorkUnit == null)
            {
                Console.WriteLine("Parse the file first!");
                return;
            }
            string line = "";
            int index = 0;
            for (int i = 0; i < workUnit.Length; i++)
            {

                
                if (workUnit[i].Contains("Name=" + String.Format("\"{0}\"", unitName)))
                {
                    line = workUnit[i];
                    index = i;
                    break;
                }
            }

            if (line == "")
            {
                Console.WriteLine(type + " " +  unitName + " not found!");
                return;
            }

            int tabs = GetTabCount(line);

            if (line.Contains("/>"))
            {
                workUnit[index] = line.Replace("/>", ">");
                WwiseNode list = WwiseNode.NewChildrenList(new List<IWwisePrintable>()
                {
                    child
                });
                list.tabs = tabs + 1;
                List<string> newLines = ParseText(list.Print(false)).ToList();
                string t = "";
                for (int i = 0; i < tabs; i++)
                {
                    t += "\t";
                }
                newLines.Add(t + String.Format("</{0}>", type));

                List<string> lines = new List<string>();
                for (int i = 0; i < index + 1; i++)
                {
                    lines.Add(workUnit[i]);
                }
                foreach (var l in newLines)
                {
                    lines.Add(l);
                }
                for (int i = index + 1; i < workUnit.Length; i++)
                {
                    lines.Add(workUnit[i]);
                }
                workUnit = lines.ToArray();
            }
            else
            {
                int endIndex = -1;
                for (int i = index + 1; i < workUnit.Length; i ++)
                {
                    if (GetTabCount(workUnit[index]) == GetTabCount(workUnit[i]) && workUnit[i].Replace("\t", "").Trim() == String.Format("</{0}>", type))
                    {
                        endIndex = i;
                        break;
                    }
                }

                if (endIndex == -1)
                {
                    Console.WriteLine("File is broken!");
                    return;
                }
                int childIndex = -1;
                for (int i = index + 1; i < endIndex; i++)
                {
                    if (GetTabCount(workUnit[index]) + 1 == GetTabCount(workUnit[i]) && workUnit[i].Replace("\t", "").Trim() == "<ChildrenList>")
                    {
                        childIndex = i;
                        break;
                    }
                }
                if (childIndex == -1) // No ChildrenList
                {
                    WwiseNode list = WwiseNode.NewChildrenList(new List<IWwisePrintable>()
                    {
                        child
                    });
                    list.tabs = tabs + 1;
                    List<string> newLines = ParseText(list.Print(false)).ToList();

                    List<string> lines = new List<string>();
                    for (int i = 0; i < endIndex; i++)
                    {
                        lines.Add(workUnit[i]);
                    }
                    foreach (var l in newLines)
                    {
                        lines.Add(l);
                    }
                    for (int i = endIndex; i < workUnit.Length; i++)
                    {
                        lines.Add(workUnit[i]);
                    }
                    workUnit = lines.ToArray();
                }
                else // Has ChildrenList
                {
                    child.tabs = tabs + 2;
                    List<string> newLines = ParseText(child.Print(false)).ToList();


                    List<string> lines = new List<string>();
                    for (int i = 0; i < childIndex + 1; i++)
                    {
                        lines.Add(workUnit[i]);
                    }
                    foreach (var l in newLines)
                    {
                        lines.Add(l);
                    }
                    for (int i = childIndex + 1; i < workUnit.Length; i++)
                    {
                        lines.Add(workUnit[i]);
                    }
                    workUnit = lines.ToArray();
                }
            }
        }

        public override string ToString()
        {
            string result = "";
            foreach (var l in workUnit)
            {
                result += l + "\n";
            }
            return result;
        }

        public WwiseUnit GetUnitByName(string name, string[] file)
        {
            string type = "";
            string guid = "";
            foreach (var l in file)
            {
                if (l.Contains(name))
                {   
                    var properties = l.Replace('\t', ' ').Trim().Split(' ');
                    string t = properties[0].Trim().Replace("<", "");
                    type = t.Trim();

                    int id_index = 0;
                    string n = null;



                    n = GetName(ref id_index, properties);

                    n = n.Replace("Name=\"", "");
                    n = n.Replace("\"", "");
                    name = n.Trim();

                    string g = properties[id_index].Replace("ID=\"{", "");
                    g = g.Replace("}\">", "");
                    guid = g.Trim();
                    break;
                }
            }

            return new WwiseUnit(name, type, guid);
        }

        public WwiseWorkUnit GetWorkUnit()
        {
            return GetWorkUnit(WorkUnit);
        }

        public WwiseWorkUnit GetWorkUnit(string[] file)
        {
            string name="";
            string type="";
            string guid="";
            foreach (var line in file)
            {
                if (line == null || line.Trim() == "")
                {
                    break;
                }
                if (GetTabCount(line) == 1)
                {
                    string tmp = line.Trim().Replace('<', ' ');
                    tmp = tmp.Trim().Replace('>', ' ');
                    type = tmp.Trim();
                }
                if (GetTabCount(line) == 2)
                {
                    var properties = line.Split(' ');

                    int id_index = 0;
                    string n = null;

                    n = GetName(ref id_index, properties);

                    n = n.Replace("Name=\"", "");
                    n = n.Replace("\"", "");
                    name = n.Trim();

                    string g = properties[id_index].Replace("ID=\"{", "");
                    g = g.Replace("}\"", "");
                    g = g.Replace(">", "");
                    guid = g.Trim();
                    Console.WriteLine( "332" + name + guid);
                    break;
                }
            }

            return new WwiseWorkUnit(name, type, guid);
        }

        private string GetName(ref int index, string[] properties)
        {
            string n = "";
            foreach (var s in properties)
            {
                if (s.StartsWith("Name="))
                {
                    n = "";
                }
                if (n != null && !s.StartsWith("ID="))
                {
                    n += s + " ";
                }
                if (s.StartsWith("ID="))
                {
                    foreach (var j in properties)
                    {
                        if (j == s)
                        {
                            break;
                        }
                        index += 1;
                    }
                    break;
                }

            }

            return n;
        }

        private int GetTabCount(string line)
        {
            return line.Count(c => c == '\t');
        }
    }
}
