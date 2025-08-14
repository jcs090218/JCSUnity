/**
 * $File: BF_WaveHandler.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;
using MyBox;

/// <summary>
/// To simulate the wave in the scene.
/// </summary>
public class BF_WaveHandler : MonoBehaviour
{

    /* Variables */

    [Separator("Check Variables (BF_WaveHandler)")]

    [SerializeField]
    [ReadOnly]
    private int mCurrentSpawnIndex = 0;

    [Separator("Initialize Variables (BF_WaveHandler)")]

    [Tooltip(@"All the object spawn form this game object, will be 
set to thie scene layer.")]
    [SerializeField]
    private int mOrderLayer = 7;

    // Transform that spawns enemy.
    [Tooltip("Position that spawns enemies. Defualt itself transform.")]
    [SerializeField]
    private Transform mSpawnTransform = null;

    [Tooltip("Wave start spawning the enmey.")]
    [SerializeField]
    private int mStartingWave = 1;

    [Tooltip("Plz enter the size same as the M Wave In Level.")]
    [SerializeField]
    private BF_LiveObject[] mLevelEnemy = null;

    [Tooltip("Time per wave. 1 sec ~ 600 sec (10 min)")]
    [SerializeField]
    [Range(1.0f, 600.0f)]
    private float mTimePerWave = 30;

    [Tooltip("How many enemy per wave.")]
    [SerializeField]
    [Range(1, 100)]
    private int mEnemyPerWave = 5;

    // Timer for per wave. Once it get to the wave time.
    // spawn the enmey.
    private float mWaveTimer = 0;

    /* Setter & Getter */

    public Transform spawnTransform { get { return mSpawnTransform; } set { mSpawnTransform = value; } }
    public int enemyPerWave { get { return mEnemyPerWave; } set { mEnemyPerWave = value; } }
    public int currentSpawnIndex { get { return mCurrentSpawnIndex; } }

    /* Functions */

    private void Awake()
    {
        // if nothing assigned then set itself 
        // as the spawn position.
        if (mSpawnTransform == null)
            mSpawnTransform = transform;
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
        if (mLevelEnemy[spawnIndex] == null)
        {
            Debug.Log("Make sure all the enemy in handler are assigned");
            return;
        }


        var gm = BF_GameManager.instance;
        var gs = BF_GameSettings.FirstInstance();

        for (int count = 0; count < mEnemyPerWave; ++count)
        {
            if (gm.MOB_CURRENT_IN_SCENE >= gs.TOTAL_MOB_IN_SCENE)
            {
                // don't spawn any more monster if there are too many monster in the scene.
                break;
            }

            // Spawn a monster.
            // add monster count in scene.
            ++gm.MOB_CURRENT_IN_SCENE;

            var bf_liveObject = (BF_LiveObject)JCS_Util.Instantiate(
                mLevelEnemy[spawnIndex],
                mSpawnTransform.position);

            // Set live object in the scene layer.
            var solo = bf_liveObject.GetComponent<JCS_OrderLayerObject>();
            JCS_2DDynamicSceneManager.FirstInstance().SetObjectParentToOrderLayerByOrderLayerIndex(ref solo, mOrderLayer);
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
            Debug.Log("Could not spawn the enemy without enemy object assign");
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
        //JCS_CollisionManager.instance.SetCollisionMode();
    }
}
