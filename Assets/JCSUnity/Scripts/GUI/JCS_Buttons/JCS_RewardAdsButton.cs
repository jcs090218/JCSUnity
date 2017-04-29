#if (UNITY_ANDRIOD || UNITY_IOS)
/**
 * $File: JCS_RewardAdsButton.cs $
 * $Date: 2017-04-28 21:59:46 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


namespace JCSUnity
{

    /// <summary>
    /// Reward type of Ads button.
    /// </summary>
    public class JCS_RewardAdsButton
        : JCS_Button
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables


        [Header("** Runtime Variables (JCS_RewardAdsButton) **")]

        [Tooltip("Time to delay the ads shows.")]
        [SerializeField]
        private float mDelayTime = 2.5f;

        // call back function.
        private Action<ShowResult> mRewardCallback = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public void SetRewardCallback(Action<ShowResult> func) { this.mRewardCallback = func; }

        //========================================
        //      Unity's function
        //------------------------------

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Default function to call this, so we dont have to
        /// search the function depends on name.
        /// 
        /// * Good for organize code and game data file in Unity.
        /// </summary>
        public override void JCS_ButtonClick()
        {
            base.JCS_ButtonClick();

            if (mRewardCallback == null)
            {
                JCS_Debug.JcsWarnings(
                    this, 
                    "Active default reward function, please fill the reward callback!");
                return;
            }

            // start reward video.
            JCS_AdvertisementManager.instance.StartDelayRewardVideo(
                mDelayTime,
                mRewardCallback);
        }

#if (UNITY_EDITOR)
        /// <summary>
        /// Reward example function. Your function should be 
        /// similar to this.
        /// </summary>
        /// <param name="result"> result for the reward video? </param>
        private void OnRewardAdWatched(ShowResult result)
        {
            if (result == ShowResult.Failed)
            {
                JCS_Debug.JcsLog(this, "Reward Ads video get Failed.");
            }
            else if (result == ShowResult.Finished)
            {
                JCS_Debug.JcsLog(this, "Reward Ads video get Finished.");
            }
            else if (result == ShowResult.Skipped)
            {
                JCS_Debug.JcsLog(this, "Reward Ads video get Skipped.");
            }
        }
#endif

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
#endif
