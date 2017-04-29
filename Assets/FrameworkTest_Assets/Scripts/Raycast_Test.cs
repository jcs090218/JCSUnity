/**
 * $File: Raycast_Test.cs $
 * $Date: 2017-04-27 21:54:48 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
public class Raycast_Test 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables
    private CharacterController mCharacterController = null;

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------
    public CharacterController GetCC() { return this.mCharacterController; }

    //========================================
    //      Unity's function
    //------------------------------
    private void Awake()
    {
        mCharacterController = this.GetComponent<CharacterController>();
    }

    private void Update()
    {
        JCSUnity.JCS_Physics.SetOnTopOfClosestBoxWithRay(
            mCharacterController, 
            100, 
            JCSUnity.JCS_Vector3Direction.DOWN);
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
