﻿/**
 * $File: BF_CharacterSpawnHandler.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

/// <summary>
/// Handler where the character spawn in the level.
/// </summary>
public class BF_CharacterSpawnHandler : MonoBehaviour
{
    /* Variables */

    [SerializeField] 
    private BF_InitCharacterInGame[] mSpawnPos = null;

    [SerializeField] 
    private int mOrderLayer = 16;

    /* Setter & Getter */

    /* Functions */

    private void Start()
    {
        // Do this in start, cuz BF_GameSetting's TransferData 
        // is at Awake function.
        SpawnPlayers();

        // Update collision after spawn, 
        // so the player and enemy will ignore each other
        // if this does not work, check the setting from 
        // 'JCS_Settings' object's setting variables.
        //JCS_CollisionManager.instance.SetCollisionMode();
    }

    /// <summary>
    /// Spawn the player according to the pointed position.
    /// </summary>
    private void SpawnPlayers()
    {
        var bfgs = BF_GameSettings.instance;

        BF_Player[] bfPlayers = bfgs.CHARACTERS_IN_GAME;

        for (int index = 0; index < bfgs.CHARACTERS_IN_TEAM; ++index)
        {
            if (mSpawnPos[index] == null)
            {
                JCS_Debug.LogReminder("No Spawn position references, plz check the transform in the array");
                break;
            }

            if (bfPlayers[index] == null)
            {
                JCS_Debug.LogError("Character you want to spawn does not exist");
                break;
            }

            // Spawn the player, and get the 
            // player we just spawned, in order 
            // to set facing.
            BF_Player bfp = (BF_Player)JCS_Util.Instantiate(
                bfPlayers[index],
                mSpawnPos[index].transform.position);

            // set the starting faceing
            bfp.TurnFace(mSpawnPos[index].Facing);

            // Set player in the order layer (scene layer).
            var solo = bfp.GetComponent<JCS_OrderLayerObject>();
            JCS_2DDynamicSceneManager.instance.SetObjectParentToOrderLayerByOrderLayerIndex(ref solo, mOrderLayer);
        }
    }
}
