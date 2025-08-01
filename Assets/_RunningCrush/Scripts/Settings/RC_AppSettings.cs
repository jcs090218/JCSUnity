/**
 * $File: RC_AppSettings.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2023 by Shen, Jen-Chieh $
 */
using System.IO;
using UnityEngine;
using JCSUnity;
using MyBox;

public class RC_AppSettings : JCS_Settings<RC_AppSettings>
{
    /* Variables */

    [Separator("Check Variables (RC_GameSettings)")]

    [SerializeField]
    [ReadOnly]
    private string mFullFilePath = "";

    [SerializeField]
    [ReadOnly]
    private string mFullFileName = "";

    [Separator("Runtime Variables (RC_AppSettings)")]

    [Header("- Save Load")]

    public string FILE_PATH = "SavedData/";

    public string FILE_NAME = "RC_AppData";

    public RC_AppData APP_DATA = null;

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        CheckInstance(this);
    }

    private void Start()
    {
        // IMPORTANT: initial the path before save and load!
        InitPath();

        // only load once
        if (!APP_DATA.Initialized())
            LoadAppData();

        // set load and save game data
        var apps = JCS_AppSettings.FirstInstance();
        apps.SAVE_APP_DATA_FUNC = SaveAppData;
        apps.LOAD_APP_DATA_FUNC = LoadAppData;
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
    protected override void TransferData(RC_AppSettings _old, RC_AppSettings _new)
    {
        _new.APP_DATA = _old.APP_DATA;
    }

    /// <summary>
    /// Initialize the path base on the JCSUnity Framework's 
    /// format.
    /// </summary>
    private void InitPath()
    {
        var apps = JCS_AppSettings.FirstInstance();

        mFullFilePath = JCS_Path.Combine(Application.persistentDataPath, apps.DATA_PATH, FILE_PATH);
        mFullFileName = FILE_NAME + apps.DATA_EXTENSION;
    }

    private void LoadAppData()
    {
        JCS_IO.CreateDirectory(mFullFilePath);

        // if file does not exist, create the default value file!
        if (!File.Exists(mFullFilePath + mFullFileName))
        {
            CreateDefaultGameData();
            return;
        }

        // else we just load the data commonly.
        APP_DATA = JCS_XMLData.LoadFromFile<RC_AppData>(mFullFilePath, mFullFileName);
    }

    /// <summary>
    /// Use only when player "First" play this game or 
    /// "Reset" the game.
    /// </summary>
    private void CreateDefaultGameData()
    {
        APP_DATA = new RC_AppData();

        // Set game data's default values
        {
            APP_DATA.Name = "";
            APP_DATA.Gold = 1500;       // [default: 1500]

            APP_DATA.ItemNo = null;
        }

        // save it once
        SaveAppData();
    }

    private void SaveAppData()
    {
        if (APP_DATA == null)
        {
            Debug.LogError("Save Data without data");
            return;
        }

        APP_DATA.Save<RC_AppData>(mFullFilePath, mFullFileName);
    }
}
