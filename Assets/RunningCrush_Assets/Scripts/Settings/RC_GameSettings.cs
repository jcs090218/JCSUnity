/**
 * $File: RC_GameSettings.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;
using System.IO;

public class RC_GameSettings 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables
    public static RC_GameSettings instance = null;


    // at least 1 player in game.
    [Tooltip("How many player in the game. (Default: at least one player in game.)")]
    public int PLAYER_IN_GAME = 1;
    public bool READY_TO_START_GAME = false;
    public bool GAME_OVER = true;

    public bool WEBCAM_MODE = true;

    [Header("** Game Settings **")]
    public RC_GameMode GAME_MODE = RC_GameMode.SINGLE_PLAYERS;

    [Header("** Save Load Settings **")]
    public string FILE_PATH = "SavedData/";
    public string FILE_NAME = "RC_GameData";
    public static RC_GameData RC_GAME_DATA = null;

    private string mFullFilePath = "";
    private string mFullFileName = "";

    [Header("** Player Settings **")]
    public RC_Player[] PLAYERS = null;
    public RC_PlayerPointer[] PLAYER_POINTERS = null;
    public RC_RevivePointer[] PLAYER_REVIVE_POINTERS = null;


    //----------------------
    // Private Variables

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
        if (instance != null)
        {
            TransferData(instance, this);

            // Delete the old one
            DestroyImmediate(instance.gameObject);
        }

        instance = this;

        // IMPORTANT(JenChieh): initial the path
        // before save and load!
        InitPath();

        // only load once
        if (RC_GAME_DATA == null)
            LoadGameData();

        GAME_MODE = FindGameMode(PLAYER_IN_GAME);
    }

    private void Start()
    {
        // set load and save game data
        JCS_GameSettings.instance.SAVE_GAME_DATA_FUNC = SaveGameData;
        JCS_GameSettings.instance.LOAD_GAME_DATA_FUNC = LoadGameData;
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
    private void TransferData(RC_GameSettings _old, RC_GameSettings _new)
    {
        _new.PLAYER_IN_GAME = _old.PLAYER_IN_GAME;
        _new.WEBCAM_MODE = _old.WEBCAM_MODE;
    }

    private void InitPath()
    {
        mFullFilePath =
            Application.dataPath +
            JCS_GameSettings.GAME_DATA_PATH +
            FILE_PATH;

        mFullFileName = FILE_NAME +
            JCS_GameSettings.JCS_EXTENSION;
    }
    private void LoadGameData()
    {
        // if Directory does not exits, create it prevent error!
        if (!Directory.Exists(mFullFilePath))
            Directory.CreateDirectory(mFullFilePath);


        // if file does not exist, create the default value file!
        if (!File.Exists(mFullFilePath + mFullFileName))
        {
            CreateDefaultGameData();
            return;
        }

        // else we just load the data commonly.
        RC_GAME_DATA = RC_GameData.LoadFromFile(mFullFilePath, mFullFileName);
    }
    /// <summary>
    /// Use only when player "First" play this game or 
    /// "Reset" the game.
    /// </summary>
    private void CreateDefaultGameData()
    {
        RC_GAME_DATA = new RC_GameData();
        RC_GAME_DATA.Name = "";
        RC_GAME_DATA.Gold = 1500;       // [default: 1500]

        RC_GAME_DATA.ItemNo = null;

        // save it once
        SaveGameData();
    }
    private void SaveGameData()
    {
        if (RC_GAME_DATA == null)
        {
            JCS_GameErrors.JcsErrors(
                "RC_GameSetting", 
                -1, 
                "Save Data without data??? (Fatal Error)");

            return;
        }

        RC_GAME_DATA.Save(mFullFilePath, mFullFileName);
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
        JCS_GameErrors.JcsErrors(
            "RC_GameSetting",
            -1,
            "Game Mode Undefined...");

        return RC_GameMode.SINGLE_PLAYERS;
    }

}
