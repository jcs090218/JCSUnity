/**
 * $File: JCS_ShootAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JCSUnity
{

    [RequireComponent(typeof(JCS_SoundPoolAction))]
    public class JCS_ShootAction
        : MonoBehaviour
        , JCS_Action
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Optional Variables **")]
        [SerializeField] private JCS_2DSideScrollerPlayer mPlayer = null;

        [Header("** Initialize Variables **")]
        [SerializeField] private JCS_Bullet mBullet = null;
        [SerializeField] private Transform mSpawnPoint = null;
        [Tooltip("Offset of spawn point.")]
        [SerializeField] private Vector3 mSpanwPointOffset = Vector3.zero;

        // if this is true, meaning there are other shoot action going on.
        private bool mOverrideShoot = false;

        [Header("** Runtime Variables **")]
        [SerializeField] private KeyCode mShootKeyCode = KeyCode.None;

        [Header("** Action Settings **")]
        [SerializeField] private float mTimeBeforeShoot = 0;
        private bool mAction = false;
        [SerializeField] private float mTimeDelayAfterShoot = 0;
        private bool mAfterDelay = false;
        private float mActionTimer = 0;

        [Header("** Auto Shoot Settings **")]
        [Tooltip("Shoot the bullet depend on the delay time.")]
        [SerializeField] private bool mAutoShoot = false;
        [SerializeField] private bool mDestroyWhenOutOfScreen = true;
        [SerializeField] private bool mCanShoot = true;
        [Tooltip("The speed of the bullet we spawn.")]
        [SerializeField] private float mBulletSpeed = 20.0f;
        [Tooltip("How long it take to shoot a bullet.")]
        [SerializeField] private float mDelayTime = 1.0f;
        private float mDelayTimer = 0;

        [Header("** Deviation Effect **")]
        [Tooltip("bullet will not go through to target directly")]
        [SerializeField] private bool mDeviationEffectX = false;
        [SerializeField] [Range(0, 10)] private float mDeviationRangeX = 1f;
        [SerializeField] private bool mDeviationEffectY = false;
        [SerializeField] [Range(0, 10)] private float mDeviationRangeY = 1f;
        [SerializeField] private bool mDeviationEffectZ = false;
        [SerializeField] [Range(0, 10)] private float mDeviationRangeZ = 1f;

        [Tooltip("Spawn Bullet randomly transform.")]
        [SerializeField] private bool mRandPosX = false;
        [SerializeField] [Range(0, 10)] private float mRandPosRangeX = 1f;
        [SerializeField] private bool mRandPosY = false;
        [SerializeField] [Range(0, 10)] private float mRandPosRangeY = 1f;
        [SerializeField] private bool mRandPosZ = false;
        [SerializeField] [Range(0, 10)] private float mRandPosRangeZ = 1f;

        // Audio
        [Header("** Audio Settings (plz use \"JCS_SoundPoolAction\") **")]
        [Tooltip("Sound when shoot action occurs.")]
        [SerializeField] private JCS_SoundPoolAction mRandomMultiSoundAction = null;

        // Ability
        [Header("** Ability Format **")]
        [Tooltip("How much damage apply to other objects.")]
        [SerializeField] private JCS_AbilityFormat mAbilityFormat = null;

        // Tracking and Detect Area
        [Header("** Tracking and Detect Area **")]
        [Tooltip("Will shoot to the target depends on Detect Area Action.")]
        [SerializeField] private bool mTrackSoot = false;
        [Tooltip("Physical area to detect the \"JCS_DetectAreaObject\". (tag)")]
        [SerializeField] private JCS_DetectAreaAction mDetectAreaAction = null;
        public enum TrackType
        {
            CLOSEST,
            FURTHEST
        };
        [SerializeField] private TrackType mTrackType = TrackType.CLOSEST;


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public TrackType GetTrackType() { return this.mTrackType; }
        public JCS_DetectAreaAction GetDetectAreaAction() { return this.mDetectAreaAction; }
        public bool OverrideShoot { get { return this.mOverrideShoot; } set { this.mOverrideShoot = value; } }
        public KeyCode ShootKeyCode { get { return this.mShootKeyCode; } set { this.mShootKeyCode = value; } }
        public JCS_Bullet Bullet { get { return this.mBullet; } set { this.mBullet = value; } }
        public bool AutoShoot { get { return this.mAutoShoot; } set { this.mAutoShoot = value; } }
        public bool CanShoot { get { return this.mCanShoot; } set { this.mCanShoot = value; } }
        public float BulletSpeed { get { return this.mBulletSpeed; } set { this.mBulletSpeed = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mRandomMultiSoundAction = this.GetComponent<JCS_SoundPoolAction>();

            // assign default spawn point
            if (mSpawnPoint == null)
                mSpawnPoint = this.transform;

            // try to get the ability format it own
            if (mAbilityFormat == null)
                this.mAbilityFormat = this.GetComponent<JCS_AbilityFormat>();

            // try to get the detect area action it own
            if (mDetectAreaAction == null)
                this.mDetectAreaAction = this.GetComponent<JCS_DetectAreaAction>();

            // try to get the player
            if (mPlayer == null)
                this.mPlayer = this.GetComponent<JCS_2DSideScrollerPlayer>();
        }

        private void Update()
        {
            if (mAutoShoot)
                AutoShootAction();

            if (!mOverrideShoot)
                ProcessInput();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public Transform Shoot()
        {
            bool direction = true;      // default: left

            if (this.transform.localScale.x < 0)
            {
                // facing right.
                direction = false;
            }

            return Shoot(mBulletSpeed, mSpawnPoint.position, direction);
        }
        public Transform Shoot(Vector3 pos, bool direction, int hit = 1, int index = 0, bool inSequence = false, Transform target = null)
        {
            return Shoot(mBulletSpeed, pos, direction, hit, index, inSequence, target);
        }
        public Transform Shoot(float bulletSpeed, Vector3 pos, bool direction, int hit = 1, int index = 0, bool inSequence = false, Transform target = null)
        {
            if (mPlayer != null)
            {
                if (mPlayer.CharacterMode != JCS_2DCharacterMode.NORMAL)
                    return null;
            }

            Vector3 spawnPos = pos + mSpanwPointOffset;
            spawnPos = RandTransform(spawnPos);

            JCS_Bullet bullet = (JCS_Bullet)JCS_UsefualFunctions.SpawnGameObject(mBullet, spawnPos, mSpawnPoint.rotation);

            float tempBulletSpeed = bulletSpeed;

            // default: facing left
            if (!direction)
                tempBulletSpeed = -tempBulletSpeed;

            // set bullet speed
            bullet.MoveSpeed = tempBulletSpeed;

            Transform targetTransform = null;

            // no object spawns
            if (bullet == null)
                return null;

            // Do devication Effect
            DeviationEffect(bullet.transform);

            if (bullet is JCS_2DBullet)
                bullet.GetComponent<JCS_2DGoStraightAction>().MoveSpeed = bulletSpeed;

           
            if (mTrackSoot && 
                target != null)
            {
                JCS_2DTrackAction ta = bullet.GetComponent<JCS_2DTrackAction>();
                if (ta != null)
                {
                    ta.SetTargetTransform(target);

                    // set to center
                    float newIndex = index - (hit / 2.0f);

                    // apply effect
                    ta.Index = newIndex;
                }
            }

            if (mAbilityFormat != null)
            {
                JCS_ApplyDamageTextAction adta = bullet.GetComponent<JCS_ApplyDamageTextAction>();
                if (adta != null)
                {
                    // set the Ability Format
                    adta.AbilityFormat = mAbilityFormat;
                    adta.Hit = hit;

                    // if hit equal to 0,
                    // meaning this bullet is in the sequence
                    if (inSequence)
                        adta.InSequence = true;

                    adta.TargetTransform = target;
                }
            }

            // part of the SFX
            mRandomMultiSoundAction.PlayRandomSound();

            return targetTransform;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void AutoShootAction()
        {
            if (!CanShoot)
                return;

            mDelayTimer += Time.deltaTime;

            if (mDelayTime < mDelayTimer)
            {
                Shoot();

                // reset timer
                mDelayTimer = 0;
            }
        }
        public void DeviationEffect(Transform target)
        {
            Vector3 newAngles = target.localEulerAngles;

            if (mDeviationEffectX)
            {
                float effectDegree = JCS_UsefualFunctions.JCS_FloatRange(-mDeviationRangeX, mDeviationRangeX);

                newAngles.x += effectDegree;
            }

            if (mDeviationEffectY)
            {
                float effectDegree = JCS_UsefualFunctions.JCS_FloatRange(-mDeviationRangeY, mDeviationRangeY);
                newAngles.y += effectDegree;
            }

            if (mDeviationEffectZ)
            {
                float effectDegree = JCS_UsefualFunctions.JCS_FloatRange(-mDeviationRangeZ, mDeviationRangeZ);
                newAngles.z += effectDegree;
            }

            target.localEulerAngles = newAngles;
        }
        public Vector3 RandTransform(Vector3 spawnPos)
        {
            Vector3 newPos = spawnPos;

            if (mRandPosX)
            {
                float effectRange = JCS_UsefualFunctions.JCS_FloatRange(-mRandPosRangeX, mRandPosRangeX);
                newPos.x += effectRange;
            }

            if (mRandPosY)
            {
                float effectRange = JCS_UsefualFunctions.JCS_FloatRange(-mRandPosRangeY, mRandPosRangeY);
                newPos.y += effectRange;
            }

            if (mRandPosZ)
            {
                float effectRange = JCS_UsefualFunctions.JCS_FloatRange(-mRandPosRangeZ, mRandPosRangeZ);
                newPos.z += effectRange;
            }

            return newPos;
        }

        private void ProcessInput()
        {
            if (!mAction)
            {
                if (JCS_Input.GetKey(mShootKeyCode))
                    mAction = true;
            }

            if (mAfterDelay)
            {
                mActionTimer += Time.deltaTime;

                if (mTimeDelayAfterShoot < mActionTimer)
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

                if (mTimeBeforeShoot < mActionTimer)
                {
                    // Do shooting effect
                    Shoot();

                    // start after delay timer.
                    mAfterDelay = true;

                    // reset timer for "mAfterDelay" Trigger.
                    mActionTimer = 0;
                }
            }
        }

    }
}
