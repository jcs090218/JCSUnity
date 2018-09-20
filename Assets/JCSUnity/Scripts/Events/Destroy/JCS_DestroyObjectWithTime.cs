/**
 * $File: JCS_DestroyObjectWithTime.cs $
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
    /// Destroy the object with the time and timer.
    /// </summary>
    [RequireComponent(typeof(JCS_FadeObject))]
    public class JCS_DestroyObjectWithTime
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Runtime Variables **")]

        [SerializeField]
        private float mDestroyTime = 10.0f;

        private float mTimer = 0;
        private bool mTimesUp = false;

        [SerializeField]
        private bool mDestroyWithAlphaEffect = true;

        private JCS_FadeObject mFadeObject = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_FadeObject GetFadeObject() { return this.mFadeObject; }
        public float DestroyTime { get { return this.mDestroyTime; } set { this.mDestroyTime = value; } }
        public bool TimesUp { get { return this.mTimesUp; } set { this.mTimesUp = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mFadeObject = this.GetComponent<JCS_FadeObject>();
        }
        private void Update()
        {
            mTimer += Time.deltaTime;

            if (mDestroyWithAlphaEffect)
            {
                if (mDestroyTime - mTimer <= mFadeObject.FadeTime)
                    mFadeObject.FadeOut();
            }

            if (mDestroyTime < mTimer)
            {
                TimesUp = true;
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
