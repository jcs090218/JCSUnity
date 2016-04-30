/**
 * $File: JCS_2DSideScrollerPlayerAudioController.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen <lkk440456@gmail.com> $
 * $Notice: $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Audio controller for player
    /// </summary>
    public class JCS_2DSideScrollerPlayerAudioController 
        : JCS_2DPlayerAudioController
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField] private AudioClip[] mJumpSound = null;
        [SerializeField] private AudioClip mWalkSound = null;

        // only attack could have multiple sound
        [SerializeField] private AudioClip[] mAttackSounds = null;


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------

        public void BasicJumpSound()
        {
            if (mJumpSound.Length < 1)
                return;

            if (mJumpSound[0] == null)
            {
                JCS_GameErrors.JcsErrors("JCS_2DPlayerAudioController", -1, "Play sound with null references...");
                return;
            }
            mJCSSoundPlayer.PlayOneShot(mJumpSound[0], JCS_GameSettings.GetSkillsSound_Volume());
        }
        public void DoubleJumpSound()
        {
            if (mJumpSound.Length < 2)
                return;

            if (mJumpSound[1] == null)
            {
                JCS_GameErrors.JcsErrors("JCS_2DPlayerAudioController", -1, "Play sound with null references...");
                return;
            }
            mJCSSoundPlayer.PlayOneShot(mJumpSound[1], JCS_GameSettings.GetSkillsSound_Volume());
        }
        public void TripleJumpSound()
        {
            if (mJumpSound.Length < 3)
                return;

            if (mJumpSound[2] == null)
            {
                JCS_GameErrors.JcsErrors("JCS_2DPlayerAudioController", -1, "Play sound with null references...");
                return;
            }
            mJCSSoundPlayer.PlayOneShot(mJumpSound[2], JCS_GameSettings.GetSkillsSound_Volume());
        }
        public void AttackSound()
        {
            if (mJCSSoundPlayer.GetAudioSource().isPlaying)
                return;

            int rand = Random.Range(0, mAttackSounds.Length);

            if (mAttackSounds[rand] == null)
            {
                JCS_GameErrors.JcsErrors("JCS_2DPlayerAudioController", -1, "Play sound with null references...");
                return;
            }
            mJCSSoundPlayer.PlayOneShot(mAttackSounds[rand], JCS_GameSettings.GetSkillsSound_Volume());
        }
        public void WalkSound()
        {
            if (mWalkSound == null)
            {
                JCS_GameErrors.JcsErrors("JCS_2DPlayerAudioController", -1, "Play sound with null references...");
                return;
            }
            mJCSSoundPlayer.PlayOneShot(mWalkSound, JCS_GameSettings.GetSkillsSound_Volume());
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
