/**
 * $File: JCS_2DPushLiveObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// Effect that pushes the 2d live object.
    /// </summary>
    public class JCS_2DPushLiveObject
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Runtime Variables (JCS_2DPushLiveObject) **")]
        [SerializeField]
        private bool mContinuousPush = false;

        [SerializeField] [Range(1, 100)]
        private float mPushForce = 10;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        
        private void OnTriggerEnter(Collider other)
        {
            CheckAvailableToPush(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!mContinuousPush)
                return;

            CheckAvailableToPush(other);
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Do once push effect to a 2d live object.
        /// </summary>
        public void PushEffect(JCS_2DLiveObject liveObject)
        {
            // knock back the object.
            liveObject.KnockBack(mPushForce, this.transform);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Check if the object are ok to do push.
        /// </summary>
        /// <param name="other"></param>
        private void CheckAvailableToPush(Collider other)
        {
            JCS_2DLiveObject liveObject = other.GetComponent<JCS_2DLiveObject>();
            if (liveObject == null)
                return;

            // do the effect.
            PushEffect(liveObject);
        }

    }
}
