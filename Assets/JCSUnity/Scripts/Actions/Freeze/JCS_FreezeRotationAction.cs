/**
 * $File: JCS_FreezeRotationAction.cs $
 * $Date: 2017-05-11 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{

    /// <summary>
    /// Do freezing for rotation from transform.
    /// </summary>
    public class JCS_FreezeRotationAction
        : JCS_UnityObject
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        [Header("** Check Variables (JCS_FreezeTransformAction) **")]

        [Tooltip("Rotation where we freeze at.")]
        [SerializeField]
        private Vector3 mRotationToFreeze = Vector3.zero;


        [Header("** Runtime Variables (JCS_FreezeTransformAction) **")]

        [Tooltip("Is this action active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Is this action active?")]
        [SerializeField]
        private bool mIsLocalRotation = true;

        [Tooltip("Freeze the rotation?")]
        [SerializeField]
        private JCS_Bool3 mFreezeRotation = JCS_Bool3.allFalse;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public bool IsLocalRotation
        {
            get { return this.mIsLocalRotation; }
            set
            {
                this.mIsLocalRotation = value;

                // get the new freeze position.
                if (mIsLocalRotation)
                    this.mRotationToFreeze = this.LocalEulerAngles;
                else
                    this.mRotationToFreeze = this.EulerAngles;
            }
        }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void LateUpdate()
        {
            if (!mActive)
                return;

            DoFreezeRotation();
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
        /// Freeze position.
        /// </summary>
        private void DoFreezeRotation()
        {
            Vector3 newRot = this.EulerAngles;
            if (IsLocalRotation)
                newRot = this.LocalEulerAngles;

            /* Freeze euler angles */
            if (mFreezeRotation.check1)
                newRot.x = mRotationToFreeze.x;
            if (mFreezeRotation.check2)
                newRot.y = mRotationToFreeze.y;
            if (mFreezeRotation.check3)
                newRot.z = mRotationToFreeze.z;

            if (IsLocalRotation)
                this.LocalEulerAngles = newRot;
            else
                this.EulerAngles = newRot;
        }
    }
}
