/**
 * $File: FT_SaveLoad_Test.cs $
 * $Date: 2019-10-16 15:16:31 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2019 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Test save load module.
/// </summary>
public class FT_SaveLoad_Test : MonoBehaviour
{
    /* Variables */

    /* Setter/Getter */

    /* Functions */
    private void Awake()
    {
        string path = Application.dataPath + "/JCS_GameData/SavedData/FT_GameData.jcs";

        FT_JSONGameData data = new FT_JSONGameData();
        data.Save<FT_JSONGameData>(path);

        data.Cash = 10;

        print(data.Cash);
        data = FT_JSONGameData.LoadFromFile<FT_JSONGameData>(path);
        print(data.Cash);
    }
}
