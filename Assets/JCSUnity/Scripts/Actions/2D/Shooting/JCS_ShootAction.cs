/**
 * $File: JCS_ShootAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace JCSUnity
{
    public delegate void ShootCallback();

    // If the function return 
    //              -> true, shoot the bullet.
    //              -> false not able to shoot the bullet.
    public delegate bool CheckAbleToShoot();

    /// <summary>
    /// 
    /// </summary>
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

        [SerializeField] [Range(1, 30)]
        private int mDefaultHit = 1;

        [Tooltip("How many bullet everytime active shoot event.")]
        [SerializeField] [Range(1, 50)]
        private int mShootCount = 1;

        [Header("** Key Variables **")]
        [SerializeField] private KeyCode mShootKeyCode = KeyCode.None;
        
        [SerializeField]
        private JCS_MouseButton mMouseButton
            = JCS_MouseButton.LEFT;
        [SerializeField]
        private JCS_KeyActionType mKeyAct
            = JCS_KeyActionType.KEY;

        // call back after we do shoot action.
        private ShootCallback mShootCallback = DefualtShootCallback;

        // Check we able to shoot or not
        private CheckAbleToShoot mCheckAbleToShoot = DefualtCheckFunction;

        [Header("** Action Settings **")]
        [SerializeField] private float mTimeBeforeShoot = 0;
        private bool mAction = false;
        [SerializeField] private float mTimeDelayAfterShoot = 0;
        private bool mAfterDelay = false;
        private float mActionTimer = 0;

        [Header("** Auto Shoot Settings **")]
        [Tooltip("Shoot the bullet depend on the delay time.")]
        [SerializeField] private bool mAutoShoot = false;
        [SerializeField] private bool mCanShoot = true;
        [Tooltip("The speed of the bullet we spawn.")]
        [SerializeField] private float mBulletSpeed = 20.0f;
        [Tooltip("How long it take to shoot a bullet.")]
        [SerializeField] private float mDelayTime = 1.0f;
        private float mDelayTimer = 0;

        [Header("** Deviation Effect **")]
        [Tooltip("bullet will not go through to target directly")]
        [SerializeField] private bool mDeviationEffectX = false;
        [SerializeField] [Range(0, 359)] private float mDeviationRangeX = 1f;
        [SerializeField] private bool mDeviationEffectY = false;
        [SerializeField] [Range(0, 359)] private float mDeviationRangeY = 1f;
        [SerializeField] private bool mDeviationEffectZ = false;
        [SerializeField] [Range(0, 359)] private float mDeviationRangeZ = 1f;

        [Header("** Random Spawn Effect **")]
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
        public JCS_Bullet Bullet { get { return this.mBullet; } set { this.mBullet = value; } }
        public bool AutoShoot { get { return this.mAutoShoot; } set { this.mAutoShoot = value; } }
        public bool CanShoot { get { return this.mCanShoot; } set { this.mCanShoot = value; } }
        public float BulletSpeed { get { return this.mBulletSpeed; } set { this.mBulletSpeed = value; } }
        public Transform SpawnPoint { get { return this.mSpawnPoint; } }

        public int ShootCount { get { return this.mShootCount; } }

        public KeyCode ShootKeyCode { get { return this.mShootKeyCode; } set { this.mShootKeyCode = value; } }
        public JCS_MouseButton MouseButton { get { return this.mMouseButton; } set { this.mMouseButton = value; } }
        public JCS_KeyActionType KeyAct { get { return this.mKeyAct; } set { this.mKeyAct = value; } }

        /// <summary>
        /// Call back during shooting a bullet.
        /// </summary>
        /// <param name="func"> function to set. </param>
        public void SetShootCallback(ShootCallback func) { this.mShootCallback = func; }
        public ShootCallback GetShootCallback() { return this.mShootCallback; }

        /// <summary>
        /// Function check before shooting the bullet.
        /// 
        /// Function itself must return a boolean, 
        /// in order to check the shoot action.
        /// </summary>
        /// <param name="func"> functin to check able to do the shoot action. </param>
        public void SetCheckAbleToShootFunction(CheckAbleToShoot func) { this.mCheckAbleToShoot = func; }
        public CheckAbleToShoot GetCheckAbleToShootFunction() { return this.mCheckAbleToShoot; }

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JCS_Bullet Shoot()
        {
            bool direction = true;      // default: left

            if (this.transform.localScale.x < 0)
            {
                // facing right.
                direction = false;
            }

            return Shoot(mBulletSpeed, mSpawnPoint.position, direction, mDefaultHit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="direction"></param>
        /// <param name="hit"></param>
        /// <param name="index"></param>
        /// <param name="inSequence"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public JCS_Bullet Shoot(Vector3 pos, bool direction, int hit, int index = 0, bool inSequence = false, Transform target = null)
        {
            return Shoot(mBulletSpeed, pos, direction, hit, index, inSequence, target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bulletSpeed"></param>
        /// <param name="pos"></param>
        /// <param name="direction"></param>
        /// <param name="hit"></param>
        /// <param name="index"></param>
        /// <param name="inSequence"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public JCS_Bullet Shoot(float bulletSpeed, Vector3 pos, bool direction, int hit, int index = 0, bool inSequence = false, Transform target = null)
        {
            if (mPlayer != null)
            {
                if (mPlayer.CharacterState != JCS_2DCharacterState.NORMAL)
                    return null;
            }

            Vector3 spawnPos = pos + mSpanwPointOffset;
            spawnPos = RandTransform(spawnPos);

            JCS_Bullet bullet = (JCS_Bullet)JCS_Utility.SpawnGameObject(mBullet, spawnPos, mSpawnPoint.rotation);

            // no object spawns
            if (bullet == null)
                return null;

            // set the attacker.
            SetAttackerInfo(bullet);

            float tempBulletSpeed = bulletSpeed;

            // default: facing left
            if (!direction)
                tempBulletSpeed = -tempBulletSpeed;

            // set bullet speed
            bullet.MoveSpeed = tempBulletSpeed;

            // Do devication Effect
            DeviationEffect(bullet.transform);

            if (bullet is JCS_2DBullet)
                bullet.GetComponent<JCS_3DGoStraightAction>().MoveSpeed = bulletSpeed;

           
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

                    ta.OrderIndex = index;
                }
            }

            if (mAbilityFormat != null)
            {
                JCS_ApplyDamageTextToLiveObjectAction adta = bullet.GetComponent<JCS_ApplyDamageTextToLiveObjectAction>();
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

            return bullet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="direction"></param>
        /// <param name="damages"></param>
        /// <param name="index"></param>
        /// <param name="inSequence"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public JCS_Bullet Shoot(Vector3 pos, bool direction, int[] damages, int index = 0, bool inSequence = false, Transform target = null)
        {
            return Shoot(mBulletSpeed, pos, direction, damages, index, inSequence, target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bulletSpeed"></param>
        /// <param name="pos"></param>
        /// <param name="direction"></param>
        /// <param name="damages"></param>
        /// <param name="index"></param>
        /// <param name="inSequence"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public JCS_Bullet Shoot(float bulletSpeed, Vector3 pos, bool direction, int[] damages, int index = 0, bool inSequence = false, Transform target = null)
        {
            if (mPlayer != null)
            {
                if (mPlayer.CharacterState != JCS_2DCharacterState.NORMAL)
                    return null;
            }

            int hit = damages.Length;

            Vector3 spawnPos = pos + mSpanwPointOffset;
            spawnPos = RandTransform(spawnPos);

            JCS_Bullet bullet = (JCS_Bullet)JCS_Utility.SpawnGameObject(mBullet, spawnPos, mSpawnPoint.rotation);

            // no object spawns
            if (bullet == null)
                return null;

            // set the attacker.
            SetAttackerInfo(bullet);

            float tempBulletSpeed = bulletSpeed;

            // default: facing left
            if (!direction)
                tempBulletSpeed = -tempBulletSpeed;

            // set bullet speed
            bullet.MoveSpeed = tempBulletSpeed;

            // Do devication Effect
            DeviationEffect(bullet.transform);

            if (bullet is JCS_2DBullet)
                bullet.GetComponent<JCS_3DGoStraightAction>().MoveSpeed = bulletSpeed;


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
                JCS_ApplyDamageTextToLiveObjectAction adta = bullet.GetComponent<JCS_ApplyDamageTextToLiveObjectAction>();
                if (adta != null)
                {
                    // set the Ability Format
                    adta.AbilityFormat = mAbilityFormat;
                    adta.Hit = hit;

                    adta.DamageApplying = damages;

                    // if hit equal to 0,
                    // meaning this bullet is in the sequence
                    if (inSequence)
                        adta.InSequence = true;

                    adta.TargetTransform = target;
                }
            }

            // part of the SFX
            mRandomMultiSoundAction.PlayRandomSound();

            return bullet;
        }

        /// <summary>
        /// Default function for "CheckAbleToShoot" function.
        /// </summary>
        /// <returns> always true </returns>
        public static bool DefualtCheckFunction()
        {
            // defualt is always true.
            return true;
        }

        /// <summary>
        /// Defualt shoot function pointer call back
        /// </summary>
        public static void DefualtShootCallback()
        {
            // defualt shoot function pointer call back

            // do nothing.
        }

        /// <summary>
        /// Shoot the bullet with the shoot count.
        /// </summary>
        public void ShootWithShootCount()
        {
            ShootWithShootCount(this.transform.position, this.transform.eulerAngles);
        }
        public void ShootWithShootCount(Vector3 pos, Vector3 angle)
        {
            for (int count = 0;
                count < ShootCount;
                ++count)
            {
                JCS_Bullet bullet = Shoot();

                if (bullet == null)
                    break;

                bullet.transform.position = pos;
                bullet.transform.eulerAngles = angle;

                DeviationEffect(bullet.transform);
                RandTransform(bullet.transform.position);
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Do the auto shoot action algorithm.
        /// </summary>
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public void DeviationEffect(Transform target)
        {
            Vector3 newAngles = target.localEulerAngles;

            if (mDeviationEffectX)
            {
                float effectDegree = JCS_Random.Range(-mDeviationRangeX, mDeviationRangeX);

                newAngles.x += effectDegree;
            }

            if (mDeviationEffectY)
            {
                float effectDegree = JCS_Random.Range(-mDeviationRangeY, mDeviationRangeY);
                newAngles.y += effectDegree;
            }

            if (mDeviationEffectZ)
            {
                float effectDegree = JCS_Random.Range(-mDeviationRangeZ, mDeviationRangeZ);
                newAngles.z += effectDegree;
            }

            target.localEulerAngles = newAngles;
        }

        /// <summary>
        /// Do the randomize the transform position 
        /// offset algorithm.
        /// </summary>
        /// <param name="spawnPos"> position we want to apply. </param>
        /// <returns> position after applied the random offset value. </returns>
        public Vector3 RandTransform(Vector3 spawnPos)
        {
            Vector3 newPos = spawnPos;

            if (mRandPosX)
            {
                float effectRange = JCS_Random.Range(-mRandPosRangeX, mRandPosRangeX);
                newPos.x += effectRange;
            }

            if (mRandPosY)
            {
                float effectRange = JCS_Random.Range(-mRandPosRangeY, mRandPosRangeY);
                newPos.y += effectRange;
            }

            if (mRandPosZ)
            {
                float effectRange = JCS_Random.Range(-mRandPosRangeZ, mRandPosRangeZ);
                newPos.z += effectRange;
            }

            return newPos;
        }

        /// <summary>
        /// Process the user input.
        /// </summary>
        private void ProcessInput()
        {
            if (!mAction)
            {
                if (JCS_Input.GetMouseByAction(mKeyAct, mMouseButton))
                    mAction = true;

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
                    // do call back
                    mShootCallback.Invoke();


                    // check able to shoot before shoot.
                    if (mCheckAbleToShoot.Invoke())
                    {
                        // Do shooting effect
                        for (int count = 0;
                            count < mShootCount;
                            ++count)
                        {
                            Shoot();
                        }
                    }

                    // start after delay timer.
                    mAfterDelay = true;

                    // reset timer for "mAfterDelay" Trigger.
                    mActionTimer = 0;
                }
            }
        }


        /// <summary>
        /// Shorthand way to do with own transform. Base function
        /// is under this function.
        /// </summary>
        /// <param name="bullet"> bullet we need to set attacker info with </param>
        private void SetAttackerInfo(JCS_Bullet bullet)
        {
            SetAttackerInfo(bullet, this.transform);
        }

        /// <summary>
        /// Set Attacker Info, this make all the bullet knows their
        /// attacker object.
        /// 
        /// Attacker throw the star, but the star does not know anything
        /// about the attacker itself. Use this function to let them know
        /// the attacker. which i name is "attacker info".
        /// </summary>
        /// <param name="bullet"> bullet we need to set attacker info with </param>
        /// <param name="trans"> transform which is the attacker, usually itself. </param>
        private void SetAttackerInfo(JCS_Bullet bullet, Transform trans)
        {
            bullet.AttackerInfo.Attacker = trans;
        }

    }
}
