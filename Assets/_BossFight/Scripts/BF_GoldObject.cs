﻿/**
 * $File: BF_GoldObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

/// <summary>
/// Gold object.
/// </summary>
public class BF_GoldObject : JCS_CashObject
{

    /* Variables */

    /* Setter & Getter */

    /* Functions */

    protected override void Awake()
    {
        base.Awake();

        // the picking object does not have to be player
        mMustBeActivePlayer = false;

        BF_PickItem.SetPickMode(this);

        // set the picking collider
        PickCollider = BF_GameManager.instance.COLLECT_GOLD_OBJECT;

        pickedCallback = AfterPicked;
    }

    /// <summary>
    /// Call back when item were picked.
    /// </summary>
    /// <param name="other"> collider detection if needed. </param>
    public void AfterPicked(Collider other)
    {
        BF_AppSettings.instance.APP_DATA.Cash += mCashValue;
    }
}
