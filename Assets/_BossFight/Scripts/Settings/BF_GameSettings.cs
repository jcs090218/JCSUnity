/**
 * $File: BF_GameSettings.cs $
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
/// Game settings for Boss Fight example game.
/// </summary>
public class BF_GameSettings : JCS_Settings<BF_GameSettings>
{
    /* Variables */

    [Separator("Runtime Variables (BF_GameSettings)")]

    [Header("Player")]

    [Tooltip("How many character on players team.")]
    public int CHARACTERS_IN_TEAM = 4;

    // After player select some player then put the player transform 
    // here in order to spawn them in the game scene.
    [Tooltip("Player in the game will be store here.")]
    public BF_Player[] CHARACTERS_IN_GAME = null;

    [Header("Game Feature")]

    [Tooltip("Maximum mob in the scene.")]
    [Range(10, 100)]
    public int TOTAL_MOB_IN_SCENE = 30;

    [Tooltip("Color that represent the freezing effect.")]
    public Color FREEZE_COLOR = Color.blue;

    [Tooltip("Color that represent the burning effect.")]
    public Color BURN_COLOR = Color.red;

    [Header("Level")]

    [Tooltip("Name of the scene player selected.")]
    public string LEVEL_SELECTED_NAME = "";

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        CheckInstance(this);
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
}
