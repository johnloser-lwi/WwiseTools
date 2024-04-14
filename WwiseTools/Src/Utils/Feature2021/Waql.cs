using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.Serialization;

namespace WwiseTools.Utils.Feature2021
{
    public class Waql : IEnumerable<WwiseObject>
    {
        private string _waqlCommand;
        private List<WwiseObject> _result;

        public List<WwiseObject> Result
        {
            get
            {
                if (_result == null) _result = new List<WwiseObject>();
                return _result;
            }
            private set
            {
                _result = value;
            }
        }

        public Waql(string waql)
        {
            _waqlCommand = FormatQuery(waql);
        }

        private string FormatQuery(string waql)
        {
            if (!waql.StartsWith("$")) waql = $"$ " + waql;

            return waql;
        }

        public async Task<bool> RunAsync(string waql = "")
        {
            if (!await WwiseUtility.Instance.TryConnectWaapiAsync()) return false;
            if (!VersionHelper.VersionVerify(VersionHelper.V2021_1_0_7575)) return false;

            if (!string.IsNullOrEmpty(waql)) _waqlCommand = FormatQuery(waql);

            else Result.Clear();

            try
            {
                var query = new
                {
                    waql =  _waqlCommand
                };

                var option = new
                {
                    @return = new string[] { "name", "id", "type", "path" }
                };
                var jresult = await WwiseUtility.Instance.CallAsync("ak.wwise.core.object.get", query, option, WwiseUtility.Instance.TimeOut);
                var returnData = WaapiSerializer.Deserialize<ReturnData<ObjectReturnData>>(jresult.ToString());
                if (returnData.Return.Length == 0) return false;
                foreach (var obj in returnData.Return)
                {
                    var name = obj.Name;
                    var id = obj.ID;
                    var type = obj.Type;
                    var path = obj.Path;

                    Result.Add(new WwiseObject(name, id, type, path));
                }
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to run query {_waqlCommand}! ======> {e.Message}");
                Result = new List<WwiseObject>();
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
