/**
 * $File: JCS_3DPlayer.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;

namespace JCSUnity
{
    public class JCS_3DPlayer 
        : JCS_Player
    {


        //----------------------
        // Public Variables

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

        private void Update()
        {
            PlayerInput();

            if (!mCharacterController.isGrounded)
                mVelocity.y -= (JCS_GameConstant.GRAVITY * Time.deltaTime * mPlayerGravity);

            mCharacterController.Move(mVelocity * Time.deltaTime);
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public override void ControlEnable(bool act)
        {
            // enable all the component here
            if (act)
            {
                mIsControllable = true;
            }
            // diable all the component here
            else
            {
                mIsControllable = false;
            }
        }

        public void PlayerInput()
        {
            if (JCS_Input.GetKey(KeyCode.RightArrow))
            {
                mVelocity.x = mMoveSpeed;
            }
            else if (JCS_Input.GetKey(KeyCode.LeftArrow))
            {
                mVelocity.x = -mMoveSpeed;
            }
            else
            {
                mVelocity.x = 0;
            }

            if (JCS_Input.GetKey(KeyCode.UpArrow))
            {
                mVelocity.z = mMoveSpeed;
            }
            else if (JCS_Input.GetKey(KeyCode.DownArrow))
            {
                mVelocity.z = -mMoveSpeed;
            }
            else
            {
                mVelocity.z = 0;
            }
        }

        

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void RotateRelativeToCamera()
        {

        }

    }
}
