﻿/**
 * $File: RC_EffectItem.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

[RequireComponent(typeof(RC_EffectObject))]
public class RC_EffectItem : JCS_Item
{
    /* Variables */

    private RC_EffectObject mEffectObject = null;

    /* Setter & Getter */

    /* Functions */

    protected override void Awake()
    {
        base.Awake();

        // set to auto pick auto matically.
        mAutoPickColliderTouched = true;

        mEffectObject = GetComponent<RC_EffectObject>();

        // disable auto detect
        mEffectObject.autoEffect = false;

        onPicked = AfterPicked;
    }

    private void AfterPicked(Collider other)
    {
        // apply effect to player
        var p = other.GetComponent<RC_Player>();

        if (p == null)
        {
            Debug.LogError("You are using RC game object but the player isn't RC game object...");
            return;
        }

        mEffectObject.DoEffect(p);
    }
}
