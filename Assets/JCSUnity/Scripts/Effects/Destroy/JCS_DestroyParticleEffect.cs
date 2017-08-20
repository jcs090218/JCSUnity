/**
 * $File: JCS_DestroyParticleEffect.cs $
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
    /// When the gameobject get destroy, spawn the gameobject and 
    /// to the particle in seconds. 
    /// </summary>
    [RequireComponent(typeof(JCS_ParticleSystem))]
    [RequireComponent(typeof(JCS_HitListEvent))]
    public class JCS_DestroyParticleEffect
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_ParticleSystem mParticleSystem = null;


        [Header("** Initialize Variables (JCS_ParticleSystem) **")]

        //-- Hit List
        [Tooltip("Active this effect by what ever this object is destoryed.")]
        [SerializeField]
        private bool mActiveWhatever = false;

        [Tooltip("Active the effect by hitting the certain object.")]
        [SerializeField]
        private bool mActiveWithHitList = true;
        private JCS_HitListEvent mHitList = null;

        //-- Time
        [Tooltip("Active the effect by the destroy time.")]
        [SerializeField]
        private bool mActiveWithDestroyTime = false;
        private JCS_DestroyObjectWithTime mDestroyObjectWithTime = null;


        [Header("** Settings **")]

        [Tooltip("The same position as the destroyed game object?")]
        [SerializeField]
        private bool mSamePosition = true;
        [Tooltip("The same rotation as the destroyed game object?")]
        [SerializeField]
        private bool mSameRotation = true;
        [Tooltip("The same scale as the destroyed game object?")]
        [SerializeField]
        private bool mSameScale = true;


        [Header("** Random Effect **")]

        [Tooltip("Enable/Disable Random Position Effect")]
        [SerializeField]
        private bool mRandPos = false;
        [SerializeField]
        [Tooltip("Range will be within this negative to positive!")]
        [Range(0, 10)]
        private float mRandPosRange = 0;

        [Tooltip("Enable/Disable Random Rotation Effect")]
        [SerializeField]
        private bool mRandRot = false;
        [SerializeField]
        [Tooltip("Range will be within this negative to positive!")]
        [Range(0, 10)]
        private float mRandRotRange = 0;

        [Tooltip("Enable/Disable Random Scale Effect")]
        [SerializeField]
        private bool mRandScale = false;
        [SerializeField]
        [Tooltip("Range will be within this negative to positive!")]
        [Range(0, 10)]
        private float mRandScaleRange = 0;


        [Header("- Peformance")]

        [Tooltip(@"Destroy the particle object by time, the 
default is be 'JCS_DestroyParticleEndEvent'.")]
        [SerializeField]
        private bool mDestroyByTime = false;

        [Tooltip("How long the object get destroy.")]
        [SerializeField]
        private float mDestroyTime = 10;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool DestroyByTime { get { return this.mDestroyByTime; } set { this.mDestroyByTime = value; } }
        public float DestroyTime { get { return this.mDestroyTime; } set { this.mDestroyTime = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mParticleSystem = this.GetComponent<JCS_ParticleSystem>();
            this.mHitList = this.GetComponent<JCS_HitListEvent>();
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

            // trigger this effect?
            bool onTrigger = false;

            if (mActiveWhatever)
            {
                onTrigger = true;
            }
            // no need to check the rest.
            else
            {
                // if checking for hit list
                if (mActiveWithHitList)
                {
                    if (mHitList.IsHit)
                        onTrigger = true;
                }

                // if checking for destroy time.
                if (mActiveWithDestroyTime)
                {
                    if (mDestroyObjectWithTime != null)
                    {
                        if (mDestroyObjectWithTime.TimesUp)
                            onTrigger = true;
                    }
                    else
                    {
                        JCS_Debug.LogError(
                            "You active the destroy time but without the JCS_DestroyObjectWithTime component...");
                    }
                }
            }

            // do not trigger this effect.
            if (!onTrigger)
                return;


            GameObject gm = new GameObject();
#if (UNITY_EDITOR)
            gm.name = "JCS_DestroyParticleEffect";
#endif

            JCS_ParticleSystem jcsps = gm.AddComponent<JCS_ParticleSystem>();

            if (mSamePosition)
                jcsps.transform.position = this.transform.position;
            if (mSameRotation)
                jcsps.transform.rotation = this.transform.rotation;
            if (mSameScale)
                jcsps.transform.localScale = this.transform.localScale;

            // Random Effect
            if (mRandPos)
                AddRandomPosition(jcsps);
            if (mRandRot)
                AddRandomRotation(jcsps);
            if (mRandScale)
                AddRandomScale(jcsps);

            // copy and paste component to new one.
            {
                jcsps.Particle = mParticleSystem.Particle;
                jcsps.NumOfParticle = mParticleSystem.NumOfParticle;

                jcsps.Active = mParticleSystem.Active;
                jcsps.ActiveThread = mParticleSystem.ActiveThread;
                jcsps.OrderLayer = mParticleSystem.OrderLayer;
                jcsps.Density = mParticleSystem.Density;
                jcsps.WindSpeed = mParticleSystem.WindSpeed;

                jcsps.AlwaysTheSameScale = mParticleSystem.AlwaysTheSameScale;

                jcsps.FreezeX = mParticleSystem.FreezeX;
                jcsps.FreezeY = mParticleSystem.FreezeY;
                jcsps.FreezeZ = mParticleSystem.FreezeZ;

                jcsps.RandPosX = mParticleSystem.RandPosX;
                jcsps.RandPosY = mParticleSystem.RandPosY;
                jcsps.RandPosZ = mParticleSystem.RandPosZ;

                jcsps.RandAngleX = mParticleSystem.RandAngleX;
                jcsps.RandAngleY = mParticleSystem.RandAngleY;
                jcsps.RandAngleZ = mParticleSystem.RandAngleZ;

                jcsps.RandScaleX = mParticleSystem.RandScaleX;
                jcsps.RandScaleY = mParticleSystem.RandScaleY;
                jcsps.RandScaleZ = mParticleSystem.RandScaleZ;

                jcsps.DoShotImmediately = mParticleSystem.DoShotImmediately;

                jcsps.SetChild = mParticleSystem.SetChild;
                jcsps.SetToSamePositionWhenActive = mParticleSystem.SetToSamePositionWhenActive;
            }

            // spawn the particle immediately.
            jcsps.SpawnParticles();

            // spawn the particle.
            jcsps.PlayOneShot();


            if (mDestroyByTime)
            {
                JCS_DestroyObjectWithTime dowt = gm.AddComponent<JCS_DestroyObjectWithTime>();
                dowt.DestroyTime = mDestroyTime;
            }
            else
            {
                // add destroy event.
                gm.AddComponent<JCS_DestroyParticleEndEvent>();
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

        /// <summary>
        /// Add random value to the effect's transform's position.
        /// </summary>
        /// <param name="dae"> effect transform </param>
        private void AddRandomPosition(JCS_ParticleSystem dae)
        {
            float addPos;
            Vector3 newPos = dae.transform.position;

            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.x += addPos;

            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.y += addPos;

            // Not sure we have to include z direction?
            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.z += addPos;

            dae.transform.position = newPos;
        }

        /// <summary>
        /// Add random value to the effect's transform's rotation.
        /// </summary>
        /// <param name="dae"> effect transform </param>
        private void AddRandomRotation(JCS_ParticleSystem dae)
        {
            float addRot;
            Vector3 newRot = dae.transform.localEulerAngles;

            addRot = Random.Range(-mRandRotRange, mRandRotRange);
            newRot.x += addRot;

            addRot = Random.Range(-mRandRotRange, mRandRotRange);
            newRot.y += addRot;

            // Not sure we have to include z direction?
            addRot = Random.Range(-mRandRotRange, mRandRotRange);
            newRot.z += addRot;

            dae.transform.localEulerAngles = newRot;
        }

        /// <summary>
        /// Add random value to the effect's transform's scale.
        /// </summary>
        /// <param name="dae"> effect transform </param>
        private void AddRandomScale(JCS_ParticleSystem dae)
        {
            float addScale;
            Vector3 newScale = dae.transform.localScale;

            addScale = Random.Range(-mRandScaleRange, mRandScaleRange);
            newScale.x += addScale;

            addScale = Random.Range(-mRandScaleRange, mRandScaleRange);
            newScale.y += addScale;

            // Not sure we have to include z direction?
            addScale = Random.Range(-mRandScaleRange, mRandScaleRange);
            newScale.z += addScale;

            dae.transform.localScale = newScale;
        }

    }
}
