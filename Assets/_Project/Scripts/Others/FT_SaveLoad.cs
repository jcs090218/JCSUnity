/**
 * $File: FT_SaveLoad.cs $
 * $Date: 2019-10-16 15:16:31 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2019 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

/// <summary>
/// Test save load module.
/// </summary>
public class FT_SaveLoad : MonoBehaviour
{
    /* Variables */

    /* Setter/Getter */

    /* Functions */
    private void Awake()
    {
        string path = JCS_Path.Combine(Application.persistentDataPath, "/Data_jcs/SavedData/FT_GameData.jcs");

        var data = new FT_JSONAppData();
        data.Save<FT_JSONAppData>(path);

        data.Cash = 10;

        print(data.Cash);
        data = FT_JSONAppData.LoadFromFile<FT_JSONAppData>(path);
        print(data.Cash);
    }
}
