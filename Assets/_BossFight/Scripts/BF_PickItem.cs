/**
 * $File: BF_PickItem.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;
using MyBox;

/// <summary>
/// Item in game that can be picked.
/// </summary>
public class BF_PickItem : JCS_Item
{
    /* Variables */

    [Separator("Optional Variables (BF_PickItem)")]

    [Tooltip("")]
    [SerializeField]
    private BF_EffectPickItem mEffectPickItem = null;

    /* Setter & Getter */

    /* Functions */

    protected override void Awake()
    {
        base.Awake();

        // the picking object does not have to be player
        mMustBeActivePlayer = false;

        SetPickMode(this);

        // set the picking collider
        PickCollider = BF_GameManager.instance.COLLECT_GOLD_OBJECT;

        pickedCallback = AfterPicked;

        // try to get the optional variables.
        if (mEffectPickItem == null)
            mEffectPickItem = this.GetComponent<BF_EffectPickItem>();
    }

    /// <summary>
    /// Call back when item were picked.
    /// </summary>
    /// <param name="other"> collider detection if needed. </param>
    public void AfterPicked(Collider other)
    { 
        if (mEffectPickItem != null)
            mEffectPickItem.DoEffect();
    }

    /// <summary>
    /// Design pick method base on the platform game 
    /// we are building.
    /// </summary>
    /// <param name="item"> item to do the effect. </param>
    public static void SetPickMode(JCS_Item item)
    {
        var am = JCS_AppManager.instance;

        if (am.IsPC())
        {
            // pc version will have to pick by mouse
            item.PickByMouseDown = true;
        }

        if (am.IsMobile())
        {
            // set to true so when the item hit the ground, 
            // pick event will be activate.
            item.AutoPickWhileCan = true;
        }
    }
}
