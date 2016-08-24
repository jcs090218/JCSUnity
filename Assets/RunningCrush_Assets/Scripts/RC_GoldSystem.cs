/**
 * $File: RC_GoldSystem.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


public class RC_GoldSystem 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables
    public static RC_GoldSystem instance = null;

    //----------------------
    // Private Variables
    private int mCurrentGold = -1;

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------
    public int GetCurrentGold() { return this.mCurrentGold; }

    //========================================
    //      Unity's function
    //------------------------------
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // get the gold from game data.
        mCurrentGold = RC_GameSettings.RC_GAME_DATA.Gold;
    }

    private void Update()
    {

    }
    
    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions
    public int DeltaGold(int val)
    {
        mCurrentGold += val;
        return mCurrentGold;
    }
    
    //----------------------
    // Protected Functions
    
    //----------------------
    // Private Functions
    
}
