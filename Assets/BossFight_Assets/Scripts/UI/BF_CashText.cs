/**
 * $File: BF_CashText.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class BF_CashText 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables

    [Tooltip("")]
    [SerializeField]
    private Text mCashText = null;


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
        if (mCashText == null)
            mCashText = this.GetComponent<Text>();
    }

    private void Update()
    {
        // lazy code, update the text every frame.
        UpdateText();
    }
    
    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions

    public void UpdateText()
    {
        UpdateText(BF_GameSettings.BF_GAME_DATA.Cash);
    }
    public void UpdateText(int val)
    {
        mCashText.text = val.ToString();
    }
    
    //----------------------
    // Protected Functions
    
    //----------------------
    // Private Functions
    
}
