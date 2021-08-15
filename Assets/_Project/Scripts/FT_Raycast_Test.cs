/**
 * $File: FT_Raycast_Test.cs $
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
public class FT_Raycast_Test
    : MonoBehaviour 
{
    /* Variables */

    private CharacterController mCharacterController = null;

    /* Setter/Getter */

    public CharacterController GetCC() { return this.mCharacterController; }

    /* Functions */

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
}
