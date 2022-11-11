﻿using System.Threading.Tasks;
using WwiseTools.Objects;
using WwiseTools.References;
using WwiseTools.Utils;

namespace WwiseTools.WwiseTypes
{
    public abstract class SwitchContainerBase : WwiseTypeBase
    {

        public async Task SetSwitchGroupOrStateGroupAsync(WwiseReference group)
        {
            await WwiseUtility.Instance.SetObjectReferenceAsync(WwiseObject, group);
        }

        public async Task SetDefaultSwitchOrStateAsync(WwiseReference switchOrState)
        {
            await WwiseUtility.Instance.SetObjectReferenceAsync(WwiseObject, switchOrState);
        }

        public SwitchContainerBase(WwiseObject wwiseObject, string typeFilter) : base(wwiseObject, typeFilter)
        {
        }
    }
}