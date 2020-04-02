/**
 * $File: JCS_SimplePathAction.cs $
 * $Date: 2020-04-02 19:06:04 $
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
    [RequireComponent(typeof(JCS_TransformTweener))]
    [RequireComponent(typeof(JCS_AdjustTimeTrigger))]
    public class JCS_SimplePathAction
        : MonoBehaviour
    {
        /* Variables */

        private JCS_TransformTweener mTransformTweener = null;
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

        /* Setter & Getter */

        public List<Transform> Points { get { return this.mPoints; } }
        public bool Random { get { return this.mRandom; } set { this.mRandom = value; } }

        /* Functions */

        private void Awake()
        {
            this.mTransformTweener = this.GetComponent<JCS_TransformTweener>();
            this.mAdjustTimerTrigger = this.GetComponent<JCS_AdjustTimeTrigger>();

            mAdjustTimerTrigger.actions = DoPath;
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

            mTransformTweener.DoTween(mPoints[mTargetPointIndex].position);
        }
    }
}
