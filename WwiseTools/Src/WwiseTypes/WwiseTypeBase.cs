using System.Linq;
using WwiseTools.Objects;
using WwiseTools.Utils;

namespace WwiseTools.WwiseTypes
{
    public abstract class WwiseTypeBase
    {
        public bool Valid => WwiseObject != null;
        public WwiseObject WwiseObject { get; protected set; }

        protected WwiseTypeBase(WwiseObject wwiseObject, string typeFilter)
        {
            
            if (string.IsNullOrEmpty(typeFilter) || CanCastToType(typeFilter, wwiseObject.Type))
            {
                WwiseObject = wwiseObject;
                return;
            }
            
            WaapiLog.InternalLog($"{wwiseObject.Name} which is a {wwiseObject.Type} doesn't meet the type requirement {typeFilter}! Cast to {GetType().Name} failed");
        }

        private bool CanCastToType(string typeFilter, string type)
        {
            bool excludeMode = false;
            if (typeFilter.StartsWith("!"))
            {
                typeFilter = typeFilter.Substring(1, typeFilter.Length - 1);
                excludeMode = true;
            }
            var types = typeFilter.Split(',')?.Select(t => t.Trim()).Distinct();
            var enumerable = types as string[] ?? types.ToArray();
            if (type == typeFilter) return true;
            
            
            
            if (excludeMode && !enumerable.Contains(type)) return true;

            if (!excludeMode && enumerable.Contains(type)) return true;

            return false;
        }
    }
}
