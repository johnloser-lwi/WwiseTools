using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WwiseTools.Serialization;

namespace WwiseTools.Utils.Feature2023;

public static class WwiseUtility2023Extension
{
    public static async Task<object> ExecuteLuaScriptAsync(this WwiseUtility utility, string luaScript, string[] luaPaths, string[] requires, string[] doFiles)
    {
        if (!await utility.TryConnectWaapiAsync()) return null;
        if (!VersionHelper.VersionVerify(VersionHelper.V2023_1_0_8367)) return null;

        var func = utility.Function.Verify("ak.wwise.core.executeLuaScript");

        try
        {
            var args = new
            {
                luaScript = luaScript,
                luaPaths = luaPaths,
                requires = requires,
                doFiles = doFiles
            };

            var res = await utility.CallAsync(func, args, null);
            var returnData = WaapiSerializer.Deserialize<Dictionary<string, object>>(res.ToString());
            
            if (returnData is not null)
            {
                return returnData["return"];
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {
            WaapiLog.InternalLog("Failed to execute Lua Script! ======> " + e.Message);
        }

        return null;
    }
}