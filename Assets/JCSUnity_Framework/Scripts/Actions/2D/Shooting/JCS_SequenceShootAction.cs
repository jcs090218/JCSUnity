/**
 * $File: JCS_SequenceShootAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{

    /// <summary>
    /// Shoot in Sequence act like damage text.(sequence)
    /// </summary>
    [RequireComponent(typeof(JCS_ShootAction))]
    public class JCS_SequenceShootAction
        : MonoBehaviour
        , JCS_Action
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_ShootAction mShootAction = null;

        [SerializeField] private int mHit = 8;
        [SerializeField] private float mTimePerShoot = 0.1f;

        [SerializeField] private bool mSequenceStay = true;
        [SerializeField] private bool mInSequenceEffect = false;

        [Header("** Action Settings **")]
        [SerializeField] private float mTimeBeforeShoot = 0;
        private bool mAction = false;
        [SerializeField] private float mTimeDelayAfterShoot = 0;
        private bool mAfterDelay = false;
        private float mActionTimer = 0;

        [Header("** Shoot Gap Effect **")]
        [SerializeField] private bool mShootGapEffect = false;
        [SerializeField] private float mShootGap = 0.1f;

        private JCS_DetectAreaObject mDetectedObject = null;

        //** Sequence Data **
        private JCS_Vector<int> mThread = null;        // main thread
        private JCS_Vector<float> mTimers = null;           // timer per thread
        private JCS_Vector<int> mShootCount = null;         // how many shoot should process per thread
        private JCS_Vector<int> mShootCounter = null;         // counter per thread
        private JCS_Vector<Vector3> mShootPos = null;
        private JCS_Vector<Transform> mTargetsPerSequence = null;
        private JCS_Vector<bool> mShootDirection = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public int Hit { get { return this.mHit; } set { this.mHit = value; } }
        public bool InSequenceEffect { get { return this.mInSequenceEffect; } set { this.mInSequenceEffect = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mShootAction = this.GetComponent<JCS_ShootAction>();

            // override the shoot effect in the base one.
            mShootAction.OverrideShoot = true;

            mThread = new JCS_Vector<int>();
            mTimers = new JCS_Vector<float>();
            mShootCount = new JCS_Vector<int>();
            mShootCounter = new JCS_Vector<int>();
            mShootPos = new JCS_Vector<Vector3>();
            mTargetsPerSequence = new JCS_Vector<Transform>();
            mShootDirection = new JCS_Vector<bool>();
        }

        private void Update()
        {
            ProcessInput();

            // process sequence of threads.
            ProccessSequences();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        /// <summary>
        /// Shoot bullets multiple times in times. (not in per frame)
        /// </summary>
        public void Shoots(int hit, Vector3 pos)
        {
            if (mShootAction.Bullet == null)
            {
                JCS_GameErrors.JcsReminders("JCS_SequenceShootAction", 
                    -1,
                    "There is no bullet assign to \"JCS_ShootAction\", so we cannot shoot a sequence...");
                return;
            }

            if (hit < 0)
            {
                JCS_GameErrors.JcsReminders("JCS_SequenceShootAction",
                    -1,
                    "Cannot shoot sequence of bullet with lower than 0 hit...");
                return;
            }

            // after finding once is still null,
            // try it agian!
            if (mDetectedObject == null)
            {
                // find the target
                switch (mShootAction.GetTrackType())
                {
                    case JCS_ShootAction.TrackType.CLOSEST:
                        mDetectedObject = mShootAction.GetDetectAreaAction().FindTheClosest();
                        break;
                    case JCS_ShootAction.TrackType.FURTHEST:
                        mDetectedObject = mShootAction.GetDetectAreaAction().FindTheFurthest();
                        break;
                }

            }

            // does not found the target to damage
            if (mDetectedObject == null)
                return;

            // found target to damage, add in to data segment
            mTargetsPerSequence.push(mDetectedObject.transform);


            // thread itself
            mThread.push(mThread.length);

            // needed data
            mTimers.push(0);
            mShootCount.push(hit);
            mShootCounter.push(0);
            mShootPos.push(pos);


            bool isLeft = true;
            if (this.transform.localScale.x < 0)
                isLeft = false;

            // shoot direction
            mShootDirection.push(isLeft);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void ProccessSequences()
        {
            for (int processIndex = 0;
                processIndex < mThread.length;
                ++processIndex)
            {
                // process all the thread
                Sequence(processIndex);
            }
        }
        private void Sequence(int processIndex)
        {
            // get the timer from the thread
            float newTimer = mTimers.at(processIndex);

            // add time to timer
            newTimer += Time.deltaTime;

            // check if we can shoot or not
            if (mTimePerShoot < newTimer)
            {
                int totalShootCount = mShootCount.at(processIndex);
                int currentShootCount = mShootCounter.at(processIndex);
                if (currentShootCount == totalShootCount)
                {
                    // Remove Thread.
                    EndProcessSequence(processIndex);
                    return;
                }

                Vector3 spawnPos = this.transform.position;

                // if stay in the same position
                if (mSequenceStay)
                    spawnPos = mShootPos.at(processIndex);

                if (mShootGapEffect)
                {
                    spawnPos.y += currentShootCount * mShootGap;
                }

                // direction.
                bool direction = mShootDirection.at(processIndex);

                // shoot a bullet
                if (mInSequenceEffect)
                {
                    bool theSub = true;

                    // meaning the head of the bullet in sequence.
                    if (currentShootCount == 0)
                    {
                        theSub = false;
                        mShootAction.Shoot(spawnPos, direction, mHit, currentShootCount, theSub, mTargetsPerSequence.at(processIndex));
                        //mTargetsPerSequence.set(processIndex, target);
                    }
                    // process the sub bullet in sequence
                    else {
                        mShootAction.Shoot(spawnPos, direction, mHit, currentShootCount, theSub, mTargetsPerSequence.at(processIndex));
                    }
                }
                else
                    mShootAction.Shoot(spawnPos, direction, 1, currentShootCount, false);
                

                ++currentShootCount;

                // update new count, in order 
                // to spawn next bullet
                mShootCounter.set(processIndex, currentShootCount);
                newTimer = 0;
            }

            // update timer
            mTimers.set(processIndex, newTimer);
        }
        private void EndProcessSequence(int processIndex)
        {
            mThread.slice(processIndex);

            mTimers.slice(processIndex);
            mShootCount.slice(processIndex);
            mShootCounter.slice(processIndex);
            mShootPos.slice(processIndex);
            mTargetsPerSequence.slice(processIndex);
            mShootDirection.slice(processIndex);
        }
        private void ProcessInput()
        {
            if (!mAction)
            {
                if (JCS_Input.GetKey(mShootAction.ShootKeyCode))
                {

                    // find the target
                    switch (mShootAction.GetTrackType())
                    {
                        case JCS_ShootAction.TrackType.CLOSEST:
                            mDetectedObject = mShootAction.GetDetectAreaAction().FindTheClosest();
                            break;
                        case JCS_ShootAction.TrackType.FURTHEST:
                            mDetectedObject = mShootAction.GetDetectAreaAction().FindTheFurthest();
                            break;
                    }

                    mAction = true;
                }
            }

            if (mAfterDelay)
            {
                mActionTimer += Time.deltaTime;

                if (mTimeDelayAfterShoot < mActionTimer)
                {
                    // reset timer
                    mActionTimer = 0;

                    // can do the next shoot
                    mAction = false;

                    // exit delay process
                    mAfterDelay = false;
                }
            }

            if (mAction && !mAfterDelay)
            {
                mActionTimer += Time.deltaTime;

                if (mTimeBeforeShoot < mActionTimer)
                {
                    // Do shooting effect
                    Shoots(mHit, this.transform.position);

                    // start after delay timer.
                    mAfterDelay = true;

                    // reset timer for "mAfterDelay" Trigger.
                    mActionTimer = 0;
                }
            }
        }

    }
}
