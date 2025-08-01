﻿/**
 * $File: RC_Goal.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

public class RC_Goal : MonoBehaviour 
{
    /* Variables */

    /* Setter & Getter */

    /* Functions */

    private void OnTriggerEnter(Collider other)
    {
        var p = other.GetComponent<RC_Player>();
        if (p != null)
        {
            // reach the goal! game is over.
            RC_GameSettings.FirstInstance().GAME_OVER = true;

            RC_GameManager.FirstInstance().DoExitGame();
        }
    }
}
