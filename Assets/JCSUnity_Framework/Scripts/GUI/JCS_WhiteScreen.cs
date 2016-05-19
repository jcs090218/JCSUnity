/**
 * $File: JCS_WhiteScreen.cs $
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

    [RequireComponent(typeof(JCS_AlphaObject))]
    public class JCS_WhiteScreen 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_AlphaObject mAO = null;
        [SerializeField] private float mFadeOutTime = 1.0f;
        [SerializeField] private float mFadeInTime = 0.2f;

        [SerializeField] private AudioClip mTakePhotoSound = null;

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
            this.mAO = this.GetComponent<JCS_AlphaObject>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                FadeIn();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                FadeOut();
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void FadeIn()
        {
            FadeIn(mFadeInTime);
        }
        public void FadeOut()
        {
            FadeOut(mFadeOutTime);
        }
        public void FadeIn(float time)
        {
            mAO.FadeIn(time);
        }
        public void FadeOut(float time)
        {
            mAO.FadeOut(time);
        }
        public bool IsFadeIn()
        {
            return mAO.IsFadeIn();
        }
        public bool IsFadeOut()
        {
            return mAO.IsFadeOut();
        }

        public void MoveToTheLastChild()
        {
            Transform parent = this.transform.parent;

            this.transform.SetParent(null);
            this.transform.SetParent(parent);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
