/**
 * $File: JCS_ApplyDamageTextAction.cs $
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

    /// <summary>
    /// Action on bullet, etc.
    /// </summary>
    public class JCS_ApplyDamageTextAction
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Check Variables **")]
        [SerializeField] private bool mInSequence = false;
        [SerializeField] private int mHit = 1;
        
        [SerializeField] private int mMinDamage = 1;
        [SerializeField] private int mMaxDamage = 5;
        [SerializeField] private int mCriticalChance = 10;

        [Header("** Lock Effect **")]
        [Tooltip("Enable/Disable the effect.")]
        [SerializeField] private bool mOnlyWithTarget = false;
        [Tooltip("Target we lock on!")]
        [SerializeField] private Transform mTargetTransform = null;


        [Header("** Runtime Variables **")]
        // Ability Format
        [Tooltip("Ability decide the min and max damage possibility.")]
        [SerializeField] private JCS_AbilityFormat mAbilityFormat = null;
        // Offset
        [Tooltip("Position + this.Offset where damage text will spawn.")]
        [SerializeField] private Vector3 mDamageTextPositionOffset = Vector3.zero;

        [Header("** Random Effect **")]
        [Tooltip("Enable/Disable Random Position Effect")]
        [SerializeField] private bool mRandPos = false;
        [SerializeField]
        [Tooltip("Range will be within this negative to positive!")]
        [Range(0, 10)] private float mRandPosRange = 0;

        [Header("** Destroy Setting **")]
        [SerializeField] private bool mDestroyByThisAction = true;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public Transform TargetTransform { get { return this.mTargetTransform; } set { this.mTargetTransform = value; } }
        public int Hit { get { return this.mHit; } set { this.mHit = value; } }
        public JCS_AbilityFormat AbilityFormat { get { return this.mAbilityFormat; } set { this.mAbilityFormat = value; } }
        public bool InSequence { get { return this.mInSequence; } set { this.mInSequence = value; } }

        //========================================
        //      Unity's function
        //------------------------------

        private void OnTriggerEnter(Collider other)
        {
            if (mInSequence)
            {
                if (mDestroyByThisAction)
                {
                    if (mTargetTransform == other.transform)
                        Destroy(this.gameObject);
                }

                return;
            }

            // doing the lock on effect
            if (mOnlyWithTarget)
            {
                // if the target isn't what we want ignore than.
                if (mTargetTransform != other.transform)
                    return;
                else
                    DestroyWithAction();
            }

            JCS_Enemy enemy = other.GetComponent<JCS_Enemy>();
            if (enemy == null)
                return;

            // enemy is already dead.
            if (!enemy.CanDamage)
            {
                DestroyWithAction();
                return;
            }

            if (mAbilityFormat != null)
            {
                mMinDamage = mAbilityFormat.GetMinDamage();
                mMaxDamage = mAbilityFormat.GetMaxDamage();
                mCriticalChance = mAbilityFormat.GetCriticalChance();
            }
            else
                JCS_GameErrors.JcsReminders("JCS_ApplyDamageTextAction", -1, "You sure to not using any \"JCS_AbilityFormat\"?");

            Vector3 currentPos = enemy.transform.position + mDamageTextPositionOffset;

            if (mRandPos)
                AddRandomPosition(ref currentPos);

            enemy.ApplyDamageText(mMinDamage, mMaxDamage, currentPos, mHit, mCriticalChance);

            DestroyWithAction();
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
        private void AddRandomPosition(ref Vector3 currentPos)
        {
            float addPos;
            Vector3 newPos = currentPos;

            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.x += addPos;

            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.y += addPos;

            currentPos = newPos;
        }
        private void DestroyWithAction()
        {
            if (mDestroyByThisAction)
                Destroy(this.gameObject);
        }

    }
}
