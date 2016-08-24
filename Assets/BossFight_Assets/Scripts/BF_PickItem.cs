/**
 * $File: BF_PickItem.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;

/// <summary>
/// Item in game that can be picked.
/// </summary>
public class BF_PickItem 
    : JCS_Item 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables

    [Header("** Optional Variables (BF_PickItem) **")]

    [Tooltip("")]
    [SerializeField]
    private BF_EffectPickItem mEffectPickItem = null;

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
        // the picking object does not have to be player
        mMustBeActivePlayer = false;

        SetPickMode(this);

        // set the picking collider
        PickCollider = BF_GameManager.instance.COLLECT_GOLD_OBJECT;

        SetPickCallback(PickCallback);

        // try to get the optional variables.
        if (mEffectPickItem == null)
            mEffectPickItem = this.GetComponent<BF_EffectPickItem>();
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
        if (mEffectPickItem != null)
            mEffectPickItem.DoEffect();
    }

    /// <summary>
    /// Design pick method base on the platform game 
    /// we are building.
    /// </summary>
    /// <param name="jcsItem"> item to do the effect. </param>
    public static void SetPickMode(JCS_Item jcsItem)
    {
        JCS_ApplicationManager am = JCS_ApplicationManager.instance;

        if (am.isPC())
        {
            // pc version will have to pick by mouse
            jcsItem.PickByMouseDown = true;
        }

        if (am.isMobile())
        {
            // set to true so when the item hit the ground, 
            // pick event will be activate.
            jcsItem.AutoPickWhileCan = true;
        }

    }

    //----------------------
    // Protected Functions

    //----------------------
    // Private Functions



}
