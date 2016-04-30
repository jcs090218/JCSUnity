/**
 * $File: JCS_DialogueType.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// type of dialogue
    /// </summary>
    public enum JCS_DialogueType
    {
        // Special Priority
        GAME_UI,         // default
        UI_EFFECT,

        //*** Low Priority ***//

        GAME_DIALOGUE,      // any screen in game
        FORCE_DIALOGUE      // screen from application layer so it have the authroized to break the game


        //*** Hight Priority ***//
    }
}
