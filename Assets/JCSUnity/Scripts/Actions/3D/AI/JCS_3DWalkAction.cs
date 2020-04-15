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
#if UNITY_5_4_OR_NEWER
using UnityEngine.AI;
#endif

namespace JCSUnity
{
    /// <summary>
    /// Simulate the walk action in 3D space.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class JCS_3DWalkAction
        : MonoBehaviour
        , JCS_Action
    {
        /// <summary>
        /// List of all the walk type.
        /// </summary>
        public enum JCS_3DWalkType
        {
            CLOSEST_POINT,
            IN_RANGE,
        };

        /* Variables */

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

        [Tooltip("Check weather you want do this action.")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Type of the walk behaviour calculation.")]
        [SerializeField]
        private JCS_3DWalkType mWalkType = JCS_3DWalkType.CLOSEST_POINT;

        [Tooltip("Range that enemy will try to get close to.")]
        [SerializeField]
        [Range(0.001f, 1000.0f)]
        private float mRangeDistance = 5.0f;

        [Tooltip("Randomly adjusts the range distance.")]
        [SerializeField]
        [Range(1.0f, 30.0f)]
        private float mAdjustRangeDistance = 0.0f;

        [Header("- Tracking")]

        [Tooltip("Time to target the enemy.")]
        [SerializeField]
        [Range(0.01f, 180.0f)]
        private float mTargetTime = 1.0f;

        [Tooltip("Adjust a bit of the attack time.")]
        [SerializeField]
        [Range(0.0f, 160.0f)]
        private float mAdjustTargetTime = 0.0f;

        // timer to simulate the time for targeting the player.
        private float mTargetTimer = 0;

        // time that actually make the enemy target the player.
        private float mRealTargetTime = 0;

        /* Setter & Getter */

        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public JCS_3DWalkType WalkType { get { return this.mWalkType; } set { this.mWalkType = value; } }

        public float RangeDistance { get { return this.mRangeDistance; } set { this.mRangeDistance = value; } }
        public float AdjustRangeDistance { get { return this.mAdjustRangeDistance; } set { this.mAdjustRangeDistance = value; } }
        public float TargetTime { get { return this.mTargetTime; } set { this.mTargetTime = value; } }
        public float AdjustTargetTime { get { return this.mAdjustTargetTime; } set { this.mAdjustTargetTime = value; } }

        /* Functions */

        private void Awake()
        {
            this.mNavMeshAgent = this.GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            DoAI();
        }

        /// <summary>
        /// Target one player and do in target action.
        /// </summary>
        /// <param name="target"> Target we are following. </param>
        public void TargetOne(Transform target)
        {
            // if target is does not exist,
            // end function call.
            if (target == null)
            {
                JCS_Debug.LogError("The transform you are targeting is null");
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
            Vector3 targetPos = GetTargetPosByWalkType(target.transform.position);

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
        /// Check if the nav mesh agent arrive the destination.
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

        /// <summary>
        /// Return the target position base on walk type.
        /// </summary>
        /// <param name="targetPos"> Target position. </param>
        /// <returns>
        /// Target position that calculated depends walk type.
        /// </returns>
        private Vector3 GetTargetPosByWalkType(Vector3 targetPos)
        {
            switch (mWalkType)
            {
                case JCS_3DWalkType.CLOSEST_POINT:
                    return CalculateClosest(targetPos);
                case JCS_3DWalkType.IN_RANGE:
                    return CalculateRange(targetPos);
            }
            JCS_Debug.LogError("Walk type can't happens");
            return targetPos;
        }

        /// <summary>
        /// Calculate the the closest point by range and targetPos.
        /// </summary>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        private Vector3 CalculateClosest(Vector3 targetPos)
        {
            Vector3 newTargetPos = targetPos;

            Vector3 vec = this.transform.position - targetPos;

            vec = vec.normalized;

            float distance = GetRangeDistance();

            float hyp = JCS_Mathf.PythagoreanTheorem(vec.x, vec.z, JCS_Mathf.TriSides.hyp);

            float ratio = distance / hyp;

            newTargetPos.x += vec.x * ratio;
            newTargetPos.z += vec.z * ratio;
            newTargetPos.y = this.transform.position.y;

            return newTargetPos;
        }

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
            float angle = JCS_Random.Range(0, 360);

            float hyp = GetRangeDistance();

            float opp = Mathf.Sin(angle) * hyp;

            float adj = JCS_Mathf.PythagoreanTheorem(hyp, opp, JCS_Mathf.TriSides.adj);

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
            if (!mActive)
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
            mRealTargetTime = mTargetTime + JCS_Random.Range(-mAdjustTargetTime, mAdjustTargetTime);


            mTargeted = false;

            // reset timer.
            mTargetTimer = 0;
        }

        /// <summary>
        /// Return the range distance.
        /// </summary>
        /// <returns> Value of the range distance. </returns>
        private float GetRangeDistance()
        {
            float hypOffset = JCS_Random.Range(-mAdjustRangeDistance, mAdjustRangeDistance);

            return mRangeDistance + hypOffset;
        }
    }
}
