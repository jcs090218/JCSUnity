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
        [SerializeField] private JCS_WhiteScreen mLightning = null;

        [SerializeField]
        [Range(0, 100)] private int mPossiblity = 50;

        [SerializeField]
        [Range(1, 5)] private float mRandomTime = 2.5f;

        // this amount of time do chance lightning
        [SerializeField] private float mLimitTime = 2.5f;
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
                JCS_GameErrors.JcsWarnings(
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
        private void DoEffect()
        {
            mLimitTimer += Time.deltaTime;

            if (mLimitTime > mLimitTimer)
                return;

            mLimitTimer = 0;

            int randNum = JCS_Utility.JCS_IntRange(0, 100);

            if (randNum <= mPossiblity)
                DoLightning();

            DoRandTime();
        }
        private void PlayLightningSound()
        {
            AudioClip ac = this.mSoundPool.GetRandomSound();

            mSoundPlayer.PlayOneShotWhileNotPlaying(ac);
        }
        private void DoLightning()
        {
            mLightning.FadeOut(0.5f);

            PlayLightningSound();
        }
        private void DoRandTime()
        {
            // set it back to original time
            mLimitTime = mRecordTime;

            // add new rand time.
            mLimitTime += JCS_Utility.JCS_FloatRange(-mRandomTime, mRandomTime);
        }

    }
}
