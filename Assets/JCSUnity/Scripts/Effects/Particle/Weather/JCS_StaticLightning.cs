/**
 * $File: JCS_StaticLightning.cs $
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
    /// Lightning Splash effect.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPool))]
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_StaticLightning
    : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Tooltip("")]
        [SerializeField]
        private JCS_WhiteScreen mLightning = null;

        [Tooltip("")]
        [SerializeField] [Range(0, 100)]
        private int mPossiblity = 50;

        [Tooltip("")]
        [SerializeField] [Range(1, 5)]
        private float mRandomTime = 2.5f;

        [Tooltip("This amount of time do chance lightning")]
        [SerializeField]
        private float mLimitTime = 2.5f;
        private float mLimitTimer = 0;

        private float mRecordTime = 0;

        // Sound settings
        private JCS_SoundPool mSoundPool = null;
        private JCS_SoundPlayer mSoundPlayer = null;

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
            this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
            this.mSoundPool = this.GetComponent<JCS_SoundPool>();

            mRecordTime = this.mLimitTime;
        }

        private void Start()
        {
            mLightning = JCS_SceneManager.instance.GetJCSWhiteScreen();
        }

        private void Update()
        {
            if (mLightning == null)
            {
                JCS_Debug.LogWarning(
                    "JCS_Lightning",
                     
                    "Lightning effect without white screen invalid!");

                return;
            }

            DoEffect();
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
        private void DoEffect()
        {
            mLimitTimer += Time.deltaTime;

            if (mLimitTime > mLimitTimer)
                return;

            mLimitTimer = 0;

            int randNum = JCS_Random.Range(0, 100);

            if (randNum <= mPossiblity)
                DoLightning();

            DoRandTime();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PlayLightningSound()
        {
            AudioClip ac = this.mSoundPool.GetRandomSound();

            mSoundPlayer.PlayOneShotWhileNotPlaying(ac);
        }

        /// <summary>
        /// 
        /// </summary>
        private void DoLightning()
        {
            mLightning.FadeOut(0.5f);

            PlayLightningSound();
        }

        /// <summary>
        /// 
        /// </summary>
        private void DoRandTime()
        {
            // set it back to original time
            mLimitTime = mRecordTime;

            // add new rand time.
            mLimitTime += JCS_Random.Range(-mRandomTime, mRandomTime);
        }

    }
}
