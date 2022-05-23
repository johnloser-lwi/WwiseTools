using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Objects;

namespace WwiseTools.Utils
{
    public class Waql
    {
        private string waql_command;
        public List<WwiseObject> Result = new List<WwiseObject>();
        public Waql(string waql)
        {
            this.waql_command = "$ " +  waql;
        }

        public async Task<bool> RunAsync()
        {
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
                var jresult = await WwiseUtility.Client.Call(ak.wwise.core.@object.get, query, option);
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
                Console.WriteLine($"Failed to run query {waql_command}! ======> {e.Message}");

                return false;
            }

            return true;
        }
    }
}
