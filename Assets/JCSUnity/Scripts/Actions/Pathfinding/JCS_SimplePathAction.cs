/**
 * $File: JCS_SimplePathAction.cs $
 * $Date: 2020-04-03 08:03:51 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// The point to point simple path action.
    /// </summary>
    [RequireComponent(typeof(JCS_3DGoStraightAction))]
    [RequireComponent(typeof(JCS_3DLookAtAction))]
    [RequireComponent(typeof(JCS_AdjustTimeTrigger))]
    public class JCS_SimplePathAction
        : MonoBehaviour
    {
        /* Variables */

        private JCS_3DGoStraightAction mGoStraightAction = null;
        private JCS_3DLookAtAction mLookAtAction = null;
        private JCS_AdjustTimeTrigger mAdjustTimerTrigger = null;

        [Header("** Check Variables (JCS_SimplePathAction) **")]

        [Tooltip("Current target point index that this object to going approach.")]
        [SerializeField]
        private int mTargetPointIndex = -1;

        [Header("** Runtime Variables (JCS_SimplePathAction) **")]

        [Tooltip("List of points for setting up the path.")]
        [SerializeField]
        private List<Transform> mPoints = null;

        [Tooltip("Random the path by randomizing the target point.")]
        [SerializeField]
        private bool mRandom = false;

        [Tooltip("Range that will stop the movement.")]
        [SerializeField]
        private float mAcceptRange = 0.3f;

        /* Setter & Getter */

        public List<Transform> Points { get { return this.mPoints; } }
        public bool Random { get { return this.mRandom; } set { this.mRandom = value; } }
        public float AcceptRange { get { return this.mAcceptRange; } set { this.mAcceptRange = value; } }

        /* Functions */

        private void Awake()
        {
            this.mGoStraightAction = this.GetComponent<JCS_3DGoStraightAction>();
            this.mLookAtAction = this.GetComponent<JCS_3DLookAtAction>();
            this.mAdjustTimerTrigger = this.GetComponent<JCS_AdjustTimeTrigger>();

            this.mAdjustTimerTrigger.actions = DoPath;

#if UNITY_EDITOR
            if (mPoints.Count == 0)
                JCS_Debug.LogWarning("Path action with 0 path point is not valid");
#endif

            GetNextPoint();
        }

        private void Update()
        {
            if (!this.mGoStraightAction.enabled)
                return;

            Transform target = mPoints[mTargetPointIndex];
            float distance = Vector3.Distance(target.position, this.transform.position);

            if (mAcceptRange >= distance)
                this.mGoStraightAction.enabled = false;
        }

        /// <summary>
        /// Generate the next new point but not the same as last point.
        /// </summary>
        private void GetNextPoint()
        {
            if (mRandom)
            {
                int len = mPoints.Count;
                int newIndex = JCS_Random.RangeInclude(0, len - 1);

                if (newIndex == mTargetPointIndex && len != 1)
                    GetNextPoint();
                else
                    mTargetPointIndex = newIndex;
            }
            else
            {
                ++mTargetPointIndex;

                if (mTargetPointIndex >= mPoints.Count)
                    mTargetPointIndex = 0;
            }
        }

        private void DoPath()
        {
            GetNextPoint();

            this.mGoStraightAction.enabled = true;

            mLookAtAction.SetTargetTransform(mPoints[mTargetPointIndex]);
        }
    }
}
