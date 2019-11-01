/**
 * $File: JCS_GamePadButton.cs $
 * $Date: 2017-10-07 04:37:53 $
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
    /// Button will listen to the gamepad. Note it compatible with
    /// PC/desktop too.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public abstract class JCS_GamePadButton
        : JCS_Button
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*
         * TODO(jenchieh): Consider this to put at `JCS_Button' class
         * instead of this subclass.
         */
        protected JCS_SoundPlayer mSoundPlayer = null;


        [Header("** Runtime Variables (JCS_GamePadButton) **")]

        [Tooltip("Still check input when the game is pause?")]
        [SerializeField]
        protected bool mIgnorePauseCheck = false;

        [Tooltip("Ignore the top two variables, listen to any key on the keyboard/gamepad.")]
        [SerializeField]
        protected bool mListenToAnyKey = false;

        [Tooltip("Key action type.")]
        [SerializeField]
        protected JCS_KeyActionType mKeyActionType = JCS_KeyActionType.KEY_DOWN;


        [Header("- Keyboard (JCS_GamePadButton)")]

        [Tooltip("Key to trigger this button.")]
        [SerializeField]
        protected KeyCode mKKeyToListen = KeyCode.None;


        [Header("- Game Pad (JCS_GamePadButton)")]

        [Tooltip("Key to trigger this button.")]
        [SerializeField]
        protected JCS_JoystickButton mJKeyToListen = JCS_JoystickButton.NONE;

        [Tooltip("Which joystick should listen?")]
        [SerializeField]
        protected JCS_JoystickIndex mJoystickLitener = JCS_JoystickIndex.FROM_ALL_JOYSTICK;


        [Header("- Audio Settings (JCS_GamePadButton)")]

        [Tooltip("Play with the global sound player.")]
        [SerializeField]
        protected bool mPlayWithGlobalSoundPlayer = false;

        [Tooltip("Sound when button is pressed.")]
        [SerializeField]
        protected AudioClip mButtonClickSound = null;

        [Tooltip("Sound method.")]
        [SerializeField]
        protected JCS_SoundMethod mSoundMethod = JCS_SoundMethod.PLAY_SOUND;

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public KeyCode KKeyToListen { get { return this.mKKeyToListen; } set { this.mKKeyToListen = value; } }
        public JCS_JoystickButton JKeyToListen { get { return this.mJKeyToListen; } set { this.mJKeyToListen = value; } }
        public JCS_JoystickIndex JoystickLitener { get { return this.mJoystickLitener; } set { this.mJoystickLitener = value; } }
        public bool ListenToAnyKey { get { return this.mListenToAnyKey; } set { this.mListenToAnyKey = value; } }
        public JCS_KeyActionType KeyActionType { get { return this.mKeyActionType; } set { this.mKeyActionType = value; } }

        public AudioClip ButtonClickSound { get { return this.mButtonClickSound; } set { this.mButtonClickSound = value; } }
        public JCS_SoundMethod SoundMethod { get { return this.mSoundMethod; } set { this.mSoundMethod = value; } }

        public bool IgnorePauseCheck { get { return this.mIgnorePauseCheck; } set { this.mIgnorePauseCheck = value; } }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        protected override void Awake()
        {
            base.Awake();

            this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
        }

        protected virtual void Update()
        {
            if (mListenToAnyKey)
            {
                if (JCS_Input.IsAnyKeyBuffer(mKeyActionType, mIgnorePauseCheck))
                {
                    JCS_ButtonClick();
                    PlayButtonClickSound();
                }
            }
            else
            {

                if (// listen to keyboard.
                    JCS_Input.GetKeyByAction(mKeyActionType, mKKeyToListen, mIgnorePauseCheck) ||
                    // listen to game pad.
                    JCS_Input.GetJoystickKeyByAction(mKeyActionType, mJoystickLitener, mJKeyToListen, mIgnorePauseCheck)
                    )
                {
                    JCS_ButtonClick();
                    PlayButtonClickSound();
                }
            }
        }

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Play the button click sound.
        /// </summary>
        private void PlayButtonClickSound()
        {
            if (mButtonClickSound == null)
                return;

            JCS_SoundPlayer soundPlayer = mSoundPlayer;
            /* Get sound plaeyr according to the chosen choice. */
            if (mPlayWithGlobalSoundPlayer)
                soundPlayer = JCS_SoundManager.instance.GetGlobalSoundPlayer();

            soundPlayer.PlayOneShotByMethod(mButtonClickSound, mSoundMethod);
        }

    }
}
