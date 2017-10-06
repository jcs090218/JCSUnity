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
    /// Button will listen to the gamepad.
    /// </summary>
    public class JCS_GamePadButton
        : JCS_Button
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        [Header("** Runtime Variables (JCS_GamePadButton) **")]

        [Tooltip("Key to trigger this button")]
        [SerializeField]
        private JCS_JoystickButton mKeyToListen = JCS_JoystickButton.NONE;

        [Tooltip("Which joystick should listen?")]
        [SerializeField]
        private JCS_JoystickIndex mJoystickLitener = JCS_JoystickIndex.JOYSTICK_00;

        [Tooltip("Ignore the top two variables, listen to any key on the keyboard/gamepad.")]
        [SerializeField]
        private bool mListenToAnyKey = false;

        [Tooltip("")]
        [SerializeField]
        private JCS_KeyActionType mKeyActionType = JCS_KeyActionType.KEY_DOWN;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public JCS_JoystickButton KeyToListen { get { return this.mKeyToListen; } set { this.mKeyToListen = value; } }
        public JCS_JoystickIndex JoystickLitener { get { return this.mJoystickLitener; } set { this.mJoystickLitener = value; } }
        public bool ListenToAnyKey { get { return this.mListenToAnyKey; } set { this.mListenToAnyKey = value; } }
        public JCS_KeyActionType KeyActionType { get { return this.mKeyActionType; } set { this.mKeyActionType = value; } }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/

        protected virtual void Update()
        {
            if (mListenToAnyKey)
            {
                if (JCS_Input.IsAnyKeyBuffer(mKeyActionType))
                    JCS_ButtonClick();
            }
            else
            {
                if (JCS_Input.GetJoystickButton(mJoystickLitener, mKeyToListen))
                    JCS_ButtonClick();
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

    }
}
