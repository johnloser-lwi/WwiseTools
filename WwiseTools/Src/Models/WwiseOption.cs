using System.Collections.Generic;
using System.Reflection;

namespace WwiseTools.Models
{
    public abstract class WwiseOption
    {
        public virtual string[] GetOptions()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var result = new List<string>();

            foreach (var property in properties)
            {
                if (property.GetValue(this).ToString() != "True") continue;
                result.Add(property.Name);
            }

            return result.ToArray();
        }
    }
}
