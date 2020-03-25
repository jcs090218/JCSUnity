/**
 * $File: BF_EffectPickItem.cs $
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
/// Item when the player will do some effect
/// base on the enum list.
/// </summary>
[RequireComponent(typeof(BF_PickItem))]
public class BF_EffectPickItem 
    : MonoBehaviour 
{
    /* Variables */

    private BF_PickItem mPickItem = null;

    [Header("** Runtime Variables (BF_EffectPickItem) **")]

    [Tooltip("What happed after pick up this item?")]
    [SerializeField]
    private BF_PickItemEffectType mEffectType 
        = BF_PickItemEffectType.NONE;


    [Header("** Liquid Variables (BF_EffectPickItem) **")]

    [Tooltip("Any value with liquid.")]
    [SerializeField] [Range(0, 30000)]
    private int mEffectValue = 0;

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        mPickItem = this.GetComponent<BF_PickItem>();
    }

    private void Start()
    {
        SetPickColliderByObject();
    }

    /// <summary>
    /// Do the design effect.
    /// </summary>
    public void DoEffect()
    {
        DoEffect(mEffectType);
    }
    /// <summary>
    /// Do the effect base on type u select.
    /// </summary>
    /// <param name="effectType"> effect type. </param>
    public void DoEffect(BF_PickItemEffectType effectType)
    {
        BF_GameManager bfGm = BF_GameManager.instance;

        switch (effectType)
        {
            case BF_PickItemEffectType.ADD_HP:
                {
                    bfGm.HEALTH_LIQUIDBAR.DeltaAdd(mEffectValue);
                }
                break;
            case BF_PickItemEffectType.SUB_HP:
                {
                    bfGm.HEALTH_LIQUIDBAR.DeltaSub(mEffectValue);
                }
                break;
            case BF_PickItemEffectType.ADD_MP:
                {
                    bfGm.MANA_LIQUIDBAR.DeltaAdd(mEffectValue);
                }
                break;
            case BF_PickItemEffectType.SUB_MP:
                {
                    bfGm.MANA_LIQUIDBAR.DeltaSub(mEffectValue);
                }
                break;
            case BF_PickItemEffectType.INC_RC_MP:
                {
                    bfGm.MANA_LIQUIDBAR.LiquidBar.RecoverValue += JCS_Mathf.ToPositive(mEffectValue);
                }
                break;
            case BF_PickItemEffectType.DEC_RC_MP:
                {
                    bfGm.MANA_LIQUIDBAR.LiquidBar.RecoverValue += JCS_Mathf.ToNegative(mEffectValue);
                }
                break;
        }
    }


    public void SetPickColliderByObject()
    {
        SetPickColliderByObject(mEffectType);
    }
    /// <summary>
    /// 根據物品的效果不同設置不同的,飛行標誌!
    /// </summary>
    public void SetPickColliderByObject(BF_PickItemEffectType effectType)
    {
        BF_GameManager bfGm = BF_GameManager.instance;

        switch (effectType)
        {
            case BF_PickItemEffectType.SUB_HP:
            case BF_PickItemEffectType.ADD_HP:
                {
                    mPickItem.PickCollider = bfGm.HEALTH_OBJECT;
                }
                break;

            case BF_PickItemEffectType.SUB_MP:
            case BF_PickItemEffectType.ADD_MP:
                {
                    mPickItem.PickCollider = bfGm.MANA_OBJECT;
                }
                break;
        }
    }
}
