/**
 * $File: BF_AbilityFormat.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;
using System;


public class BF_AbilityFormat 
    : JCS_AbilityFormat 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables
    [SerializeField] private int mStr = 1;
    [SerializeField] private int mDex = 1;
    [SerializeField] private int mInt = 1;
    [SerializeField] private int mLuc = 1;
    [SerializeField] private int mMinDamage = 87536;
    [SerializeField] private int mMaxDamage = 125432;
    [SerializeField] private int mCriticalChance = 60;


    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------

    //========================================
    //      Unity's function
    //------------------------------


    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions
    public override int GetAbsoluteDamage()
    {
        // Not implement yet.
        return 0;
    }

    public override int GetMaxDamage()
    {
        return this.mMaxDamage;
    }

    public override int GetMinDamage()
    {
        return this.mMinDamage;
    }

    public override int GetCriticalChance()
    {
        return mCriticalChance;
    }
    //----------------------
    // Protected Functions

    //----------------------
    // Private Functions

}
