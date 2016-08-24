/**
 * $File: JCS_DestinationDestroy.cs $
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
    /// While hit the destination destroy it
    /// </summary>
    [RequireComponent(typeof(JCS_AlphaObject))]
    public class JCS_DestinationDestroy
        : JCS_UnityObject
    {

        //----------------------
        // Public Variables
        public enum FadeType
        {
            IN,
            OUT
        }

        //----------------------
        // Private Variables

        [Header("** Runtime Variables **")]
        [SerializeField] private Transform mTargetTransform = null;
        [Tooltip("Accept range to destroy this object.(circle)")]
        [SerializeField] private float mDestroyDistance = 0.3f;

        [Header("** Fade Effect **")]
        [SerializeField] private bool mFadeEffect = true;
        [SerializeField] private FadeType mFadeType = FadeType.IN;
        [SerializeField] private float mFadeDistance = 500;
        private JCS_AlphaObject mAlphaObject = null;


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public void SetTargetTransform(Transform pos) { this.mTargetTransform = pos; }
        public float FadeDistance { get { return this.mFadeDistance; } set { this.mFadeDistance = value; } }
        public float DestroyDistance { get { return this.mDestroyDistance; } set { this.mDestroyDistance = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mAlphaObject = this.GetComponent<JCS_AlphaObject>();

            this.mAlphaObject.SetObjectType(this.GetObjectType());

            UpdateUnityData();
        }

        private void Update()
        {
            if (mTargetTransform == null)
            {
                JCS_GameErrors.JcsErrors(
                    "JCS_DestinationDestroy",
                     
                    "No target found...");

                return;
            }

            float currentDistance = Vector3.Distance(this.transform.position, mTargetTransform.position);

            if (mFadeEffect)
            {
                if (currentDistance <= mFadeDistance)
                {
                    float alphaDeltaDistance = mFadeDistance - mDestroyDistance;
                    if (mFadeType == FadeType.IN)
                        mAlphaObject.TargetAlpha = (currentDistance - mDestroyDistance) / alphaDeltaDistance;
                    else if (mFadeType == FadeType.OUT)
                        mAlphaObject.TargetAlpha = 1 - (currentDistance - mDestroyDistance) / alphaDeltaDistance;
                }
            }


            if (currentDistance <= mDestroyDistance)
            {
                Destroy(this.gameObject);
            }
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
