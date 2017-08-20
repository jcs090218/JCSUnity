/**
 * $File: RC_UIManager.cs $
 * $Date: 2017-05-26 20:40:28 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCSUnity;


/// <summary>
/// Hold all the UI that will interact with player.
/// </summary>
public class RC_UIManager 
    : MonoBehaviour 
{

    /*******************************************/
    /*            Public Variables             */
    /*******************************************/

    public static RC_UIManager instance = null;


    [Header("** Runtime Variables (RC_UIManager) **")]

    [Tooltip("Panel when one player reach the goal.")]
    public JCS_TweenPanel EXIT_PANEL = null;

    /*******************************************/
    /*           Private Variables             */
    /*******************************************/

    /*******************************************/
    /*           Protected Variables           */
    /*******************************************/

    /*******************************************/
    /*             setter / getter             */
    /*******************************************/

    /*******************************************/
    /*            Unity's function             */
    /*******************************************/
    private void Awake()
    {
        instance = this;
    }
    
    /*******************************************/
    /*              Self-Define                */
    /*******************************************/
    //----------------------
    // Public Functions
    
    //----------------------
    // Protected Functions
    
    //----------------------
    // Private Functions
    
}
