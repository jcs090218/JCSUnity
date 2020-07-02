/**
 * $File: BF_WaveHandler.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;

/// <summary>
/// To simulate the wave in the scene.
/// </summary>
public class BF_WaveHandler 
    : MonoBehaviour 
{

    /* Variables */

    [Header("** Check Variables **")]
    [SerializeField] private int mCurrentSpawnIndex = 0;

    [Header("** Initialize Variables **")]

    [Tooltip(@"All the object spawn form this gameobject, will be 
set to thie scene layer.")]
    [SerializeField] private int mOrderLayer = 7;

    // Transform that spawns enemy.
    [Tooltip("Position that spawns enemies. Defualt itself transform.")]
    [SerializeField] private Transform mSpawnTransform = null;

    [Tooltip("Wave start spawning the enmey.")]
    [SerializeField] private int mStartingWave = 1;

    [Tooltip("Plz enter the size same as the M Wave In Level.")]
    [SerializeField]
    private BF_LiveObject[] mLevelEnemy = null;

    [Tooltip("Time per wave. 1 sec ~ 600 sec (10 min)")]
    [SerializeField] [Range(1.0f, 600.0f)]
    private float mTimePerWave = 30;

    [Tooltip("How many enemy per wave.")]
    [SerializeField] [Range(1, 100)]
    private int mEnemyPerWave = 5;

    // Timer for per wave. Once it get to the wave time.
    // spawn the enmey.
    private float mWaveTimer = 0;

    /* Setter & Getter */

    public Transform SpawnTransform { get { return this.mSpawnTransform; } set { this.mSpawnTransform = value; } }
    public int EnemyPerWave { get { return this.mEnemyPerWave; } set { this.mEnemyPerWave = value; } }
    public int CurrentSpawnIndex { get { return this.mCurrentSpawnIndex; } }

    /* Functions */

    private void Awake()
    {
        // if nothing assigned then set itself 
        // as the spawn position.
        if (mSpawnTransform == null)
            mSpawnTransform = this.transform;
    }

    private void Update()
    {
        // if the game is over returned.
        if (BF_GameManager.instance.GAME_IS_OVER)
            return;

        DoSpawn();
    }

    /// <summary>
    /// Spawn a wave by passing in wave count.
    /// </summary>
    /// <param name="spawnIndex"> spawn wave index. </param>
    public void SpawnAWave(int spawnIndex)
    {
        // if the enemy we assign is null, will cause errors.
        if (this.mLevelEnemy[spawnIndex] == null)
        {
            JCS_Debug.LogReminder("Make sure all the enemy in handler are assigned");
            return;
        }


        for (int count = 0;
            count < mEnemyPerWave;
            ++count)
        {
            if (BF_GameManager.instance.MOB_CURRENT_IN_SCENE >= BF_GameSettings.instance.TOTAL_MOB_IN_SCENE)
            {
                // don't spawn any more monster if there are too many monster in the scene.
                break;
            }

            // Spawn a monster.
            // add monster count in scene.
            ++BF_GameManager.instance.MOB_CURRENT_IN_SCENE;

            BF_LiveObject bf_liveObject = (BF_LiveObject)JCS_Utility.SpawnGameObject(
                this.mLevelEnemy[spawnIndex],
                this.mSpawnTransform.position);

            // Set live object in the scene layer.
            JCS_OrderLayerObject jcsolo = bf_liveObject.GetComponent<JCS_OrderLayerObject>();
            JCS_2DDynamicSceneManager.instance.SetObjectParentToOrderLayerByOrderLayerIndex(ref jcsolo, mOrderLayer);
        }
    }

    /// <summary>
    /// Do the spawning, timer etc.
    /// </summary>
    private void DoSpawn()
    {
        // start the counter
        mWaveTimer += Time.deltaTime;

        if (mWaveTimer > mTimePerWave)
        {
            SpawnAWaveByLevel();

            // reset timer ready fo next wave.
            mWaveTimer = 0;
        }
    }

    /// <summary>
    /// Spawn one wave enemies base on the game's level.
    /// </summary>
    private void SpawnAWaveByLevel()
    {
        // check the length of the enemy array prevent errors.
        if (mLevelEnemy.Length == 0)
        {
            JCS_Debug.LogReminder( "Could not spawn the enemy without enemy object assign");
            return;
        }

        // 1) Check if the wave is ready to spawn.

        // get the current level
        int currentLevel = BF_GameManager.instance.CURRENT_LEVEL;

        // check if the current level reach to the starting level
        if (currentLevel < mStartingWave)
            return;

        // 2) check to see we can spawn enemies or not.

        // get spawning index. 
        // current spawning index = current level - starting level 
        int spawnIndex = (currentLevel - mStartingWave);

        // make sure the spawning index lower than 
        // the length to prevent error.
        if (mLevelEnemy.Length <= spawnIndex)
            return;

        // if spawn check successfully than record down 
        // the current spawn index.
        mCurrentSpawnIndex = spawnIndex;

        // 3) Spawn a wave.
        SpawnAWave(spawnIndex);

        // Update collision after spawn, 
        // so the player and enemy will ignore each other
        // if this does not work, check the setting from 
        // 'JCS_Settings' object's setting variables.
        JCS_CollisionManager.instance.SetCollisionMode();
    }
}
