/**
 * $File: CastToMePosition.cs $
 * $Date: 2016-12-01 03:33:58 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;


/// <summary>
/// 
/// </summary>
public class CastToMePosition 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables

    public JCS_PositionCastAction castAction = null;

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------

    //========================================
    //      Unity's function
    //------------------------------
    private void Awake()
    {

    }

    private void Update()
    {
        if (JCS_Input.GetKeyDown(KeyCode.G))
            castAction.CastToScreen(this.transform.position);
        if (JCS_Input.GetKeyDown(KeyCode.T))
            castAction.CastToScreen(this.transform.localPosition);
    }
    
    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions
    
    //----------------------
    // Protected Functions
    
    //----------------------
    // Private Functions
    
}
