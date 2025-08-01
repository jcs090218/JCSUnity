﻿/**
 * $File: RC_GoldObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

/// <summary>
/// If player collect the gold will earn gold
/// </summary>
public class RC_GoldObject : JCS_CashObject
{
    /* Variables */

    /* Setter & Getter */

    /* Functions */

    protected override void Awake()
    {
        base.Awake();

        // set to auto pick auto matically.
        mAutoPickColliderTouched = true;

        onPicked = AfterPicked;
    }

    private void AfterPicked(Collider other)
    {
        // apply value to gold system.
        var p = other.GetComponent<RC_Player>();
        if (p == null)
        {
            Debug.LogError("You are using RC game object but the player isn't RC game object...");
            return;
        }

        p.CurrentGold += mCashValue;

        // if is single player mode, 
        // then we can just save to data directly.
        if (RC_GameSettings.FirstInstance().GAME_MODE == RC_GameMode.SINGLE_PLAYERS)
            RC_AppSettings.FirstInstance().APP_DATA.Gold += mCashValue;
    }
}
