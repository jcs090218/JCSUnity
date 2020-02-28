/**
 * $File: JCS_SlideInput.cs $
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
    /// Use this to receive the slide input from the device buffer.
    /// </summary>
    public class JCS_SlideInput
        : MonoBehaviour
    {
        /* Variables */

        [Header("** Check Variables (JCS_SlideInput) **")]

        [Tooltip("Drag distance.")]
        [SerializeField]
        private Vector2 mDragDistance = Vector2.zero;

        [Tooltip("Drag displacement.")]
        [SerializeField]
        private Vector2 mDragDisplacement = Vector2.zero;

#if (UNITY_EDITOR || UNITY_STANDALONE)
        [Tooltip("Previous position.")]
        [SerializeField]
        private Vector3 mPrePos = Vector3.zero;

        [Tooltip("Is the application currently focused?")]
        [SerializeField]
        private bool mFocus = false;
#endif

        [Header("** Runtime Variables (JCS_SlideInput) **")]

        [Tooltip("Is the screen touches?")]
        [SerializeField]
        private bool mTouched = false;

        [Tooltip("Delta value changes on the screen.")]
        [SerializeField]
        private Vector2 mDeltaPos = Vector2.zero;

        private Vector2 mDragStartPosition = Vector2.zero;

        /* Setter & Getter */

        public bool Touched { get { return this.mTouched; } }
        public Vector2 DeltaPos { get { return this.mDeltaPos; } }
        public Vector2 DragDistance { get { return this.mDragDistance; } }
        public Vector2 DragDisplacement { get { return this.mDragDisplacement; } }


        /* Functions */

#if (UNITY_EDITOR || UNITY_STANDALONE)
        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                mFocus = true;
            }
            else
            {
                // Do something when not focus?
            }
        }
#endif

        private void Start()
        {
            // set to manager in order to get manage by "JCS_InputManager"
            JCS_InputManager.instance.SetJCSSlideInput(this);
        }

        private void Update()
        {
#if (UNITY_EDITOR || UNITY_STANDALONE)

            mTouched = Input.GetMouseButton(0);

            Vector3 currPos = Input.mousePosition;

            // Don't update delta pos when window just focus.
            if (mTouched && !mFocus)
                WhenTouched();
            else
                WhenUntouched();

            mPrePos = currPos;

#elif (UNITY_ANDROID || UNITY_IPHIONE || UNITY_IOS)

            // Detect Touch
            mTouched = (Input.touchCount == 1);
            if (mTouched)
                WhenTouched();
            else 
                WhenUntouched();
#endif
        }

        /// <summary>
        /// Do thing when is touched.
        /// </summary>
        private void WhenTouched()
        {
            Vector3 currPos = Input.mousePosition;

            if (mDeltaPos == Vector2.zero)
                this.mDragStartPosition = currPos;
            else
            {
                Vector2 dragEndPosition = currPos;

                this.mDragDistance.x = JCS_Mathf.DistanceOfUnitVector(mDragStartPosition.x, dragEndPosition.x);
                this.mDragDistance.y = JCS_Mathf.DistanceOfUnitVector(mDragStartPosition.y, dragEndPosition.y);

                this.mDragDisplacement.x = mDragDistance.x * JCS_Mathf.GetSign(mDeltaPos.x);
                this.mDragDisplacement.y = mDragDistance.y * JCS_Mathf.GetSign(mDeltaPos.y);
            }

#if (UNITY_EDITOR || UNITY_STANDALONE)
            mDeltaPos = currPos - mPrePos;
#elif (UNITY_ANDROID || UNITY_IPHIONE || UNITY_IOS)
            mDeltaPos = Input.GetTouch(0).deltaPosition;
#endif
        }

        /// <summary>
        /// Do thing when is not touched.
        /// </summary>
        private void WhenUntouched()
        {
            mDragDistance = Vector2.zero;
            mDragDisplacement = Vector2.zero;

            mDeltaPos = Vector2.zero;

#if (UNITY_EDITOR || UNITY_STANDALONE)
            // If focus, ignore one frame.
            mFocus = false;
#endif
        }
    }
}
