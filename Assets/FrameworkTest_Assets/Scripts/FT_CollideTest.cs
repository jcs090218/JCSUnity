/**
 * $File: FT_CollideTest.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

public class FT_CollideTest : MonoBehaviour 
{

    //----------------------
    // Public Variables
    public BoxCollider mBoxCollider = null;
    public CharacterController mCharacterController = null;


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
        
    }
    
    private void Update() 
    {
        print(JCSUnity.JCS_Physics.JcsOnTriggerCheck(mCharacterController, mBoxCollider));
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
