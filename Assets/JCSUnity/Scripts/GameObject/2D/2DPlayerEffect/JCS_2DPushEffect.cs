/**
 * $File: JCS_2DPushEffect.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;


namespace JCSUnity
{
    public class JCS_2DPushEffect
    : MonoBehaviour
    , JCS_2DPlayerEffect
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField] private JCS_2DFaceType mDirection = JCS_2DFaceType.FACE_LEFT;
        [SerializeField] private float mPushSpeed = 10.0f;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void OnTriggerStay(Collider other)
        {
            JCS_Player player = other.GetComponent<JCS_Player>();
            if (player != null)
            {
                if (player.GetCharacterController().isGrounded)
                    player.VelX += mPushSpeed * -(int)mDirection;
            }

        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void Effect()
        {
            // override   
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
