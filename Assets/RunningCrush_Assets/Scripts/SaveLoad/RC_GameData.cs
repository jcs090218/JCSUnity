/**
 * $File: RC_GameData.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;
using System.Xml.Serialization;
using System.IO;

/// <summary>
/// Game Data design here.
/// </summary>
public class RC_GameData
    : JCS_XMLGameData
{

    //---------------------------------------------------
    // Data Struct
    public string Name { get; set; }        // name of the player
    public int Gold { get; set; }           // cash in the game
    public int[] ItemNo { get; set; }       // Array of item unlocked.


}
