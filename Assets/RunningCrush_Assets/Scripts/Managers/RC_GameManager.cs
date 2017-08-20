/**
 * $File: RC_GameManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;


/// <summary>
/// 
/// </summary>
public class RC_GameManager 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables
    public static RC_GameManager instance = null;

    public int ORDER_LAYER_FOR_ALL_PLAYER = 4;

    //----------------------
    // Private Variables
    private bool mDoIgnoreOnce = false;

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
        instance = this;
    }

    private void Start()
    {
        // spawn the player in the game
        SpawnPlayer();

    }

    private void Update()
    {
#if (UNITY_EDITOR)
        Test();
#endif

        if (!mDoIgnoreOnce)
        {
            if (JCS_PlayerManager.instance.GetJCSPlayerList().Count == RC_GameSettings.instance.PLAYER_IN_GAME)
            {
                // ignore player once.
                JCS_PlayerManager.instance.DoIgnorePlayersToEachOthers();
                JCS_PlayerManager.instance.AddAllPlayerToMultiTrack();
                mDoIgnoreOnce = true;
            }
        }
    }

#if (UNITY_EDITOR)
    private void Test()
    {
        
    }
#endif

    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions

    /// <summary>
    /// Do exit the game.
    /// </summary>
    public void DoExitGame()
    {
        RC_UIManager uim = RC_UIManager.instance;

        if (uim.EXIT_PANEL == null)
        {
            JCS_Debug.Log(
                "No exit panel assign...");
            return;
        }

        uim.EXIT_PANEL.Active();
    }

    //----------------------
    // Protected Functions

    //----------------------
    // Private Functions

    /// <summary>
    /// Spawn the player at the beginning of the game.
    /// </summary>
    private void SpawnPlayer()
    {
        RC_GameSettings gs = RC_GameSettings.instance;

        for (int index = 0;
            index < RC_GameSettings.instance.PLAYER_IN_GAME;
            ++index)
        {
            if (gs.PLAYERS[index] == null)
            {
                JCS_Debug.LogError(
                    "Player List in RC_GameSetting are null...");
                return;
            }

            RC_Player rcp = (RC_Player)JCS_Utility.SpawnGameObject(gs.PLAYERS[index]);

            // set control index
            rcp.ControlIndex = index;

            JCS_OrderLayerObject jcsOlo = rcp.GetComponent<JCS_OrderLayerObject>();
            if (jcsOlo != null)
            {
                JCS_2DDynamicSceneManager jcs2ddsm = JCS_2DDynamicSceneManager.instance;
                jcs2ddsm.SetObjectParentToOrderLayerByOrderLayerIndex(
                    ref jcsOlo, 
                    ORDER_LAYER_FOR_ALL_PLAYER);
            }

            if (gs.LIQUID_MODE)
            {
                if (gs.GLOBAL_LIQUIDBAR != null)
                {
                    // spawn a 3d liquid bar
                    JCS_3DLiquidBar lb = (JCS_3DLiquidBar)JCS_Utility.SpawnGameObject(gs.GLOBAL_LIQUIDBAR);
                    rcp.SetLiquidBar(lb);
                }
                else {
                    JCS_Debug.LogError(
                        "No liquid bar attach to \"RC_GameSetting\" and u still want to access it.");
                }
            }

            // only webcam mode need the pointer.
            if (gs.WEBCAM_MODE)
            {
                RC_PlayerPointer rcpp = (RC_PlayerPointer)JCS_Utility.SpawnGameObject(gs.PLAYER_POINTERS[index]);

                // let rc pp knows the rc player.
                rcpp.SetRCPlayer(rcp);

                // let rc player know his rc player pointer
                rcp.SetRCPlayerPointer(rcpp);

                // set player to player pointer's transform.
                // so the player can follow the player
                rcpp.transform.SetParent(rcp.transform);
            }

            // create Revive Pointer
            {
                RC_RevivePointer rcrp = (RC_RevivePointer)JCS_Utility.SpawnGameObject(gs.PLAYER_REVIVE_POINTERS[index]);
                rcrp.SetRCPlayer(rcp);
                rcp.SetRCRevivePointer(rcrp);

                rcrp.transform.SetParent(rcp.transform);
            }
        }

        
    }

}
