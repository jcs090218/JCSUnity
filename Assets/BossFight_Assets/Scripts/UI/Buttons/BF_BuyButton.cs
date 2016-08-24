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
/// 
/// </summary>
public class BF_BuyButton 
    :  JCS_Button
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables

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

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------

    //========================================
    //      Unity's function
    //------------------------------

    private void Start()
    {
        if (mCashText != null)
            mCashText.text = mBaseString + mBuyValue.ToString();
    }

    private void Update()
    {
        if (BF_GameSettings.BF_GAME_DATA.Cash < mBuyValue)
            this.Interactable = false;
    }

    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions

    public override void JCS_ButtonClick()
    {
        base.JCS_ButtonClick();

        BF_GameSettings.BF_GAME_DATA.Cash -= mBuyValue;
    }

    //----------------------
    // Protected Functions

    //----------------------
    // Private Functions

}
