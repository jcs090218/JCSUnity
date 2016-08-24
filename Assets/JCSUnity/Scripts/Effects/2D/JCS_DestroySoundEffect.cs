/**
 * $File: JCS_DestroySoundEffect.cs $
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

    [RequireComponent(typeof(JCS_HitListEvent))]
    [RequireComponent(typeof(JCS_SoundPool))]
    public class JCS_DestroySoundEffect
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Runtime Variables **")]
        [Tooltip("Sound to display when this occurs")]
        [SerializeField] private JCS_SoundPool mRandomSoundAction = null;

        //-- Hit List
        [SerializeField] private bool mOccurDestroyWithHitList = true;
        private JCS_HitListEvent mHitList = null;

        //-- Time
        [SerializeField] private bool mOccurDestroyWithTime = false;
        private JCS_DestroyObjectWithTime mDestroyObjectWithTime = null;

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
            this.mHitList = this.GetComponent<JCS_HitListEvent>();
            this.mRandomSoundAction = this.GetComponent<JCS_SoundPool>();

            mDestroyObjectWithTime = this.GetComponent<JCS_DestroyObjectWithTime>();
        }

        private void OnDestroy()
        {
            // if is quitting the application don't spawn object,
            // or else will cause memory leak!
            if (JCS_ApplicationManager.APP_QUITTING)
                return;

            // if switching the scene, don't spawn new gameObject.
            if (JCS_SceneManager.instance.IsSwitchingScene())
                return;

            if (!mOccurDestroyWithHitList)
            {
                if (mHitList.IsHit)
                    return;
            }

            if (!mOccurDestroyWithTime)
            {
                
                if (mDestroyObjectWithTime != null)
                {
                    if (mDestroyObjectWithTime.TimesUp)
                        return;
                }
            }

            // Add Destroy Sound
            GameObject gm = new GameObject();
            JCS_DestroySoundEndEvent dse = gm.AddComponent<JCS_DestroySoundEndEvent>();
#if (UNITY_EDITOR)
            gm.name = "JCS_DestroySoundEffect";
#endif
            AudioClip ac = this.mRandomSoundAction.GetRandomSound();
            if (ac != null)
                dse.SetAudioClipAndPlayOneShot(ac, mRandomSoundAction.SoundType);
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
