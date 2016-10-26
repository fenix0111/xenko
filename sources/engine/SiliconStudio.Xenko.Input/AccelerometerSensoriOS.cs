﻿// Copyright (c) 2016 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

#if SILICONSTUDIO_PLATFORM_IOS
using SiliconStudio.Core.Mathematics;

namespace SiliconStudio.Xenko.Input
{
    public class AccelerometerSensoriOS : SensoriOS, IAccelerometerSensor
    {
        public Vector3 Acceleration => AccelerationInternal;
        internal Vector3 AccelerationInternal;

        public AccelerometerSensoriOS() : base("Accelerometer")
        {
        }
    }
}
#endif