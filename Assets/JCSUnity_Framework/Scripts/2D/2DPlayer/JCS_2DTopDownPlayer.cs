/**
 * $File: JCS_2DTopDownPlayer.cs $
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
    
    [RequireComponent(typeof(JCS_2DTopDownPlayerAudioController))]
    public class JCS_2DTopDownPlayer : JCS_Player
    {

        //----------------------
        // Public Variables
        public int mIndex = 0;

        //----------------------
        // Private Variables
        private JCS_2DTopDownPlayerAudioController mTopDownAudioController = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------


        //========================================
        //      Unity's function
        //------------------------------
        protected override void Awake()
        {
            base.Awake();
            this.mTopDownAudioController = this.GetComponent<JCS_2DTopDownPlayerAudioController>();
        }

        protected override void Update()
        {
            base.Update();

            if (JCS_Input.GetKeyDown(KeyCode.H))
                JCS_SceneManager.instance.LoadScene("JCS_SideScrollerDemo");


            if (mIndex == 0)
            {
                if (JCS_Input.GetKey(KeyCode.UpArrow))
                    GoUp();
                else if (JCS_Input.GetKey(KeyCode.DownArrow))
                    GoDown();

                else if (JCS_Input.GetKey(KeyCode.RightArrow))
                    GoRight();
                else if (JCS_Input.GetKey(KeyCode.LeftArrow))
                    GoLeft();
                else
                    Idle();
            }
            
            if (mIndex == 1)
            {
                if (JCS_Input.GetKey(KeyCode.W))
                    GoUp();
                else if (JCS_Input.GetKey(KeyCode.S))
                    GoDown();

                else if (JCS_Input.GetKey(KeyCode.D))
                    GoRight();
                else if (JCS_Input.GetKey(KeyCode.A))
                    GoLeft();
                else
                    Idle();
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public override void ControlEnable(bool act)
        {
            if (mTopDownAudioController.GetAudioListener() == null)
                return;

            // enable all the component here
            if (act)
            {
                if (JCS_GameSettings.instance.CAMERA_TYPE != JCS_CameraType.MULTI_TARGET)
                    mTopDownAudioController.GetAudioListener().enabled = true;
                mIsControllable = true;
            }
            // diable all the component here
            else
            {
                mTopDownAudioController.GetAudioListener().enabled = false;
                mIsControllable = false;
            }
        }
        public void GoUp()
        {
            if (!mIsControllable)
                return;

            this.mVelocity.y = MoveSpeed;
        }
        public void GoDown()
        {
            if (!mIsControllable)
                return;

            this.mVelocity.y = -MoveSpeed;
        }
        public void GoRight()
        {
            if (!mIsControllable)
                return;

            this.mVelocity.x = MoveSpeed;
        }
        public void GoLeft()
        {
            if (!mIsControllable)
                return;

            this.mVelocity.x = -MoveSpeed;
        }
        public void Idle()
        {
            this.mVelocity = Vector3.zero;
        }

        

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
