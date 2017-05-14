/**
 * $File: RC_GoldObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;

/// <summary>
/// If player collect the gold will earn gold
/// </summary>
public class RC_GoldObject 
    : JCS_CashObject
{

    //----------------------
    // Public Variables

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
    protected override void Awake()
    {
        base.Awake();

        // set to auto pick auto matically.
        mAutoPickColliderTouched = true;

        SetPickCallback(PickCallback);
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
    private void PickCallback(Collider other)
    {

        // apply value to gold system.
        RC_Player p = other.GetComponent<RC_Player>();
        if (p == null)
        {
            JCS_Debug.LogError(
                "RC_GoldObjec",
                "U are using RC game object but the player isn't RC gameobject...");

            return;
        }

        p.CurrentGold += mCashValue;

        // if is single player mode, 
        // then we can just save to data directly.
        if (RC_GameSettings.instance.GAME_MODE == RC_GameMode.SINGLE_PLAYERS)
            RC_GameSettings.RC_GAME_DATA.Gold += mCashValue;
    }

}
