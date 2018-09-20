/**
 * $File: JCS_2DMoveTile.cs $
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
    /// usually Background
    /// </summary>
    public class JCS_2DMoveTile 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField] private float mMoveSpeed = 10.0f;
        private Vector3 mVelocity = Vector3.zero;
        private Vector3 mOriginPosition = Vector3.zero;
        [SerializeField] private bool mIsYAxis = false;
        [SerializeField] private float mWidth = 0.0f;
        [SerializeField] private float mHeight = 0.0f;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            SpriteRenderer sp = this.GetComponent<SpriteRenderer>();

            Vector2 spriteRect = JCS_Utility.GetSpriteRendererRectWithNoScale(sp);

            mWidth = spriteRect.x;
            mHeight = spriteRect.y;

            mOriginPosition = this.transform.position;
        }

        private void Update()
        {
            if (!mIsYAxis)
            {
                this.mVelocity.x = mMoveSpeed;

                float distance = this.transform.position.x - mOriginPosition.x;
                distance = JCS_Mathf.AbsoluteValue(distance);
                if (distance > mWidth)
                    this.transform.position = mOriginPosition;
            }
            else
            {
                this.mVelocity.y = mMoveSpeed;

                float distance = this.transform.position.y - mOriginPosition.y;
                distance = JCS_Mathf.AbsoluteValue(distance);
                if (distance > mHeight)
                    this.transform.position = mOriginPosition;
            }

            this.transform.position += mVelocity * Time.deltaTime;
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