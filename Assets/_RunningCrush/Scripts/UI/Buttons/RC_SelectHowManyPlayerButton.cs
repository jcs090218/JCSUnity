/**
 * $File: RC_SelectHowManyPlayerButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class RC_SelectHowManyPlayerButton : JCS_Button
{
    /* Variables */

    [Tooltip("After hitting this button decide how many player in this game.")]
    [SerializeField] 
    private int mPlayersInGame = 1;

    /* Setter & Getter */

    /* Functions */

    public override void JCS_OnClickCallback()
    {
        RC_GameSettings.instance.PLAYER_IN_GAME = mPlayersInGame;
    }
}
