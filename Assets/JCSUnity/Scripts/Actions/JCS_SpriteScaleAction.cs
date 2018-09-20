/**
 * $File: JCS_SpriteScaleAction.cs $
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
    /// This will auto find the facing with correct direction.
    /// 
    /// In addition, u can use it inversely!
    /// </summary>
    public class JCS_SpriteScaleAction
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("-- X Facing --")]
        [Tooltip("Enable/Disabel the effect in X axis")]
        [SerializeField]
        private bool mFreezeX = false;
        [Tooltip("Effect Only when Direction is Horizontal")]
        [SerializeField]
        private bool mIsFacingRight = true;

        [Header("-- Y Facing --")]
        [Tooltip("Enable/Disabel the effect in Y axis")]
        [SerializeField]
        private bool mFreezeY = false;
        [Tooltip("Effect Only when Direction is Vertical")]
        [SerializeField]
        private bool mIsFacingUp = true;

        private Vector3 mLastPosition = Vector3.zero;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool IsFacingUp { get { return this.mIsFacingUp; } }
        public bool IsFacingRight { get { return this.mIsFacingRight; } }

        //========================================
        //      Unity's function
        //------------------------------

        private void LateUpdate()
        {
            Vector3 currentPos = this.transform.position;

            // if the position are the same, meaning the object
            // is idle. (have not been move)
            if (currentPos == mLastPosition)
                return;

            Vector3 newScale = this.transform.localScale;

            if (!mFreezeX)
            {
                // object going left
                if (currentPos.x < mLastPosition.x)
                {
                    if (mIsFacingRight)
                        newScale.x = JCS_Mathf.ToNegative(newScale.x);
                    else
                        newScale.x = JCS_Mathf.ToPositive(newScale.x);
                }
                // object going right
                else if (currentPos.x > mLastPosition.x)
                {
                    if (mIsFacingRight)
                        newScale.x = JCS_Mathf.ToPositive(newScale.x);
                    else
                        newScale.x = JCS_Mathf.ToNegative(newScale.x);
                }

                if (JCS_Utility.WithInRange(90, 270, this.transform.localEulerAngles.z))
                    newScale.x = JCS_Mathf.ToReverse(newScale.x);
            }

            if (!mFreezeY)
            {
                // object going down
                if (currentPos.y < mLastPosition.y)
                {
                    if (mIsFacingUp)
                        newScale.y = JCS_Mathf.ToNegative(newScale.y);
                    else
                        newScale.y = JCS_Mathf.ToPositive(newScale.y);
                }
                // object going up
                else if (currentPos.y > mLastPosition.y)
                {
                    if (mIsFacingUp)
                        newScale.y = JCS_Mathf.ToPositive(newScale.y);
                    else
                        newScale.y = JCS_Mathf.ToNegative(newScale.y);
                }

                // TODO(JenChieh): this have not test yet!!!
                if (JCS_Utility.WithInRange(90, 270, this.transform.localEulerAngles.z))
                    newScale.y = JCS_Mathf.ToReverse(newScale.y);
            }


            // update last position
            mLastPosition = currentPos;

            // apply new scale
            this.transform.localScale = newScale;
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
