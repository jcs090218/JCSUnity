/**
 * $File: RC_SelectHowManyPlayerButton.cs $
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

public class RC_SelectHowManyPlayerButton
    : JCS_Button
{
    [Tooltip("After hitting this button decide how many player in this game.")]
    [SerializeField] private int mPlayersInGame = 1; 

    public override void JCS_ButtonClick()
    {
        RC_GameSettings.instance.PLAYER_IN_GAME = mPlayersInGame;
    }
}
