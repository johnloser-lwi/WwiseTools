using System.Collections.Generic;
using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.Utils;

namespace WwiseTools.Components
{
    public class Hierarchy : ComponentBase
    {
        public async Task<List<WwiseObject>> GetChildrenAsync()
        {
            return await WwiseUtility.Instance.GetWwiseObjectChildrenAsync(WwiseObject);
        }

        public async Task AddChildAsync(WwiseObject wwiseObject)
        {
            if (wwiseObject == null) return;
            await WwiseUtility.Instance.MoveToParentAsync(wwiseObject, WwiseObject);
        }

        public Hierarchy(WwiseObject wwiseObject) : base(wwiseObject, "!SoundBank")
        {
        }
    }
}
