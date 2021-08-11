/**
 * $File: BF_BuyButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;
using UnityEngine.UI;

/// <summary>
/// Button that buys the item.
/// </summary>
public class BF_BuyButton 
    :  JCS_Button
{
    /* Variables */

    [Header("** Initialize Variables (BF_BuyButton) **")]

    [Tooltip("")]
    [SerializeField] [Range(0, 99999999)]
    private int mBuyValue = 1;

    [Tooltip("")]
    [SerializeField]
    private Text mCashText = null;

    [Tooltip("")]
    [SerializeField]
    private string mBaseString = "$ ";

    /* Setter & Getter */

    /* Functions */

    private void Start()
    {
        if (mCashText != null)
            mCashText.text = mBaseString + mBuyValue.ToString();
    }

    private void Update()
    {
        if (BF_GameSettings.GAME_DATA.Cash < mBuyValue)
            this.Interactable = false;
    }

    public override void JCS_OnClickCallback()
    {
        BF_GameSettings.GAME_DATA.Cash -= mBuyValue;
    }
}
