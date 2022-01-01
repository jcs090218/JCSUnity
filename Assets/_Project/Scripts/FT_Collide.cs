/**
 * $File: FT_Collide.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

/// <summary>
/// Test if the two collider collide.
/// </summary>
public class FT_Collide : MonoBehaviour 
{
    /* Variables */

    public BoxCollider mBoxCollider = null;
    public CharacterController mCharacterController = null;

    /* Setter/Getter */

    /* Functions */

    private void Awake() 
    {
        
    }
    
    private void Update() 
    {
        print(JCSUnity.JCS_Physics.JcsOnTriggerCheck(mCharacterController, mBoxCollider));
    }
}
