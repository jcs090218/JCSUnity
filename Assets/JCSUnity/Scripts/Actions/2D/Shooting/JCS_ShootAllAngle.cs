/**
 * $File: JCS_ShootAllAngle.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(JCS_ShootAction))]
    public class JCS_ShootAllAngle
        : MonoBehaviour
        , JCS_Action
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_ShootAction mShootAction = null;

        [Header("** Runtime Variables **")]

        [Tooltip("Automatically shoot the bullet itself, use of AI!")]
        [SerializeField] private bool mAutoShootInFrame = false;

        [Tooltip("Automatically shoot the bullet itself, use of AI!")]
        [SerializeField] private bool mAutoShootByOrder = false;

        [Tooltip("Degree per bullet shoot.")]
        [SerializeField] private float mDegreePerShoot = 10;

        [Tooltip("Check if the enemy can shoot or not depends on the Delay Time!")]
        [SerializeField] private bool mCanShoot = true;


        [Tooltip("How long it take to shoot a bullet.")]
        [SerializeField] [Range(0.01f, 15.0f)]
        private float mDelayTime = 1.0f;

        [Tooltip("Time that will randomly affect the Time Zone.")]
        [SerializeField] [Range(0.0f, 3.0f)]
        private float mAdjustTimeZone = 1.5f;

        private float mDelayTimer = 0;

        private bool mShooted = false;

        private float mRealTimeZone = 0;

        private float mCount = 0;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool AutoShootInFrame { get { return this.mAutoShootInFrame; } set { this.mAutoShootInFrame = value; } }
        public bool AutoShootByOrder { get { return this.mAutoShootByOrder; } set { this.mAutoShootByOrder = value; } }
        public bool CanShoot { get { return this.mCanShoot; } set { this.mCanShoot = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mShootAction = this.GetComponent<JCS_ShootAction>();

            mShootAction.CanShoot = false;

            // disable auto shoot be default
            mShootAction.AutoShoot = false;
        }

        private void Update()
        {
            if (AutoShootInFrame)
                AutoShootActionByFrame();

            if (AutoShootByOrder)
                AutoShootActionByOrder();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void ShootAllAngleByFrame()
        {
            Vector3 newRotation = this.transform.localEulerAngles;
            Vector3 recordRotation = newRotation;

            for (int count = 0;
                count < 360 / mDegreePerShoot;
                ++count)
            {
                newRotation.z = mDegreePerShoot * count;
                transform.localEulerAngles = newRotation;

                mShootAction.Shoot();
            }
            transform.localEulerAngles = recordRotation;

            mShooted = true;
        }
        public void ShootAllAngleByOrder()
        {
            Vector3 newRotation = this.transform.localEulerAngles;
            Vector3 recordRotation = newRotation;

            newRotation.z = mDegreePerShoot * mCount;
            transform.localEulerAngles = newRotation;

            mShootAction.Shoot();

            ++mCount;

            transform.localEulerAngles = recordRotation;

            mShooted = true;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void AutoShootActionByFrame()
        {
            if (!CanShoot)
                return;

            if (mShooted)
                ResetTimeZone();

            mDelayTimer += Time.deltaTime;

            if (mRealTimeZone < mDelayTimer)
            {
                ShootAllAngleByFrame();

                // reset timer
                mDelayTimer = 0;
            }
        }

        private void AutoShootActionByOrder()
        {
            if (!CanShoot)
                return;

            if (mShooted)
                ResetTimeZone();

            mDelayTimer += Time.deltaTime;

            if (mRealTimeZone < mDelayTimer)
            {
                ShootAllAngleByOrder();

                // reset timer
                mDelayTimer = 0;
            }
        }

        private void ResetTimeZone()
        {
            float adjustTime = JCS_Utility.JCS_FloatRange(-mAdjustTimeZone, mAdjustTimeZone);
            mRealTimeZone = mDelayTime + adjustTime;

            mShooted = false;
            mDelayTimer = 0;
        }

    }
}
