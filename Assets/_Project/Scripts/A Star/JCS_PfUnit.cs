/**
 * $File: JCS_PfUnit.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Path finding unit.
    /// </summary>
    public class JCS_PfUnit : MonoBehaviour
    {
        /* Variables */

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_PfUnit)")]

        [Tooltip("Test this component with key.")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Default without any params active path finding.")]
        [SerializeField]
        private KeyCode mActivePathfindingDefault = KeyCode.A;

        [Tooltip("Test with position.")]
        [SerializeField]
        private KeyCode mActivePathfindingPos = KeyCode.S;

        [Tooltip("Pass in with active path finding position.")]
        [SerializeField]
        private Vector3 mParamTargetPos = new Vector3(0.0f, 0.0f, 32.2f);

        [Tooltip("Test with target transform.")]
        [SerializeField]
        private KeyCode mActivePathfindingTransform = KeyCode.D;

        [Tooltip("Pass in with active pathing finding transform key.")]
        [SerializeField]
        private Transform mParamTargetTransform = null;
#endif

        [Separator("Check Variables (JCS_PfUnit)")]

        [Tooltip("Target we want to track.")]
        [SerializeField]
        private Transform mTarget = null;

        [SerializeField]
        private Vector3[] mPath = null;

        [SerializeField]
        private int mTargetIndex = 0;

        [Separator("Runtime Variables (JCS_PfUnit)")]

        [Tooltip("Move speed.")]
        [SerializeField]
        private float mSpeed = 20.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public Transform Target { get { return mTarget; } set { mTarget = value; } }
        public float Speed { get { return mSpeed; } set { mSpeed = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        /* Functions */

#if UNITY_EDITOR
        private void Update()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(mActivePathfindingDefault))
                ActivePathfinding();
            if (JCS_Input.GetKeyDown(mActivePathfindingPos))
                ActivePathfinding(mParamTargetPos);
            if (JCS_Input.GetKeyDown(mActivePathfindingTransform))
                ActivePathfinding(mParamTargetTransform);
        }
#endif

        /// <summary>
        /// Draw out the path.
        /// </summary>
        public void OnDrawGizmos()
        {
            if (mPath != null)
            {
                for (int i = mTargetIndex; i < mPath.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(mPath[i], Vector3.one);

                    if (i == mTargetIndex)
                    {
                        Gizmos.DrawLine(transform.position, mPath[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(mPath[i - 1], mPath[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Make the path finding request and 
        /// start doing the track movement.
        /// </summary>
        public void ActivePathfinding()
        {
            ActivePathfinding(mTarget);
        }
        /// <summary>
        /// Make the path finding request and 
        /// start doing the track movement.
        /// </summary>
        /// <param name="trans"> Move to this transform. </param>
        public void ActivePathfinding(Transform trans)
        {
            ActivePathfinding(trans.position);
        }
        /// <summary>
        /// Make the path finding request and 
        /// start doing the track movement.
        /// </summary>
        /// <param name="pos"> Move to this position. </param>
        public void ActivePathfinding(Vector3 pos)
        {
            // clear the array
            mPath = null;

            // reset target index
            mTargetIndex = 0;

            // make path finding request
            JCS_PathRequestManager.RequestPath(
                transform.position,
                pos,
                OnPathFound);
        }

        /// <summary>
        /// Delay the request function call.
        /// </summary>
        /// <param name="newPath"></param>
        /// <param name="pathSuccessful"></param>
        public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                mPath = newPath;
                mTargetIndex = 0;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        /// <summary>
        /// Do follow the path.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FollowPath()
        {
            // do nothing if the length is zero
            if (mPath == null || mPath.Length == 0)
                yield break;

            Vector3 currentWaypoint = mPath[0];
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    ++mTargetIndex;

                    if (mPath == null || 
                        mTargetIndex >= mPath.Length)
                        yield break;

                    currentWaypoint = mPath[mTargetIndex];
                }

                transform.position = Vector3.MoveTowards(
                    transform.position, 
                    currentWaypoint, 
                    mSpeed * JCS_Time.ItTime(mTimeType));

                yield return null;
            }
        }
    }
}
