/**
 * $File: JCS_3DPlayer.cs $
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

    /// <summary>
    /// 
    /// </summary>
    public class JCS_3DPlayer 
        : JCS_Player
    {


        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField] private float mRotateSpeed = 25f;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------


        //========================================
        //      Unity's function
        //------------------------------

        protected override void FixedUpdate()
        {
            PlayerInput();

            if (!mCharacterController.isGrounded)
                mVelocity.y -= (JCS_GameConstant.GRAVITY * Time.deltaTime * JCS_GameSettings.instance.GRAVITY_PRODUCT);

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

            if (JCS_Input.GetKey(KeyCode.L))
                this.transform.Rotate(Vector3.up * mRotateSpeed * Time.deltaTime);
            else if (JCS_Input.GetKey(KeyCode.J))
                this.transform.Rotate(Vector3.up * -mRotateSpeed * Time.deltaTime);
        }

        public override void Stand()
        {
            throw new NotImplementedException();
        }
        public override void Attack()
        {
            throw new NotImplementedException();
        }
        public override void Jump()
        {
            throw new NotImplementedException();
        }
        public override void Prone()
        {
            throw new NotImplementedException();
        }
        public override void Alert()
        {
            throw new NotImplementedException();
        }
        public override void Ladder()
        {
            throw new NotImplementedException();
        }
        public override void Rope()
        {
            throw new NotImplementedException();
        }
        public override void Die()
        {
            throw new NotImplementedException();
        }
        public override void Dance()
        {
            throw new NotImplementedException();
        }
        public override void Swim()
        {
            throw new NotImplementedException();
        }
        public override void Fly()
        {
            throw new NotImplementedException();
        }
        public override void Sit()
        {
            throw new NotImplementedException();
        }
        public override void Hit()
        {
            throw new NotImplementedException();
        }
        public override void Ghost()
        {
            throw new NotImplementedException();
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
