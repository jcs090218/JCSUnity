/**
 * $File: JCS_2DSkills.cs $
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
    /// Do not use add this script directly to Unity.
    /// This is for Scripter only file.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public abstract class JCS_2DSkills
        : MonoBehaviour
    {
        [Header("** JCS_2DSkills Variables (JCS_2DSkills) **")]

        /// <summary>
        /// While by pressing this key active skill
        /// </summary>
        [SerializeField]
        protected KeyCode mKeyCode = KeyCode.None;


        [SerializeField]
        protected RuntimeAnimatorController mMainAnim1 = null;
        [SerializeField]
        protected RuntimeAnimatorController mMainAnim2 = null;
        [SerializeField]
        protected Vector3 mAnimPosOffset1 = Vector3.zero;
        [SerializeField]
        protected Vector3 mAnimPosOffset2 = Vector3.zero;
        [SerializeField]
        protected int mOrderLayer = 15;

        /// <summary>
        /// animate action type displayed in the skills.
        /// </summary>
        [SerializeField]
        protected JCS_2DAnimActionType mAnimType 
            = JCS_2DAnimActionType.DESTROY;

        [Header("** Spawn Settings (JCS_2DSkills) **")]
        [SerializeField]
        protected bool mSamePosition = true;
        [SerializeField]
        protected bool mSameRotation = true;
        [SerializeField]
        protected bool mSameScale = true;

        [Header("** Runtime Settings (JCS_2DSkills) **")]
        [SerializeField]
        protected bool mStayWithActiveTarget = true;

        [Tooltip("Time before activate the action.")]
        [SerializeField] protected float mTimeToActive = 0.0f;
        protected bool mAction = false;           // trigger action.
        [Tooltip("Delay Time after activate this skills.")]
        [SerializeField] protected float mActiveTime = 0.0f;
        protected bool mAfterDelay = false;       // trigger after delay.
        protected float mActionTimer = 0;

        [Header("** Sound Settings **")]
        protected JCS_SoundPlayer mSoundPlayer = null;
        [Tooltip("Sound when u use the skill.")]
        [SerializeField] protected AudioClip mUseSound = null;
        [Tooltip("Sound when the skill hit the object.")]
        [SerializeField] protected AudioClip mHitSound = null;


        protected virtual void Awake()
        {
            mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
        }

        protected virtual void Update()
        {
            ProcessInput();
        }


        /// <summary>
        /// Call this function to active the skill u want.
        /// </summary>
        public abstract void ActiveSkill();
        
        /// <summary>
        /// Handle skill prevent the skill go to fast.
        /// </summary>
        private void ProcessInput()
        {
            if (!mAction)
            {
                if (JCS_Input.GetKey(mKeyCode))
                    mAction = true;
            }

            if (mAfterDelay)
            {
                mActionTimer += Time.deltaTime;

                if (mActiveTime < mActionTimer)
                {
                    // reset timer
                    mActionTimer = 0;

                    // can do the next shoot
                    mAction = false;

                    // exit delay process
                    mAfterDelay = false;
                }
            }

            if (mAction && !mAfterDelay)
            {
                mActionTimer += Time.deltaTime;

                if (mTimeToActive < mActionTimer)
                {
                    // Do shooting effect
                    ActiveSkill();

                    // start after delay timer.
                    mAfterDelay = true;

                    // reset timer for "mAfterDelay" Trigger.
                    mActionTimer = 0;
                }
            }
        }

        protected void SpawnMainAnim1()
        {
            if (mMainAnim1 == null)
            {
                JCS_Debug.LogReminders(
                    "Assigning active skill action without animation is not allowed.");

                return;
            }

            GameObject obj = JCS_Utility.SpawnAnimateObject(mMainAnim1, mOrderLayer);

            if (mSamePosition)
                obj.transform.position = this.transform.position + mAnimPosOffset1;
            if (mSameRotation)
                obj.transform.rotation = this.transform.rotation;
            if (mSameScale)
                obj.transform.localScale = this.transform.localScale;

            // if stay with player, simple set the position to
            // same position and set to child so it will follows
            // the active target!
            if (mStayWithActiveTarget)
                obj.transform.SetParent(this.transform);


            // add anim death event,
            // so when animation ends destroy itself.
            obj.AddComponent<JCS_DestroyAnimEndEvent>();
        }

        protected void SpawnMainAnim2()
        {
            if (mMainAnim2 == null)
            {
                JCS_Debug.LogReminders(
                    "Assigning active skill action without animation is not allowed.");

                return;
            }

            GameObject obj = JCS_Utility.SpawnAnimateObject(mMainAnim2, mOrderLayer + 1);

            if (mSamePosition)
                obj.transform.position = this.transform.position + mAnimPosOffset2;
            if (mSameRotation)
                obj.transform.rotation = this.transform.rotation;
            if (mSameScale)
                obj.transform.localScale = this.transform.localScale;

            // if stay with player, simple set the position to
            // same position and set to child so it will follows
            // the active target!
            if (mStayWithActiveTarget)
                obj.transform.SetParent(this.transform);


            // add anim death event,
            // so when animation ends destroy itself.
            obj.AddComponent<JCS_DestroyAnimEndEvent>();
        }

    }
}
