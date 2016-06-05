/**
 * $File: RC_EffectObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


public class RC_EffectObject 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables
    [SerializeField] private RC_EffectType mEffectType = RC_EffectType.BLOCK;

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
