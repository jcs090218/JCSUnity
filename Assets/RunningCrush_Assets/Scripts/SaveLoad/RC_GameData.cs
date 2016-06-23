/**
 * $File: RC_GameData.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;
using System.Xml.Serialization;
using System.IO;

/// <summary>
/// Game Data design here.
/// </summary>
public class RC_GameData 
{

    //---------------------------------------------------
    // Data Struct
    public string Name { get; set; }        // name of the player
    public int Gold { get; set; }           // money in the game
    public int[] ItemNo { get; set; }       // Array of item unlocked.


    //---------------------------------------------------
    public void Save(string filePath, string fileName)
    {
        // if Directory does not exits, create it prevent error!
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        using (var stream = new FileStream(filePath + fileName, FileMode.Create))
        {
            var XML = new XmlSerializer(typeof(RC_GameData));
            XML.Serialize(stream, this);
        }
    }

    public static RC_GameData LoadFromFile(string filePath, string fileName)
    {
        // if Directory does not exits, create it prevent error!
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        using (var stream = new FileStream(filePath + fileName, FileMode.Open))
        {
            var xml = new XmlSerializer(typeof(RC_GameData));
            return (RC_GameData)xml.Deserialize(stream);
        }
    }

}
