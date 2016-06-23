/**
 * $File: RC_GameManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;


public class RC_GameManager 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables
    public static RC_GameManager instance = null;

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

    
    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions
    
    //----------------------
    // Protected Functions
    
    //----------------------
    // Private Functions
    private void SpawnPlayer()
    {
        RC_GameSettings gs = RC_GameSettings.instance;

        for (int index = 0;
            index < RC_GameSettings.instance.PLAYER_IN_GAME;
            ++index)
        {
            if (gs.PLAYERS[index] == null)
            {
                JCS_GameErrors.JcsErrors(
                    "RC_GameManager",
                    -1,
                    "Player List in RC_GameSetting are null...");

                return;
            }

            RC_Player rcp = (RC_Player)JCS_UsefualFunctions.SpawnGameObject(gs.PLAYERS[index]);

            // set back to list so it could be manage.
            //gs.PLAYERS[index] = rcp;

            // set control index
            rcp.ControlIndex = index;

            JCS_OrderLayerObject jcsOlo = rcp.GetComponent<JCS_OrderLayerObject>();
            if (jcsOlo != null)
            {
                JCS_2DDynamicSceneManager jcs2ddsm = JCS_2DDynamicSceneManager.instance;
                jcs2ddsm.SetObjectParentToOrderLayerByOrderLayerIndex(ref jcsOlo, 4);
            }

            // only webcam mode need the pointer.
            if (gs.WEBCAM_MODE)
            {
                RC_PlayerPointer rcpp = (RC_PlayerPointer)JCS_UsefualFunctions.SpawnGameObject(gs.PLAYER_POINTERS[index]);

                // let rc pp knows the rc player.
                rcpp.SetRCPlayer(rcp);

                // let rc player know his rc player pointer
                rcp.SetRCPlayerPointer(rcpp);

                // set back to list so it could be manage.
                //gs.PLAYER_POINTERS[index] = rcpp;

                // set player to player pointer's transform.
                // so the player can follow the player
                rcpp.transform.SetParent(rcp.transform);
            }

            // create Revive Pointer
            {
                RC_RevivePointer rcrp = (RC_RevivePointer)JCS_UsefualFunctions.SpawnGameObject(gs.PLAYER_REVIVE_POINTERS[index]);
                rcrp.SetRCPlayer(rcp);
                rcp.SetRCRevivePointer(rcrp);

                rcrp.transform.SetParent(rcp.transform);
            }
        }

        
    }
    

}
