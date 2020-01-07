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

#if (UNITY_EDITOR || UNITY_STANDALONE)
        [Header("** Check Variables (JCS_SlideInput) **")]

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


        /* Setter & Getter */

        public bool Touched { get { return this.mTouched; } }
        public Vector2 DeltaPos { get { return this.mDeltaPos; } }


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
            {
                mDeltaPos = currPos - mPrePos;
            }
            else
            {
                mDeltaPos = Vector2.zero;

                // If focus, ignore one frame.
                mFocus = false;
            }

            mPrePos = currPos;

#elif (UNITY_ANDROID || UNITY_IPHIONE || UNITY_IOS)

            // Detect Touch
            mTouched = (Input.touchCount == 1);
            if (mTouched) {
                mDeltaPos = Input.GetTouch(0).deltaPosition;
            }

#endif
        }
    }
}
