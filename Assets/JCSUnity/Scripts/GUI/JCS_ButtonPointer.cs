/**
 * $File: JCS_ButtonPointer.cs $
 * $Date: 2017-05-29 06:26:00 $
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
    /// GUI object that will goes toward a button, so this make the 
    /// player knows where is the current gui select option goes.
    /// 
    /// Like super mash brother selecting the character.
    /// </summary>
    [RequireComponent(typeof(JCS_SimpleTrackAction))]
    public class JCS_ButtonPointer
        : MonoBehaviour
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/
        private JCS_SimpleTrackAction mSimpleTrackAction = null;

        [Header("** Initialize Variables (JCS_ButtonPointer) **")]

        [Tooltip("Button that this pointer could point.")]
        [SerializeField]
        private JCS_Button[] mButtons = null;


        [Header("** Runtime Variables (JCS_ButtonPointer) **")]

        [Tooltip("Do we point to the button that are not active?")]
        [SerializeField]
        private bool mDontPointIfButtonNotActive = false;

        [Tooltip("Position offset for the pointer.")]
        [SerializeField]
        private Vector3 mPointerOffset = Vector3.zero;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public bool DontPointIfButtonNotActive { get { return this.mDontPointIfButtonNotActive; } set { this.mDontPointIfButtonNotActive = value; } }
        public Vector3 PointerOffset { get { return this.mPointerOffset; } set { this.mPointerOffset = value; } }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void Awake()
        {
            this.mSimpleTrackAction = this.GetComponent<JCS_SimpleTrackAction>();

            // start at the current position. 
            // so trick make the trackaction not moving at all at the 
            // beginning of the action.
            this.mSimpleTrackAction.TargetPosition = this.transform.localPosition;

            for (int index = 0;
                index < mButtons.Length;
                ++index)
            {
                if (mButtons[index] == null)
                    continue;

                mButtons[index].SetSystemCallback(PointToButton);
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
        /// Callback if the button get focus.
        /// </summary>
        private void PointToButton(JCS_Button btn)
        {
            if (mDontPointIfButtonNotActive)
            {
                if (!btn.Interactable)
                    return;
            }

            mSimpleTrackAction.TargetPosition = btn.GetRectTransfom().localPosition + mPointerOffset;
        }

    }
}
