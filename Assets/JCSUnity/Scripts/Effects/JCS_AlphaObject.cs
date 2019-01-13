/**
 * $File: JCS_AlphaObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace JCSUnity
{
    /// <summary>
    /// Transition with alpha channel.
    /// </summary>
    public class JCS_AlphaObject
        : JCS_UnityObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private Color mRecordColor;

        private float mAlpha = 1;


        [Header("** Runtime Variables (JCS_AlphaObject) **")]

        [Tooltip("Can only be within range: 0 ~ 1 .")]
        [SerializeField] [Range(0.0f, 1.0f)]
        private float mTargetAlpha = 1;

        [Tooltip("How fast the alpha channel changes.")]
        [SerializeField] [Range(0.1f, 5.0f)]
        private float mFadeFriction = 1;        

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public float TargetAlpha { get { return this.mTargetAlpha; } set { this.mTargetAlpha = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Update()
        {
            if (mAlpha == mTargetAlpha)
                return;

            this.mAlpha += (mTargetAlpha - mAlpha) / mFadeFriction * Time.deltaTime;

            switch (GetObjectType())
            {
                case JCS_UnityObjectType.GAME_OBJECT:
                    this.mRenderer.material.color = new Color(mRecordColor.r, mRecordColor.g, mRecordColor.b, mAlpha);
                    break;
                case JCS_UnityObjectType.UI:
                    this.mImage.color = new Color(mRecordColor.r, mRecordColor.g, mRecordColor.b, mAlpha);
                    break;
                case JCS_UnityObjectType.SPRITE:
                    this.mSpriteRenderer.color = new Color(mRecordColor.r, mRecordColor.g, mRecordColor.b, mAlpha);
                    break;
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public override void UpdateUnityData()
        {
            switch (GetObjectType())
            {
                case JCS_UnityObjectType.GAME_OBJECT:
                    this.mRenderer = this.GetComponent<Renderer>();
                    this.mRecordColor = this.transform.GetComponent<Renderer>().material.color;
                    break;
                case JCS_UnityObjectType.UI:
                    this.mImage = this.GetComponent<Image>();
                    this.mRecordColor = this.transform.GetComponent<Image>().color;
                    break;
                case JCS_UnityObjectType.SPRITE:
                    this.mSpriteRenderer = this.GetComponent<SpriteRenderer>();
                    this.mRecordColor = this.mSpriteRenderer.color;
                    break;
            }
        }
        public void FadeTo(float alpha, float friction)
        {
            mTargetAlpha = alpha;
            mFadeFriction = friction;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
