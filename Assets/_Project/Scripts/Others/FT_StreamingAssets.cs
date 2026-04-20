/**
 * $File: FT_StreamingAssets.cs $
 * $Date: 2020-08-04 00:23:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using JCSUnity;
using MyBox;

/// <summary>
/// Test streaming assets module.
/// </summary>
public class FT_StreamingAssets : MonoBehaviour 
{
    /* Variables */

    [Separator("Runtime Variables (FT_StreamingAssets)")]

    [Tooltip("")]
    public List<string> dataPath = null;

    /* Setter & Getter */

    /* Functions */

    private void Update()
    {
        byte[] data = null;

        if (Input.GetKeyDown(KeyCode.Alpha0))
            data = JCS_Glob.streams.ReadAllBytes(dataPath[0]);

        if (Input.GetKeyDown(KeyCode.Alpha1))
            data = JCS_Glob.streams.ReadAllBytes(dataPath[1]);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            data = JCS_Glob.streams.ReadAllBytes(dataPath[2]);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            data = JCS_Glob.streams.ReadAllBytes(dataPath[3]);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            data = JCS_Glob.streams.ReadAllBytes(dataPath[4]);

        if (data != null)
        {
            string test = JCS_ResConverter.AsText(data, JCS_CharsetType.ASCII);
            print(test);
        }
    }
}
