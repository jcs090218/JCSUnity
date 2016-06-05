/**
 * $File: RC_GameManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;


public class RC_GameManager 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables
    public static RC_GameManager instance = null;

    //----------------------
    // Private Variables

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------

    //========================================
    //      Unity's function
    //------------------------------
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {

    }
    
    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions
    
    //----------------------
    // Protected Functions
    
    //----------------------
    // Private Functions
    
}
