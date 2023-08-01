/**
 * $File: RC_UIManager.cs $
 * $Date: 2017-05-26 20:40:28 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;
using MyBox;

/// <summary>
/// Hold all the UI that will interact with player.
/// </summary>
public class RC_UIManager : JCS_Manager<RC_UIManager>
{
    /* Variables */

    [Separator("Runtime Variables (RC_UIManager)")]

    [Tooltip("Panel when one player reach the goal.")]
    public JCS_TweenPanel EXIT_PANEL = null;

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        instance = this;
    }
}
