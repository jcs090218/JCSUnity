/**
 * $File: JCS_DestroySoundEffect.cs $
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

        [SerializeField] private bool mActiveOnlyWhenGetHit = true;
        private JCS_HitListEvent mHitList = null;


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
        }

        private void OnDestroy()
        {
            // if is quitting the application don't spawn object,
            // or else will cause memory leak!
            if (JCS_ApplicationManager.APP_QUITTING)
                return;

            if (mActiveOnlyWhenGetHit)
            {
                if (!mHitList.IsHit)
                    return;
            }

            // Add Destroy Sound
            GameObject gm = new GameObject();
            JCS_DestroySoundEndEvent dse = gm.AddComponent<JCS_DestroySoundEndEvent>();
#if (UNITY_EDITOR)
            gm.name = "JCS_DestroySoundEffect";
#endif
            AudioClip ac = this.mRandomSoundAction.GetRandomSound();
            if (ac != null)
                dse.SetAudioClipAndPlayOneShot(ac);
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
