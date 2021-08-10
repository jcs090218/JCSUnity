/**
 * $File: FT_SetBox_Test.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;


public class FT_SetBox_Test
    : MonoBehaviour 
{
    /* Variables */

    public BoxCollider mBoxCollider = null;
    public CharacterController mCharacterController = null;

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {

    }

    private void Update()
    {
        JCS_Physics.SetOnTopOfBox(mCharacterController, mBoxCollider);
        //print(JCS_Physics.TopOfBox(mCharacterController, mBoxCollider));
    }
}
