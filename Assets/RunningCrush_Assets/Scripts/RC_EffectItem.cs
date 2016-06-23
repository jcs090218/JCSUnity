/**
 * $File: RC_EffectItem.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;


[RequireComponent(typeof(RC_EffectObject))]
public class RC_EffectItem 
    : JCS_Item
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables
    private RC_EffectObject mEffectObject = null;

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
        // set to auto pick auto matically.
        mAutoPick = true;

        mEffectObject = this.GetComponent<RC_EffectObject>();

        // disable auto detect
        mEffectObject.AutoEffect = false;
    }

    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions
    public override void SubclassCallBack(Collider other)
    {
        base.SubclassCallBack(other);

        // apply effect to player
        RC_Player p = other.GetComponent<RC_Player>();
        if (p == null)
        {
            JCS_GameErrors.JcsErrors(
                "RC_GoldObjec",
                -1,
                "U are using RC game object but the player isn't RC gameobject...");

            return;
        }

        mEffectObject.DoEffect(p);
    }

    //----------------------
    // Protected Functions

    //----------------------
    // Private Functions

}
