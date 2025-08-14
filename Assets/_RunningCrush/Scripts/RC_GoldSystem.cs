/**
 * $File: RC_GoldSystem.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

/// <summary>
/// Gold system.
/// </summary>
public class RC_GoldSystem : MonoBehaviour
{
    /* Variables */

    public static RC_GoldSystem instance = null;

    private int mCurrentGold = -1;

    /* Setter & Getter */

    public int GetCurrentGold() { return mCurrentGold; }

    /* Functions */

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // get the gold from game data.
        mCurrentGold = RC_AppSettings.FirstInstance().APP_DATA.gold;
    }

    private void Update()
    {

    }

    public int DeltaGold(int val)
    {
        mCurrentGold += val;
        return mCurrentGold;
    }
}
