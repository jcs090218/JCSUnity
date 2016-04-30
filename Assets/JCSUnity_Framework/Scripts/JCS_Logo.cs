/**
 * $File: JCS_Logo.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace JCSUnity
{
    [RequireComponent(typeof(JCS_AlphaObject))]
    public class JCS_Logo : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField] private float mFadeOutTime = 1.0f;
        [SerializeField] private float mFadeInTime = 1.0f;
        private JCS_AlphaObject mJCSAlphaObject = null;
        [SerializeField] private string mNextLevel = "JCS_Demo";

        // second to show logo
        [SerializeField]
        private float mDelayTime = 1.0f;
        private float mDelayTimer = 0.0f;

        private bool mCycleThrough = false;
        private bool mShowing = false;

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
            // Fade In/Out time cannot be lower than zero
            if (mFadeOutTime <= 0)
                mFadeOutTime = 1.0f;
            if (mFadeInTime <= 0)
                mFadeInTime = 1.0f;

            this.mJCSAlphaObject = this.GetComponent<JCS_AlphaObject>();
        }

        private void Start()
        {
            JCS_GameManager.instance.GAME_PAUSE = true;
            JCS_UIManager.instance.HideAllOpenDialogue();
        }

        private void Update()
        {

            // Fade out first
            if (mJCSAlphaObject.IsFadeIn() && !mCycleThrough)
            {
                mJCSAlphaObject.FadeOut(mFadeOutTime);
                mShowing = true;
            }
            // check fade in later
            else if (mJCSAlphaObject.IsFadeOut() && !mShowing)
            {
                mJCSAlphaObject.FadeIn(mFadeInTime);
                mCycleThrough = true;
            }

            if (mShowing)
            {
                mDelayTimer += Time.deltaTime;

                if (mDelayTime < mDelayTimer)
                {
                    mShowing = false;
                    mDelayTimer = 0;
                }
            }

            if (mCycleThrough && mJCSAlphaObject.IsFadeIn())
            {
                JCS_GameManager.instance.GAME_PAUSE = false;
                SceneManager.LoadScene(mNextLevel);
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
