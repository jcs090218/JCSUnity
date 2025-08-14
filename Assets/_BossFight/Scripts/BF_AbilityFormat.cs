/**
 * $File: BF_AbilityFormat.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class BF_AbilityFormat : JCS_AbilityFormat 
{
    /* Variables */

    [SerializeField] private int mStr = 1;
    [SerializeField] private int mDex = 1;
    [SerializeField] private int mInt = 1;
    [SerializeField] private int mLuc = 1;
    [SerializeField] private int mMinDamage = 87536;
    [SerializeField] private int mMaxDamage = 125432;
    [SerializeField] private int mCriticalChance = 60;
    [SerializeField] private int mAttackValue = 0;
    [SerializeField] private int mDefenseValue = 0;

    /* Setter & Getter */

    public int str { get { return mStr; } set { mStr = value; } }
    public int dex { get { return mDex; } set { mDex = value; } }
    public int inte { get { return mInt; } set { mInt = value; } }
    public int luc { get { return mLuc; } set { mLuc = value; } }
    public int minDamage { get { return mMinDamage; } set { mMinDamage = value; } }
    public int maxDamage { get { return mMaxDamage; } set { mMaxDamage = value; } }
    public int criticalChance { get { return mCriticalChance; } set { mCriticalChance = value; } }
    public int attackValue { get { return mAttackValue; } set { mAttackValue = value; } }
    public int defenseValue { get { return mDefenseValue; } set { mDefenseValue = value; } }

    /* Functions */

    public override int GetAbsoluteDamage()
    {
        // Not implement yet.
        return 0;
    }

    public override int GetMaxDamage()
    {
        return mMaxDamage;
    }

    public override int GetMinDamage()
    {
        return mMinDamage;
    }

    public override int GetCriticalChance()
    {
        return mCriticalChance;
    }

    public override int GetAttackValue()
    {
        // Do calculation about attack value
        return mAttackValue;
    }

    public override int GetDefenseValue()
    {
        // Do calculation about defense value
        return mDefenseValue;
    }
}
