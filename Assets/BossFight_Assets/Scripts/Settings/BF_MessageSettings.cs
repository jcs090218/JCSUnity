/**
 * $File: BF_MessageSettings.cs $
 * $Date: $
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
public class BF_MessageSettings 
    :  JCS_Settings<BF_MessageSettings>
{

    //----------------------
    // Public Variables

    public string EXP_BASE = "EXP: ";

    //----------------------
    // Private Variables

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
        instance = CheckSingleton(instance, this);

        // do something?
    }

    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions
    protected override void TransferData(BF_MessageSettings _old, BF_MessageSettings _new)
    {
        _new.EXP_BASE = _old.EXP_BASE;
    }

    //----------------------
    // Protected Functions

    //----------------------
    // Private Functions

}
