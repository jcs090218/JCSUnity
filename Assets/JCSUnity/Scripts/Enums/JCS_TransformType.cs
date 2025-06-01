/**
 * $File: JCS_TransformType.cs $
 * $Date: 2017-09-10 07:06:47 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// List of transform properties.
    /// </summary>
    public enum JCS_TransformType
    {
        /* Transform */
        POSITION,
        ROTATION,
        SCALE,

        /* RectTransform */
        ANCHOR_MIN,
        ANCHOR_MAX,
        SIZE_DELTA,
        PIVOT,
        ANCHOR_POSITION,
        ANCHOR_POSITION_3D,
        OFFSET_MIN,
        OFFSET_MAX,
    }
}
