/**
 * $File: RC_GameSettings.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;
using MyBox;

/// <summary>
/// Game settings for Running Crush example game.
/// </summary>
public class RC_GameSettings : JCS_Settings<RC_GameSettings>
{
    /* Variables */

    [Separator("Runtime Variables (RC_GameSettings)")]

    // at least 1 player in game.
    [Tooltip("How many player in the game. (Default: at least one player in game.)")]
    public int PLAYER_IN_GAME = 1;

    public bool READY_TO_START_GAME = false;

    public bool GAME_OVER = true;

    public bool WEBCAM_MODE = true;

    public bool LIQUID_MODE = true;

    [Header("- Game")]

    public RC_GameMode GAME_MODE = RC_GameMode.SINGLE_PLAYERS;

    public string LEVEL_SELECTED_NAME = "RC_Game";

    [Tooltip("Any button u need to load the correct level.")]
    public JCS_LoadSceneButton[] SCENE_BUTTONS = null;

    [Header("- Player")]

    public JCS_3DLiquidBar GLOBAL_LIQUIDBAR = null;

    public Vector3 LIQUIDBAR_OFFSET = Vector3.zero;

    public RC_Player[] PLAYERS = null;

    public RC_PlayerPointer[] PLAYER_POINTERS = null;

    public RC_RevivePointer[] PLAYER_REVIVE_POINTERS = null;

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        instance = CheckSingleton(instance, this);
    }

    private void Start()
    {
        GAME_MODE = FindGameMode(PLAYER_IN_GAME);
    }

    /// <summary>
    /// In order to let the designer do all the job needed, 
    /// set any button that will use to load the game scene
    /// in the array than call this function after scene
    /// name have been set, then it will make all the possible
    /// buttons that will load the game scene in the RC_Lobby to
    /// the correct scene name!
    /// </summary>
    public void SetCorrectSceneNameToAllButtonInScene()
    {
        foreach (JCS_LoadSceneButton btn in SCENE_BUTTONS)
        {
            if (btn == null)
            {
                JCS_Debug.LogError("You have open a space for button load in the scene, but does not assigned...");
                continue;
            }

            btn.SceneName = LEVEL_SELECTED_NAME;
        }
    }

    /// <summary>
    /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
    /// I would like to use own define to transfer the old instance
    /// to the newer instance.
    /// 
    /// Every time when unity load the scene. The script have been
    /// reset, in order not to lose the original setting.
    /// transfer the data from old instance to new instance.
    /// </summary>
    /// <param name="_old"> old instance </param>
    /// <param name="_new"> new instance </param>
    protected override void TransferData(RC_GameSettings _old, RC_GameSettings _new)
    {
        _new.PLAYER_IN_GAME = _old.PLAYER_IN_GAME;
        _new.WEBCAM_MODE = _old.WEBCAM_MODE;
    }

    private RC_GameMode FindGameMode(int players)
    {
        switch (players)
        {
            case 1:
                return RC_GameMode.SINGLE_PLAYERS;
            case 2:
                return RC_GameMode.TWO_PLAYERS;
            case 4:
                return RC_GameMode.FOUR_PLAYERS;
            case 8:
                return RC_GameMode.EIGHT_PLAYERS;
        }

        // This should not happens...
        JCS_Debug.LogError("Game mode undefined");

        return RC_GameMode.SINGLE_PLAYERS;
    }
}
