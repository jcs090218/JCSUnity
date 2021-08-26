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

    public int Str { get { return this.mStr; } set { this.mStr = value; } }
    public int Dex { get { return this.mDex; } set { this.mDex = value; } }
    public int Int { get { return this.mInt; } set { this.mInt = value; } }
    public int Luc { get { return this.mLuc; } set { this.mLuc = value; } }
    public int MinDamage { get { return this.mMinDamage; } set { this.mMinDamage = value; } }
    public int MaxDamage { get { return this.mMaxDamage; } set { this.mMaxDamage = value; } }
    public int CriticalChance { get { return this.mCriticalChance; } set { this.mCriticalChance = value; } }
    public int AttackValue { get { return this.mAttackValue; } set { this.mAttackValue = value; } }
    public int DefenseValue { get { return this.mDefenseValue; } set { this.mDefenseValue = value; } }

    /* Functions */

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
