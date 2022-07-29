using System.Linq;
using WwiseTools.Objects;
using WwiseTools.Utils;

namespace WwiseTools.Components
{
    public abstract class ComponentBase
    {
        public bool Valid => WwiseObject != null;
        public WwiseObject WwiseObject { get; protected set; }

        protected ComponentBase(WwiseObject wwiseObject, string typeFilter)
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
            var types = typeFilter.Split(',')?.Select(t => t.Trim()).Distinct();
            var enumerable = types as string[] ?? types.ToArray();
            if (enumerable.Contains(type) || type == typeFilter) return true;

            if (enumerable.Contains("!" + type)) return false;

            return false;
        }
    }
}
