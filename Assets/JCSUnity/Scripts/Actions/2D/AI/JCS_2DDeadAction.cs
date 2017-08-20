/**
 * $File: JCS_2DDeadAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// Work with the "JCS_2DLiveObject" class. (Optional/Effect Class)
    /// </summary>
    [RequireComponent(typeof(JCS_2DLiveObject))]
    public class JCS_2DDeadAction
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private JCS_2DLiveObject mLiveObject = null;

        [Header("** Optional Settings (JCS_2DWalkAction) **")]

        [Tooltip("Plz fill this is there is animation going on to this game object.")]
        [SerializeField] private JCS_2DLiveObjectAnimator mLiveObjectAnimator = null;


        [Header("** Sound Settings (JCS_2DWalkAction) **")]

        [Tooltip("Play one shot when this action active.")]
        [SerializeField] private AudioClip mDieSound = null;

        // check to play the die sound.
        private bool mSoundPlayed = false;


        [Header("** Effect Setting (JCS_2DWalkAction) **")]

        [Tooltip("Disable un-necessary componenet while die.")]
        [SerializeField] private bool mDisableOtherComponentWhileDie = true;

        [Tooltip("Freeze the object while displayin dead animation.")]
        [SerializeField] private bool mFreezeWhileDie = true;


        [Tooltip("Array of componenet u want to disable after die.")]
        [SerializeField] private MonoBehaviour[] mDisableComponents = null;


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
            mLiveObject = this.GetComponent<JCS_2DLiveObject>();

            if (mLiveObjectAnimator == null)
                mLiveObjectAnimator = this.GetComponent<JCS_2DLiveObjectAnimator>();
        }

        private void Update()
        {
            // if still could damage this live object, 
            // mean this object isn't dead yet.
            if (0 < mLiveObject.HP)
                return;

            // if dies
            if (!mSoundPlayed)
            {
                // just play one time.
                JCS_SoundManager.instance.GetGlobalSoundPlayer().PlayOneShot(mDieSound);
                mSoundPlayed = true;
            }

            // do the dead animation
            if (mLiveObjectAnimator.Animator.AnimDisplayHolder != null)
                mLiveObjectAnimator.Animator.AnimDisplayHolder.StopHolding();
            mLiveObjectAnimator.DoAnimation(JCS_LiveObjectState.DIE);

            if (mFreezeWhileDie)
                mLiveObject.VelocityInfo.Freeze();

            // disable other un-necessary components
            if (mDisableOtherComponentWhileDie)
            {
                for (int index = 0;
                    index < mDisableComponents.Length;
                    ++index)
                {
                    // disable all un-necessary component
                    if (mDisableComponents[index] == null)
                        continue;

                    // disable the component
                    mDisableComponents[index].enabled = false;
                }

                // after disable it dont do it agian in next frame.
                mDisableOtherComponentWhileDie = false;
            }

            // if the animation starts, 
            // start timer and check the 
            // dead animation is end or not.
            if (mLiveObjectAnimator.IsInState(JCS_LiveObjectState.DIE))
            {
                if (mLiveObjectAnimator.Animator.CurrentAnimation.IsDonePlaying)
                {
                    // if is end destroy the object itself.
                    Destroy(this.gameObject);
                }
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
