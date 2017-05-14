/**
 * $File: JCS_AdjustTimeTrigger.cs $
 * $Date: 2017-05-10 17:30:23 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace JCSUnity
{
    public delegate void EmptyAction();

    /// <summary>
    /// Trigger a event while the time is reached.
    /// Script base.
    /// </summary>
    public class JCS_AdjustTimeTrigger
        : MonoBehaviour
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        // action to trigger if the time is reached.
        public EmptyAction actions = null;

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        [Header("** Check Variables (JCS_AdjustTimeTrigger) **")]

        [Tooltip(@"time to record down the real time to do one 
action after we calculate the real time.")]
        [SerializeField]
        private float mRealTimeZone = 0;

        [Tooltip("Timer to check if reach the real time zone.")]
        [SerializeField]
        private float mTimer = 0;

        [Tooltip("check if the action trigger.")]
        [SerializeField]
        private bool mDidAction = false;


        [Header("** Runtime Variables (JCS_AdjustTimeTrigger) **")]

        [Tooltip("Is this component active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Time to do one action.")]
        [SerializeField]
        [Range(0.0f, 30.0f)]
        private float mTimeZone = 2.0f;

        [Tooltip("Time that will randomly affect the Time Zone.")]
        [SerializeField]
        [Range(0.0f, 20.0f)]
        private float mAdjustTimeZone = 1.5f;

        [Tooltip("Event to call if the action happens.")]
        [SerializeField]
        private UnityEvent mUnityEvents = null;


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
        private void Update()
        {
            if (!mActive)
                return;

            DoAction();
        }

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        /// <summary>
        /// Do the timer action, execute the function pointer if the 
        /// timer had reached repeatedly.
        /// </summary>
        public void DoAction()
        {
            if (mDidAction)
                ResetTimeZone();

            mTimer += Time.deltaTime;

            if (mRealTimeZone > mTimer)
                return;

            // active actions.
            if (actions != null)
                actions.Invoke();

            if (mUnityEvents != null)
                mUnityEvents.Invoke();

            mDidAction = true;
        }

        /// <summary>
        /// Algorithm to calculate the time to do 
        /// walk action include direction.
        /// </summary>
        public void ResetTimeZone()
        {
            float adjustTime = JCS_Random.Range(-mAdjustTimeZone, mAdjustTimeZone);
            mRealTimeZone = mTimeZone + adjustTime;

            mDidAction = false;
            mTimer = 0;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
