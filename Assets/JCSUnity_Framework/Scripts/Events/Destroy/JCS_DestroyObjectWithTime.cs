/**
 * $File: JCS_DestroyObjectWithTime.cs $
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

    [RequireComponent(typeof(JCS_FadeObject))]
    public class JCS_DestroyObjectWithTime
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Runtime Variables **")]
        [SerializeField] private float mDestroyTime = 10.0f;
        private float mTimer = 0;

        [SerializeField] private bool mDestroyWithAlphaEffect = true;
        private JCS_FadeObject mAlphaObject = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_FadeObject GetAlphaObject() { return this.mAlphaObject; }
        public float DestroyTime { get { return this.mDestroyTime; } set { this.mDestroyTime = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mAlphaObject = this.GetComponent<JCS_FadeObject>();
        }
        private void Update()
        {
            if (mDestroyWithAlphaEffect)
            {
                mTimer += Time.deltaTime;

                if (mDestroyTime - mTimer <= mAlphaObject.FadeTime)
                    mAlphaObject.FadeOut();
            }

            Destroy(this.gameObject, mDestroyTime);
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
