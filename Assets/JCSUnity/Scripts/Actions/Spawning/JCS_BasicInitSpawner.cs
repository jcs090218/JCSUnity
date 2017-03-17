/**
 * $File: JCS_BasicInitSpawner.cs $
 * $Date: 2017-03-06 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{
    /// <summary>
    /// Spawn a Transform at the initialize time.
    /// </summary>
    public class JCS_BasicInitSpawner
            : MonoBehaviour
    {
        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Initialize Variables (JCS_BasicInitSpawner) **")]

        [Tooltip("List of spawn object's transform.")]
        [SerializeField]
        private List<Transform> mSpawnList = null;

        [Tooltip("How many transform you want to spawn?")]
        [SerializeField] [Range(1, 300)]
        private int mSpawnCount = 1;

        [Header("- Randomize Position (JCS_BasicWaveSpawner)")]

        [Tooltip("Spawn the item random position, in x-axis.")]
        [SerializeField]
        private bool mRandPosX = false;
        [Tooltip("Randomize value in x-axis.")]
        [SerializeField]
        [Range(0, 10)]
        private float mRandPosRangeX = 1f;

        [Tooltip("Spawn the item random position, in y-axis.")]
        [SerializeField]
        private bool mRandPosY = false;
        [Tooltip("Randomize value in y-axis.")]
        [SerializeField]
        [Range(0, 10)]
        private float mRandPosRangeY = 1f;

        [Tooltip("Spawn the item random position, in z-axis.")]
        [SerializeField]
        private bool mRandPosZ = false;
        [Tooltip("Randomize value in z-axis.")]
        [SerializeField]
        [Range(0, 10)]
        private float mRandPosRangeZ = 1f;


        [Header("- Randomize Rotation (JCS_BasicWaveSpawner)")]

        [Tooltip("Randomize the rotation in x-axis?")]
        [SerializeField]
        private bool mRandRotationX = false;
        [Tooltip("Random rotation in range in x-axis.")]
        [SerializeField]
        [Range(0, 360)]
        private float mRandRotRangeX = 0;

        [Tooltip("Randomize the rotation in y-axis?")]
        [SerializeField]
        private bool mRandRotationY = false;
        [Tooltip("Random rotation in range in y-axis.")]
        [SerializeField]
        [Range(0, 360)]
        private float mRandRotRangeY = 0;

        [Tooltip("Randomize the rotation in z-axis?")]
        [SerializeField]
        private bool mRandRotationZ = false;
        [Tooltip("Random rotation in range in z-axis.")]
        [SerializeField]
        [Range(0, 360)]
        private float mRandRotRangeZ = 0;


        [Header("- Randomize Scale (JCS_BasicWaveSpawner)")]

        [Tooltip("Randomize the scale in x-axis?")]
        [SerializeField]
        private bool mRandScaleX = false;
        [Tooltip("Random scale in range in x-axis.")]
        [SerializeField]
        [Range(0, 10)]
        private float mRandScaleRangeX = 0;

        [Tooltip("Randomize the scale in y-axis?")]
        [SerializeField]
        private bool mRandScaleY = false;
        [Tooltip("Random scale in range in y-axis.")]
        [SerializeField]
        [Range(0, 10)]
        private float mRandScaleRangeY = 0;

        [Tooltip("Randomize the scale in z-axis?")]
        [SerializeField]
        private bool mRandScaleZ = false;
        [Tooltip("Random scale in range in z-axis.")]
        [SerializeField]
        [Range(0, 10)]
        private float mRandScaleRangeZ = 0;

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
            // resize the spawn list.
            ResizeSpawnList();

            // do the spawn.
            InitSpawn();
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

        /// <summary>
        /// Main idea of this component do.
        /// </summary>
        public void InitSpawn()
        {
            for (int count = 0;
                count < mSpawnCount;
                ++count)
            {
                // spawn a transform.
                SpawnATransform();
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

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

    }
}
