/**
 * $File: FT_Slope.cs $
 * $Date: 2017-05-25 00:41:28 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;
using MyBox;

public class FT_Slope : MonoBehaviour 
{

    /* Variables */

    [Separator("Check Variables (FT_SlopeTest)")]

    public CharacterController mCharacterController = null;

    public BoxCollider mBoxCollider = null;

    public BoxCollider mTopBoxCollider = null;
    public BoxCollider mBotBoxCollider = null;

    /* Setter & Getter */

    /* Functions */

    private void Update()
    {
        //print(JCS_Physics.SetOnTopOfBoxWithSlope(mCharacterController, mBoxCollider));
        print(JCS_Physics.SetOnTopOfBoxWithSlope(mTopBoxCollider, mBotBoxCollider));
    }
}
