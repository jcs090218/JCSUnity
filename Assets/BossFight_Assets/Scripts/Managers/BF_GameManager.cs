/**
 * $File: BF_GameManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;
using UnityEngine.UI;

/// <summary>
/// Record down all the game information in order 
/// to have the correct conditional judgment.
/// </summary>
public class BF_GameManager 
    : MonoBehaviour 
{
    /* Variables */

    public static BF_GameManager instance = null;

    [Header("** Check Varaibles (BF_GameManager) **")]

    [Tooltip("Is the game over?")]
    public bool GAME_IS_OVER = false;

    [Tooltip("Current Level in the scene right now.")]
    public int CURRENT_LEVEL = 1;

    [Tooltip("Current exp in game scene.")]
    [SerializeField] private int mCurrentExp = 0;

    [Header("** Initialize Varaibles (BF_GameManager) **")]
    [Tooltip("How many level in this scene.")]
    public int TOTAL_LEVEL = 30;

    [Tooltip("EXP value to level up each level.")]
    public int[] EXP_TO_LEVEL_UP_PER_LEVEL = null;

    [Tooltip("Object we collect gold in this scene.")]
    public Collider COLLECT_GOLD_OBJECT = null;

    [Tooltip("Panel we want to pop when the game is over.")]
    public JCS_TweenPanel GAME_OVER_PANEL = null;

    [Tooltip("Check weather player win the game or lose the game.")]
    public BF_GameState GAME_STATE = BF_GameState.NONE;


    [Header("** Liquid Bar **")]

    [Tooltip("Object need to be protected.")]
    public JCS_2DLiveObject PROTECT_OBJECT = null;


    [Header("** Liquid Bar **")]

    [Tooltip("Health Liquid bar should be in the scene.")]
    public BF_LiquidBarHandler HEALTH_LIQUIDBAR = null;
    public Collider HEALTH_OBJECT = null;

    [Tooltip("Mana Liquid bar should be in the scene.")]
    public BF_LiquidBarHandler MANA_LIQUIDBAR = null;
    public Collider MANA_OBJECT = null;


    [Header("** GUI Settings **")]

    [Tooltip("String in-front of level.")]
    public string LEVEL_STRING = "Level ";

    [Tooltip("Text to show the level.")]
    public Text LEVEL_TEXT = null;

    [Tooltip("Current monster count in the scene.")]
    public int MOB_CURRENT_IN_SCENE = 0;


    [Header("** Health Target **")]

    [Tooltip("Win/Lose condition dependency.")]
    public BF_HealthTarget mHealthTarget = null;

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        instance = this;

        // initialize the level string.
        if (LEVEL_TEXT != null)
            LEVEL_TEXT.text = LEVEL_STRING + CURRENT_LEVEL.ToString();

        if (HEALTH_LIQUIDBAR != null)
        {
            // set call back when the hp bar is zero.
            HEALTH_LIQUIDBAR.LiquidBar.ZeroCallbackFunc = LostTheGame;
        }
    }

    public void DeltaCurrentExp(int val)
    {
        mCurrentExp += val;

        CheckLevelUp();
    }

    /// <summary>
    /// Call this when the game is over.
    /// </summary>
    public void EndGame()
    {
        // if game is already over, 
        // exit function call.
        if (GAME_IS_OVER)
            return;

        GAME_IS_OVER = true;

        if (mHealthTarget != null)
            mHealthTarget.LiveObject.CanDamage = false;
        else
        {
            JCS_Debug.LogReminder(
                "No health object in the assign...");
        }

        if (GAME_OVER_PANEL == null)
        {
            JCS_Debug.LogError(
                "No game over panel have been set.");

            return;
        }

        // active the game over panel.
        GAME_OVER_PANEL.Active();

        // Destroy all the live object in the scene.
        JCS_2DLiveObjectManager.instance.DestroyAllLiveObject();
    }

    private void CheckLevelUp()
    {
        // check if the current exp reach to the level's exp.
        if (EXP_TO_LEVEL_UP_PER_LEVEL[CURRENT_LEVEL] < mCurrentExp)
        {
            // REACHED!

            // level up
            ++CURRENT_LEVEL;

            // TODO(Jen-Chieh): level up event.
            LEVEL_TEXT.text = LEVEL_STRING + CURRENT_LEVEL.ToString();

            // reset exp for next level up
            mCurrentExp = 0;
        }

        if (CURRENT_LEVEL == TOTAL_LEVEL)
        {
            // beat the game!
            GAME_STATE = BF_GameState.WIN;

            // end the game when the level is reached.
            EndGame();

            // end the level text
            LEVEL_TEXT.text = "";
        }
    }

    private void LostTheGame()
    {
        GAME_STATE = BF_GameState.LOSE;

        EndGame();
    }

}
