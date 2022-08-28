/**
 * $File: FT_CheckVisible.cs $
 * $Date: 2017-05-14 21:17:19 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

/// <summary>
/// Check if a Unity Object visible in the render field.
/// </summary>
public class FT_CheckVisible : JCS_UnityObject 
{
    /* Variables */

    /* Setter/Getter */

    /* Functions */

    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        // 試著關閉場景的camera.
        Camera.current.enabled = false;
#endif
    }

    private void Update()
    {
        print(this.LocalIsVisible);
    }
}
