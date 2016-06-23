/**
 * $File: JCS_SlideScreenButton.cs $
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

    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_SlideScreenButton
        : JCS_Button
    {
        private JCS_SoundPlayer mSoundPlayer = null;

        [Header("** Runtime Variables (JCS_SlideScreenButton) **")]
        [Tooltip("This action are using \"JCS_2DSlideScreenCamera\".")]
        [SerializeField] private JCS_2DSlideScreenCamera mSlideCamera = null;
        [Tooltip("Direction u want to go.")]
        [SerializeField] private JCS_2D8Direction mDirection = JCS_2D8Direction.TOP;
        [Tooltip("How many times to go to that direction? (Default is 1)")]
        [SerializeField] private int mCount = 1;

        [Header("** Sound Settings **")]
        [Tooltip("Sound when sliding screen. (Switch Scene)")]
        [SerializeField] private AudioClip mSlideScreenSound = null;

        [Header("** Delay Settings **")]
        [SerializeField] private float mDelayTime = 0;
        private float mDelayTimer = 0;
        private bool mStartDelay = false;

        public void SetDirection(JCS_2D8Direction direction) { this.mDirection = direction; }

        protected override void Awake()
        {
            base.Awake();

            mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            // try to get it from the scene by type.
            if (mSlideCamera == null)
                mSlideCamera = (JCS_2DSlideScreenCamera)FindObjectOfType(typeof(JCS_2DSlideScreenCamera));
        }

        private void Update()
        {

            if (!mStartDelay)
                return;

            mDelayTimer += Time.deltaTime;

            if (mDelayTime < mDelayTimer)
            {
                // switch scene.
                SwitchScene();

                // play sound.
                mSoundPlayer.PlayOneShot(mSlideScreenSound);

                // reset timer (ready for next use)
                mDelayTimer = 0;

                // disable delay (ready for next use)
                mStartDelay = false;
            }
        }

        public override void JCS_ButtonClick()
        {
            if (mSlideCamera == null)
            {
                JCS_GameErrors.JcsReminders(
                    "JCS_SlideScreenButton", 
                    -1,
                    "Assign the button without camera is not allowed...");
                return;
            }

            // start counting delay
            mStartDelay = true;

            // do call back
            base.JCS_ButtonClick();
        }

        private void SwitchScene()
        {
            for (int index = 0;
                index < mCount;
                ++index)
            {
                mSlideCamera.SwitchScene(mDirection);
            }
        }
    }
}
