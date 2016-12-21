/**
 * $File: RC_StartGameButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;
using UnityEngine.UI;


/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class RC_StartGameButton 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables
    private RectTransform mRectTransform = null;
    private Image mImage = null;

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
        mRectTransform = this.GetComponent<RectTransform>();
        mImage = this.GetComponent<Image>();

        mImage.enabled = false;
        JCS_Utility.SetActiveToAllChildren(this.transform, false);
    }

    private void Update()
    {
        if (RC_GameSettings.instance.READY_TO_START_GAME)
        {
            mImage.enabled = true;
            JCS_Utility.SetActiveToAllChildren(this.transform, true);
        }
    }
    
    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions
    
    //----------------------
    // Protected Functions
    
    //----------------------
    // Private Functions
    
}
