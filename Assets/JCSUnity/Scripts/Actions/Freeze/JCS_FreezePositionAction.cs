/**
 * $File: JCS_FreezePositionAction.cs $
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
    /// Do freezing for position from transform.
    /// </summary>
    public class JCS_FreezePositionAction
        : JCS_UnityObject
    {
        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        [Header("** Check Variables (JCS_FreezePositionAction) **")]

        [Tooltip("Position where we freeze at.")]
        [SerializeField]
        private Vector3 mPositionToFreeze = Vector3.zero;


        [Header("** Runtime Variables (JCS_FreezePositionAction) **")]

        [Tooltip("Is this action active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Is this action active?")]
        [SerializeField]
        private bool mIsLocalPosition = true;

        [Tooltip("Freeze the position?")]
        [SerializeField]
        private JCS_Bool3 mFreezePosition = JCS_Bool3.allFalse;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public bool IsLocalPosition
        {
            get { return this.mIsLocalPosition; }
            set
            {
                this.mIsLocalPosition = value;

                // get the new freeze position.
                if (mIsLocalPosition)
                    this.mPositionToFreeze = this.LocalPosition;
                else
                    this.mPositionToFreeze = this.Position;
            }
        }
        public JCS_Bool3 FreezePosition { get { return this.mFreezePosition; } set { this.mFreezePosition = value; } }
        public Vector3 PositionToFreeze { get { return this.mPositionToFreeze; } set { this.mPositionToFreeze = value; } }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void Start()
        {
            // get the new freeze position.
            if (IsLocalPosition)
                this.mPositionToFreeze = this.LocalPosition;
            else
                this.mPositionToFreeze = this.Position;
        }

        private void Update()
        {
            if (!mActive)
                return;

            DoFreezePosition();
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
        private void DoFreezePosition()
        {
            Vector3 newPos = this.Position;
            if (IsLocalPosition)
                newPos = this.LocalPosition;

            /* Freeze position */
            if (mFreezePosition.check1)
                newPos.x = mPositionToFreeze.x;
            if (mFreezePosition.check2)
                newPos.y = mPositionToFreeze.y;
            if (mFreezePosition.check3)
                newPos.z = mPositionToFreeze.z;

            /* Apply new value to transform. */
            if (IsLocalPosition)
                this.LocalPosition = newPos;
            else
                this.Position = newPos;
        }
    }
}
