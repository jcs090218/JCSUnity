/**
 * $File: JCS_BasicWaveSpawner.cs $
 * $Date: 2017-03-04 23:49:16 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{

    /// <summary>
    /// Do the basic wave spawn.
    /// </summary>
    public class JCS_BasicWaveSpawner
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Runtime Variables (JCS_BasicWaveSpawner) **")]

        [Tooltip("Active this component?")]
        [SerializeField]
        private bool mActive = false;

        [Tooltip("List of spawn object's transform.")]
        [SerializeField]
        private List<Transform> mSpawnList = null;


        [Header("- Spawn Settings (JCS_BasicWaveSpawner)")]

        [Tooltip("Time to spawn a object.")]
        [SerializeField] [Range(0, 60)]
        private float mSpawnTime = 0;

        [Tooltip("Randomize the spawn time a bit.")]
        [SerializeField] [Range(0, 10)]
        private float mRandomizeSpawnTime = 0;

        private float mSpawnTimer = 0;

        // real spawn time to compare with mSpawnTimer
        private float mRealSpawnTime = 0;

        // check if do the spawn?
        private bool mSpawned = false;


        [Header("- Randomize Position (JCS_BasicWaveSpawner)")]

        [Tooltip("Spawn the item random position, in x-axis.")]
        [SerializeField]
        private bool mRandPosX = false;
        [Tooltip("Randomize value in x-axis.")]
        [SerializeField] [Range(0, 10)]
        private float mRandPosRangeX = 1f;

        [Tooltip("Spawn the item random position, in y-axis.")]
        [SerializeField]
        private bool mRandPosY = false;
        [Tooltip("Randomize value in y-axis.")]
        [SerializeField] [Range(0, 10)]
        private float mRandPosRangeY = 1f;

        [Tooltip("Spawn the item random position, in z-axis.")]
        [SerializeField]
        private bool mRandPosZ = false;
        [Tooltip("Randomize value in z-axis.")]
        [SerializeField] [Range(0, 10)]
        private float mRandPosRangeZ = 1f;


        [Header("- Randomize Rotation (JCS_BasicWaveSpawner)")]

        [Tooltip("Randomize the rotation in x-axis?")]
        [SerializeField]
        private bool mRandRotationX = false;
        [Tooltip("Random rotation in range in x-axis.")]
        [SerializeField] [Range(0, 360)]
        private float mRandRotRangeX = 0;

        [Tooltip("Randomize the rotation in y-axis?")]
        [SerializeField]
        private bool mRandRotationY = false;
        [Tooltip("Random rotation in range in y-axis.")]
        [SerializeField] [Range(0, 360)]
        private float mRandRotRangeY = 0;

        [Tooltip("Randomize the rotation in z-axis?")]
        [SerializeField]
        private bool mRandRotationZ = false;
        [Tooltip("Random rotation in range in z-axis.")]
        [SerializeField] [Range(0, 360)]
        private float mRandRotRangeZ = 0;


        [Header("- Randomize Scale (JCS_BasicWaveSpawner)")]

        [Tooltip("Randomize the scale in x-axis?")]
        [SerializeField]
        private bool mRandScaleX = false;
        [Tooltip("Random scale in range in x-axis.")]
        [SerializeField] [Range(0, 10)]
        private float mRandScaleRangeX = 0;

        [Tooltip("Randomize the scale in y-axis?")]
        [SerializeField]
        private bool mRandScaleY = false;
        [Tooltip("Random scale in range in y-axis.")]
        [SerializeField] [Range(0, 10)]
        private float mRandScaleRangeY = 0;

        [Tooltip("Randomize the scale in z-axis?")]
        [SerializeField]
        private bool mRandScaleZ = false;
        [Tooltip("Random scale in range in z-axis.")]
        [SerializeField] [Range(0, 10)]
        private float mRandScaleRangeZ = 0;


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool Active { get { return this.mActive; } set { this.mActive = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            // resize the spawn list.
            ResizeSpawnList();
        }

        private void Update()
        {
            // if not active, return.
            if (!mActive)
                return;

            // run the spawned algorithm
            DoSpawnRandomTransform();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Spawn a random transform.
        /// </summary>
        public void SpawnATransform()
        {
            int spawnIndex = JCS_Utility.JCS_IntRange(0, mSpawnList.Count);

            // check null ref.
            if (mSpawnList[spawnIndex] == null)
            {
                JCS_Debug.JcsLog(
                    this, "Cannot spawn a null reference. Plz check the spawn list if there are transform attach or empty slot.");
                return;
            }

            // spawn a object
            Transform objSpawned = (Transform)JCS_Utility.SpawnGameObject(
                mSpawnList[spawnIndex], 
                this.transform.position);


            // randomize the position a bit.
            Vector3 randPos = JCS_Utility.ApplyRandVector3(
                // use the current spawner position.
                objSpawned.transform.position,
                new Vector3(mRandPosRangeX, mRandPosRangeY, mRandPosRangeZ),
                new JCS_Bool3(mRandPosX, mRandPosY, mRandPosZ));

            // randomize the rotation a bit.
            Vector3 randRot = JCS_Utility.ApplyRandVector3(
                // use the current spawner position.
                objSpawned.transform.eulerAngles,
                new Vector3(mRandRotRangeX, mRandRotRangeY, mRandRotRangeZ),
                new JCS_Bool3(mRandRotationX, mRandRotationY, mRandRotationZ));

            // randomize the rotation a bit.
            Vector3 randScale = JCS_Utility.ApplyRandVector3(
                // use the current spawner position.
                objSpawned.transform.localScale,
                new Vector3(mRandScaleRangeX, mRandScaleRangeY, mRandScaleRangeZ),
                new JCS_Bool3(mRandScaleX, mRandScaleY, mRandScaleZ));

            objSpawned.transform.position = randPos;
            objSpawned.transform.eulerAngles = randRot;
            objSpawned.transform.localScale = randScale;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Spawn a random item.
        /// </summary>
        private void DoSpawnRandomTransform()
        {
            // reset time zone everytime if spawned
            if (mSpawned)
                ResetTimeZone();

            mSpawnTimer += Time.deltaTime;

            if (mSpawnTimer < mRealSpawnTime)
                return;

            // turn on the trigger so it will reset time zone.
            mSpawned = true;

            SpawnATransform();
        }

        /// <summary>
        /// Resize the spawn list.
        /// </summary>
        private void ResizeSpawnList()
        {
            for (int index = 0;
                index < mSpawnList.Count;
                ++index)
            {
                // remove everythin that are empty or null reference.
                if (mSpawnList[index] == null)
                    mSpawnList.RemoveAt(index);
            }
        }

        /// <summary>
        /// Re-calculate the real time zone.
        /// </summary>
        private void ResetTimeZone()
        {
            float adjustTime = JCS_Utility.JCS_FloatRange(-mRandomizeSpawnTime, mRandomizeSpawnTime);
            mRealSpawnTime = mSpawnTime + adjustTime;

            mSpawned = false;
            mSpawnTimer = 0;
        }
    }
}
