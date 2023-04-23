﻿namespace IPT.Common.API
{
    #pragma warning disable 1591, SA1602

    /// <summary>
    /// The cursor type.
    /// </summary>
    public enum CursorType
    {
        Default,
        Pointing,
        Resizing,
    }

    /// <summary>
    /// The mouse status.
    /// </summary>
    public enum MouseStatus
    {
        Up,
        Down,
    }

    /// <summary>
    /// A scroll or mouse wheel status.
    /// </summary>
    public enum ScrollWheelStatus
    {
        Up,
        Down,
        None,
    }

    #pragma warning restore 1591, SA1602
}
