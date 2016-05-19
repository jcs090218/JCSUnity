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
        [Header("** Initialize Variables **")]
        [SerializeField] private JCS_Bullet mBullet = null;
        [SerializeField] private Transform mSpawnPoint = null;

        [Header("** Runtime Variables **")]
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
        [SerializeField] private bool mDeviationEffect = false;

        // Audio
        [Header("** Audio Settings (plz use \"JCS_SoundPoolAction\") **")]
        [SerializeField] private JCS_SoundPoolAction mRandomMultiSoundAction = null;


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
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
        }

        private void Update()
        {
            if (mAutoShoot)
                AutoShootAction();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void Shoot()
        {
            Shoot(mBulletSpeed);
        }
        public void Shoot(float bulletSpeed)
        {
            JCS_Bullet bullet = (JCS_Bullet)JCS_UsefualFunctions.SpawnGameObject(mBullet, mSpawnPoint.position, mSpawnPoint.rotation);

            // no object spawns
            if (bullet == null)
                return;

            if (bullet is JCS_2DBullet)
                bullet.GetComponent<JCS_2DGoStraightAction>().MoveSpeed = bulletSpeed;

            // part of the SFX
            mRandomMultiSoundAction.PlayRandomSound();
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

    }
}
