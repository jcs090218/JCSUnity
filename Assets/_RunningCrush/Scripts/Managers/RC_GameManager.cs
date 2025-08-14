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
            var pm = JCS_PlayerManager.FirstInstance();

            if (pm.GetPlayerList().Count == RC_GameSettings.FirstInstance().PLAYER_IN_GAME)
            {
                // ignore player once.
                pm.DoIgnorePlayersToEachOthers();
                pm.AddAllPlayerToMultiTrack();
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
        var uim = RC_UIManager.FirstInstance();

        if (uim.EXIT_PANEL == null)
        {
            Debug.Log("No exit panel assign");
            return;
        }

        uim.EXIT_PANEL.Active();
    }

    /// <summary>
    /// Spawn the player at the beginning of the game.
    /// </summary>
    private void SpawnPlayer()
    {
        var gs = RC_GameSettings.FirstInstance();

        for (int index = 0; index < gs.PLAYER_IN_GAME; ++index)
        {
            if (gs.PLAYERS[index] == null)
            {
                Debug.LogError("Player List in RC_GameSetting are null");
                return;
            }

            var rcp = (RC_Player)JCS_Util.Instantiate(gs.PLAYERS[index]);

            // set control index
            rcp.controlIndex = index;

            var olo = rcp.GetComponent<JCS_OrderLayerObject>();
            if (olo != null)
            {
                var dsm = JCS_2DDynamicSceneManager.FirstInstance();
                dsm.SetObjectParentToOrderLayerByOrderLayerIndex(
                    ref olo,
                    ORDER_LAYER_FOR_ALL_PLAYER);
            }

            if (gs.LIQUID_MODE)
            {
                if (gs.GLOBAL_LIQUIDBAR != null)
                {
                    // spawn a 3d liquid bar
                    var lb = (JCS_3DLiquidBar)JCS_Util.Instantiate(gs.GLOBAL_LIQUIDBAR);
                    rcp.SetLiquidBar(lb);
                }
                else
                {
                    Debug.LogError("No liquid bar attach to `RC_GameSetting` and u still want to access it");
                }
            }

            // only webcam mode need the pointer.
            if (gs.WEBCAM_MODE)
            {
                var rcpp = (RC_PlayerPointer)JCS_Util.Instantiate(gs.PLAYER_POINTERS[index]);

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
                var rcrp = (RC_RevivePointer)JCS_Util.Instantiate(gs.PLAYER_REVIVE_POINTERS[index]);
                rcrp.SetRCPlayer(rcp);
                rcp.SetRCRevivePointer(rcrp);

                rcrp.transform.SetParent(rcp.transform);
            }
        }
    }
}
