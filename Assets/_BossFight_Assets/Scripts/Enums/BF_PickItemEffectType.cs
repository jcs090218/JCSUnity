/**
 * $File: BF_PickItemEffectType.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

/// <summary>
/// Item which can pick have different effect
/// in the game. Design the effect type here.
/// </summary>
public enum BF_PickItemEffectType 
{
    NONE,

    ADD_HP,
    SUB_HP,

    ADD_MP,
    SUB_MP,

    // increase the amount of recovering the value (mana)
    INC_RC_MP,
    DEC_RC_MP
}
