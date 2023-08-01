﻿/**
 * $File: BF_GameSettings.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.IO;
using UnityEngine;
using JCSUnity;
using MyBox;

/// <summary>
/// Game settings for Boss Fight example game.
/// </summary>
public class BF_GameSettings : JCS_Settings<BF_GameSettings>
{
    /* Variables */

    [Separator("Check Variables (BF_GameSettings)")]

    [SerializeField]
    [ReadOnly]
    private string mFullFilePath = "";

    [SerializeField]
    [ReadOnly]
    private string mFullFileName = "";

    [Header("- Player")]

    [Tooltip("How many character on players team.")]
    public int CHARACTERS_IN_TEAM = 4;

    // After player select some player then put the player transform 
    // here in order to spawn them in the game scene.
    [Tooltip("Player in the game will be store here.")]
    public BF_Player[] CHARACTERS_IN_GAME = null;

    [Header("- Save Load")]

    public string FILE_PATH = "SavedData/";

    public string FILE_NAME = "BF_GameData";

    public static BF_GameData GAME_DATA = null;

    [Header("- Game Feature")]

    [Tooltip("Maximum mob in the scene.")]
    [Range(10, 100)]
    public int TOTAL_MOB_IN_SCENE = 30;

    [Tooltip("Color that represent the freezing effect.")]
    public Color FREEZE_COLOR = Color.blue;

    [Tooltip("Color that represent the burning effect.")]
    public Color BURN_COLOR = Color.red;

    [Header("- Level")]

    [Tooltip("Name of the scene player selected.")]
    public string LEVEL_SELECTED_NAME = "";

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        instance = CheckSingleton(instance, this);
    }

    private void Start()
    {
        // IMPORTANT: initial the path before save and load!
        InitPath();

        // only load once
        if (GAME_DATA == null)
            LoadGameData();

        // set load and save game data
        var apps = JCS_ApplicationSettings.instance;
        apps.SAVE_APP_DATA_FUNC = SaveGameData;
        apps.LOAD_APP_DATA_FUNC = LoadGameData;
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
    protected override void TransferData(BF_GameSettings _old, BF_GameSettings _new)
    {
        _new.CHARACTERS_IN_TEAM = _old.CHARACTERS_IN_TEAM;
        _new.CHARACTERS_IN_GAME = _old.CHARACTERS_IN_GAME;
    }

    /// <summary>
    /// Initialize the path base on the JCSUnity Framework's 
    /// format.
    /// </summary>
    private void InitPath()
    {
        var apps = JCS_ApplicationSettings.instance;

        mFullFilePath = JCS_Path.Combine(Application.persistentDataPath, apps.DATA_PATH, FILE_PATH);
        mFullFileName = FILE_NAME + apps.DATA_EXTENSION;
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
        GAME_DATA = BF_GameData.LoadFromFile<BF_GameData>(mFullFilePath, mFullFileName);
    }

    /// <summary>
    /// Use only when player "First" play this game or 
    /// "Reset" the game.
    /// </summary>
    private void CreateDefaultGameData()
    {
        GAME_DATA = new BF_GameData();

        // Set game data's default values
        {
            GAME_DATA.Name = "";
            GAME_DATA.Cash = 1500;       // [default: 1500]
        }

        SaveGameData();
    }

    private void SaveGameData()
    {
        if (GAME_DATA == null)
        {
            JCS_Debug.LogError("Save Data without data");
            return;
        }

        GAME_DATA.Save<BF_GameData>(mFullFilePath, mFullFileName);
    }
}
