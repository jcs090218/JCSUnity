/**
 * $File: JCS_SlidePanel.cs $
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
    
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(JCS_IgnoreDialogueObject))]
    public class JCS_SlidePanel
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private RectTransform mRectTransform = null;

        [Header("** Runtime Variables **")]
        [SerializeField] private float mSlideFrictionX = 0.2f;
        [SerializeField] private float mSlideFrictionY = 0.2f;

        [SerializeField] private Vector3 mTargetPosition = Vector3.zero;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public float SlideFrictionX { get { return this.mSlideFrictionX; } set { this.mSlideFrictionX = value; } }
        public float SlideFrictionY { get { return this.mSlideFrictionY; } set { this.mSlideFrictionY = value; } }
        public void SetTargetPosition(Vector3 pos) { this.mTargetPosition = pos; }
        public Vector3 GetTargetPosition() { return this.mTargetPosition; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Start()
        {
            this.mRectTransform = this.GetComponent<RectTransform>();

            // apply default value
            if (mSlideFrictionX == 0)
                mSlideFrictionX = 0.2f;
            if (mSlideFrictionY == 0)
                mSlideFrictionY = 0.2f;

            this.mTargetPosition = this.mRectTransform.localPosition;
        }

        private void Update()
        {
            Vector3 newPosition = this.mRectTransform.localPosition;

            newPosition.x += (mTargetPosition.x - newPosition.x) / mSlideFrictionX * Time.deltaTime;
            newPosition.y += (mTargetPosition.y - newPosition.y) / mSlideFrictionY * Time.deltaTime;


            this.mRectTransform.localPosition = newPosition;
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
