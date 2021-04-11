/**
 * $File: RC_GameSettings.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;
using System.IO;

/// <summary>
/// Game settings for Running Crush example game.
/// </summary>
public class RC_GameSettings
    : MonoBehaviour
{
    /* Variables */

    public static RC_GameSettings instance = null;

    [Header("** Check Variables (RC_GameSettings) **")]

    [SerializeField]
    private string mFullFilePath = "";

    [SerializeField]
    private string mFullFileName = "";

    [Header("** Runtime Variables (RC_GameSettings) **")]

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

    [Header("- Save Load")]

    public string FILE_PATH = "SavedData/";

    public string FILE_NAME = "RC_GameData";

    public static RC_GameData GAME_DATA = null;

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
        if (instance != null)
        {
            TransferData(instance, this);

            // Delete the old one
            DestroyImmediate(instance.gameObject);
        }

        instance = this;
    }

    private void Start()
    {
        // IMPORTANT: initial the path before save and load!
        InitPath();

        // only load once
        if (GAME_DATA == null)
            LoadGameData();

        GAME_MODE = FindGameMode(PLAYER_IN_GAME);

        // set load and save game data
        JCS_GameSettings.instance.SAVE_GAME_DATA_FUNC = SaveGameData;
        JCS_GameSettings.instance.LOAD_GAME_DATA_FUNC = LoadGameData;
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
    private void TransferData(RC_GameSettings _old, RC_GameSettings _new)
    {
        _new.PLAYER_IN_GAME = _old.PLAYER_IN_GAME;
        _new.WEBCAM_MODE = _old.WEBCAM_MODE;
    }

    /// <summary>
    /// Initialize the path base on the JCSUnity Framework's 
    /// format.
    /// </summary>
    private void InitPath()
    {
        var gs = JCS_GameSettings.instance;

        mFullFilePath = JCS_Path.Combine(Application.persistentDataPath, gs.DATA_PATH, FILE_PATH);
        mFullFileName = FILE_NAME + gs.DATA_EXTENSION;
    }
    private void LoadGameData()
    {
        JCS_IO.CreateDirectory(mFullFilePath);

        // if file does not exist, create the default value file!
        if (!File.Exists(mFullFilePath + mFullFileName))
        {
            CreateDefaultGameData();
            return;
        }

        // else we just load the data commonly.
        GAME_DATA = JCS_XMLGameData.LoadFromFile<RC_GameData>(mFullFilePath, mFullFileName);
    }
    /// <summary>
    /// Use only when player "First" play this game or 
    /// "Reset" the game.
    /// </summary>
    private void CreateDefaultGameData()
    {
        GAME_DATA = new RC_GameData();
        
        // Set game data's default values
        {
            GAME_DATA.Name = "";
            GAME_DATA.Gold = 1500;       // [default: 1500]

            GAME_DATA.ItemNo = null;
        }

        // save it once
        SaveGameData();
    }
    private void SaveGameData()
    {
        if (GAME_DATA == null)
        {
            JCS_Debug.LogError("Save Data without data");
            return;
        }

        GAME_DATA.Save<RC_GameData>(mFullFilePath, mFullFileName);
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
