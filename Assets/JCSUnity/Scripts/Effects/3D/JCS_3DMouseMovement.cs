/**
 * $File: JCS_3DMouseMovement.cs $
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
    /// Simulate the mouse movement in 3d games.
    /// 
    /// plz attach this script to any game object, you 
    /// want to simulate the mouse movement check.
    /// </summary>
    public class JCS_3DMouseMovement
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private Vector3 mVelocity = Vector3.zero;


        [Header("** Scroll Screen Effect (JCS_3DMouseMovement) **")]

        [Tooltip("")]
        [SerializeField] private bool mScreenScroll = false;

        [Tooltip("Distance to 4 bounds. (Top/Bottom/Right/Left)")]
        [SerializeField] [Range(0, 0.5f)]
        private float mScrollScreenRange = 0.1f;

        [Tooltip("Speeed to scroll the screen.")]
        [SerializeField] [Range(1, 75)]
        private float mScrollScreenSpeed = 2;

        [Tooltip("Scroll depth direction.")]
        [SerializeField]
        private bool mScrollDepth = true;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool ScreenScroll { get { return this.mScreenScroll; } set { this.mScreenScroll = value; } }

        //========================================
        //      Unity's function
        //------------------------------

        private void Update()
        {
            DoScreenScroll();

            // move the camera.
            this.transform.localPosition += mVelocity;
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

        /// <summary>
        /// Do the screen scroll algorithm.
        /// </summary>
        private void DoScreenScroll()
        {
            if (!mScreenScroll)
                return;

            // get mouse position range from 0 to 1
            Vector2 mouse01 = JCS_Input.MousePosition0To1();

            // get the 4 bounds.
            float rightBound = 1 - mScrollScreenRange;
            float leftBound = mScrollScreenRange;

            float topBound = 1 - mScrollScreenRange;
            float botBound = mScrollScreenRange;


            //-- check x direction. --//
            if (mouse01.x <= leftBound)     // left bound check
            {
                // if the mouse pos reach the left bound
                // minus the velocity so it will go left.
                mVelocity.x = -mScrollScreenSpeed;
            }
            else if (mouse01.x >= rightBound)       // right bound check
            {
                // if the mouse pos reach the right bound
                // plus the velocity so it will go right.
                mVelocity.x = mScrollScreenSpeed;
            }
            else
                // else apply zero, so it wont move.
                mVelocity.x = 0;


            //-- check y direction. --//
            if (mouse01.y <= botBound)  // bot bound check
            {
                
                if (mScrollDepth)
                {
                    // if the mouse pos reach the bottom bound
                    // minus the velocity so it will go backward.
                    mVelocity.z = -mScrollScreenSpeed;
                }
                else
                {
                    // if the mouse pos reach the bottom bound
                    // minus the velocity so it will go down.
                    mVelocity.y = mScrollScreenSpeed;
                }
            }
            else if (mouse01.y >= topBound)     // top bound check
            {
                
                if (mScrollDepth)
                {
                    // if the mouse pos reach the top bound
                    // plus the velocity so it will go forward.
                    mVelocity.z = mScrollScreenSpeed;
                }
                else
                {
                    // if the mouse pos reach the top bound
                    // plus the velocity so it will go top.
                    mVelocity.y = mScrollScreenSpeed;
                }
            }
            else
                // else apply zero, so it wont move.
                mVelocity.z = 0;
        }

    }
}
