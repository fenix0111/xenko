// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.

namespace SiliconStudio.Xenko.Rendering
{
    /// <summary>
    /// Implementation of <see cref="ResourceGroupLayout"/> specifically for PerView cbuffer of <see cref="RenderSystem"/>.
    /// </summary>
    public class ViewResourceGroupLayout : RenderSystemResourceGroupLayout
    {
        public ResourceGroupEntry[] Entries;
    }
}
