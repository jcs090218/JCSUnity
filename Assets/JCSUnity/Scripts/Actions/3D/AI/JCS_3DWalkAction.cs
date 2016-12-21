/**
 * $File: JCS_3DWalkAction.cs $
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
    /// 
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class JCS_3DWalkAction
        : MonoBehaviour
        , JCS_Action
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        // All enemy should have the nav 
        // mesh agent for the path finding.
        private NavMeshAgent mNavMeshAgent = null;

        [Header("** Check Variables (JCS_3DWalkAction) **")]

        [SerializeField]
        private Transform mTargetTransform = null;

        // count for how many search per frame. 
        // try to avoid stack overflow function call...
        private int mSearchCount = 0;

        // use for checking the action is approve or not.
        private bool mTargeted = false;


        [Header("** Runtime Variables (JCS_3DWalkAction) **")]

        [Tooltip("Check weather u do this action.")]
        [SerializeField]
        private bool mDoAI = true;

        [Tooltip("Range that enemy will try to get close to.")]
        [SerializeField]
        [Range(1, 1000)]
        private float mRangeDistance = 5;

        [Tooltip("")]
        [SerializeField]
        [Range(1, 5)]
        private float mAdjustRangeDistance = 0;


        [Header("- Tracking")]

        [Tooltip("")]
        [SerializeField]
        [Range(0.01f, 10.0f)]
        private float mTargetTime = 1;

        [Tooltip("Adjust a bit of the attack time.")]
        [SerializeField]
        [Range(0, 5.0f)]
        private float mAdjustTargetTime = 0;

        // timer to simulate the time for targeting the player.
        private float mTargetTimer = 0;

        // time that actually make the enemy target the player.
        private float mRealTargetTime = 0;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mNavMeshAgent = this.GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            DoAI();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Target One player and do in range action.
        /// </summary>
        /// <param name="target"> Target we are following. </param>
        public void TargetOne(Transform target)
        {
            // if target is does not exist, 
            // end function call.
            if (target == null)
            {
                JCS_Debug.JcsErrors(
                    this, "This transform u are targeting are null...");
                return;
            }

            if (mSearchCount == 2)
            {
                // reset search count.
                mSearchCount = 0;

                // exit out of recursive function call...
                return;
            }

            // reset the path every time it request.
            mNavMeshAgent.ResetPath();

            // calculate the distance and range relationship, 
            // and find out the position enemy are approach to.
            Vector3 targetPos = CalculateRange(target.transform.position);

            // set to the destination.
            bool found = mNavMeshAgent.SetDestination(targetPos);


            // increase the search count.
            ++mSearchCount;

            // if faild, try it again.
            if (!found)
                TargetOne(target);
            else
            {
                // if succesed.
                // reset search count.
                mSearchCount = 0;
            }

            // make sure the action is approve.
            mTargeted = true;
        }

        /// <summary>
        /// Check the nav mesh agent arrive the destination.
        /// </summary>
        public bool NavMeshArrive(NavMeshAgent agent)
        {
            float dist = agent.remainingDistance;

            if (dist != Mathf.Infinity &&
                agent.pathStatus == NavMeshPathStatus.PathComplete &&
                agent.remainingDistance == 0)
            {
                return true;
            }

            return false;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Calculate the range and position relationship 
        /// in order to find the best destination in the 
        /// navigation map.
        /// 
        /// IMPORTANT(JenChieh): if the vector does not in the range, 
        /// enemy will stay at the place they are, which mean enemy
        /// will do nothing...
        /// </summary>
        /// <returns> result destination </returns>
        private Vector3 CalculateRange(Vector3 targetPos)
        {
            Vector3 newTargetPos = targetPos;

            // set up the unknown angle
            // 隨機"內角"
            float angle = JCS_Utility.JCS_FloatRange(0, 360);

            // define offset
            float hypOffset = JCS_Utility.JCS_FloatRange(
                    -mAdjustRangeDistance,
                    mAdjustRangeDistance);

            // add offset to current distance (hyp)
            float hyp = mRangeDistance + hypOffset;

            float opp = Mathf.Sin(angle) * hyp;

            float adj = JCS_Mathf.PythagoreanTheorem(hyp, opp, JCS_Mathf.Sides.adj);

            bool flipX = JCS_Mathf.IsPossible(50);
            bool flipZ = JCS_Mathf.IsPossible(50);


            if (flipX)
                newTargetPos.x -= adj;
            else
                newTargetPos.x += adj;

            if (flipZ)
                newTargetPos.z -= opp;
            else
                newTargetPos.z += opp;

            return newTargetPos;
        }

        /// <summary>
        /// Do AI algorithm here...
        /// </summary>
        private void DoAI()
        {
            // Check function trigger.
            if (!mDoAI)
                return;

            // start timer
            mTargetTimer += Time.deltaTime;

            // if is target, reset the real time (mRealTargetTime).
            if (mTargeted)
                ResetTargetTimeZone();

            // check if the timer have been reach.
            if (mTargetTimer < mRealTargetTime)
                return;

            // Target the player
            TargetOne(mTargetTransform);

            // reset timer
            mTargetTimer = 0;
        }

        /// <summary>
        /// Target time zone algorithm design here...
        /// </summary>
        private void ResetTargetTimeZone()
        {
            // calculate the real time.
            mRealTargetTime = mTargetTime + JCS_Utility.JCS_FloatRange(-mAdjustTargetTime, mAdjustTargetTime);


            mTargeted = false;

            // reset timer.
            mTargetTimer = 0;
        }

    }
}
