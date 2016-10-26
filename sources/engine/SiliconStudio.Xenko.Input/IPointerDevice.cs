﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using System;
using System.Collections.Generic;
using SiliconStudio.Core.Mathematics;

namespace SiliconStudio.Xenko.Input
{
    public interface IPointerDevice : IInputDevice
    {
        /// <summary>
        /// Raised when a pointer pointer event is triggered, this event will guarantee that a pointer Move event is always surrounded with a pointer Down and Up event.
        /// </summary>
        EventHandler<PointerEvent> OnPointer { get; set; }

        /// <summary>
        /// Raised whenever a pointer is moved, this doesn't nessecarily have to happen between Down/Up events (in the case of mice/pens)
        /// </summary>
        EventHandler<PointerEvent> OnMoved { get; set; }

        /// <summary>
        /// Raised when the sureface size of this pointer changed
        /// </summary>
        EventHandler OnSurfaceSizeChanged { get; set; }

        /// <summary>
        /// List of pointer events since the last update
        /// </summary>
        IReadOnlyList<PointerEvent> PointerEvents { get; }

        /// <summary>
        /// The type of the pointer device
        /// </summary>
        PointerType Type { get; }

        /// <summary>
        /// Normalized position(0,1) of the pointer
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Difference from the last position to the current position
        /// </summary>
        Vector2 Delta { get; }
        
        // TODO: Add interface for multiple pointer Id's, so that the Position,Delta,State,etc. can be retrieved

        /// <summary>
        /// The size of the surface used by the pointer, for a mouse this is the size of the window, for a touch device, the size of the touch area, etc.
        /// </summary>
        Vector2 SurfaceSize { get; }

        /// <summary>
        /// The size of the surface used by the pointer, for a mouse this is the size of the window, for a touch device, the size of the touch area, etc.
        /// </summary>
        float SurfaceAspectRatio { get; }
    }
}