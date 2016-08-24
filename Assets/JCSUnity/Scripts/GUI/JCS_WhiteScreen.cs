/**
 * $File: JCS_WhiteScreen.cs $
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

    [RequireComponent(typeof(JCS_FadeObject))]
    public class JCS_WhiteScreen 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_FadeObject mAO = null;
        [SerializeField] private float mFadeOutTime = 1.0f;
        [SerializeField] private float mFadeInTime = 0.2f;

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
            this.mAO = this.GetComponent<JCS_FadeObject>();

            mAO.LocalAlpha = 0;
        }

#if (UNITY_EDITOR)
        private void Update()
        {
            Test();
        }

        private void Test()
        {
            if (JCS_Input.GetKeyDown(KeyCode.U))
                FadeOut();
        }
#endif

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
            JCS_Utility.MoveToTheLastChild(this.transform);
        }
        public void FadeOut(float time)
        {
            mAO.FadeOut(time);
            JCS_Utility.MoveToTheLastChild(this.transform);
        }
        public bool IsFadeIn()
        {
            return mAO.IsFadeIn();
        }
        public bool IsFadeOut()
        {
            return mAO.IsFadeOut();
        }


        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
