/**
 * $File: JCS_SlideInput.cs $
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

    public class JCS_SlideInput
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Runtime Variables(Check) **")]
        [SerializeField] private bool mTouched = false;
        [SerializeField] private Vector2 mDeltaPos = Vector2.zero;

        private Vector3 prePos = Vector3.zero;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool Touched { get { return this.mTouched; } }
        public Vector2 DeltaPos { get { return this.mDeltaPos; } } 

        //========================================
        //      Unity's function
        //------------------------------

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
            if (mTouched)
                mDeltaPos = currPos - prePos;
            else
                mDeltaPos = Vector2.zero;
            prePos = currPos;

#elif (UNITY_ANDROID || UNITY_IPHIONE || UNITY_IOS)

            //Detect Touch
            mTouched = (Input.touchCount == 1);
		    if (mTouched) {
			    mDeltaPos = Input.GetTouch(0).deltaPosition;
		    }

#endif
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
