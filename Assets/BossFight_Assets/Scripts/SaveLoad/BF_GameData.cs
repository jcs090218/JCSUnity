/**
 * $File: BF_GameData.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using System.Collections;
using UnityEngine;
using JCSUnity;


/// <summary>
/// Game Data design here.
/// </summary>
[Serializable]
public class BF_GameData 
    : JCS_BinGameData
{
    [Serializable]
    public struct ItemIdInclude
    {
        public bool ItemInclude;
        public BF_ItemId ItemId;
    };


    //---------------------------------------------------
    // Data Struct
    public string Name = "";        // name of the player
    public int Cash = 0;            // cash in the game

    public ItemIdInclude[] ItemsInclude = null;


    //---------------------------------------------------
    public BF_GameData()
    {

        // get the length of the enum.
        int enumLength = Enum.GetNames(typeof(BF_ItemId)).Length;

        // create the list of the enum.
        ItemsInclude = new ItemIdInclude[enumLength];

        // initialize the item list.
        InitItemInclude();
    }

    /// <summary>
    /// Careful, if you call this incorrectly it will 
    /// clear all the game data.
    /// </summary>
    private void InitItemInclude()
    {
        int index = 0;
        foreach (BF_ItemId val in Enum.GetValues(typeof(BF_ItemId)))
        {
            ItemsInclude[index].ItemId = val;
            ItemsInclude[index].ItemInclude = false;
            ++index;
        }
    }

}
