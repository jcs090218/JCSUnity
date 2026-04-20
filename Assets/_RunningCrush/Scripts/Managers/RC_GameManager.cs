/**
 * $File: RC_GameManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class RC_GameManager : JCS_Manager<RC_GameManager>
{
    /* Variables */

    public int ORDER_LAYER_FOR_ALL_PLAYER = 4;

    private bool mDoIgnoreOnce = false;

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        RegisterInstance(this);
    }

    private void Start()
    {
        // spawn the player in the game
        SpawnPlayer();
    }

    private void Update()
    {
#if UNITY_EDITOR
        Test();
#endif

        if (!mDoIgnoreOnce)
        {
            if (JCS_Glob.playerm.GetPlayerList().Count == RC_Glob.games.PLAYER_IN_GAME)
            {
                // ignore player once.
                JCS_Glob.playerm.DoIgnorePlayersToEachOthers();
                JCS_Glob.playerm.AddAllPlayerToMultiTrack();
                mDoIgnoreOnce = true;
            }
        }
    }

#if UNITY_EDITOR
    private void Test()
    {
        // ..
    }
#endif

    /// <summary>
    /// Do exit the game.
    /// </summary>
    public void DoExitGame()
    {
        if (RC_Glob.uim.EXIT_PANEL == null)
        {
            Debug.Log("No exit panel assign");
            return;
        }

        RC_Glob.uim.EXIT_PANEL.Active();
    }

    /// <summary>
    /// Spawn the player at the beginning of the game.
    /// </summary>
    private void SpawnPlayer()
    {
        for (int index = 0; index < RC_Glob.games.PLAYER_IN_GAME; ++index)
        {
            if (RC_Glob.games.PLAYERS[index] == null)
            {
                Debug.LogError("Player List in RC_GameSetting are null");
                return;
            }

            var rcp = (RC_Player)JCS_Util.Instantiate(RC_Glob.games.PLAYERS[index]);

            // set control index
            rcp.controlIndex = index;

            var olo = rcp.GetComponent<JCS_OrderLayerObject>();

            if (olo != null)
            {
                JCS_Glob.dsm2d.SetObjectParentToOrderLayerByOrderLayerIndex(
                    ref olo,
                    ORDER_LAYER_FOR_ALL_PLAYER);
            }

            if (RC_Glob.games.LIQUID_MODE)
            {
                if (RC_Glob.games.GLOBAL_LIQUIDBAR != null)
                {
                    // spawn a 3d liquid bar
                    var lb = (JCS_3DLiquidBar)JCS_Util.Instantiate(RC_Glob.games.GLOBAL_LIQUIDBAR);
                    rcp.SetLiquidBar(lb);
                }
                else
                {
                    Debug.LogError("No liquid bar attach to `RC_GameSetting` and u still want to access it");
                }
            }

            // only webcam mode need the pointer.
            if (RC_Glob.games.WEBCAM_MODE)
            {
                var rcpp = (RC_PlayerPointer)JCS_Util.Instantiate(RC_Glob.games.PLAYER_POINTERS[index]);

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
                var rcrp = (RC_RevivePointer)JCS_Util.Instantiate(RC_Glob.games.PLAYER_REVIVE_POINTERS[index]);
                rcrp.SetRCPlayer(rcp);
                rcp.SetRCRevivePointer(rcrp);

                rcrp.transform.SetParent(rcp.transform);
            }
        }
    }
}
