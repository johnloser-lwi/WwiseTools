using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Objects;

namespace WwiseTools.Utils.Feature2021
{
    public class Waql : IEnumerable<WwiseObject>
    {
        private string waql_command;
        public List<WwiseObject> Result;
        public Waql(string waql)
        {
            waql_command = FormatQuery(waql);
        }

        private bool VersionVerify()
        {
            if (WwiseUtility.ConnectionInfo.Version.Year < 2021)
            {
                WaapiLog.Log($"Warning: Class Waql is a Wwise 2021 feature! " +
                             $"Current Wwise version is {WwiseUtility.ConnectionInfo.Version.ToString()}.");
                return false;
            }
            return true;
        }

        private string FormatQuery(string waql)
        {
            if (!waql.StartsWith("$")) waql = $"$ " + waql;

            return waql;
        }

        public async ValueTask<bool> RunAsync(string waql = "")
        {
            if (!await WwiseUtility.TryConnectWaapiAsync()) return false;
            if (!VersionVerify()) return false;

            if (!string.IsNullOrEmpty(waql)) waql_command = FormatQuery(waql);

            if (Result == null) Result = new List<WwiseObject>();
            else Result.Clear();

            try
            {
                var query = new
                {
                    waql =  waql_command
                };

                var option = new
                {
                    @return = new string[] { "name", "id", "type" }
                };
                var jresult = await WwiseUtility.Client.Call("ak.wwise.core.object.get", query, option);
                foreach (var obj in jresult["return"])
                {
                    string name = obj["name"].ToString();
                    string id = obj["id"].ToString();
                    string type = obj["type"].ToString();

                    Result.Add(new WwiseObject(name, id, type));
                }
            }
            catch (Exception e)
            {
                WaapiLog.Log($"Failed to run query {waql_command}! ======> {e.Message}");
                Result = null;
                return false;
            }

            return true;
        }

        public IEnumerator<WwiseObject> GetEnumerator()
        {
            foreach (var wwiseObject in Result)
            {
                yield return wwiseObject;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
