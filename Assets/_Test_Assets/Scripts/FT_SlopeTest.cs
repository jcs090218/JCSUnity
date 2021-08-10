/**
 * $File: FT_SlopeTest.cs $
 * $Date: 2017-05-25 00:41:28 $
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
public class FT_SlopeTest 
    : MonoBehaviour 
{

    /* Variables */

    [Header("** Check Variables (FT_SlopeTest) **")]

    public CharacterController mCharacterController = null;

    public BoxCollider mBoxCollider = null;

    public BoxCollider mTopBoxCollider = null;
    public BoxCollider mBotBoxCollider = null;

    /* Setter & Getter */

    /* Functions */

    private void Update()
    {
        //print(JCSUnity.JCS_Physics.SetOnTopOfBoxWithSlope(mCharacterController, mBoxCollider));
        print(JCSUnity.JCS_Physics.SetOnTopOfBoxWithSlope(mTopBoxCollider, mBotBoxCollider));
    }
}
