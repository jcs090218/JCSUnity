﻿/**
 * $File: RC_AppData.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using JCSUnity;

/// <summary>
/// Game Data design here.
/// </summary>
[Serializable]
public class RC_AppData : JCS_XMLData
{
    /* Variables */

    public string Name { get; set; }        // name of the player
    public int Gold { get; set; }           // cash in the game
    public int[] ItemNo { get; set; }       // Array of item unlocked.

    /* Setter & Getter */

    /* Functions */
}
