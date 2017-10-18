/**
 * $File: JCS_ButtonSelectionGroupController.cs $
 * $Date: 2017-10-07 14:58:41 $
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
    /// Control the 'JCS_ButtonSelectionGroup' class with certain ket input, 
    /// without that class this class is meaningless.
    /// </summary>
    [RequireComponent(typeof(JCS_ButtonSelectionGroup))]
    public class JCS_ButtonSelectionGroupController
        : MonoBehaviour
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/
        private JCS_ButtonSelectionGroup mButtonSelectionGroup = null;


        [Header("** Runtime Variables (JCS_ButtonSelectionGroupController) **")]

        [Tooltip("Active key listener?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("")]
        [SerializeField]
        private JCS_KeyActionType mKeyActionType = JCS_KeyActionType.KEY_DOWN;


        [Header("- Keyboard Settings (JCS_ButtonSelectionGroupController)")]

        [Tooltip("Key for next selection. (Keyboard)")]
        [SerializeField]
        private KeyCode mMNext = KeyCode.None;

        [Tooltip("Key for previous selection. (Keyboard)")]
        [SerializeField]
        private KeyCode mMPrev = KeyCode.None;

        [Tooltip("Okay for this selection. (Keyboard)")]
        [SerializeField]
        private KeyCode mMOkay = KeyCode.None;


        [Header("- Game Pad Settings (JCS_ButtonSelectionGroupController)")]

        [Tooltip("Which device we are listening to?")]
        [SerializeField]
        private JCS_JoystickIndex mGamePadId = JCS_JoystickIndex.FROM_ALL_JOYSTICK;

        [Tooltip("Next selection. (Game Pad)")]
        [SerializeField]
        private JCS_JoystickButton mJNext = JCS_JoystickButton.NONE;

        [Tooltip("Previous selection. (Game Pad)")]
        [SerializeField]
        private JCS_JoystickButton mJPrev = JCS_JoystickButton.NONE;


        [Tooltip("Okay for this selection. (Game Pad)")]
        [SerializeField]
        private JCS_JoystickButton mJOkay = JCS_JoystickButton.NONE;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public bool Active { get { return this.mActive; } set { this.mActive = value; } }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void Awake()
        {
            this.mButtonSelectionGroup = this.GetComponent<JCS_ButtonSelectionGroup>();
        }

        private void Update()
        {
            if (!mActive)
                return;

            if (ActiveNext())
                mButtonSelectionGroup.NextSelection();

            if (ActivePrev())
                mButtonSelectionGroup.PrevSelection();

            if (ActiveOkay())
                mButtonSelectionGroup.OkaySelection();
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
        /// Active next selection? (Key Depends)
        /// </summary>
        /// <returns></returns>
        private bool ActiveNext()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMNext) || 
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJNext);
        }

        /// <summary>
        /// Active previous selection? (Key Depends)
        /// </summary>
        /// <returns></returns>
        private bool ActivePrev()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMPrev) || 
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJPrev);
        }

        /// <summary>
        /// Active okay selection? (Key Depends)
        /// </summary>
        /// <returns></returns>
        private bool ActiveOkay()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMOkay) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJOkay);
        }

    }
}
