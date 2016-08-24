/**
 * $File: BF_GoldObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;


public class BF_GoldObject 
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

        // the picking object does not have to be player
        mMustBeActivePlayer = false;

        BF_PickItem.SetPickMode(this);

        // set the picking collider
        PickCollider = BF_GameManager.instance.COLLECT_GOLD_OBJECT;

        SetPickCallback(PickCallback);
    }

    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions

    /// <summary>
    /// Call back when item were picked.
    /// </summary>
    /// <param name="other"> collider detection if needed. </param>
    public void PickCallback(Collider other)
    {
        BF_GameSettings.BF_GAME_DATA.Cash += mCashValue;
    }

    //----------------------
    // Protected Functions

    //----------------------
    // Private Functions

}
