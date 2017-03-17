/**
 * $File: JCS_InvincibleTimeAction.cs $
 * $Date: 2016-12-22 05:22:24 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// 無敵時間的counter. 
    /// ex: Super Mario ate the star.
    /// </summary>
    [RequireComponent(typeof(JCS_LiveObject))]
    public class JCS_InvincibleTimeAction
        : JCS_UnityObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private JCS_LiveObject mLiveObject = null;

        [Header("** Runtime Variables (JCS_InvincibleTimeAction) **")]

        [Tooltip("")]
        [SerializeField]
        private float mInvicibleTime = 1.0f;

        // timer.
        private float mInvicibleTimer = 0;

        // trigger the invincible time action?
        private bool mTriggerAction = false;

        [Header("- Flash Effect (JCS_InvincibleTimeAction) ")]

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

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mLiveObject = this.GetComponent<JCS_LiveObject>();

            // get unity data.
            UpdateUnityData();
        }

        private void Update()
        {
            // check if effect?
            if (!mTriggerAction)
                return;


            mInvicibleTimer += Time.deltaTime;

            // do flash algorithm.
            DoFlash();

            // check if the time reach?
            if (mInvicibleTime > mInvicibleTimer)
                return;

            mLiveObject.CanDamage = true;

            // set back the local color to record color.
            LocalColor = mRecordColor;

            // reset timer.
            mInvicibleTimer = 0;

            this.mFlashToggle = false;

            // cancel action
            mTriggerAction = false;
        }

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

            // set the live object's can damage immdeiate to false.
            // so it won't get damage in the same frame.
            mLiveObject.CanDamage = false;

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
