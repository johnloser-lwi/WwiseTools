using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WwiseTools.Models
{
    public class WwiseInfo
    {
        public WwiseVersion Version { get; set; }

        public int ProcessID { get; set; }

        public bool IsCommandLine { get; set; }

        public string ProjectName { get; set; }
        
        public string ProjectPath { get; set; }

        public string ProjectFolder => Path.GetDirectoryName(ProjectPath);
        
        public List<string> Languages { get; set; }

        public override string ToString()
        {
            string result = "";
            result += "Project Name: " + ProjectName + "\n";
            result += "Project Path: " + ProjectPath + "\n";
            result += "Wwise Version: " + Version.VersionString + "\n";
            result += "Schema Version: " + Version.Schema.ToString() + "\n";
            result += "Process ID: " + ProcessID + "\n";

            return result;
        }
    }

    public class WwiseVersion
    {
        public int Year { get; set; }

        public int Major { get; set; }
        public int Minor { get; set; }

        public int Build { get; set; }

        public int Schema { get; set; }

        public string VersionString
        {
            get
            {
                return string.Format("v{0}.{1}.{2}.{3}", Year, Major, Minor, Build);
            }
        }

        public WwiseVersion(int year, int major, int minor, int build, int schema = 0)
        {
            Year = year;
            Major = major;
            Minor = minor;
            Build = build;
            Schema = schema;
        }

        public override bool Equals(object obj)
        {
            WwiseVersion other = obj as WwiseVersion;
            return VersionString == other?.VersionString;
        }

        public override string ToString()
        {
            return VersionString;
        }

        public override int GetHashCode()
        {
            return VersionString.GetHashCode();
        }

        public static bool operator ==(WwiseVersion left, WwiseVersion right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WwiseVersion left, WwiseVersion right)
        {
            return !Equals(left, right);
        }

        public static bool operator >(WwiseVersion left, WwiseVersion right)
        {
            if (left.Year > right.Year) return true;
            if (left.Year < right.Year) return false;
            if (left.Major > right.Major) return true;
            if (left.Major < right.Major) return false;
            if (left.Minor > right.Minor) return true;
            if (left.Minor < right.Minor) return false;
            if (left.Build > right.Build) return true;
            if (left.Build < right.Build) return false;
            return false;
        }

        public static bool operator <(WwiseVersion left, WwiseVersion right)
        {
            return !(left > right);
        }

        public static bool operator >=(WwiseVersion left, WwiseVersion right)
        {
            return left > right || left == right;
        }

        public static bool operator <=(WwiseVersion left, WwiseVersion right)
        {
            return left < right || left == right;
        }
    }
}
