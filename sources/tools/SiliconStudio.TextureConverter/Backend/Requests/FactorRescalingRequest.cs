// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
namespace SiliconStudio.TextureConverter.Requests
{
    /// <summary>
    /// Request a texture to be rescale according to the given factor, new size will be : width = width * widthFactor and height = height * heightFactor
    /// </summary>
    internal class FactorRescalingRequest : RescalingRequest
    {

        /// <summary>
        /// The width factor
        /// </summary>
        private float widthFactor;

        /// <summary>
        /// The height factor
        /// </summary>
        private float heightFactor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FactorRescalingRequest"/> class.
        /// </summary>
        /// <param name="widthFactor">The width factor.</param>
        /// <param name="heightFactor">The height factor.</param>
        /// <param name="filter">The filter.</param>
        public FactorRescalingRequest(float widthFactor, float heightFactor, Filter.Rescaling filter) : base(filter)
        {
            this.widthFactor = widthFactor;
            this.heightFactor = heightFactor;
        }

        public override int ComputeWidth(TexImage texImage)
        {
            return (int)(texImage.Width * widthFactor);
        }

        public override int ComputeHeight(TexImage texImage)
        {
            return (int)(texImage.Height * heightFactor);
        }
    }
}
