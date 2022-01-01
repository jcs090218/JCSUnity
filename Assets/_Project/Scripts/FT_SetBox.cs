/**
 * $File: FT_SetBox.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class FT_SetBox : MonoBehaviour
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
