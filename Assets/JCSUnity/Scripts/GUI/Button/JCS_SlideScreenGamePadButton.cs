/**
 * $File: JCS_SlideScreenGamePadButton.cs $
 * $Date: 2018-01-02 09:00:20 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Button will do the slide screen. (Game Pad)
    /// </summary>
    public class JCS_SlideScreenGamePadButton
        : JCS_GamePadButton
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        [Header("** Check Variables (JCS_SlideScreenGamePadButton) **")]

        [Tooltip("This action are using \"JCS_2DSlideScreenCamera\".")]
        [SerializeField]
        private JCS_2DSlideScreenCamera[] mSlideCameras = null;


        [Header("** Runtime Variables (JCS_SlideScreenGamePadButton) **")]

        [Tooltip("Direction u want to go.")]
        [SerializeField]
        private JCS_2D8Direction mDirection = JCS_2D8Direction.TOP;

        [Tooltip("How many times to go to that direction? (Default is 1)")]
        [SerializeField] [Range(1, 5)]
        private int mCount = 1;


        [Header("** Sound Settings (JCS_SlideScreenGamePadButton) **")]

        [Tooltip("Sound when sliding screen. (Switch Scene)")]
        [SerializeField]
        private AudioClip mSlideScreenSound = null;


        [Header("** Delay Settings (JCS_SlideScreenGamePadButton) **")]

        [Tooltip("Time delay when slide screen.")]
        [SerializeField]
        private float mDelayTime = 0;

        private float mDelayTimer = 0;

        private bool mStartDelay = false;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public void SetDirection(JCS_2D8Direction direction) { this.mDirection = direction; }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        protected override void Awake()
        {
            base.Awake();

            mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            // try to get it from the scene by type.
            this.mSlideCameras = (JCS_2DSlideScreenCamera[])FindObjectsOfType(typeof(JCS_2DSlideScreenCamera));
        }

        protected override void Update()
        {
            base.Update();

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



        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        /// <summary>
        /// Button click function pointer override.
        /// </summary>
        public override void JCS_OnClickCallback()
        {
            if (mSlideCameras.Length == 0)
            {
                JCS_Debug.LogReminders(
                    "Assign the button without camera is not allowed...");
                return;
            }

            // start counting delay
            mStartDelay = true;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Switch the scene.
        /// </summary>
        private void SwitchScene()
        {
            JCS_2DSlideScreenCamera slideCamera = null;

            for (int index = 0;
                index < mCount;
                ++index)
            {
                for (int index2 = 0;
                    index2 < mSlideCameras.Length;
                    ++index2)
                {
                    slideCamera = this.mSlideCameras[index2];

                    // do the slide screen effect.
                    if (slideCamera != null)
                        slideCamera.SwitchScene(mDirection);
                }
            }
        }

    }
}
