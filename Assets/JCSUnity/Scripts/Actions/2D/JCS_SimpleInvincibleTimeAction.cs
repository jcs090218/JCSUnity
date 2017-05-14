/**
 * $File: JCS_SimpleInvincibleTimeAction.cs $
 * $Date: 2017-03-16 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{

    /// <summary>
    /// Simple call invincible time action.
    /// </summary>
    public class JCS_SimpleInvincibleTimeAction
        : JCS_UnityObject
    {
        //----------------------
        // Public Variables

        //----------------------
        // Private Variables


        [Header("** Runtime Variables (JCS_SimpleInvincibleTimeAction) **")]

        [Tooltip("")]
        [SerializeField]
        private float mInvicibleTime = 1.0f;

        // timer.
        private float mInvicibleTimer = 0;

        // trigger the invincible time action?
        private bool mTriggerAction = false;

        [Header("- Flash Effect (JCS_SimpleInvincibleTimeAction) ")]

        [Tooltip("Color when is invincible.")]
        [SerializeField]
        private Color mInvincibleColor = new Color(141.0f, 141.0f, 141.0f);

        // record down the previoud color
        private Color mRecordColor = Color.white;

        [Tooltip("How fast it flash back and forth?")]
        [SerializeField]
        [Range(0.01f, 3.0f)]
        private float mFlashTime = 0.1f;

        // timer to do flashy effect.
        private float mFlashTimer = 0;

        // just a boolean record down what the 
        // current color is.
        private bool mFlashToggle = false;

        [Tooltip("Play once while trigger")]
        [SerializeField]
        private AudioClip mTriggerSound = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public float InvicibleTime { get { return this.mInvicibleTime; } set { this.mInvicibleTime = value; } }
        // Use to check if this effect is active?
        public bool IsInvincible { get { return this.mTriggerAction; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            // get unity data.
            UpdateUnityData();
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            // check if effect?
            if (!mTriggerAction)
                return;


            mInvicibleTimer += Time.deltaTime;

            // do flash algorithm.
            DoFlash();

            // check if the time reach?
            if (mInvicibleTime > mInvicibleTimer)
                return;

            // set back the local color to record color.
            LocalColor = mRecordColor;

            // reset timer.
            mInvicibleTimer = 0;

            this.mFlashToggle = false;

            // cancel action
            mTriggerAction = false;
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (JCS_Input.GetKeyDown(KeyCode.L))
            {
                TriggerAction();
            }

        }
#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Trigger invincible action to true, start 
        /// the timer.
        /// </summary>
        public void TriggerAction()
        {
            TriggerAction(mInvicibleTime);
        }

        /// <summary>
        /// Trigger invincible action to true, start 
        /// the timer.
        /// </summary>
        /// <param name="time"> time to be invincible </param>
        public void TriggerAction(float time)
        {
            // if effect is on, return.
            if (mTriggerAction)
                return;

            // get the time.
            this.mInvicibleTime = time;

            // reset timer.
            this.mInvicibleTimer = 0;

            // record down the current color.
            this.mRecordColor = LocalColor;

            // reset the flash toggle.
            this.mFlashToggle = false;

            // start the action.
            mTriggerAction = true;

            // play the sound once.
            JCS_SoundManager.instance.GetGlobalSoundPlayer().PlayOneShot(mTriggerSound);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Do flash from color 1 to color 2 and color 1... continue.
        /// </summary>
        private void DoFlash()
        {
            mFlashTimer += Time.deltaTime;

            if (mFlashTime > mFlashTimer)
                return;

            // set the start is "false"
            if (!mFlashToggle)
                LocalColor = this.mInvincibleColor;
            else
                LocalColor = this.mRecordColor;

            // toggle it.
            mFlashToggle = !mFlashToggle;

            // reset flash timer.
            mFlashTimer = 0;
        }
    }
}
