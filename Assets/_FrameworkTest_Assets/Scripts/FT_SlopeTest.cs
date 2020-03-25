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

    /*******************************************/
    /*            Public Variables             */
    /*******************************************/

    /*******************************************/
    /*           Private Variables             */
    /*******************************************/

    [Header("** Check Variables **")]

    public CharacterController mCharacterController = null;

    public BoxCollider mBoxCollider = null;


    public BoxCollider mTopBoxCollider = null;
    public BoxCollider mBotBoxCollider = null;

    /*******************************************/
    /*           Protected Variables           */
    /*******************************************/

    /*******************************************/
    /*             setter / getter             */
    /*******************************************/

    /*******************************************/
    /*            Unity's function             */
    /*******************************************/

    private void Update()
    {
        //print(JCSUnity.JCS_Physics.SetOnTopOfBoxWithSlope(mCharacterController, mBoxCollider));
        print(JCSUnity.JCS_Physics.SetOnTopOfBoxWithSlope(mTopBoxCollider, mBotBoxCollider));
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
