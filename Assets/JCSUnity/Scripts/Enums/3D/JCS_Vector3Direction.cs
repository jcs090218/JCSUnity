/**
 * $File: JCS_Vector3Direction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// Vector 3d type in enum use in Unity Engine.
    /// </summary>
    public enum JCS_Vector3Direction
    {
        // (0, 0, 0)
        CENTER,

        UP,     // (0, 1, 0)
        DOWN,   // (0, -1, 0)

        FORWARD,  // (0, 0, 1)
        BACK,     // (0, 0, -1)

        RIGHT,  // (1, 0, 0)
        LEFT,   // (-1, 0, 0)

        FORWARD_LEFT,   // (-1, 0, 1)
        FORWARD_RIGHT,  // (1, 0, 1)

        BACK_LEFT,  // (-1, 0, -1)
        BACK_RIGHT, // (1, 0, -1)

        UP_LEFT,    // (-1, 1, 0)
        UP_RIGHT,   // (1, 1, 0)

        FORWARD_UP,     // (0, 1, 1)
        FORWARD_DOWN,   // (0, -1, 1)

        BACK_UP,    // (0, 1, -1)
        BACK_DOWN,  // (0, -1, -1)

        DOWN_LEFT,  // (-1, -1, 0)
        DOWN_RIGHT, // (1, -1, 0)

        FORWARD_UP_LEFT,    // (-1, 1, 1)
        FORWARD_UP_RIGHT,   // (1, 1, 1)

        FORWARD_DOWN_LEFT,  // (-1, -1, 1)
        FORWARD_DOWN_RIGHT, // (1, -1, 1)

        BACK_UP_LEFT,   // (-1, 1, -1)
        BACK_UP_RIGHT,  // (1, 1, -1)

        BACK_DOWN_LEFT,     // (-1, -1, -1)
        BACK_DOWN_RIGHT,    // (1, -1, -1)

    }
}
