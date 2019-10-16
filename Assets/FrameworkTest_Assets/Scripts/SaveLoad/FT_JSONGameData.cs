/**
 * $File: FT_JSONGameData.cs $
 * $Date: 2019-10-16 14:57:50 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2019 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCSUnity;

/// <summary>
/// Test of the JSON game data.
/// </summary>
[System.Serializable]
public class FT_JSONGameData 
    : JCS_JSONGameData
{
    /* Variables */
    public string Name = "";        // name of the player
    public int Cash = 0;            // cash in the game

    /* Setter/Getter */

    /* Functions */

}
