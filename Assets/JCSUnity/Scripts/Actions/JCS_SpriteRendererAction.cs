/**
 * $File: JCS_SpriteRendererAction.cs $
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
    public class JCS_SpriteRendererAction
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        /* Down compatible. */
        private SpriteRenderer mSpriteRenderer = null;
        private Vector3 mLastPosition = Vector3.zero;


        [Header("** Runtime Variables (JCS_SpriteRendererAction) **")]

        [SerializeField]
        private SpriteRenderer[] mSpriteRenderers = null;

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
        private void Awake()
        {
            // try to get the sprite renderer
            this.mSpriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            Vector3 currentPos = this.transform.position;

            // if the position are the same, meaning the object
            // is idle. (have not been move)
            if (currentPos == mLastPosition)
                return;

            DoSpriteAction(mSpriteRenderer, currentPos);

            foreach (SpriteRenderer sr in mSpriteRenderers)
            {
                DoSpriteAction(sr, currentPos);
            }

            // update last position
            mLastPosition = currentPos;
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
        /// 
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="currentPos"></param>
        private void DoSpriteAction(SpriteRenderer sr, Vector3 currentPos)
        {
            if (sr == null)
                return;

            if (!mFreezeX)
            {
                // object going left
                if (currentPos.x < mLastPosition.x)
                {
                    if (mIsFacingRight)
                        sr.flipX = false;
                    else
                        sr.flipX = true;
                }
                // object going right
                else if (currentPos.x > mLastPosition.x)
                {
                    if (mIsFacingRight)
                        sr.flipX = true;
                    else
                        sr.flipX = false;
                }

                if (JCS_Utility.WithInRange(90, 270, this.transform.localEulerAngles.z))
                    sr.flipX = !sr.flipX;
            }

            if (!mFreezeY)
            {
                // object going down
                if (currentPos.y < mLastPosition.y)
                {
                    if (mIsFacingUp)
                        sr.flipY = false;
                    else
                        sr.flipY = true;
                }
                // object going up
                else if (currentPos.y > mLastPosition.y)
                {
                    if (mIsFacingUp)
                        sr.flipY = true;
                    else
                        sr.flipY = false;
                }

                // TODO(JenChieh): this have not test yet!!!
                if (JCS_Utility.WithInRange(90, 270, this.transform.localEulerAngles.z))
                    sr.flipY = !sr.flipY;
            }
        }

    }
}
