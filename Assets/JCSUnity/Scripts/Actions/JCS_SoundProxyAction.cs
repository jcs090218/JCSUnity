/**
 * $File: JCS_SoundProxyAction.cs $
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
    /// This will help to spawn a sound and play it. 
    /// In one object layer.
    /// </summary>
    [RequireComponent(typeof(JCS_DestroyReminder))]
    public class JCS_SoundProxyAction
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField]
        private AudioClip mAudioClip = null;
        private JCS_SoundSettingType mSoundSettingType = JCS_SoundSettingType.NONE;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public void SetSoundSettingType(JCS_SoundSettingType type) { mSoundSettingType = type; }
        public void SetAudioClip(AudioClip ac) { this.mAudioClip = ac; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Start()
        {
            GameObject obj = new GameObject();
            JCS_DestroySoundEndEvent dsee = obj.AddComponent<JCS_DestroySoundEndEvent>();

#if (UNITY_EDITOR)
            obj.name = "JCS_SoundProxyAction";
#endif

            if (mAudioClip == null)
            {
                JCS_Debug.JcsErrors(
                    "JCS_SoundProxyAction", 
                      
                    "U called a proxy action but with no data in it...");
                return;
            }

            dsee.SetAudioClipAndPlayOneShot(mAudioClip, mSoundSettingType);
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
